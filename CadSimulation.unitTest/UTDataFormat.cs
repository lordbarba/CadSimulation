using CadSimulation.circle;
using CadSimulation.customDataFormat;
using CadSimulation.Interfaces;
using CadSimulation.jsonDataFormat;
using CadSimulation.rectangle;
using CadSimulation.square;
using CardSimulation.triangle;
using System.Text;

namespace CadSimulation.unitTest
{
    public class UTDataFormat
    {
        private List<IShape> shapes = new List<IShape>();

        [SetUp]
        public void Setup()
        {
            shapes.Add(new Triangle(12, 14));
            shapes.Add(new Rectangle(10, 12));
            shapes.Add(new Square(7));
            shapes.Add(new Circle(19));
        }

        [Test]
        public void TestDataJson()
        {
            JsonDataFormat jsonDataFormat = new JsonDataFormat();
            string data = jsonDataFormat.GetStringFromList(shapes);


            List<IShape> shapes2 = jsonDataFormat.GetListFromString(data);

            Assert.AreEqual(shapes.Count, shapes2.Count);
            Assert.AreEqual(shapes[1].GetType(), typeof(Rectangle));
            // make some checks on the other shapes
            Assert.AreEqual((shapes[1] as Rectangle).Width, 12);
            Assert.AreEqual((shapes[1] as Rectangle).Height, 10);
            Assert.AreEqual((shapes[0] as Triangle).Base, 12);
            Assert.AreEqual((shapes[0] as Triangle).Height, 14);
        }

        [Test]
        public void TestDataCustom()
        {
            CustomDataFormat customDataFormat = new CustomDataFormat();
            string data = customDataFormat.GetStringFromList(shapes);


            List<IShape> shapes2 = customDataFormat.GetListFromString(data);

            Assert.AreEqual(shapes.Count, shapes2.Count);
            Assert.AreEqual(shapes[0].GetType(), typeof(Triangle));
            // make some checks on the other shapes
            Assert.AreEqual((shapes[1] as Rectangle).Width, 12);
            Assert.AreEqual((shapes[1] as Rectangle).Height, 10);
        }
    }
}