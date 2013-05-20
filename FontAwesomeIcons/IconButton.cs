using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FontAwesomeIcons
{
    /// <summary>
    /// Win Forms icon button. Clickable, with hover colour & tooltip.
    /// </summary>
    public class IconButton : PictureBox
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon" /> class using default values - star icon, normal color = gray, hover color = black.
        /// </summary>
        public IconButton()
            : this(IconType.Star, 16, Color.DimGray, Color.Black)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Icon" /> class.
        /// </summary>
        /// <param name="type">The icon type.</param>
        /// <param name="size">The size of the icon (number of pixels - 24 creates an icon 24x24px).</param>
        /// <param name="normalColor">Color to use when not hovered over.</param>
        /// <param name="hoverColor">Color to use when hovered over.</param>
        /// <param name="selectable">NOT YET IMPLEMENTED. If set to <c>true</c> the icon will be selectable using the keyboard (tab-key).</param>
        /// <param name="toolTip">The tool tip text to use. Leave as null to not use a tooltip.</param>
        public IconButton(IconType type, int size, Color normalColor, Color hoverColor, bool selectable = false, string toolTip = null)
        {
            IconFont = null;
            BackColor = Color.Transparent;

            // need more than this to make picturebox selectable
            if (selectable)
            {
                SetStyle(ControlStyles.Selectable, true);
                TabStop = true;
            }

            Width = size;
            Height = size;

            IconType = type;

            InActiveColor = normalColor;
            ActiveColor = hoverColor;

            ToolTipText = toolTip;

            MouseEnter += Icon_MouseEnter;
            MouseLeave += Icon_MouseLeave;
        }

        #endregion

        #region Public

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the icon to draw.
        /// </summary>
        public IconType IconType
        {
            get { return _iconType; }
            set
            {
                _iconType = value;
                // see http://fortawesome.github.io/Font-Awesome/cheatsheet/ for a list of hex values
                IconChar = char.ConvertFromUtf32((int)value);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text.
        /// </summary>
        /// <value>
        /// The tool tip text.
        /// </value>
        public string ToolTipText
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                if (value != null)
                {
                    var buttonTt = new ToolTip();
                    //buttonTt.ToolTipIcon = ToolTipIcon.Info;
                    buttonTt.IsBalloon = true;
                    buttonTt.ShowAlways = true;
                    buttonTt.SetToolTip(this, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color when active (hovered over)
        /// </summary>
        public Color ActiveColor
        {
            get { return _activeColor; }
            set
            {
                Debug.Print("Setting active brush color to " + value.ToString());
                _activeColor = value;
                ActiveBrush = new SolidBrush(value);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color when inactive (not hovered over)
        /// </summary>
        public Color InActiveColor
        {
            get { return _inActiveColor; }
            set
            {
                _inActiveColor = value;
                InActiveBrush = new SolidBrush(value);
                IconBrush = InActiveBrush;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <returns>The width of the control in pixels.</returns>
        public new int Width
        {
            set
            {
                // force the font size to be recalculated & redrawn
                base.Width = value;
                IconFont = null;
                Invalidate();
            }
            get { return base.Width; }
        }

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        /// <returns>The height of the control in pixels.</returns>
        public new int Height
        {
            set
            {
                // force the font size to be recalculated & redrawn
                base.Height = value;
                IconFont = null;
                Invalidate();
            }
            get { return base.Height; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the icon character using a unicode value.
        /// It is recommended the character be set via the IconType property.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public void SetIconChar(char newValue)
        {
            IconChar = newValue.ToString();
            Invalidate();
        }

        #endregion

        #endregion

        #region Private

        #region Properties & Attributes
        private IconType _iconType = IconType.Star;
        private string _tooltip = null;
        private Color _activeColor = Color.Black;
        private Color _inActiveColor = Color.Black;

        private string IconChar { get; set; }
        private Font IconFont { get; set; }
        private Brush IconBrush { get; set; } // brush currently in use
        private Brush ActiveBrush { get; set; } // brush to use when hovered over
        private Brush InActiveBrush { get; set; } // brush to use when not hovered over
        #endregion

        #region Event Handlers
        protected void Icon_MouseLeave(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            IconBrush = InActiveBrush;
            Invalidate();
        }
        protected void Icon_MouseEnter(object sender, EventArgs e)
        {
            // change the brush and force a redraw
            IconBrush = ActiveBrush;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            // Set best quality
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            // is the font ready to go?
            if (IconFont == null)
            {
                SetFontSize(graphics);
            }

            // Measure string so that we can center the icon.
            SizeF stringSize = graphics.MeasureString(IconChar, IconFont, Width);
            float w = stringSize.Width;
            float h = stringSize.Height;

            // center icon
            float left = (Width - w) / 2;
            float top = (Height - h) / 2;

            // Draw string to screen.
            graphics.DrawString(IconChar, IconFont, IconBrush, new PointF(left, top));

            base.OnPaint(e);

            if (Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(e.Graphics, rc);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets a new font with approprate size for the allocated space.
        /// </summary>
        /// <param name="g">The g.</param>
        private void SetFontSize(Graphics g)
        {
            IconFont = GetAdjustedFont(g, IconChar, Width, Height, 4, true);
        }

        /// <summary>
        /// Returns a font instance using the resource icon font.
        /// </summary>
        /// <param name="size">The size of the font in points.</param>
        /// <returns>A new System.Drawing.Font instance</returns>
        private Font GetIconFont(float size)
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
        private Font GetAdjustedFont(Graphics g, string graphicString, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
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

        #endregion

        #endregion

        #region Static

        /// <summary>
        /// Initializes the <see cref="Icon" /> class by loading the font from resources upon first use.
        /// </summary>
        static IconButton()
        {
            InitialiseFont();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
           IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        /// <summary>
        /// Store the icon font in a static variable to reuse between icons
        /// </summary>
        private static readonly PrivateFontCollection Fonts = new PrivateFontCollection();

        /// <summary>
        /// Loads the icon font from the resources.
        /// </summary>
        private static void InitialiseFont()
        {
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
            catch (Exception ex)
            {
                // log?
            }
        }

        #endregion

    }
}
