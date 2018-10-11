using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

/* Finding nearest colors using Euclidean distance
 * http://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

namespace Cyotek.ColorDistanceDemonstration
{
  [DefaultProperty(nameof(Color))]
  [DefaultEvent(nameof(ColorChanged))]
  internal class ColorBox : Control, IButtonControl
  {
    #region Constants

    private static readonly object _eventColorChanged = new object();

    #endregion

    #region Fields

    private Color _color;

    private DialogResult _dialogResult;

    #endregion

    #region Constructors

    public ColorBox()
    {
      this.DoubleBuffered = true;

      _color = Color.White;
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the Color property value changes
    /// </summary>
    [Category("Property Changed")]
    public event EventHandler ColorChanged
    {
      add { this.Events.AddHandler(_eventColorChanged, value); }
      remove { this.Events.RemoveHandler(_eventColorChanged, value); }
    }

    #endregion

    #region Properties

    [Category("Appearance")]
    [DefaultValue(typeof(Color), "White")]
    public Color Color
    {
      get { return _color; }
      set
      {
        if (_color != value)
        {
          _color = value;

          this.OnColorChanged(EventArgs.Empty);
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Font Font
    {
      get { return base.Font; }
      set { base.Font = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
      get { return base.Text; }
      set { base.Text = value; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Raises the <see cref="ColorChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected virtual void OnColorChanged(EventArgs e)
    {
      EventHandler handler;

      this.Invalidate();

      handler = (EventHandler)this.Events[_eventColorChanged];

      handler?.Invoke(this, e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
      base.OnGotFocus(e);

      this.Invalidate();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space && e.Modifiers == Keys.None)
      {
        e.Handled = true;
        this.PerformClick();
      }

      base.OnKeyUp(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
      base.OnLostFocus(e);

      this.Invalidate();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
      base.OnMouseClick(e);

      if (e.Button == MouseButtons.Left)
      {
        this.PerformClick();
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      Graphics g;
      Rectangle bounds;

      base.OnPaint(e);

      bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

      g = e.Graphics;

      if (_color.A != 255)
      {
        using (Brush brush = new HatchBrush(HatchStyle.DiagonalCross, Color.Silver, Color.White))
        {
          g.FillRectangle(brush, bounds);
        }
      }

      using (Brush brush = new SolidBrush(_color))
      {
        g.FillRectangle(brush, bounds);
      }

      g.DrawRectangle(SystemPens.WindowFrame, bounds);

      if (this.ShowFocusCues && this.Focused)
      {
        NativeMethods.RECT rect;

        rect = new NativeMethods.RECT(2, 2, bounds.Width - 1, bounds.Height - 1);

        // The Win32 API DrawFocusRect draws using an inverted brush and so works on black,
        // whereas ControlPaint.DrawFocusRect decidedly does not
        NativeMethods.DrawFocusRect(g.GetHdc(), ref rect);
        g.ReleaseHdc();
      }
    }

    #endregion

    #region IButtonControl Interface

    public void NotifyDefault(bool value)
    {
      this.Invalidate();
    }

    public void PerformClick()
    {
      using (ColorDialog dialog = new ColorDialog
                                  {
                                    Color = _color,
                                    AllowFullOpen = true,
                                    AnyColor = true,
                                    FullOpen = true
                                  })
      {
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
          this.Color = dialog.Color;
        }
      }
    }

    DialogResult IButtonControl.DialogResult
    {
      get { return _dialogResult; }
      set { _dialogResult = value; }
    }

    #endregion
  }
}
