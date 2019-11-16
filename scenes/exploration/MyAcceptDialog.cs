using Godot;
using Sulimn.Actors;

namespace Sulimn.Scenes.Exploration
{
    public class MyAcceptDialog : AcceptDialog
    {
        private Player player;
        private float timer;
        private float expiration = 1f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            player = (Player)GetTree().CurrentScene.FindNode("Player");
        }

        /// <summary>Sets a custom expiration for the popup.</summary>
        /// <param name="expire">Time before expiring</param>
        public void SetExpiration(float expire) => expiration = expire;

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            player.Disabled = Visible;
            if (Visible)
                timer += delta;
            if (timer > expiration)
            {
                Visible = false;
                timer = 0;
            }
        }
    }
}