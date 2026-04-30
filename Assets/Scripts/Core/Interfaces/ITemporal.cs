using System.Numerics;

namespace Core.Interfaces
{
    public interface ITemporal
    {
        public Vector2 Range { get; set; }
        public void TimeBehavior();
    }
}