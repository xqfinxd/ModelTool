using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private Scene scene = new Scene();
        private FileInfo fileInfo = null;
        private string name = null;
        private DirectoryInfo outDirInfo = null;
        private string luaScript = null;
        private string shared = "";
        public string SharedDir
        {
            get { return shared; }
            set { shared = value?.TrimEnd('\\'); }
        }

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
        }

        public void DoWrite()
        {
            for (int i = 0; i < scene.MeshCount; i++)
            {
                Console.WriteLine(@"{0}>>start to generate meshes[{1}]...", name, i);
                Console.WriteLine(@"{0}>>  meshes[{1}] >> name : {2}", name, i, scene.Meshes[i].Name);
                WriteMesh(i);
                Console.WriteLine(@"{0}>>generate meshes[{0}] success!", name, i);
            }
            for (int i = 0; i < scene.CameraCount; i++)
            {
                Console.WriteLine(@"{0}>>start to generate camera[{1}]...", name, i);
                Console.WriteLine(@"{0}>>  cameras[{1}] >> name : {2}", name, i, scene.Cameras[i].Name);
                WriteCamera(i);
                Console.WriteLine(@"{0}>>generate cameras[{0}] success!", name, i);
            }
            for (int i = 0; i < scene.LightCount; i++)
            {
                Console.WriteLine(@"{0}>>start to generate lights[{1}]...", name, i);
                Console.WriteLine(@"{0}>>  lights[{1}] >> name : {2}", name, i, scene.Lights[i].Name);
                WriteLight(i);
                Console.WriteLine(@"{0}>>generate lights[{0}] success!", name, i);
            }
            WriteScript();
        }

        public string GetMeshDir(int index)
        {
            return string.Format(@"{0}\{1:D4}-{2}",
                outDirInfo.FullName, index, scene.Meshes[index].Name);
        }

        public string GetLightPath(int index)
        {
            return string.Format(@"{0}\{1:D4}-{2}.light",
                outDirInfo.FullName, index, scene.Lights[index].Name);
        }

        public string GetCameraPath(int index)
        {
            return string.Format(@"{0}\{1:D4}-{2}.camera",
                outDirInfo.FullName, index, scene.Cameras[index].Name);
        }

        private bool WriteMesh(int index)
        {
            if (index >= scene.MeshCount)
            {
                return false;
            }
            Mesh mesh = scene.Meshes[index];
            //根据index和mesh.name创建文件夹
            string dirName = GetMeshDir(index);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            //在文件夹内创建数据文件并写入二进制数据
            if (mesh.HasVertices)
            {
                WriteData(dirName + @"\vertex.mesh", ToArray(mesh.Vertices));
            }
            if (mesh.HasNormals)
            {
                WriteData(dirName + @"\normal.mesh", ToArray(mesh.Normals));
            }
            if (mesh.HasTangentBasis)
            {
                WriteData(dirName + @"\tangent.mesh", ToArray(mesh.Tangents));
                WriteData(dirName + @"\bitangent.mesh", ToArray(mesh.BiTangents));
            }
            if (mesh.HasFaces && mesh.FaceCount > 0)
            {
                WriteData(dirName + @"\face.mesh", mesh.GetUnsignedIndices());
            }
            for (int i = 0; i < mesh.TextureCoordinateChannelCount; i++)
            {
                if (mesh.HasTextureCoords(i))
                {
                    WriteData(
                        string.Format(@"{0}\uv{1}.mesh", dirName, i),
                        ToArray(mesh.TextureCoordinateChannels[i], true)
                    );
                }
            }
            for (int i = 0; i < mesh.VertexColorChannelCount; i++)
            {
                if (mesh.HasVertexColors(i))
                {
                    WriteData(
                        string.Format(@"{0}\color{1}.mesh", dirName, i),
                        ToArray(mesh.VertexColorChannels[i])
                    );
                }
            }
            return true;
        }

        private void WriteData(string fullPath, byte[] datas)
        {
            Console.WriteLine(@"{0}>>    write file {1}", name, fullPath);
            FileStream file = new FileStream(fullPath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(datas);
            writer.Close();
            file.Close();
            Console.WriteLine(@"{0}>>    file size {1}(bytes)", name, datas.Length);
        }

        private void WriteData(string fullPath, uint[] datas)
        {
            Console.WriteLine(@"{0}>>    write file {1}", name, fullPath);
            FileStream file = new FileStream(fullPath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            for (int i = 0; i < datas.Length; i++)
            {
                writer.Write(datas[i]);
            }
            writer.Close();
            file.Close();
            Console.WriteLine(@"{0}>>    file size {1}(bytes)", name, datas.Length * sizeof(uint));
        }

        private void WriteData(string fullPath, string data)
        {
            Console.WriteLine(@"{0}>>    write file {1}", name, fullPath);
            StreamWriter writer = new StreamWriter(fullPath, false);
            writer.Write(data);
            writer.Close();
            Console.WriteLine(@"{0}>>    file size {1}(cahrs)", name, data.Length);
        }

        private void WriteCamera(int index)
        {
            var camera = scene.Cameras[index];
            List<byte> datas = new List<byte>(160);
            datas.AddRange(BitConverter.GetBytes(camera.Position.X));
            datas.AddRange(BitConverter.GetBytes(camera.Position.Y));
            datas.AddRange(BitConverter.GetBytes(camera.Position.Z));
            datas.AddRange(BitConverter.GetBytes(camera.Up.X));
            datas.AddRange(BitConverter.GetBytes(camera.Up.Y));
            datas.AddRange(BitConverter.GetBytes(camera.Up.Z));
            datas.AddRange(BitConverter.GetBytes(camera.Direction.X));
            datas.AddRange(BitConverter.GetBytes(camera.Direction.Y));
            datas.AddRange(BitConverter.GetBytes(camera.Direction.Z));
            datas.AddRange(BitConverter.GetBytes(camera.FieldOfview));
            datas.AddRange(BitConverter.GetBytes(camera.ClipPlaneNear));
            datas.AddRange(BitConverter.GetBytes(camera.ClipPlaneFar));
            datas.AddRange(BitConverter.GetBytes(camera.AspectRatio));
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    datas.AddRange(BitConverter.GetBytes(camera.ViewMatrix[i, j]));
                }
            }
            var enc = Encoding.GetEncoding("utf-8");
            datas.AddRange(enc.GetBytes(camera.Name));
            string fullpath = string.Format(
                @"{0}\{1:D4}-{2}.camera",
                outDirInfo.FullName, index, camera.Name);
            WriteData(fullpath, datas.ToArray());
        }

        private void WriteLight(int index)
        {
            var light = scene.Lights[index];
            List<byte> datas = new List<byte>(160);
            datas.AddRange(BitConverter.GetBytes((UInt32)light.LightType));
            datas.AddRange(BitConverter.GetBytes(light.AngleInnerCone));
            datas.AddRange(BitConverter.GetBytes(light.AngleOuterCone));
            datas.AddRange(BitConverter.GetBytes(light.AttenuationConstant));
            datas.AddRange(BitConverter.GetBytes(light.AttenuationLinear));
            datas.AddRange(BitConverter.GetBytes(light.AttenuationQuadratic));
            datas.AddRange(BitConverter.GetBytes(light.Position.X));
            datas.AddRange(BitConverter.GetBytes(light.Position.Y));
            datas.AddRange(BitConverter.GetBytes(light.Position.Z));
            datas.AddRange(BitConverter.GetBytes(light.Direction.X));
            datas.AddRange(BitConverter.GetBytes(light.Direction.Y));
            datas.AddRange(BitConverter.GetBytes(light.Direction.Z));
            datas.AddRange(BitConverter.GetBytes(light.Up.X));
            datas.AddRange(BitConverter.GetBytes(light.Up.Y));
            datas.AddRange(BitConverter.GetBytes(light.Up.Z));
            datas.AddRange(BitConverter.GetBytes(light.ColorDiffuse.R));
            datas.AddRange(BitConverter.GetBytes(light.ColorDiffuse.G));
            datas.AddRange(BitConverter.GetBytes(light.ColorDiffuse.B));
            datas.AddRange(BitConverter.GetBytes(light.ColorSpecular.R));
            datas.AddRange(BitConverter.GetBytes(light.ColorSpecular.G));
            datas.AddRange(BitConverter.GetBytes(light.ColorSpecular.B));
            datas.AddRange(BitConverter.GetBytes(light.ColorAmbient.R));
            datas.AddRange(BitConverter.GetBytes(light.ColorAmbient.G));
            datas.AddRange(BitConverter.GetBytes(light.ColorAmbient.B));
            datas.AddRange(BitConverter.GetBytes(light.AreaSize.X));
            datas.AddRange(BitConverter.GetBytes(light.AreaSize.Y));

            var enc = Encoding.GetEncoding("utf-8");
            datas.AddRange(enc.GetBytes(light.Name));
            string fullpath = string.Format(
                @"{0}\{1:D4}-{2}.light",
                outDirInfo.FullName, index, light.Name);
            WriteData(fullpath, datas.ToArray());
        }

        private void WriteScript()
        {
            var root = scene.RootNode;
            luaScript = string.Format(@"
local scene = dofile('{1}/model.lua')

local root = scene.rootnode
local name = '{0}'
local node = scene.rootnode

", name, SharedDir.Replace(@"\", @"/"));

            WriteNode(root);
            WriteData(Path.Combine(fileInfo.DirectoryName, name + ".lua"), luaScript);
        }

        private void WriteNode(Node node)
        {
            luaScript += string.Format("node.name = \"{0}\"\n", node.Name);
            luaScript += string.Format("node:settransform({0})\n", MakeString(node.Transform));
            if (node.HasMeshes)
            {
                for (int i = 0; i < node.MeshCount; i++)
                {
                    var index = node.MeshIndices[i];
                    luaScript += string.Format("node:addmesh(\"{0}\",\"{1}\")\n",
                        scene.Meshes[index].Name, GetMeshDir(index).Replace(@"\", @"/"));
                }
            }
            var lightIndex = isLight(node);
            if (lightIndex > -1)
            {
                luaScript += string.Format("node:setlight(\"{0}\")\n",
                    GetLightPath(lightIndex).Replace(@"\", @"/"));
            }
            var cameraIndex = isCamera(node);
            if (cameraIndex > -1)
            {
                luaScript += string.Format("node:setcamera(\"{0}\")\n",
                    GetCameraPath(cameraIndex).Replace(@"\", @"/"));
            }
            for (int i = 0; i < node.ChildCount; i++)
            {
                luaScript += "node = node:addchild()\n";
                WriteNode(node.Children[i]);
            }
            luaScript += "node = node.parent\n\n";
        }

        private int isLight(Node node)
        {
            if (!scene.HasLights)
            {
                return -2;
            }
            for (int i = 0; i < scene.LightCount; i++)
            {
                if (node.Name == scene.Lights[i].Name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int isCamera(Node node)
        {
            if (!scene.HasCameras)
            {
                return -2;
            }
            for (int i = 0; i < scene.CameraCount; i++)
            {
                if (node.Name == scene.Cameras[i].Name)
                {
                    return i;
                }
            }
            return -1;
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

        private byte[] ToArray(List<Color4D> colors, bool is3c = false)
        {
            if (colors.Count <= 0)
            {
                return null;
            }
            int step = (is3c ? 3 : 4) * sizeof(float);
            List<byte> array = new List<byte>(colors.Count * step);
            for (int i = 0; i < colors.Count; i++)
            {
                array.AddRange(BitConverter.GetBytes(colors[i].R));
                array.AddRange(BitConverter.GetBytes(colors[i].G));
                array.AddRange(BitConverter.GetBytes(colors[i].B));
                if (!is3c)
                {
                    array.AddRange(BitConverter.GetBytes(colors[i].A));
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

        private string MakeString(Matrix4x4 mat)
        {
            return "{" + string.Format(@"
    {0}, {1}, {2}, {3},
    {4}, {5}, {6}, {7},
    {8}, {9}, {10}, {11},
    {12}, {13}, {14}, {15},
",
                mat.A1, mat.A2, mat.A3, mat.A4,
                mat.B1, mat.B2, mat.B3, mat.B4,
                mat.C1, mat.C2, mat.C3, mat.C4,
                mat.D1, mat.D2, mat.D3, mat.D4
            ) + "}";
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