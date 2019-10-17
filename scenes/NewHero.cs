using Godot;
using Sulimn.Classes;
using Sulimn.Classes.HeroParts;
using System.Collections.Generic;

public class NewHero : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private List<HeroClass> _classes = new List<HeroClass>();

    private HeroClass _compareClass = new HeroClass();
    private HeroClass _selectedClass = new HeroClass();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadClasses();
        Update();
    }

    private void LoadClasses()
    {
        _classes = GameState.AllClasses;
        ItemList list = (ItemList)GetNode("Class/ItemList");
        list.Items.Clear();
        foreach (HeroClass cls in _classes)
            list.Items.Add(cls.Name);
    }

    private void Update()
    {
        Label health = (Label)GetNode("Column3/Statistics/TextLabels/LblHealth");
        health.Text = _selectedClass.HealthToStringWithText;
        Label magic = (Label)GetNode("Column3/Statistics/TextLabels/LblMagic");
        magic.Text = _selectedClass.MagicToStringWithText;
        Label skillPoints = (Label)GetNode("Column3/Statistics/TextLabels/LblSkillPoints");
        skillPoints.Text = _selectedClass.SkillPointsToString;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}