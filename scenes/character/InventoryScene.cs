using Godot;
using Sulimn.Classes;

public class InventoryScene : CanvasLayer
{
    private TextureRect Head, Body, Hands, Legs, Feet, Weapon, LeftRing, RightRing;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        SetTooltips();
        //TODO I need to figure out a way to remember my navigation so I can return to the proper scene from the Inventory Scene.
    }

    /// <summary>Assign all controls.</summary>
    private void AssignControls()
    {
        Head = (TextureRect)GetNode("Inventory/Head");
        Body = (TextureRect)GetNode("Inventory/Body");
        Hands = (TextureRect)GetNode("Inventory/Hands");
        Legs = (TextureRect)GetNode("Inventory/Legs");
        Feet = (TextureRect)GetNode("Inventory/Feet");
        Weapon = (TextureRect)GetNode("Inventory/Weapon");
        LeftRing = (TextureRect)GetNode("Inventory/LeftRing");
        RightRing = (TextureRect)GetNode("Inventory/RightRing");
    }

    private void SetTooltips()
    {
        Head.SetTooltip(GameState.CurrentHero.Equipment.Head.TooltipText);
        Body.SetTooltip(GameState.CurrentHero.Equipment.Body.TooltipText);
        Hands.SetTooltip(GameState.CurrentHero.Equipment.Hands.TooltipText);
        Legs.SetTooltip(GameState.CurrentHero.Equipment.Legs.TooltipText);
        Feet.SetTooltip(GameState.CurrentHero.Equipment.Feet.TooltipText);
        Weapon.SetTooltip(GameState.CurrentHero.Equipment.Weapon.TooltipText);
        LeftRing.SetTooltip(GameState.CurrentHero.Equipment.LeftRing.TooltipText);
        RightRing.SetTooltip(GameState.CurrentHero.Equipment.RightRing.TooltipText);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}