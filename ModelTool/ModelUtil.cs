using Assimp;
using System.IO;
using System;
using System.Collections.Generic;

namespace ModelTool
{
    internal class ModelUtil
    {
        public Scene scene = new Scene();
        private FileInfo info = null;
        private string name = null;
        private Matrix4x4[] matrix4s;
        private string require = "Mesh = require(\"mesh\")\n";

        static private Func<Vector2D, string> printVec2d = v => {
            return string.Format("    {0}, {1},\n", v.X, v.Y);
        };

        static private Func<Vector3D, string> printVec3d2 = v => {
            return string.Format("    {0}, {1},\n", v.X, v.Y);
        };

        static private Func<Vector3D, string> printVec3d = v => {
            return string.Format("    {0}, {1}, {2},\n", v.X, v.Y, v.Z);
        };

        static private Func<Face, string> printFace = v => {
            return string.Format("    {0}, {1}, {2},\n", v.Indices[0], v.Indices[1], v.Indices[2]);
        };

        static private Func<Matrix4x4, string> printMat4x4 = v => {
            string result = "";
            for (int i = 1; i <= 4; i++)
            {
                result += "    ";
                for (int j = 1; j <= 4; j++)
                {
                    result += v[i, j].ToString() + ", ";
                }
                result = result.TrimEnd(' ');
                result += '\n';
            }
            return result;
        };

        public ModelUtil(string file)
        {
            AssimpContext importer = new AssimpContext();
            info = new FileInfo(file);

            if (info.Exists && importer.IsImportFormatSupported(info.Extension))
            {
                scene = importer.ImportFile(file,
                    PostProcessSteps.GenerateNormals |
                    PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.Triangulate);
                Console.WriteLine("Load File <{0}> {1}!", info.FullName, 
                    IsEmpty() ? "Failure" : "Success");
            }
            else
            {
                Console.WriteLine("File <{0}> doesn't exist or " +
                    "extension <{1}> isn't supported!", 
                    info.FullName, info.Extension);
            }
            name = info.Name.Substring(0, info.Name.LastIndexOf('.'));
            matrix4s = new Matrix4x4[scene.MeshCount];
            CalcMeshesMatrix();
        }

        public bool IsEmpty()
        {
            return scene.RootNode == null || !scene.HasMeshes;
        }

        public string Mesh2Lua(Int32 meshIndex)
        {
            if (meshIndex >= scene.MeshCount)
            {
                return null;
            }
            Mesh mesh = scene.Meshes[meshIndex];
            string allData = "";
            if (mesh.HasVertices)
            {
                string tmp = name + ".vertex = ";
                tmp += Data2Lua<Vector3D>(mesh.Vertices, printVec3d);
                allData += tmp + "\n\n";
            }

            if (mesh.HasNormals)
            {
                string tmp = name + ".normal = ";
                tmp += Data2Lua<Vector3D>(mesh.Normals, printVec3d);
                allData += tmp + "\n\n";
            }

            if (mesh.HasTextureCoords(0))
            {
                string tmp = name + ".texCoords = ";
                tmp += Data2Lua<Vector3D>(mesh.TextureCoordinateChannels[0], printVec3d2);
                allData += tmp + "\n\n";
            }

            if (mesh.HasFaces)
            {
                string tmp = name + ".face = ";
                tmp += Data2Lua<Face>(mesh.Faces, printFace);
                allData += tmp + "\n\n";
            }

            {
                string tmp = name + ".transform = ";
                List<Matrix4x4> mat = new List<Matrix4x4>() { matrix4s[meshIndex] };
                tmp += Data2Lua<Matrix4x4>(mat, printMat4x4);
                allData += tmp + "\n\n";
            }

            string meshStr = string.Format(@"
local {0} = Mesh:new()

{0}.name = '{0}'
{1}
return {0}
", name, allData);
            //Console.WriteLine(meshStr);
            return meshStr;
        }

        private string Data2Lua<DATA>(List<DATA> data, Func<DATA, string> func)
        {
            string prefix = "{\n";
            string suffix = "}";
            string dataStr = "";
            for (int i = 0; i < data.Count; i++)
            {
                dataStr += func(data[i]);
            }
            return prefix + dataStr + suffix;
        }

        private void CalcMeshesMatrix()
        {
            Node root = scene.RootNode;
            CalcNodeMatrix(root, Matrix4x4.Identity);
        }

        private void CalcNodeMatrix(Node node, Matrix4x4 parent)
        {
            Matrix4x4 curMat = parent * node.Transform;
            if (node.HasMeshes)
            {
                for (int i = 0; i < node.MeshCount; i++)
                {
                    matrix4s[node.MeshIndices[i]] = curMat;
                }
            }
            if (node.HasChildren)
            {
                for (int i = 0; i < node.ChildCount; i++)
                {
                    CalcNodeMatrix(node.Children[i], curMat);
                }
            }
        }
    }
}