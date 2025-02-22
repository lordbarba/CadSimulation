using CadSimulation.Interfaces;

namespace CadSimulation.business
{
    public class Manager
    {
        private IRepositoryStore repositoryStore;
        private List<IShape> shapeList = new List<IShape>();
        public IEnumerable<IShape> ShapeList { get => shapeList; }

        public Manager(IRepositoryStore repositoryStore)
        {
            this.repositoryStore = repositoryStore;
        }
        public void AddShape(IShape shape)
        {
            shapeList.Add(shape);
        }

        public void Load()
        {
            shapeList= this.repositoryStore.GetAllShapes();
        }

        public void Save()
        {
            this.repositoryStore.SaveAllShapes(this.shapeList);
        }

        public double CalculateTotalArea()
        {
            double area = 0;
            foreach (var s in shapeList)
                area += s.area();
            return area;
        }
    }
}
