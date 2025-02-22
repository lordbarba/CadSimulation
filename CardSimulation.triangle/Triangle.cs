using CadSimulation.Interfaces;

namespace CardSimulation.triangle
{
    public class Triangle : IShape
    {
        int _base;
        int _height;
        public int Base { get => _base; set => _base = value; }
        public int Height { get => _height; set => _height = value; }
        public static string Name { get => "Triangle"; }
        public static char Code { get => 't'; }

        public Triangle() { }
        public Triangle(int b, int h)
        {
            _base = b;
            _height = h;
        }
        double IShape.area()
        {
            return _base * _height / 2;
        }
        void IShape.descr()
        {
            Console.WriteLine($"Triangle, base: {_base}, height: {_height}");
        }
    }
}
