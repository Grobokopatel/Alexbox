namespace Alexbox.Domain
{
    public class Viewer : Entity
    {
        public readonly string Name;

        public Viewer(long id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}