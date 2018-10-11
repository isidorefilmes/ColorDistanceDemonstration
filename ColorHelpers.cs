using System.Drawing;

/* Finding nearest colors using Euclidean distance
 * http://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

namespace Cyotek.ColorDistanceDemonstration
{
  internal static class ColorHelpers
  {
    #region Static Methods

    public static int FindNearestColor(Color[] map, Color current)
    {
      int shortestDistance;
      int index;

      index = -1;
      shortestDistance = int.MaxValue;

      for (int i = 0; i < map.Length; i++)
      {
        Color match;
        int distance;

        match = map[i];
        distance = GetDistance(current, match);

        if (distance < shortestDistance)
        {
          index = i;
          shortestDistance = distance;
        }
      }

      return index;
    }

    public static int GetDistance(Color current, Color match)
    {
      int redDifference;
      int greenDifference;
      int blueDifference;
      int alphaDifference;

      alphaDifference = current.A - match.A;
      redDifference = current.R - match.R;
      greenDifference = current.G - match.G;
      blueDifference = current.B - match.B;

      return alphaDifference * alphaDifference + redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference;
    }

    #endregion
  }
}
