using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox
{
    public interface ISerializer
    {
        public byte[] Serialize<TObject>(TObject obj);
        public TObject Deserialize<TObject>(byte[] bytes);
    }
}
