using GeoUtils;

/* 形状的参数
    形状的参数分为两类：
    锚参数anchor(x,y,a)，决定了形状自然坐标和原始平面坐标的关系
    形态参数size(args)，决定了形状本身的参数，例如矩形有长宽，圆形有半径
*/

namespace Shapes
{
    public class Rectangle
    {
        // properties

        // form parameters
        private double _w=0.0, _h=0.0;
        public double w
        {
            get
            {
                return _w;
            }
            set
            {
                if (value < 0) throw (new InvalidParameterException("Width must be positive!"));
                _w = value;
            }
        }
        public double h
        {
            get
            {
                return _h;    
            }
            set
            {
                if (value < 0) throw (new InvalidParameterException("Height must be positive!"));
                _h = value;
            }
        }
        // pose parameters
        public Pose pose = new Pose();

        // construction method: form and position
        public Rectangle(double width, double height, Pose position)
        {
            // Form参数+Pose参数组成形状的几何描述
            this.setForm(width, height);
            this.pose = position;
        }
        public Rectangle(double width, double height, double loc_x = 0.0, double loc_y = 0.0, double angle = 0.0)
        {
            // 允许使用基本参数定义形状
            this.setForm(width, height);
            this.pose = new Pose(loc_x, loc_y, angle);
        }
        public void setForm(double width, double height)
        {
            // 允许同时定义所有形态参数
            this.w = width;
            this.h = height;
        }
        public void setPose(Point loc, double angle)
        {
            // 直接把Pose的set拿过来
            this.pose.setPose(loc, angle);
        }
        public void setPose(double loc_x=0.0, double loc_y=0.0, double angle=0.0)
        {
            // 直接把Pose的set拿过来
            this.pose.setPose(loc_x, loc_y, angle);
        }

        public double sdf(Point p)
        {
            // 计算本形状的SDF在指定点的值

            // 首先转换到相对坐标系（loc = (0,0,0)）
            Point loc = new Point(0.0, 0.0);
            p = this.pose.toRelativeCoordinate(p);

            /* 判断点与矩形的位置关系
            分为9块区域，小键盘是个好东西
            内部的点，直接求个最小值，计算结果加负号就行
            */
            double sdf_ = 0;

            int xflag = 0;
            int yflag = 0;
            if (p.x < 0) xflag = 1;
            else if (p.x > this.w) xflag = 3;
            else xflag = 2;
            if (p.y < 0) yflag = 0;
            else if (p.y > this.h) yflag = 6;
            else yflag = 3;
            int flag = xflag + yflag;

            if (flag == 5)
            {
                double[] tmp = { p.x, this.w - p.x, p.y, this.h - p.y };
                Array.Sort(tmp); // min
                sdf_ = -tmp[0];
            }
            else
            {
                switch (flag)
                {
                    case 1: sdf_ = MathTools.getDistance(p, loc); break;
                    case 2: sdf_ = 0 - p.y; break;
                    case 3: sdf_ = MathTools.getDistance(p.x, p.y, this.w, 0); break;
                    case 4: sdf_ = 0 - p.x; break;
                    case 6: sdf_ = p.x - (this.w); break;
                    case 7: sdf_ = MathTools.getDistance(p.x, p.y, 0, this.h); break;
                    case 8: sdf_ = p.y - (this.h); break;
                    case 9: sdf_ = MathTools.getDistance(p.x, p.y, this.w, this.h); break;
                    default: throw (new InvalidParameterException("Impossible point!"));
                }
            }
            return sdf_;
        }

        public double sdf(double px, double py)
        {
            Point p = new Point(px, py);
            return sdf(p);
        }

    }

    public class Circle
    {
        private double _r=0.0;
        public double r
        {
            get
            {
                return _r;
            }
            set
            {
                if (value<0) throw (new InvalidParameterException("Radius must be positave!"));
                _r = value;
            }
        }
        public Pose pose = new Pose();

        public Circle(double radius = 1, double loc_x = 0, double loc_y = 0)
        {
            this.setForm(radius);
            this.setPose(loc_x,loc_y);
        }

        public void setForm(double radius)
        {
            this.r = radius;
        }

        public void setPose(double loc_x, double loc_y, double angle=0.0)
        {
            // 圆形没有angle来着。。
            this.pose.loc = new Point(loc_x,loc_y);
        }

        public double sdf(Point p)
        {
            return MathTools.getDistance(this.pose.loc, p) - this.r;
        }

    }
}