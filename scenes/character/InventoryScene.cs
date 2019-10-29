using Godot;
using Sulimn.Classes;
using System;

public class InventoryScene : CanvasLayer
{
    private TextureRect Head, Body, Hands, Legs, Feet, Weapon, LeftRing, RightRing;
    private AnimationPlayer player;
    private bool showScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (AnimationPlayer)GetNode("AnimationPlayer");
        SlideIn();
        AssignControls();
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

    #region Animation

    private void SlideIn() => player.Play("slide");

    private void SlideOut()
    {
        player.PlayBackwards("slide");
        SetTooltips();
    }

    #endregion Animation

    private void _on_Button_pressed()
    {
        if (!showScene)
        {
            SlideOut();
        }
        else
        {
            SlideIn();
        }
        showScene = !showScene;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}