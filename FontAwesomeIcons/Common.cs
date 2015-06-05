using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace FontAwesome
{
    public static class Common
    {

        /// <summary>
        /// Initializes the <see cref="Icon" /> class by loading the font from resources upon first use.
        /// </summary>
        static Common()
        {
            InitializeFont();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
           IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        /// <summary>
        /// Store the icon font in a static variable to reuse between icons
        /// </summary>
        internal static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

        /// <summary>
        /// Store the font in a static variable to quick reference
        /// </summary>
        private static Font iconFont;

        /// <summary>
        /// Loads the icon font from the resources.
        /// </summary>
        private static void InitializeFont()
        {
            //try
            //{
            //    byte[] pFontData = Properties.Resources.fontawesome_webfont;
            //    IntPtr fontBuffer = Marshal.AllocCoTaskMem(pFontData.Length);
            //    Marshal.Copy(pFontData, 0, fontBuffer, pFontData.Length);

            //    uint dummy = 0;
            //    Fonts.AddMemoryFont(fontBuffer, pFontData.Length);
            //    AddFontMemResourceEx((IntPtr)fontBuffer, (uint)pFontData.Length, IntPtr.Zero, ref dummy);

            //    // log?
            //}
            //catch //(Exception ex)
            //{
            //}

            try
            {
                unsafe
                {
                    fixed (byte* pFontData = Properties.Resources.fontawesome_webfont)
                    {
                        uint dummy = 0;
                        Fonts.AddMemoryFont((IntPtr)pFontData, Properties.Resources.fontawesome_webfont.Length);
                        AddFontMemResourceEx((IntPtr)pFontData, (uint)Properties.Resources.fontawesome_webfont.Length, IntPtr.Zero, ref dummy);
                    }
                }
            }
            catch //(Exception ex)
            {
                // log?
            }
        }

        #region Methods

        /// <summary>
        /// Sets a new font with approprate size for the allocated space.
        /// </summary>
        /// <param name="g">The g.</param>
        private static void SetFontSize(Graphics g, string IconChar)
        {
            var Width = (int)g.VisibleClipBounds.Width;
            var Height = (int)g.VisibleClipBounds.Height;
            iconFont = GetAdjustedFont(g, IconChar, Width, Height, 4, true);
        }

        /// <summary>
        /// Returns a font instance using the resource icon font.
        /// </summary>
        /// <param name="size">The size of the font in points.</param>
        /// <returns>A new System.Drawing.Font instance</returns>
        private static Font GetIconFont(float size)
        {
            return new Font(Fonts.Families[0], size, GraphicsUnit.Point);
        }

        /// <summary>
        /// Returns a new font that fits the specified character into the allocated space.
        /// </summary>
        /// <param name="g">The graphics object.</param>
        /// <param name="graphicString">The string (icon character) to render as a graphic.</param>
        /// <param name="containerWidth">Width of the container.</param>
        /// <param name="maxFontSize">Size of the max font.</param>
        /// <param name="minFontSize">Size of the min font.</param>
        /// <param name="smallestOnFail">if set to <c>true</c> [smallest on fail].</param>
        /// <returns></returns>
        private static Font GetAdjustedFont(Graphics g, string graphicString, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            for (double adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize = adjustedSize - 0.5)
            {
                Font testFont = GetIconFont((float)adjustedSize);
                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont);
                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Fits! return it
                    return testFont;
                }
            }

            // Could not find a font size
            // return min or max or maxFontSize?
            return GetIconFont(smallestOnFail ? minFontSize : maxFontSize);
        }


        public static System.Drawing.Image GetIcon(IconType type, int size, Color color)
        {
            return GetIcon(type, size, new SolidBrush(color));
        }

        public static System.Drawing.Image GetIcon(IconType type, int size, Brush brush)
        {
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(size, size);
            string IconChar = char.ConvertFromUtf32((int)type);

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(result))
            {
                // Set best quality
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // is the font ready to go?
                if (iconFont == null)
                {
                    SetFontSize(graphics, IconChar);
                }

                // Measure string so that we can center the icon.
                SizeF stringSize = graphics.MeasureString(IconChar, iconFont, size);
                float w = stringSize.Width;
                float h = stringSize.Height;

                // center icon
                float left = (size - w) / 2;
                float top = (size - h) / 2;

                // Draw string to screen.
                graphics.DrawString(IconChar, iconFont, brush, new PointF(left, top));

            }

            return result;
        }

        #endregion

    }
}
