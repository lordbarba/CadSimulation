using CadSimulation;
using CadSimulation.business;
using CadSimulation.customDataFormat;
using CadSimulation.Interfaces;
using CadSimulation.jsonDataFormat;
using CadSimulation.localStore;
using CadSimulation.remoteStore;
using CommandLine;
using System.Text;
using CadSimulation.consoleUI;

string sTargetFilename = string.Empty;
bool bUseJsonFormat = false;
string sRemoteUri = string.Empty;
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

// initialize store
store.Initialize(configurationForStore, dataFormat);


// initialize manager
Manager manager = new Manager(store);
IUserInterface userInterface = new ConsoleUI(manager);


userInterface.Run();



class CommandLineOptions
{
    [Option("path", Required = false, HelpText = "Write path to file")]
    public string Path { get; set; }
    [Option("json", Required = false, Default =false, HelpText = "Export in JSON format")]
    public bool Json { get; set; }

    [Option("uri", Required = false, HelpText = "Remote repository url")]
    public string Uri { get; set; }

}

