using System;
using System.Drawing;

namespace Alexbox.Domain
{
    public class GamePage : IGamePage
    {
        public IGamePage WithBackground(Image image)
        {
            throw new NotImplementedException();
        }

        public IGamePage WithTask(string task)
        {
            throw new NotImplementedException();
        }

        public IGamePage WithNextButton(string buttonText, Action action)
        {
            throw new NotImplementedException();
        }
    }
}