using CadSimulation.Interfaces;
using System.Drawing;
using System.Text.Json;

namespace CadSimulation.jsonDataFormat
{
    public class JsonDataFormat : IRepositoryDataFormat
    {
        public List<IShape> GetListFromString(string data)
        {
            var shapesList = JsonSerializer.Deserialize<List<IShape>>(data,
                new JsonSerializerOptions
                {
                    Converters = { new ShapeDataConverter() }
                }
                );
            return shapesList;
        }

        public string GetStringFromList(List<IShape> shapes)
        {
            var json = JsonSerializer.Serialize(shapes,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new ShapeDataConverter() }
                }
             );
            return json;
        }
    }
}

