using CadSimulation.Interfaces;
using System.Net.Http;
using System.Text;

namespace CadSimulation.remoteStore
{
    public class RemoteStore : IRepositoryStore
    {
        private RemoteStoreConfiguration? configuration;
        private HttpClient client = new HttpClient();
        public List<IShape> GetAllShapes()
        {
            throw new NotImplementedException();
            try
            {
                HttpResponseMessage response = client.GetAsync(configuration.URI).Result; // Bloccante
                response.EnsureSuccessStatusCode();
                string stringContent = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("GET Response: " + stringContent);
            }
            catch (Exception e)
            {
                Console.WriteLine("GET Error: " + e.Message);
            }
        }

        public void Initialize(IConfigurationForStore configuration, IRepositoryDataFormat dataFormat)
        {
            this.configuration = configuration as RemoteStoreConfiguration;
        }

        public bool SaveAllShapes(List<IShape> shapes)
        {
            throw new NotImplementedException();
            string sData = String.Empty;
            try
            {
                StringContent content = new StringContent(sData, Encoding.UTF8);
                HttpResponseMessage response = client.PostAsync(configuration.URI, content).Result; // Bloccante
                response.EnsureSuccessStatusCode();
                string sReply = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("POST Response: " + sReply);
            }
            catch (Exception e)
            {
                Console.WriteLine("POST Error: " + e.Message);
            }
        }
    }
}
