using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FontAwesomeWinForms
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You clicked me");
        }
    }
}
