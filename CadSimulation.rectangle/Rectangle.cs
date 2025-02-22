using CadSimulation.Interfaces;

namespace CadSimulation.rectangle
{
    public class Rectangle : IShape
    {
        private  int _height;
        private int _width;

        public int Height { get => _height; set => _height = value; }
        public int Width { get => _width; set => _width = value; }
        public static string Name { get => "Rectangle"; }
        public static char Code { get => 'r'; }
        public Rectangle() { }
        public Rectangle(int height, int weidth)
        {
            _height = height;
            _width = weidth;
        }
        double IShape.area()
        {
            return _height * _width;
        }

        void IShape.descr()
        {
            Console.WriteLine($"Rectangle, height: {_height}, weidth: {_width}");
        }
    }
}
