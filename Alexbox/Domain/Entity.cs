namespace Alexbox.Domain
{
    public abstract class Entity
    {
		public long Id { get; set; }

		private bool Equals(Entity other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj is null) 
				return false;
			if (ReferenceEquals(this, obj)) 
				return true;

			return obj is Entity entity && Equals(entity);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"{GetType().Name} ({nameof(Id)}: {Id})";
		}
	}
}
