using Godot;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using System;

namespace Sulimn.Scenes.Exploration
{
    internal class Rect
    {
        internal int Width { get; set; }
        internal int Height { get; set; }
        internal Vector2 Position { get; set; } = new Vector2();

        public Rect(int x, int y, int w, int h)
        {
            Position = new Vector2(x, y);
            Width = x + w;
            Height = y + h;
        }

        internal Vector2 Center()
        {
            int centerx = Int32Helper.Parse(Math.Floor((Position.x + Width) / 2));
            int centery = Int32Helper.Parse(Math.Floor((Position.y + Height) / 2));
            return new Vector2(centerx, centery);
        }

        internal bool Intersect(Rect other) => (Position.x <= other.Width) && (Width >= other.Position.x) && (Position.y <= other.Height) && (Height >= other.Position.y);
    }
}