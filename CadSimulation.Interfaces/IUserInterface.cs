using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.Interfaces
{
    public interface IUserInterface
    {
        
        //string GetInput();
        void Run();
    }
    public interface INotifier
    {
        void ShowMessage(string message);
    }
}
