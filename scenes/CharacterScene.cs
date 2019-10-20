using Godot;
using Sulimn.Classes;
using System;

public class CharacterScene : Control
{
    private Label LblName, LblLevel, LblExperience, LblSkillPoints, LblHardcore, LblGold, LblStrength, LblVitality, LblDexterity, LblWisdom, LblHealth, LblMagic;

    private Button BtnStrengthMinus, BtnStrengthPlus, BtnVitalityMinus, BtnVitalityPlus, BtnDexterityMinus, BtnDexterityPlus, BtnWisdomMinus, BtnWisdomPlus, BtnInventory, BtnCastSpell, BtnReset, BtnClose;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        UpdateLabels();
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
        BtnInventory = (Button)GetNode("Buttons/BtnInventory");
        BtnCastSpell = (Button)GetNode("Buttons/BtnCastSpell");
        BtnReset = (Button)GetNode("Buttons/BtnReset");
        BtnClose = (Button)GetNode("Buttons/BtnClose");

        BtnStrengthMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnStrengthMinus");
        BtnVitalityMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnVitalityMinus");
        BtnDexterityMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnDexterityMinus");
        BtnWisdomMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnWisdomMinus");
        BtnStrengthPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnStrengthPlus");
        BtnVitalityPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnVitalityPlus");
        BtnDexterityPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnDexterityPlus");
        BtnWisdomPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnWisdomPlus");
        LblStrength = (Label)GetNode("Vitals/Attributes/AttributeValues/LblStrength");
        LblVitality = (Label)GetNode("Vitals/Attributes/AttributeValues/LblVitality");
        LblDexterity = (Label)GetNode("Vitals/Attributes/AttributeValues/LblDexterity");
        LblWisdom = (Label)GetNode("Vitals/Attributes/AttributeValues/LblWisdom");
        LblHealth = (Label)GetNode("Vitals/Statistics/TextLabels/LblHealth");
        LblMagic = (Label)GetNode("Vitals/Statistics/TextLabels/LblMagic");
    }

    private void UpdateLabels()
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

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}