using System;
using Shapes;
using GeoUtils;


public class TestScripts
{
    // 这里写要运行的测试代码
    public static void test1()
    {
        Point p1 = new Point(3,4);
        Point O = new Point(0,0);
        double d1 = MathTools.getDistance(p1, O);
        Console.WriteLine(d1.ToString());

        Rectangle rect1 = new Rectangle(5,12);
        rect1.pose.angle = 30;
        double sdf_p1 = rect1.sdf(p1);
        Console.WriteLine(sdf_p1.ToString());

        Circle c1 = new Circle(1,2,2);
        Console.WriteLine(c1.sdf(p1).ToString());
    }
    public static void test2()
    {
        // 这里写要运行的测试代码
        double a = new double();
        a = 1;
        Console.WriteLine(a.ToString());
    }

}
public class Program
{


    public static void Main(String[] args)
    {
        TestScripts.test1();
        // TestScripts.test2();


    }
}