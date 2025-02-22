using CadSimulation.Interfaces;
using System.Reflection;
using System.Text;

namespace CadSimulation.customDataFormat
{
    public class CustomDataFormat : IRepositoryDataFormat
    {
        public List<IShape> GetListFromString(string data)
        {
            List<IShape> retList = new List<IShape>();
            int iCounter = 0;
            string[] sLines = data.Split(Environment.NewLine);
            string sCode = "";
            foreach (var item in sLines)
            {
                if (string.IsNullOrEmpty(item)) continue;
                string[] sValues = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (sValues.Length == 0) continue;

                sCode=sValues[0];
                Type type = Helper.GetShapeTypeFromCode(Convert.ToChar( sCode));
                if (type != null)
                {
                    IShape shape = (IShape)Activator.CreateInstance(type);
                    iCounter = 1;
                    foreach (var prop in type.GetProperties().Where(x=>x.CanWrite==true && x.CanRead==true).OrderBy(x => x.Name))
                    {
                        prop.SetValue(shape,Convert.ToInt32( sValues[iCounter]));
                        iCounter++;
                    }
                    retList.Add(shape);
                }
            }
            return retList;
        }

        public string GetStringFromList(List<IShape> shapes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in shapes)
            {
                char code = (char)item.GetType().GetProperty(nameof(IShape.Code), 
                    BindingFlags.Static | BindingFlags.Public).GetValue(null);

                sb.Append(Char.ToUpper(code));
                foreach (var prop in item.GetType().GetProperties().OrderBy(x=>x.Name))
                {
                    if (prop.CanWrite == false || prop.CanRead == false) continue;
                    sb.Append(" ").Append(prop.GetValue(item).ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
