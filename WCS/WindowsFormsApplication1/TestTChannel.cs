using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class TestTChannel : Form
    {
        public TestTChannel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.label1.Text = channelTest<object>.GetInstance();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.label2.Text = channelTest<string>.GetInstance();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.label3.Text = channelTest<string>.a;
        }
    }
}
