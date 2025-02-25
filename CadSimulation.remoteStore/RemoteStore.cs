using CadSimulation.Interfaces;
using System.Net.Http;
using System.Text;

namespace CadSimulation.remoteStore
{
    public class RemoteStore : IRepositoryStore
    {
        private RemoteStoreConfiguration? configuration;
        private IRepositoryDataFormat dataFormat;
        private HttpClient client = new HttpClient();
        public List<IShape> GetAllShapes()
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(configuration.URI).Result; // Bloccante
                response.EnsureSuccessStatusCode();
                string stringContent = response.Content.ReadAsStringAsync().Result;

                return dataFormat.GetListFromString(stringContent);
            }
            catch (Exception e)
            {
                throw new Exception("GET Error: " + e.Message);
            }
        }

        public void Initialize(IConfigurationForStore configuration, IRepositoryDataFormat dataFormat)
        {
            this.configuration = configuration as RemoteStoreConfiguration;
            this.dataFormat= dataFormat;
        }

        public bool SaveAllShapes(List<IShape> shapes)
        {
            string sData = dataFormat.GetStringFromList(shapes);
            try
            {
                StringContent content = new StringContent(sData, Encoding.UTF8);
                HttpResponseMessage response = client.PostAsync(configuration.URI, content).Result; // Bloccante
                response.EnsureSuccessStatusCode();
                string sReply = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                throw new Exception("POST Error: " + e.Message);
            }
            return true;
        }
    }
}
