using Godot;
using Godot.Collections;

public class Player : Area2D
{
    private readonly int TileSize = 64;
    public bool Disabled;
    private RayCast2D ray;

    private readonly Dictionary<string, Vector2> inputs = new Dictionary<string, Vector2> {
        { "up", Vector2.Up },
        { "down", Vector2.Down },
        { "left", Vector2.Left },
        { "right", Vector2.Right } };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ray = (RayCast2D)GetNode("RayCast2D");
        Disabled = false;
    }

    public override void _Input(InputEvent @event)
    {
        foreach (string dir in inputs.Keys)
            if (!Disabled && @event.IsActionPressed(dir))
                Move(dir);
    }

    public void Move(string dir)
    {
        ray.CastTo = inputs[dir] * TileSize;
        ray.ForceRaycastUpdate();
        if (!ray.IsColliding())
            Position += inputs[dir] * TileSize;
    }

    public void DisablePlayer() => Disabled = true;

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}