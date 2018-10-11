using System;
using System.Runtime.InteropServices;

/* Finding nearest colors using Euclidean distance
 * http://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

// ReSharper disable InconsistentNaming

namespace Cyotek.ColorDistanceDemonstration
{
  internal static class NativeMethods
  {
    #region Externals

    [DllImport("user32.dll")]
    public static extern bool DrawFocusRect(IntPtr hDC, [In] ref RECT lprc);

    #endregion

    #region Nested type: RECT

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int left;

      public int top;

      public int right;

      public int bottom;

      public RECT(int left, int top, int right, int bottom)
      {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
      }
    }

    #endregion
  }
}
