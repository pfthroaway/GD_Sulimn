using Godot;
using Sulimn.Classes;
using System;

public class CharacterScene : Control
{
    private Label LblName, LblLevel, LblExperience, LblSkillPoints, LblHardcore, LblGold, LblStrength, LblVitality, LblDexterity, LblWisdom, LblHealth, LblMagic;

    private Button BtnStrengthMinus, BtnStrengthPlus, BtnVitalityMinus, BtnVitalityPlus, BtnDexterityMinus, BtnDexterityPlus, BtnWisdomMinus, BtnWisdomPlus, BtnInventory, BtnCastSpell, BtnReset, BtnClose;

    private bool showScene = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Visible = false;
        AssignControls();
    }

    /// <summary>Assigns all controls to something usable in code.</summary>
    private void AssignControls()
    {
        LblName = (Label)GetNode("Info/LblName");
        LblLevel = (Label)GetNode("Info/LblLevel");
        LblExperience = (Label)GetNode("Info/LblExperience");
        LblSkillPoints = (Label)GetNode("Info/LblSkillPoints");
        LblHardcore = (Label)GetNode("Info/LblHardcore");
        LblGold = (Label)GetNode("Info/LblGold");
        BtnInventory = (Button)GetNode("Info/Buttons/Buttons/Top/BtnInventory");
        BtnCastSpell = (Button)GetNode("Info/Buttons/Buttons/Top/BtnCastSpell");
        BtnReset = (Button)GetNode("Info/Buttons/Buttons/Bottom/BtnReset");
        BtnClose = (Button)GetNode("Info/Buttons/Buttons/Bottom/BtnClose");

        BtnStrengthMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnStrengthMinus");
        BtnVitalityMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnVitalityMinus");
        BtnDexterityMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnDexterityMinus");
        BtnWisdomMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnWisdomMinus");
        BtnStrengthPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnStrengthPlus");
        BtnVitalityPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnVitalityPlus");
        BtnDexterityPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnDexterityPlus");
        BtnWisdomPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnWisdomPlus");
        LblStrength = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblStrength");
        LblVitality = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblVitality");
        LblDexterity = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblDexterity");
        LblWisdom = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblWisdom");
        LblHealth = (Label)GetNode("Info/Vitals/Statistics/TextLabels/LblHealth");
        LblMagic = (Label)GetNode("Info/Vitals/Statistics/TextLabels/LblMagic");
    }

    public void UpdateLabels()
    {
        LblName.Text = GameState.CurrentHero.Name;
        LblLevel.Text = GameState.CurrentHero.LevelAndClassToString;
        LblExperience.Text = GameState.CurrentHero.ExperienceToStringWithText;
        LblSkillPoints.Text = GameState.CurrentHero.SkillPointsToString;
        LblHardcore.Text = GameState.CurrentHero.HardcoreToString;
        LblGold.Text = GameState.CurrentHero.GoldToStringWithText;

        UpdateAttributeLabels();
    }

    private void UpdateAttributeLabels()
    {
        LblStrength.Text = GameState.CurrentHero.TotalStrength.ToString("N0");
        LblVitality.Text = GameState.CurrentHero.TotalVitality.ToString("N0");
        LblDexterity.Text = GameState.CurrentHero.TotalDexterity.ToString("N0");
        LblWisdom.Text = GameState.CurrentHero.TotalWisdom.ToString("N0");
        LblHealth.Text = GameState.CurrentHero.Statistics.HealthToStringWithText;
        LblMagic.Text = GameState.CurrentHero.Statistics.MagicToStringWithText;
    }

    private void _on_BtnCharacter_pressed()
    {
        AnimationPlayer player = (AnimationPlayer)GetNode("AnimationPlayer");
        if (!showScene)
            player.Play("slide_out");
        else
            player.PlayBackwards("slide_out");
        showScene = !showScene;
    }

    private void _on_Control_focus_entered()
    {
        GD.Print("ENTERED");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}