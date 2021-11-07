using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexBox.Domain
{
    public class Entity
    {
		public Entity()
		{
			Id = new Guid();
		}

		public Entity(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }

		protected bool Equals(Entity other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (obj is null) 
				return false;
			if (ReferenceEquals(this, obj)) 
				return true;
			if (!obj.GetType().IsAssignableTo(GetType()))
				return false;

			return Equals((Entity)obj);
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
