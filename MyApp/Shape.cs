using System;
using Tools;

/* 形状的参数
    形状的参数分为两类：
    位置参数(x,y,a)，决定了形状自然坐标和平面坐标的关系
    形态参数(varargs)，例如矩形有长宽，圆形有半径，决定了形状本身的参数
*/

namespace Shapes{
    public class Point {
        public double x = 0, y = 0;
        public Point(double x=0.0, double y=0.0){
            this.x = x;
            this.y = y;
        }
        public void setLoc(double x, double y){
            this.x = x;
            this.y = y;
        }
        // 讲道理应该再做一个getLoc的，但是不太懂如何设计属性的权限和返回值形式
    }
    public class Rectangle {
        protected double w = 0, h = 0;
        protected double x = 0, y = 0;
        protected double angle = 0;
        private double cosa = 1, sina = 0; 
        public Rectangle(double width, double height, double loc_x=0.0, double loc_y=0.0, double angle=0.0){
            this.setLoc(loc_x,loc_y);
            this.setSize(width,height);
            this.angle = angle;
        }
        public void setSize(double width, double height){
            System.Diagnostics.Debug.Assert(w>=0 && h>=0, "Size must be positive!");
            this.w = width;
            this.h = height;
        }
        public void setLoc(double loc_x, double loc_y){
            this.x = loc_x;
            this.y = loc_y;
        }
        public void setAngle(double angle){
            /*
            angle: recommend [0,360) deg
            */
            this.angle = angle;
            this.cosa = Math.Cos(this.angle/180*Math.PI);
            this.sina = Math.Sin(this.angle/180*Math.PI);
        }

        private Point getStandardCoordinate(Point p){
            // 将点按照this.angle旋转到正规方向
            // FIXME 这个函数有问题，只考虑了a没考虑x，y
            Point pt = new Point( p.x*this.cosa+p.y*this.sina, -p.x*this.sina+p.y*this.cosa );
            return pt;
        }
        public double sdf(Point p){
            // 考虑angle != 0 时，需要转换到正规方向
            Point loc = new Point(this.x, this.y);
            loc = this.getStandardCoordinate(loc);
            p = this.getStandardCoordinate(p);

            /* 首先判断点与矩形的位置关系
            分为9块区域，小键盘是个好东西
            内部的点，直接求个最小值，计算结果加负号就行
            */
            double sdf_ = 0;

            int xflag = 0;
            int yflag = 0;
            if (p.x < loc.x) xflag = 1;
            else if (p.x > loc.x+this.w) xflag = 3;
            else xflag = 2;
            if (p.y < loc.y) yflag = 0;
            else if (p.y > loc.y+this.h) yflag = 6;
            else yflag = 3;
            int flag = xflag+yflag;

            if (flag==5) {
                double[] tmp = {p.x-loc.x, loc.x+this.w-p.x, p.y-loc.y, loc.y+this.h-p.y};
                Array.Sort(tmp);
                sdf_ = - tmp[0];
            }
            else {
                switch (flag) {
                    case 1: sdf_ = MathTools.getDistance(p,loc); break;
                    case 2: sdf_ = loc.y - p.y; break;
                    case 3: sdf_ = MathTools.getDistance(p.x,p.y,loc.x+this.w,loc.y); break;
                    case 4: sdf_ = loc.x - p.x; break;
                    case 6: sdf_ = p.x - (loc.x+this.w); break;
                    case 7: sdf_ = MathTools.getDistance(p.x,p.y,loc.x,loc.y+this.h); break;
                    case 8: sdf_ = p.y - (loc.y+this.h); break;
                    case 9: sdf_ = MathTools.getDistance(p.x,p.y,loc.x+this.w,loc.y+this.h); break; 
                    default: throw(new System.NullReferenceException("Impossible point!"));
                }
            }
            return sdf_;
        }

        public double sdf(double px, double py){
            Point p = new Point(px, py);
            return sdf(p);
        }

    }

    public class Circle {
        public double r = 0;
        public double x = 0, y = 0;

        public Circle(double radius=1, double loc_x=0, double loc_y=0){
            this.setR(radius);
            this.setLoc(loc_x,loc_y);
        }

        public void setR(double radius){
            System.Diagnostics.Debug.Assert(radius>=0, "Invalid radius!");
            this.r = radius;
        }

        public void setLoc(double loc_x, double loc_y){
            this.x = loc_x;
            this.y = loc_y;
        }

        public double sdf(Point p){
            return MathTools.getDistance(this.x, this.y, p.x, p.y) - this.r;
        }

    }
}