using Assimp;
using System.IO;

namespace ModelTool
{
    internal class ModelUtil
    {
        public Scene scene = new Scene();

        public ModelUtil(string file)
        {
            AssimpContext importer = new AssimpContext();
            FileInfo info = new FileInfo(file);

            if (info.Exists && importer.IsImportFormatSupported(info.Extension))
            {
                scene = importer.ImportFile(file,
                    PostProcessSteps.GenerateNormals |
                    PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.Triangulate);
            }
        }
    }
}