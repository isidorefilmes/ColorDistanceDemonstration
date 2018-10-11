using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

/* Finding nearest colors using Euclidean distance
 * http://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

namespace Cyotek.ColorDistanceDemonstration
{
  public partial class MainForm : Form
  {
    #region Fields

    private int _distance;

    private int _lhsNearest;

    private Color[] _palette;

    private int _rhsNearest;

    #endregion

    #region Constructors

    public MainForm()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    protected override void OnLoad(EventArgs e)
    {
      this.DoubleBuffered = true;

      base.OnLoad(e);

      this.Font = SystemFonts.MessageBoxFont;

      _palette = new[]
                 {
                   Color.FromArgb(0, 0, 0),
                   Color.FromArgb(128, 0, 0),
                   Color.FromArgb(0, 128, 0),
                   Color.FromArgb(128, 128, 0),
                   Color.FromArgb(0, 0, 128),
                   Color.FromArgb(128, 0, 128),
                   Color.FromArgb(0, 128, 128),
                   Color.FromArgb(192, 192, 192),
                   Color.FromArgb(128, 128, 128),
                   Color.FromArgb(255, 0, 0),
                   Color.FromArgb(0, 255, 0),
                   Color.FromArgb(255, 255, 0),
                   Color.FromArgb(0, 0, 255),
                   Color.FromArgb(255, 0, 255),
                   Color.FromArgb(0, 255, 255),
                   Color.FromArgb(255, 255, 255)
                 };

      this.UpdatePalette();

      lhsColorBox.Color = Color.SeaGreen;
      rhsColorBox.Color = Color.Firebrick;

      //lhsColorBox.Color = Color.FromArgb(0, 0, 220, 0);
      //rhsColorBox.Color = Color.FromArgb(255, 0, 220, 0);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Graphics g;

      base.OnPaint(e);

      g = e.Graphics;

      g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      this.PaintDistanceLabel(g);
      this.PaintNearestColor(g, lhsColorBox.Bounds, _lhsNearest, lhsColorBox.Color, -4);
      this.PaintNearestColor(g, rhsColorBox.Bounds, _rhsNearest, rhsColorBox.Color, 4);
    }

    private void CalculateNearestColors()
    {
      _lhsNearest = ColorHelpers.FindNearestColor(_palette, lhsColorBox.Color);
      _rhsNearest = ColorHelpers.FindNearestColor(_palette, rhsColorBox.Color);

      this.Invalidate();
    }

    private ColorBox GetColorBox(int index)
    {
      ColorBox result;
      Control.ControlCollection controls;

      result = null;
      controls = this.Controls;

      for (int i = 0; i < controls.Count; i++)
      {
        ColorBox box;

        box = controls[i] as ColorBox;

        if (box?.Tag != null)
        {
          if (Convert.ToInt32(box.Tag) == index)
          {
            result = box;
            break;
          }
        }
      }

      return result;
    }

    private void lhsColorBox_ColorChanged(object sender, EventArgs e)
    {
      _distance = ColorHelpers.GetDistance(lhsColorBox.Color, rhsColorBox.Color);

      this.CalculateNearestColors();
    }

    private void PaintDistanceLabel(Graphics g)
    {
      int offset;
      Rectangle textBounds;

      offset = 16;
      textBounds = new Rectangle(lhsColorBox.Right + offset, lhsColorBox.Top + offset, rhsColorBox.Left - (lhsColorBox.Right + offset * 2), lhsColorBox.Height - offset * 2);

      using (Font font = new Font(this.Font.FontFamily, 24))
      {
        TextRenderer.DrawText(g, "→ " + _distance.ToString() + " →", font, textBounds, this.ForeColor, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
      }
    }

    private void PaintNearestColor(Graphics g, Rectangle bounds, int index, Color lineColor, int offset)
    {
      Rectangle destBounds;
      int x1;
      int y1;
      int x2;
      int y2;
      int my;

      if (lineColor.A != 255)
      {
        lineColor = Color.FromArgb(255, lineColor);
      }

      destBounds = this.GetColorBox(index).Bounds;

      x1 = bounds.Left + (bounds.Width >> 1);
      y1 = bounds.Bottom;
      x2 = destBounds.Left + (destBounds.Width >> 1);
      y2 = destBounds.Top;
      my = y1 + (y2 - y1) / 2 + offset;

      if (_lhsNearest == _rhsNearest)
      {
        x2 += offset;
      }

      using (Pen pen = new Pen(lineColor, 2))
      {
        g.DrawLine(pen, x1, y1, x1, my);
        g.DrawLine(pen, x2, y2, x2, my);
        g.DrawLine(pen, x1, my, x2, my);
      }
    }

    private void paletteColorBox0_ColorChanged(object sender, EventArgs e)
    {
      ColorBox box;
      int index;

      box = (ColorBox)sender;
      index = Convert.ToInt32(box.Tag);

      _palette[index] = box.Color;

      this.CalculateNearestColors();
    }

    private void UpdatePalette()
    {
      Control.ControlCollection controls;

      controls = this.Controls;

      for (int i = 0; i < controls.Count; i++)
      {
        ColorBox box;

        box = controls[i] as ColorBox;

        if (box?.Tag != null)
        {
          int index;

          index = Convert.ToInt32(box.Tag);

          box.Color = _palette[index];
        }
      }
    }

    #endregion
  }
}
