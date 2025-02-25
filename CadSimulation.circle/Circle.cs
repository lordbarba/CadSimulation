using CadSimulation.Interfaces;

namespace CadSimulation.circle
{
    public class Circle : IShape
    {
        int _radius;
        public int Radius { get => _radius; set => _radius = value; }
        public static string Name { get => "Circle"; }
        public static char Code { get => 'c'; }

        public Circle() { }
        public Circle(int radius)
        {
            _radius = radius;
        }

        double IShape.area()
        {
            return _radius * _radius * 3.1416;
        }

        string IShape.descr()
        {
            return ($"Circle, radius: {_radius}");
        }
    }
}
