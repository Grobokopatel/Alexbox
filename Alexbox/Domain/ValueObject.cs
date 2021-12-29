using System;
using System.Linq;
using System.Reflection;

namespace Alexbox.Domain
{
    public abstract class ValueObject<T>
    {
        private readonly static PropertyInfo[] publicProperties;

        static ValueObject()
        {
            publicProperties = typeof(T)
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
            return other is T t && Equals(t);
        }

        public bool Equals(T other)
        {
            return ReferenceEquals(this, other)
                   || (other is ValueObject<T> t
                       && GetPropertiesValues()
                           .SequenceEqual(t.GetPropertiesValues()));
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