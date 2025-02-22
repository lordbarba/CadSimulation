using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.Interfaces
{
    public static class Helper
    {
        private static Dictionary<string, Type> shapeTypes=null;

        public static Dictionary<string, Type> GetShapeTypes()
        {
            if(shapeTypes != null)
            {
                return shapeTypes;
            }
            shapeTypes = new Dictionary<string, Type>();

            string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var dllFiles = Directory.GetFiles(executingAssemblyPath, "*.dll");

            foreach (var dllFile in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dllFile);
                var types = assembly.GetTypes().Where(t => typeof(IShape).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                foreach (var type in types)
                {
                    var nameProperty = type.GetProperty("Name", BindingFlags.Static | BindingFlags.Public);
                    if (nameProperty != null)
                    {
                        var name = (string)nameProperty.GetValue(null);
                        shapeTypes[name] = type;
                    }
                }
            }

            return shapeTypes;
        }
    }
}
