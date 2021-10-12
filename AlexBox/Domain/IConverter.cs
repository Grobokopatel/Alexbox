using System;
using System.Collections.Generic;
using System.Text;

namespace AlexBox.Domain
{
    public interface IConverter
    {
        public TMessage ToMessage<TObject, TMessage>(TObject obj);
        public TObject ToObject<TObject, TMessage>(TMessage message);
    }
}
