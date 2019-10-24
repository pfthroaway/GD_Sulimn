using Godot;
using Sulimn.Classes;
using System;

public class Info : Control
{
    private Label LblLevel, LblExperience, LblGold, LblHealth, LblMagic;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Visible = false;
        AssignControls();
    }

    /// <summary>Assigns all controls to local variables for easy use.</summary>
    private void AssignControls()
    {
        LblHealth = (Label)GetNode("LblHealth");
        LblMagic = (Label)GetNode("LblMagic");
        LblLevel = (Label)GetNode("LblLevel");
        LblExperience = (Label)GetNode("LblExperience");
        LblGold = (Label)GetNode("LblGold");
    }

    /// <summary>Updates labels to current values.</summary>
    public void DisplayStats()
    {
        LblHealth.Text = GameState.CurrentHero.Statistics.HealthToStringWithText;
        LblMagic.Text = GameState.CurrentHero.Statistics.MagicToStringWithText;
        LblLevel.Text = GameState.CurrentHero.LevelAndClassToString;
        LblExperience.Text = GameState.CurrentHero.ExperienceToStringWithText;
        LblGold.Text = GameState.CurrentHero.GoldToStringWithText;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}