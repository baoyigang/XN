using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Class1
    {
        private static readonly Class1 class1 = new Class1();

        private Class1() { }

        static  Dictionary<string, string> dc = new Dictionary<string, string>();

        public static Class1 GetInstance() 
        {
            return class1;
        }
        public static void testAdd() 
        {
            if (dc.ContainsKey("a"))
            {
                return;
            }
            Thread.Sleep(3000);
            dc.Add("a", "b");
        }

        public string a;

        public void TestReturn(ClassTest f, string x) 
        {
            Thread.Sleep(3000);
            Label l =(Label)f.Controls.Find("Label",true)[0];
            l.Text = x;
        }
    }
}
