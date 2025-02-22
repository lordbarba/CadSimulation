using CadSimulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CadSimulation.jsonDataFormat
{
    class ShapeDataConverter : System.Text.Json.Serialization.JsonConverter<IShape>
    {
        public override IShape Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IShape shape = null;
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = jsonDoc.RootElement;
                var type = jsonObject.GetProperty("Type").GetString();

                Helper.GetShapeTypes().TryGetValue(type, out Type shapeType);
                if (shapeType == null)
                {
                    throw new Exception("Shape type not found");
                }
                shape = (IShape)Activator.CreateInstance(shapeType);
                foreach (var property in jsonObject.EnumerateObject())
                {
                    if (property.Name == "Type") continue;
                    var propInfo = shapeType.GetProperty(property.Name, 
                        BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        if (propInfo.PropertyType == typeof(int))
                        {
                            propInfo.SetValue(shape, property.Value.GetInt32());
                        }
                        else if (propInfo.PropertyType == typeof(string))
                        {
                            propInfo.SetValue(shape, property.Value.GetString());
                        }
                        else if (propInfo.PropertyType == typeof(Color))
                        {
                            propInfo.SetValue(shape, Color.FromName(property.Value.GetString()));
                        }
                    }
                }
            }
            return shape;
        }

        public override void Write(Utf8JsonWriter writer, IShape value, JsonSerializerOptions options)
        {
            Type type = value.GetType();
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);

            // cycle all properties, and check propertytype.
            foreach (var property in type.GetProperties())
            {
                if (property.CanWrite == false || property.CanRead == false) continue;
                if (property.PropertyType == typeof(int))
                {
                    writer.WriteNumber(property.Name, (int)property.GetValue(value));
                }
                else if (property.PropertyType == typeof(string))
                {
                    writer.WriteString(property.Name, (string)property.GetValue(value));
                }
                else if (property.PropertyType == typeof(Color))
                {
                    writer.WriteString(property.Name, ((Color)property.GetValue(value)).Name);
                }
            }


            writer.WriteEndObject();
        }
    }
}
