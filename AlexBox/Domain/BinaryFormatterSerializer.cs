using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AlexBox
{
    public class BinaryFormatterSerializer : ISerializer
    {
        private readonly BinaryFormatter formatter = new BinaryFormatter();

        public byte[] Serialize<TObject>(TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public TObject Deserialize<TObject>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var deserilizedObject = (TObject)formatter.Deserialize(stream);

                return deserilizedObject;
            }
        }

    }
}
