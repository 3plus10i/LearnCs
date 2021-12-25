using System;
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
        public double w
        {
            get
            {
                return w;
            }
            set
            {
                if (value < 0) throw (new InvalidParameterException("Width must be positive!"));
                w = value;
            }
        }
        public double h
        {
            get
            {
                return h;    
            }
            set
            {
                if (value < 0) throw (new InvalidParameterException("Height must be positive!"));
                h = value;
            }
        }
        public Anchor anchor = new Anchor();

        public Rectangle(double width, double height, Anchor anchor)
        {
            this.w = width;
            this.h = height;
            this.anchor = anchor;
        }
        public Rectangle(double width, double height, double loc_x = 0.0, double loc_y = 0.0, double angle = 0.0)
        {
            // 允许使用基本参数定义形状
            this.w = width;
            this.h = height;
            this.anchor = new Anchor(loc_x, loc_y, angle);
        }
        public void setSize(double width, double height)
        {
            // 允许同时定义所有形态参数
            this.w = width;
            this.h = height;
        }

        public double sdf(Point p)
        {
            // 计算本形状的SDF在指定点的值

            // 首先转换到相对坐标系（loc = (0,0,0)）
            Point loc = new Point(0.0, 0.0);
            p = this.anchor.toRelativeCoordinate(p);

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
        public double r
        {
            get
            {
                return r;
            }
            set
            {
                if (value<0) throw (new InvalidParameterException("Radius must be positave!"));
                r = value;
            }
        }
        public Anchor anchor = new Anchor();

        public Circle(double radius = 1, double loc_x = 0, double loc_y = 0)
        {
            this.setR(radius);
            this.setLoc(loc_x,loc_y);
        }

        public void setR(double radius)
        {
            // 这种函数真的有存在的必要吗
            this.r = radius;
        }

        public void setLoc(double loc_x, double loc_y)
        {
            this.anchor.loc = new Point(loc_x,loc_y);
        }

        public double sdf(Point p)
        {
            return MathTools.getDistance(this.anchor.loc, p) - this.r;
        }

    }
}