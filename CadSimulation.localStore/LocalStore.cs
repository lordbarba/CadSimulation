using CadSimulation.Interfaces;

namespace CadSimulation.localStore
{
    public class LocalStore : IRepositoryStore
    {
        private LocalStoreConfiguration configuration;
        private IRepositoryDataFormat dataFormat;

        public List<IShape> GetAllShapes()
        {
            string sDataRead= System.IO.File.ReadAllText(configuration.Filename);
            return dataFormat.GetListFromString(sDataRead);
        }

        public void Initialize(IConfigurationForStore configuration, IRepositoryDataFormat dataFormat)
        {
            this.configuration = configuration as LocalStoreConfiguration;
            this.dataFormat= dataFormat;
        }

        public bool SaveAllShapes(List<IShape> shapes)
        {
            string sDataToWrite = dataFormat.GetStringFromList(shapes);
            System.IO.File.WriteAllText(configuration.Filename, sDataToWrite);
            return true;
        }
    }
}
