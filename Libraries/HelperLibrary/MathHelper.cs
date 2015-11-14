using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace HelperLibrary
{
    public static class MathHelper
    {

        public static float ComputeDistance(Point pointA, Point pointB)
        {
            if (pointA == null || pointB == null)
                return -1;

            double xDistance = pointA.X - pointB.X;
            double yDistance = pointA.Y - pointB.Y;

            return (float)Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
        }

    }
}
