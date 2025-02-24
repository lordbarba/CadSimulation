// See https://aka.ms/new-console-template for more information
using CadSimulation;
using CommandLine;
using System.Runtime.CompilerServices;
using System.Text;

List<Shape> shapes = new List<Shape>();
string targetFilename = string.Empty;

Parser.Default.ParseArguments<CommandLineOptions>(args)
           .WithParsed(o =>
           {
               targetFilename = o.Path;
           });

while (true)
{
    Console.WriteLine(
"\nOptions:\n" +
"   's': insert a square\n" +
"   't': insert a triangle\n" +
"   'c': insert a circle\n" +
"   'r': insert a rectangle\n" +
"   'l': list all inserted shapes\n" +
"   'a': all shapes total area\n" +
"   'k': store data\n" +
"   'w': get stored data\n" +
"   'q': quit");

    var k = Console.ReadKey(true);
    if (k.KeyChar == 'q')
        break;

    Shape? shape = null;
    switch (k.KeyChar)
    {
        case 'l':
            {
                shapes.ForEach(s =>
                {
                    if(s!=null)
                        s.descr();
                });
            }
            continue;
        case 's':
            {
                Console.WriteLine("Square. Value for side:\t");
                var side = Int32.Parse(Console.ReadLine()!);
                shape = new Square(side); // Console.WriteLine("Square");
            }
            break;
        case 'r':
            {
                Console.WriteLine("Rectangle.\nValue for height:\t");
                var height = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for width:\t");
                var width = Int32.Parse(Console.ReadLine()!);
                shape = new Rectangle(height, width); // Console.WriteLine("Rectangle");
            }
            break;
        case 't':
            {
                Console.WriteLine("Triangle.\nValue for height:\t");
                var height = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for base:\t");
                var width = Int32.Parse(Console.ReadLine()!);
                shape = new Triangle(height, width); // Console.WriteLine("Triangle");
            }
            break;
        case 'c':
            Console.WriteLine("Circle. Value for radius:\t");
            var radius = Int32.Parse(Console.ReadLine()!);
            shape = new Circle(radius); // Console.WriteLine("Circle");
            break;
        case 'k':
            if (string.IsNullOrEmpty(targetFilename))
            {
                Console.WriteLine("Path is not set");
                continue;
            }
            Console.WriteLine($"Storing data to {targetFilename}:\t");
            executeStoreData();
            break;
        case 'w':
            if (!System.IO.File.Exists(targetFilename))
            {
                Console.WriteLine("File does not esist ");
                continue;
            }
            Console.WriteLine($"Retrieving data from {targetFilename}:\t");
            executoRetrieveData();
            break;
        case 'a':
            {
                double area = 0;
                foreach (var s in shapes)
                    area += s.area();

                Console.WriteLine("Total area: {0}", area);
            }
            continue;
    }
    if(shape!= null)    
        shapes.Add(shape);

}

void executoRetrieveData()
{
    string[] sLines=System.IO.File.ReadAllLines(targetFilename);
    shapes.Clear();
    foreach (var item in sLines)
    {
        if (string.IsNullOrEmpty(item)) continue;
        string[] sValues = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (sValues.Length == 0) continue;
        if (sValues[0] == "C")
        {
            shapes.Add(new Circle(Int32.Parse(sValues[1])));
        }
        else if (sValues[0] == "S")
        {
            shapes.Add(new Square(Int32.Parse(sValues[1])));
        }
        else if (sValues[0] == "R")
        {
            shapes.Add(new Rectangle(Int32.Parse(sValues[1]), Int32.Parse(sValues[2])));
        }
        else if (sValues[0] == "T")
        {
            shapes.Add(new Triangle(Int32.Parse(sValues[1]), Int32.Parse(sValues[2])));
        }
    }
}

void executeStoreData()
{
    StringBuilder sb = new StringBuilder();
    foreach (var s in shapes)
    {
        if (s == null) continue;
        if (s is Circle shapeCirle)
        {
            sb.Append("C").Append(" ").AppendLine(shapeCirle.Radius.ToString());
        }
        else if (s is Square shapeSquare)
        {
            sb.Append("S").Append(" ").AppendLine(shapeSquare.Side.ToString());
        }
        else if (s is Rectangle shapeRectangle)
        {
            sb.Append("R").Append(" ").Append(shapeRectangle.Height).Append(" ").AppendLine(shapeRectangle.Width.ToString());
        }
        else if (s is Triangle shapeTriangle)
        {
            sb.Append("T").Append(" ").Append(shapeTriangle.Base).Append(" ").AppendLine(shapeTriangle.Height.ToString());
        }
        sb.AppendLine();
    }

    System.IO.File.WriteAllText(targetFilename, sb.ToString());
}


namespace CadSimulation
{
    internal interface Shape
    {
        void descr();
        double area();
    }
    internal class Square : Shape
    {
        readonly int _side;
        public int Side { get => _side; }
        public Square(int side)
        {
            _side = side;
        }
        double Shape.area()
        {
            return _side * _side;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Square, side: {_side}");
        }
    }
    internal class Rectangle : Shape
    {
        readonly int _height;
        readonly int _width;

        public int Height { get => _height; }
        public int Width { get => _width; }
        public Rectangle(int height, int width)
        {
            _height = height;
            _width = width;
        }
        double Shape.area()
        {
            return _height * _width;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Rectangle, height: {_height}, width: {_width}");
        }
    }
    internal class Circle : Shape
    {
        int _radius;
        public int Radius { get => _radius;  }
        public Circle(int radius)
        {
            _radius = radius;
        }

        double Shape.area()
        {
            return _radius * _radius * 3.1416;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Circle, radius: {_radius}");
        }
    }
    internal class Triangle : Shape
    {
        int _base;
        int _height;
        public int Base { get => _base;  }
        public int Height { get => _height; }
        public Triangle(int b, int h)
        {
            _base = b;
            _height = h;
        }
        double Shape.area()
        {
            return _base * _height / 2;
        }
        void Shape.descr()
        {
            Console.WriteLine($"Triangle, base: {_base}, height: {_height}");
        }
    }
}


class CommandLineOptions
{
    [Option("path", Required = true, HelpText = "Write path to file")]
    public string Path { get; set; } = string.Empty;


}

