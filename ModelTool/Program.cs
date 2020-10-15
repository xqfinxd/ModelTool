using System;

namespace ModelTool
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ModelUtil modelUtil = new ModelUtil(@"E:\GITHUB\assimp\test\models\FBX\spider.fbx");
            Console.ReadKey();
        }
    }
}