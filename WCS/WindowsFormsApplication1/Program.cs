﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program1
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClassTest());
        }
    }
    public class RtnMessage
    {
        public string id { get; set; }
        public string returnCode { get; set; }
        public string message { get; set; }
        public string finishDate { get; set; }
        public string field1 { get; set; }
    }
}
