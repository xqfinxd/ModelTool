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
            ModelUtil modelUtil = new ModelUtil(@"..\..\..\assets\spider.fbx");
            Console.ReadKey();
        }
    }
}