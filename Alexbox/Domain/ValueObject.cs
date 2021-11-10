using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alexbox.Domain
{
    public class ValueObject
    {
        private static PropertyInfo[] publicProperties;

        public ValueObject()
        {
            publicProperties ??= GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToArray();
        }

        public object[] GetPropertiesValues()
        {
            return publicProperties
                .Select(property => property.GetValue(this))
                .ToArray();
        }

        public override bool Equals(object other)
        {
            return other.GetType().IsAssignableTo(GetType())
                && Equals((ValueObject)other);
        }

        public bool Equals(ValueObject other)
        {
            return ReferenceEquals(this, other)
                || GetPropertiesValues()
                .SequenceEqual(other.GetPropertiesValues());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return GetPropertiesValues()
                .Aggregate(17, (hash, value) => hash * 31 + value.GetHashCode());
            }
        }

        public override string ToString()
        {
            var propertiesAsString =
                string.Join("; ",
                publicProperties
                .Select(property => $"{property.Name}: {property.GetValue(this)}")
                .OrderBy(str => str, StringComparer.Ordinal));

            return $"{GetType().Name}({propertiesAsString})";
        }
    }
}
