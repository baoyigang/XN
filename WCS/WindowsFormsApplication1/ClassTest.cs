using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class ClassTest : Form
    {
        public ClassTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(Class1.testAdd);
            //th.Start();
            // Class1 a =  Class1.GetInstance();
            // a.a = "abcdef";
            // label1.Text = a.a;
            Class1 a =  Class1.GetInstance();

            //label1.Text = r;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(Class1.testAdd);
            //th.Start();
            //Class1 b = Class1.GetInstance();
            //label2.Text = b.a;
            Class1 b = Class1.GetInstance();
           // Func<string, string> func = b.TestReturn;
            //string r = func.Invoke("a");
           // label2.Text = r;
        }
    }
}
