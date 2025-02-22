using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.Interfaces
{
    public interface IRepositoryDataFormat
    {
        string GetStringFromList(List<IShape> shapes);
        List<IShape> GetListFromString(string data);
    }
}
