using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace FontAwesome.Windows.Forms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pictureBox.Image = FontAwesome.Common.GetIcon(IconType.Certificate, 128, Color.White);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var gadient = new LinearGradientBrush(new Point(0,0), new Point(127,127), Color.Blue, Color.Red );
            this.pictureBox.Image = FontAwesome.Common.GetIcon(IconType.Money, 128, gadient);
        }

    }
}
