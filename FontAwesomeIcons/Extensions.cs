using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontAwesome.Windows.Forms
{
    public static class Extensions
    {

        public static void SetIcon(ref ToolStripItem Control, IconType type)
        {
            Control.Font = new Font(Common.Fonts.Families[0], Control.Font.Size, GraphicsUnit.Point);
            Control.Text = char.ConvertFromUtf32((int)type);
        }


    }
}
