using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.Interfaces
{
    public interface IConfigurationForStore
    {
    }

    public interface IRepositoryStore
    {
        void Initialize(IConfigurationForStore configuration, IRepositoryDataFormat dataFormat);
        List<IShape> GetAllShapes();
        bool SaveAllShapes(List<IShape> shapes);
    }
}
