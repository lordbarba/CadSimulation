using CadSimulation.Interfaces;

namespace CadSimulation.localStore
{
    public class LocalStore : IRepositoryStore
    {
        private LocalStoreConfiguration? configuration;

        public List<IShape> GetAllShapes()
        {
            throw new NotImplementedException();
            string sDataRead= System.IO.File.ReadAllText(configuration.Filename);
        }

        public void Initialize(IConfigurationForStore configuration, IRepositoryDataFormat dataFormat)
        {
            this.configuration = configuration as LocalStoreConfiguration;
        }

        public bool SaveAllShapes(List<IShape> shapes)
        {
            throw new NotImplementedException();
            string sDataToWrite = string.Empty;
            System.IO.File.WriteAllText(configuration.Filename, sDataToWrite);
        }
    }
}
