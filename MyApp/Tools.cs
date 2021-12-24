using System;
using Shapes;
namespace Tools{ 
    public class MathTools{
        public static double getDistance(double x1, double y1, double x2, double y2){
            // 别看形参列表繁琐，实则最灵活多用
            return Math.Sqrt( (x1-x2)*(x1-x2) + (y1-y2)*(y1-y2) );
        }
        public static double getDistance(Point p1, Point p2){
            return getDistance(p1.x, p1.y, p2.x, p2.y);
        }
    }
}