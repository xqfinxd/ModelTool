﻿using System;

namespace ModelTool
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string inputFile = null;
            string inputShared = null;
            for (int i = 0; i < args.Length; i++)
            {
                var param = args[i].Trim(' ');
                param = param.Trim('\"');
                if (param.Contains("--file="))
                {
                    inputFile = param.Substring(7);
                }
                if (param.Contains("--shared="))
                {
                    inputShared = param.Substring(9);
                }
            }
            if (inputFile == null)
            {
                Console.WriteLine("using --file=\"...\" --shared=\"...\"");
                return;
            }
            ModelUtil modelUtil = new ModelUtil(inputFile);
            if (inputShared == null)
            {
                modelUtil.SharedDir = "shared";
            }
            else
            {
                modelUtil.SharedDir = inputShared;
            }
            modelUtil.DoWrite();
            //Console.ReadKey();
        }
    }
}