namespace Alexbox.Domain
{
    public class Viewer : Entity
    {
        public readonly string Name;

        public Viewer(string name)
        {
            Name = name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}