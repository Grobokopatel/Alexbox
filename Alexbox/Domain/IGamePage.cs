using System;
using System.Drawing;

namespace Alexbox.Domain
{
    public interface IGamePage
    {
        public IGamePage WithBackground(Image image);
        public IGamePage WithTask(string task);
        public IGamePage WithNextButton(string buttonText, Action action);
    }
}