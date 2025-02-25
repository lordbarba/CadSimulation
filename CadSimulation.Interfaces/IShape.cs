namespace CadSimulation.Interfaces
{
    public interface IShape
    {
        static string Name { get; }
        static char Code { get => throw new NotImplementedException(); }
        string descr();
        double area();
    }
}
