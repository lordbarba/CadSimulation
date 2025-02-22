using CadSimulation.business;
using CadSimulation.Interfaces;
using static System.Formats.Asn1.AsnWriter;
using System.Text;
using System.Reflection;

namespace CadSimulation.consoleUI
{
    public class ConsoleUI : IUserInterface, INotifier
    {
        private CadSimulation.business.Manager manager;

        public ConsoleUI(Manager manager)
        {
            this.manager = manager;
        }

        public void Run()
        {
            StringBuilder sbMenu=new StringBuilder();

            // create the menu
            sbMenu.Append("\nOptions:\n");

            foreach (var item in Helper.GetShapeTypes())
            {
                char code = (char)item.Value.GetProperty(nameof(IShape.Code),
                        BindingFlags.Static | BindingFlags.Public).GetValue(null);
                sbMenu.Append($"   '{code}': insert a {item.Key}\n");
            }
            
            sbMenu.Append(
            "   'l': list all inserted shapes\n" +
            "   'a': all shapres total area\n" +
            "   'k': store data\n" +
            "   'w': get stored data\n" +
            "   'q': quit");

            while (true)
            {
                Console.WriteLine(sbMenu.ToString());

                var k = Console.ReadKey(true);
                if (k.KeyChar == 'q')
                    break;

                switch (k.KeyChar)
                {
                    case 'l':
                        {
                            foreach (var item in this.manager.ShapeList)
                            {
                                item.descr();
                            }
                        }
                        continue;
                    case 'k':
                        this.manager.Save();
                        break;
                    case 'w':
                        this.manager.Load();
                        break;
                    case 'a':
                        {
                            double area = this.manager.CalculateTotalArea();
                            Console.WriteLine("Total area: {0}", area);
                        }
                        continue;
                     default:
                        {
                            Type shapeType = Helper.GetShapeTypeFromCode(k.KeyChar);
                            if (shapeType == null) continue;
                            IShape shape = Activator.CreateInstance(shapeType) as IShape;
                            foreach (var prop in shapeType.GetProperties().Where(x => x.CanWrite == true && x.CanRead == true).OrderBy(x => x.Name))
                            {
                                Console.WriteLine($"value for {prop.Name}:\t");
                                var iValue = Int32.Parse(Console.ReadLine()!);
                                prop.SetValue(shape, iValue);
                            }
                            manager.AddShape(shape);
                            continue;
                        }

                }
            }

        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
