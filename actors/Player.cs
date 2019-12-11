using Godot;
using Godot.Collections;

namespace Sulimn.Actors
{
    /// <summary>Represents a moveable <see cref="Player"/>.</summary>
    public class Player : Area2D
    {
        private readonly int TileSize = 64;
        public bool Disabled;
        private RayCast2D ray;
        private float moveDelay;

        /// <summary>Available directions to move the <see cref="Player"/>.</summary>
        private readonly Dictionary<string, Vector2> inputs = new Dictionary<string, Vector2> {
        { "up", Vector2.Up },
        { "down", Vector2.Down },
        { "left", Vector2.Left },
        { "right", Vector2.Right } };

        /// <summary>Moves the <see cref="Player"/> in a given direction.</summary>
        /// <param name="dir">Direction in which to move the <see cref="Player"/></param>
        public void Move(string dir)
        {
            ray.CastTo = inputs[dir] * TileSize;
            ray.ForceRaycastUpdate();
            if (!ray.IsColliding())
                Position += inputs[dir] * TileSize;
            moveDelay = 0;
        }

        /// <summary>Disables the <see cref="Player"/> from moving.</summary>
        public void DisablePlayer() => Disabled = true;

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            moveDelay += delta;
            foreach (string dir in inputs.Keys)
                if (!Disabled && Input.IsActionPressed(dir) && moveDelay >= 0.15)
                    Move(dir);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            ray = (RayCast2D)GetNode("RayCast2D");
            Disabled = false;
        }
    }
}