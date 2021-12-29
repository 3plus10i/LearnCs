using System;
namespace GeoUtils
{
    public class Point
    {
        // “点”是万物之母
        public double x = 0;
        public double y = 0;
        public Point(double x = 0.0, double y = 0.0)
        {
            this.setPose(x, y);
        }
        public void setPose(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // 重载加减法为向量加减法
        public static Point operator+ (Point p1, Point p2)
        {
            return new Point(p1.x+p1.x, p1.y+p2.y);
        }
        public static Point operator- (Point p1, Point p2)
        {
            return new Point(p1.x-p1.x, p1.y-p2.y);
        }
    }

    public class MathTools
    {
        public static double getDistance(double x1, double y1, double x2, double y2)
        {
            // 计算两个点之间的距离
            // 别看形参列表繁琐，实则最灵活多用
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }
        public static double getDistance(Point p1, Point p2)
        {
            // 计算两个点之间的距离
            return getDistance(p1.x, p1.y, p2.x, p2.y);
        }
    }


    public class Pose
    {
        // 位置（和方向/角度）的参数(x,y,a)
        // Pose = Location + Orientation
        // pose = loc + angle

        // properties
        public Point loc = new Point();
        private double _angle = 0.0, _cosa = 1, _sina = 0;
        public double angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                _cosa = Math.Cos(value / 180 * Math.PI);
                _sina = Math.Sin(value / 180 * Math.PI);
            }
        }

        // methods
        public Pose(Point loc, double angle = 0.0)
        {
            this.setPose(loc,angle);
        }
        public Pose(double loc_x=0.0, double loc_y=0.0, double angle=0.0)
        {
            this.setPose(loc_x,loc_y,angle);
        }
        public void setPose(Point loc, double angle = 0.0)
        {
            this.loc = loc;
            this.angle = angle;
        }
        public void setPose(double loc_x=0.0, double loc_y=0.0, double angle=0.0)
        {
            // 允许通过基本数据定义
            this.loc = new Point(loc_x, loc_y);
            this.angle = angle;
        }
        public Point toRelativeCoordinate(Point p)
        {
            // 将原始坐标转换为相对此位置的相对坐标
            p = p - this.loc;
            p.setPose(p.x * this._cosa + p.y * this._sina, -p.x * this._sina + p.y * this._cosa);
            return p;
        }

        public Point toOriginalCoordinate(Point p)
        {
            // 将相对坐标转换到原始坐标
            p.setPose(p.x * this._cosa - p.y * this._sina, p.x * this._sina + p.y * this._cosa);
            p = p + this.loc;
            return p;
        }
    }

    // 异常之类的东西放在这里其实不太合适
    //  只是玩玩而已
    public class InvalidParameterException : ApplicationException
    {
        public InvalidParameterException(string message) : base(message)
        {
        }
    }

}