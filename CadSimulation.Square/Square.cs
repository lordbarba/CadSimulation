using CadSimulation.Interfaces;

namespace CadSimulation.square
{
    public class Square : IShape
    {
        private int _side;
        public int Side { get => _side; set => _side = value; }
        public static string Name { get => "Square"; }
        public  static char Code { get => 's'; }
        public Square() { }
        public Square(int side)
        {
            _side = side;
        }
        double IShape.area()
        {
            return _side * _side;
        }

        string IShape.descr()
        {
            return ($"Square, side: {_side}");
        }
    }
}
