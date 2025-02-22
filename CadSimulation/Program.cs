// See https://aka.ms/new-console-template for more information
using CadSimulation;
using CadSimulation.customDataFormat;
using CadSimulation.Interfaces;
using CadSimulation.jsonDataFormat;
using CadSimulation.localStore;
using CadSimulation.remoteStore;
using CommandLine;
using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<IShape> shapes = new List<IShape>();
string sTargetFilename = string.Empty;
bool bUseJsonFormat = false;
string sRemoteUri = string.Empty;
HttpClient client = new HttpClient();
string sData;


Parser.Default.ParseArguments<CommandLineOptions>(args)
           .WithParsed(o =>
           {
               sTargetFilename = o.Path; 
               bUseJsonFormat= o.Json;
               sRemoteUri = o.Uri;
           });

Console.WriteLine("Filename selected: {0}", sTargetFilename);
Console.WriteLine("Export as JSON: {0}", bUseJsonFormat);

IRepositoryStore store;
IRepositoryDataFormat dataFormat;
IConfigurationForStore configurationForStore;
// store selection. Could be a Factory
if (!string.IsNullOrEmpty(sTargetFilename))
{
    store = new LocalStore();
    configurationForStore = new LocalStoreConfiguration(sTargetFilename);
}
else if (!string.IsNullOrEmpty(sRemoteUri))
{
    store = new RemoteStore();
    configurationForStore = new RemoteStoreConfiguration(sRemoteUri);
}
else
{
    Console.WriteLine("No store selected");
    return;
}

// format selection. Could be a Factory
if (bUseJsonFormat)
    dataFormat = new JsonDataFormat();
else
    dataFormat = new CustomDataFormat();

store.Initialize(configurationForStore, dataFormat);

while (true)
{
    Console.WriteLine(
"\nOptions:\n" +
"   's': insert a square\n" +
"   't': insert a triangle\n" +
"   'c': insert a circle\n" +
"   'r': insert a rectangle\n" +
"   'l': list all inserted shapes\n" +
"   'a': all shapres total area\n" +
"   'k': store data\n" +
"   'w': get stored data\n" +
"   'q': quit");

    var k = Console.ReadKey(true);
    if (k.KeyChar == 'q')
        break;

    IShape? shape = null;
    switch (k.KeyChar)
    {
        case 'l':
            {
                shapes.ForEach(s =>
                {
                    if(s!=null)
                        s.descr();
                });
            }
            continue;
        case 's':
            {
                Console.WriteLine("Square. Value for side:\t");
                var side = Int32.Parse(Console.ReadLine()!);
                shape = new Square(side); // Console.WriteLine("Square");
            }
            break;
        case 'r':
            {
                Console.WriteLine("Rectangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for weidth:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Rectangle(hight, weidth); // Console.WriteLine("Rectangle");
            }
            break;
        case 't':
            {
                Console.WriteLine("Triangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for base:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Triangle(hight, weidth); // Console.WriteLine("Triangle");
            }
            break;
        case 'c':
            Console.WriteLine("Circle. Value for radius:\t");
            var radius = Int32.Parse(Console.ReadLine()!);
            shape = new Circle(radius); // Console.WriteLine("Circle");
            break;
        case 'k':
            sData = "";

            if (!string.IsNullOrEmpty(sTargetFilename))
            {
                Console.WriteLine($"Storing data to {sTargetFilename}");
                System.IO.File.WriteAllText(sTargetFilename, sData);
            }
            else if (!string.IsNullOrEmpty(sRemoteUri))
            {
                store.SaveAllShapes(shapes);

                Console.WriteLine($"Storing data to remote {sRemoteUri}");
                try
                {
                    StringContent content = new StringContent(sData, Encoding.UTF8);
                    HttpResponseMessage response = client.PostAsync(sRemoteUri, content).Result; // Bloccante
                    response.EnsureSuccessStatusCode();
                    string sReply = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("POST Response: " + sReply);
                }
                catch (Exception e)
                {
                    Console.WriteLine("POST Error: " + e.Message);
                    break;
                }
            }
            break;
        case 'w':
            shapes.Clear();
            shapes = store.GetAllShapes();
            string stringContent = string.Empty;
            if (!string.IsNullOrEmpty(sTargetFilename))
            {
                Console.WriteLine($"Retrieving  data from {sTargetFilename}");
                stringContent = System.IO.File.ReadAllText(sTargetFilename);
            }
            else if (!string.IsNullOrEmpty(sRemoteUri))
            {
                Console.WriteLine($"Retrieving  data from remote {sRemoteUri}");

                try
                {
                    HttpResponseMessage response = client.GetAsync(sRemoteUri).Result; // Bloccante
                    response.EnsureSuccessStatusCode();
                    stringContent = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("GET Response: " + stringContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine("GET Error: " + e.Message);
                }

            }
            break;
        case 'a':
            {
                double area = 0;
                foreach (var s in shapes)
                    area += s.area();

                Console.WriteLine("Total area: {0}", area);
            }
            continue;
    }
    if(shape != null)
        shapes.Add(shape);

}

class CommandLineOptions
{
    [Option("path", Required = false, HelpText = "Write path to file")]
    public string Path { get; set; }
    [Option("json", Required = false, Default =false, HelpText = "Export in JSON format")]
    public bool Json { get; set; }

    [Option("uri", Required = false, HelpText = "Remote repository url")]
    public string Uri { get; set; }

}

