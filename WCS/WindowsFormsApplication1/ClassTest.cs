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
    //测试server.Getchannel多线程中的问题
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
            Func<string, string> func = a.getString;
            IAsyncResult r = func.BeginInvoke("abc", new AsyncCallback(ync), "abc");
            //string St = a.getString("abc");
            //label1.Text = r;
            if (r.AsyncWaitHandle.WaitOne())
            {
                label1.Text = r.AsyncState.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread th = new Thread(Class1.testAdd);
            //th.Start();
            //Class1 b = Class1.GetInstance();
            //label2.Text = b.a;
            Class1 b = Class1.GetInstance();
            Func<string, string> funb = b.getString;
            IAsyncResult r = funb.BeginInvoke("abc", new AsyncCallback(ync), "def");
            
            if (r.AsyncWaitHandle.WaitOne())
            {
                label2.Text = r.AsyncState.ToString();   
            }
           // Func<string, string> func = b.TestReturn;
            //string r = func.Invoke("a");
           // label2.Text = r;
        }

        public void ync(IAsyncResult r) { }
    }
}
