﻿// See https://aka.ms/new-console-template for more information
using CadSimulation;
using CommandLine;
using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<Shape> shapes = new List<Shape>();
string sTargetFilename = string.Empty;
bool bUseJsonFormat = false;
string sRemoteUri = string.Empty;
HttpClient client = new HttpClient();
string sData;

Parser.Default.ParseArguments<CommandLineOptions>(args)
           .WithParsed(o =>
           {
               sTargetFilename = o.Path; 
               bUseJsonFormat= o.Json;
               sRemoteUri = o.Uri;
           });

Console.WriteLine("Filename selected: {0}", sTargetFilename);
Console.WriteLine("Export as JSON: {0}", bUseJsonFormat);
while (true)
{
    Console.WriteLine(
"\nOptions:\n" +
"   's': insert a square\n" +
"   't': insert a triangle\n" +
"   'c': insert a circle\n" +
"   'r': insert a rectangle\n" +
"   'l': list all inserted shapes\n" +
"   'a': all shapres total area\n" +
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
                Console.WriteLine("Rectangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for weidth:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Rectangle(hight, weidth); // Console.WriteLine("Rectangle");
            }
            break;
        case 't':
            {
                Console.WriteLine("Triangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for base:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Triangle(hight, weidth); // Console.WriteLine("Triangle");
            }
            break;
        case 'c':
            Console.WriteLine("Circle. Value for radius:\t");
            var radius = Int32.Parse(Console.ReadLine()!);
            shape = new Circle(radius); // Console.WriteLine("Circle");
            break;
        case 'k':
            if(bUseJsonFormat)
                sData = getStoreDataAsJson();
            else
                sData = executeStoreData();

            if (!string.IsNullOrEmpty(sTargetFilename))
            {
                Console.WriteLine($"Storing data to {sTargetFilename}");
                System.IO.File.WriteAllText(sTargetFilename, sData);
            }
            else if (!string.IsNullOrEmpty(sRemoteUri))
            {
                Console.WriteLine($"Storing data to remote {sRemoteUri}");
                try
                {
                    StringContent content = new StringContent(sData, Encoding.UTF8);
                    HttpResponseMessage response = client.PostAsync(sRemoteUri, content).Result; // Bloccante
                    response.EnsureSuccessStatusCode();
                    string sReply = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("POST Response: " + sReply);
                }
                catch (Exception e)
                {
                    Console.WriteLine("POST Error: " + e.Message);
                    break;
                }
            }
            break;
        case 'w':

            string stringContent = string.Empty;
            if (!string.IsNullOrEmpty(sTargetFilename))
            {
                Console.WriteLine($"Retrieving  data from {sTargetFilename}");
                stringContent = System.IO.File.ReadAllText(sTargetFilename);
            }
            else if (!string.IsNullOrEmpty(sRemoteUri))
            {
                Console.WriteLine($"Retrieving  data from remote {sRemoteUri}");

                try
                {
                    HttpResponseMessage response = client.GetAsync(sRemoteUri).Result; // Bloccante
                    response.EnsureSuccessStatusCode();
                    stringContent = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("GET Response: " + stringContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine("GET Error: " + e.Message);
                }

            }
            if (bUseJsonFormat)
                executoRetrieveDataFromJson(stringContent);
            else
                executoRetrieveData(stringContent);
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
    if(shape != null)
        shapes.Add(shape);

}

#region JSON FORMAT
void executoRetrieveDataFromJson(string sJson)
{
    var shapesList = JsonSerializer.Deserialize<List<Shape>>(sJson, 
        new JsonSerializerOptions
            {
                Converters = { new ShapeDataConverter() }
            }
        );
    shapes.Clear();
    shapes.AddRange(shapesList);
}

string getStoreDataAsJson()
{
    var json = JsonSerializer.Serialize(shapes,
        new JsonSerializerOptions
        {
            WriteIndented = true ,
            Converters = { new ShapeDataConverter() }
        }
     );
    return json;
}
#endregion

#region CUSTOM FORMAT
void executoRetrieveData(string sContent)
{
    string[] sLines=sContent.Split(Environment.NewLine);
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

string executeStoreData()
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

    return sb.ToString();
}
#endregion

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
        public Rectangle(int height, int weidth)
        {
            _height = height;
            _width = weidth;
        }
        double Shape.area()
        {
            return _height * _width;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Rectangle, height: {_height}, weidth: {_width}");
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
    [Option("path", Required = false, HelpText = "Write path to file")]
    public string Path { get; set; }
    [Option("json", Required = false, Default =false, HelpText = "Export in JSON format")]
    public bool Json { get; set; }

    [Option("uri", Required = false, HelpText = "Remote repository url")]
    public string Uri { get; set; }

}


class ShapeDataConverter : System.Text.Json.Serialization.JsonConverter<Shape>
{
    public override Shape Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = jsonDoc.RootElement;
            var type = jsonObject.GetProperty("Type").GetString();
            switch(type)
            {
                case nameof(Circle):
                    return new Circle(jsonObject.GetProperty(nameof(Circle.Radius)).GetInt32());
                case nameof(Square):
                    {
                      return   new Square(jsonObject.GetProperty(nameof(Square.Side)).GetInt32());
                }
                case nameof(Rectangle):
                    return new Rectangle(
                            jsonObject.GetProperty(nameof(Rectangle.Width)).GetInt32(),
                            jsonObject.GetProperty(nameof(Rectangle.Height)).GetInt32()
                        );
                case nameof(Triangle):
                    return new Triangle(
                            jsonObject.GetProperty(nameof(Triangle.Base)).GetInt32(),
                            jsonObject.GetProperty(nameof(Triangle.Height)).GetInt32()
                        );
                default:
                    throw  new Exception("Unable to parse file");
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, Shape value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Type", value.GetType().Name);
        switch (value)
        {
            case Circle circle:
                writer.WriteNumber(nameof(Circle.Radius), circle.Radius);
                break;
            case Square square:
                writer.WriteNumber(nameof(Square.Side), square.Side);
                break;
            case Rectangle rectangle:
                writer.WriteNumber(nameof(Rectangle.Height), rectangle.Height);
                writer.WriteNumber(nameof(Rectangle.Width), rectangle.Width);
                break;
            case Triangle triangle:
                writer.WriteNumber(nameof(Triangle.Base), triangle.Base);
                writer.WriteNumber(nameof(Triangle.Height), triangle.Height);
                break;
            default:
                throw new InvalidOperationException("Unknown shape type");
        }

        writer.WriteEndObject();
    }
}