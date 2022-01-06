namespace Alexbox.Domain
{
    public class Viewer : Entity
    {
        public readonly long Id;
        private readonly string Name;
        public Viewer(string name, long id)
        {
            Name = name;
            Id = id;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}