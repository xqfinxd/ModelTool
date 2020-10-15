﻿using Assimp;
using System.IO;
using System;
using System.Collections.Generic;

namespace ModelTool
{
    /*文件目录格式如下 []：表示文件夹 ||：表示文件
     * **>|| sample.fbx
     * **>[] sample
     * **>  [] 0000-body
     * **>    || vertex.bin
     * **>    || face.bin
     * **>    || ...
     * **>  [] 0001-head
     * **>  [] 0002-leg
     * **>  [] ...
     */
    internal class ModelUtil
    {
        public Scene scene = new Scene();
        private FileInfo fileInfo = null;
        private string name = null;
        private DirectoryInfo outDirInfo = null;

        public ModelUtil(string file)
        {
            AssimpContext importer = new AssimpContext();
            fileInfo = new FileInfo(file);

            if (fileInfo.Exists && importer.IsImportFormatSupported(fileInfo.Extension))
            {
                scene = importer.ImportFile(file,
                    PostProcessSteps.GenerateNormals |
                    PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.Triangulate);
            }
            else
            {
                Console.WriteLine("Extension <{0}> isn't supported!", fileInfo.Extension);
            }

            name = fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf('.'));

            string outDir = fileInfo.DirectoryName + @"\" + name;
            if (!Directory.Exists(outDir))
            {
                outDirInfo = Directory.CreateDirectory(outDir);
            }
            else
            {
                outDirInfo = new DirectoryInfo(outDir);
            }
            WriteMesh(0);
        }

        private bool WriteMesh(int index)
        {
            if (index >= scene.MeshCount)
            {
                return false;
            }
            Mesh mesh = scene.Meshes[index];
            //根据index和mesh.name创建文件夹
            string dirName = string.Format(@"{0}\{1:D4}-{2}",
                outDirInfo.FullName, index, mesh.Name);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            //在文件夹内创建数据文件并写入二进制数据
            if (mesh.HasVertices)
            {
                WriteData(dirName + @"\vertex.bin", ToArray(mesh.Vertices));
            }
            if (mesh.HasNormals)
            {
                WriteData(dirName + @"\normal.bin", ToArray(mesh.Normals));
            }
            if (mesh.HasTangentBasis)
            {
                WriteData(dirName + @"\tangent.bin", ToArray(mesh.Tangents));
                WriteData(dirName + @"\bitangent.bin", ToArray(mesh.BiTangents));
            }
            if (mesh.HasFaces && mesh.FaceCount > 0)
            {
                WriteData(dirName + @"\face.bin", mesh.GetUnsignedIndices());
            }

            return true;
        }

        private void WriteData(string fullPath, byte[] datas)
        {
            FileStream file = new FileStream(fullPath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(datas);
            writer.Close();
            file.Close();
        }

        private void WriteData(string fullPath, uint[] datas)
        {
            FileStream file = new FileStream(fullPath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            for (int i = 0; i < datas.Length; i++)
            {
                writer.Write(datas[i]);
            }
            writer.Close();
            file.Close();
        }

        public bool IsEmpty()
        {
            return scene.RootNode == null || !scene.HasMeshes;
        }

        private byte[] ToArray(List<Vector2D> vectors)
        {
            if (vectors.Count <= 0)
            {
                return null;
            }
            List<byte> array = new List<byte>(vectors.Count * 2 * sizeof(float));
            for (int i = 0; i < vectors.Count; i++)
            {
                array.AddRange(BitConverter.GetBytes(vectors[i].X));
                array.AddRange(BitConverter.GetBytes(vectors[i].Y));
            }
            return array.ToArray();
        }

        private byte[] ToArray(List<Vector3D> vectors, bool is2d = false)
        {
            if (vectors.Count <= 0)
            {
                return null;
            }
            int step = (is2d ? 2 : 3) * sizeof(float);
            List<byte> array = new List<byte>(vectors.Count * step);
            for (int i = 0; i < vectors.Count; i++)
            {
                array.AddRange(BitConverter.GetBytes(vectors[i].X));
                array.AddRange(BitConverter.GetBytes(vectors[i].Y));
                if (!is2d)
                {
                    array.AddRange(BitConverter.GetBytes(vectors[i].Z));
                }
            }
            return array.ToArray();
        }

        private float[] ToArray(Matrix4x4 mat)
        {
            return new float[4 * 4] {
                mat.A1, mat.A2, mat.A3, mat.A4,
                mat.B1, mat.B2, mat.B3, mat.B4,
                mat.C1, mat.C2, mat.C3, mat.C4,
                mat.D1, mat.D2, mat.D3, mat.D4,
            };
        }

        private float[] ToArray(Matrix3x3 mat)
        {
            return new float[3 * 3] {
                mat.A1, mat.A2, mat.A3,
                mat.B1, mat.B2, mat.B3,
                mat.C1, mat.C2, mat.C3,
            };
        }
    }
}