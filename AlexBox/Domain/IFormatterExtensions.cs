using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Alexbox.Domain
{
    public static class IFormatterExtenstion
    {
        public static byte[] Serialize<TObject>(this IFormatter formatter, TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static TObject Deserialize<TObject>(this IFormatter formatter, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var deserilizedObject = (TObject)formatter.Deserialize(stream);
                return deserilizedObject;
            }
        }
    }
}
