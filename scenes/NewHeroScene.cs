using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;

public class NewHeroScene : Control
{
    private Button BtnStrengthMinus, BtnStrengthPlus, BtnVitalityMinus, BtnVitalityPlus, BtnDexterityMinus, BtnDexterityPlus, BtnWisdomMinus, BtnWisdomPlus, BtnCreate, BtnReset, BtnCancel;
    private HeroClass _compareClass = new HeroClass();
    private HeroClass _selectedClass = new HeroClass();
    private ItemList LstClasses;
    private LineEdit TxtHeroName, PswdPassword, PswdConfirm;
    private TextEdit TxtDescription;
    private Label LblStrength, LblVitality, LblDexterity, LblWisdom, LblHealth, LblMagic, LblSkillPoints, LblError;
    private CheckButton ChkHardcore;

    #region Load Scene

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed)
        {
            if (eventKey.Scancode == (int)KeyList.Enter || (eventKey.Scancode == (int)KeyList.KpEnter && (!BtnCreate.Disabled)))
                _on_BtnCreate_pressed();
            else if (eventKey.Scancode == (int)KeyList.Escape)
                GetTree().Quit();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        LoadClasses();
        Clear();
    }

    /// <summary>Assigns all controls to something usable in code.</summary>
    private void AssignControls()
    {
        BtnStrengthMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnStrengthMinus");
        BtnVitalityMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnVitalityMinus");
        BtnDexterityMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnDexterityMinus");
        BtnWisdomMinus = (Button)GetNode("Vitals/Attributes/MinusButtons/BtnWisdomMinus");
        BtnStrengthPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnStrengthPlus");
        BtnVitalityPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnVitalityPlus");
        BtnDexterityPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnDexterityPlus");
        BtnWisdomPlus = (Button)GetNode("Vitals/Attributes/PlusButtons/BtnWisdomPlus");
        BtnCreate = (Button)GetNode("Bottom/Buttons/BtnCreate");
        BtnReset = (Button)GetNode("Bottom/Buttons/BtnReset");
        BtnCancel = (Button)GetNode("Bottom/Buttons/BtnCancel");
        TxtHeroName = (LineEdit)GetNode("Info/HeroName");
        PswdPassword = (LineEdit)GetNode("Info/Password");
        ChkHardcore = (CheckButton)GetNode("Info/CenterContainer/ChkHardcore");
        PswdConfirm = (LineEdit)GetNode("Info/ConfirmPassword");
        TxtDescription = (TextEdit)GetNode("Class/TxtDescription");
        LstClasses = (ItemList)GetNode("Class/ItemList");
        LblStrength = (Label)GetNode("Vitals/Attributes/AttributeValues/LblStrength");
        LblVitality = (Label)GetNode("Vitals/Attributes/AttributeValues/LblVitality");
        LblDexterity = (Label)GetNode("Vitals/Attributes/AttributeValues/LblDexterity");
        LblWisdom = (Label)GetNode("Vitals/Attributes/AttributeValues/LblWisdom");
        LblHealth = (Label)GetNode("Vitals/Statistics/TextLabels/LblHealth");
        LblMagic = (Label)GetNode("Vitals/Statistics/TextLabels/LblMagic");
        LblSkillPoints = (Label)GetNode("Vitals/Statistics/TextLabels/LblSkillPoints");
        LblError = (Label)GetNode("LblError");
    }

    /// <summary>Displays all classes available for selection.</summary>
    private void LoadClasses()
    {
        LstClasses.Items.Clear();
        foreach (HeroClass cls in GameState.AllClasses)
            LstClasses.AddItem(cls.Name);
    }

    #endregion Load Scene

    #region Display Manipulation

    /// <summary>Clears all text from the labels and resets the Page to default.</summary>
    private void Clear()
    {
        _selectedClass = new HeroClass();
        _compareClass = new HeroClass();
        LstClasses.UnselectAll();
        TxtHeroName.Clear();
        TxtHeroName.GrabFocus();
        PswdPassword.Clear();
        PswdConfirm.Clear();
        ChkHardcore.Pressed = false;
        LblError.Text = "";
        CheckSkillPoints();
        DisableMinus();
        TogglePlus(false);
    }

    /// <summary>Enables/disables buttons based on the Hero's available Skill Points.</summary>
    private void CheckSkillPoints()
    {
        if (LstClasses.IsAnythingSelected() && _selectedClass.SkillPoints > 0)
        {
            if (_selectedClass.SkillPoints >= _compareClass.SkillPoints)
                DisableMinus();
            TogglePlus(true);
            BtnCreate.Disabled = true;
        }
        else if (LstClasses.IsAnythingSelected() && _selectedClass.SkillPoints == 0)
        {
            TogglePlus(false);
            BtnCreate.Disabled = TxtHeroName.Text.Length == 0 || PswdPassword.Text.Length == 0
            || PswdConfirm.Text.Length == 0;
        }
        else if (LstClasses.IsAnythingSelected() && _selectedClass.SkillPoints < 0)
        {
            LblError.Text = "Somehow you have negative skill points. Please try creating your character again.";
            Clear();
        }
        else if (!LstClasses.IsAnythingSelected())
            BtnCreate.Disabled = true;

        UpdateLabels();
    }

    /// <summary>Updates all labels with current values.</summary>
    private void UpdateLabels()
    {
        LblStrength.Text = _selectedClass.Strength.ToString();
        LblVitality.Text = _selectedClass.Vitality.ToString();
        LblDexterity.Text = _selectedClass.Dexterity.ToString();
        LblWisdom.Text = _selectedClass.Wisdom.ToString();
        LblHealth.Text = _selectedClass.HealthToStringWithText;
        LblMagic.Text = _selectedClass.MagicToStringWithText;
        LblSkillPoints.Text = _selectedClass.SkillPointsToString;
    }

    #endregion Display Manipulation

    #region Attribute Modification

    /// <summary>Increases specified Attribute.</summary>
    /// <param name="attribute">Attribute to be increased.</param>
    /// <returns>Increased attribute</returns>
    private int IncreaseAttribute(int attribute)
    {
        if (Input.IsActionPressed("shift"))
        {
            if (_selectedClass.SkillPoints >= 5)
            {
                attribute += 5;
                _selectedClass.SkillPoints -= 5;
            }
            else
            {
                attribute += _selectedClass.SkillPoints;
                _selectedClass.SkillPoints = 0;
            }
        }
        else
        {
            attribute++;
            _selectedClass.SkillPoints--;
        }

        return attribute;
    }

    /// <summary>Decreases specified Attribute.</summary>
    /// <param name="attribute">Attribute to be decreased.</param>
    /// <param name="original">Original value of the attribute for the selected class.</param>
    /// <returns>Decreased attribute</returns>
    private int DecreaseAttribute(int attribute, int original)
    {
        if (Input.IsActionPressed("shift"))
        {
            if (attribute - original >= 5)
            {
                attribute -= 5;
                _selectedClass.SkillPoints += 5;
            }
            else
            {
                _selectedClass.SkillPoints += attribute - original;
                attribute -= attribute - original;
            }
        }
        else
        {
            attribute--;
            _selectedClass.SkillPoints++;
        }

        return attribute;
    }

    #endregion Attribute Modification

    #region Toggle Buttons

    /// <summary>Toggles the Disabled Property of the Plus Buttons.</summary>
    /// <param name="enabled">Should the Buttons be enabled?</param>
    private void TogglePlus(bool enabled)
    {
        BtnDexterityPlus.Disabled = !enabled;
        BtnStrengthPlus.Disabled = !enabled;
        BtnWisdomPlus.Disabled = !enabled;
        BtnVitalityPlus.Disabled = !enabled;
    }

    /// <summary>Disables attribute Minus Buttons.</summary>
    private void DisableMinus()
    {
        BtnDexterityMinus.Disabled = true;
        BtnStrengthMinus.Disabled = true;
        BtnWisdomMinus.Disabled = true;
        BtnVitalityMinus.Disabled = true;
    }

    #endregion Toggle Buttons

    #region Text Manipulation

    private void _on_HeroName_focus_entered() => TxtHeroName.SelectAll();

    private void _on_HeroName_focus_exited() => TxtHeroName.Deselect();

    private void _on_Password_focus_entered() => PswdPassword.SelectAll();

    private void _on_Password_focus_exited() => PswdPassword.Deselect();

    private void _on_ConfirmPassword_focus_entered() => PswdConfirm.SelectAll();

    private void _on_ConfirmPassword_focus_exited() => PswdConfirm.Deselect();

    private void _on_HeroName_text_changed(string new_text) => CheckSkillPoints();

    private void _on_Password_text_changed(String new_text) => CheckSkillPoints();

    private void _on_ConfirmPassword_text_changed(String new_text) => CheckSkillPoints();

    #endregion Text Manipulation

    #region Button Click

    private void _on_BtnCancel_pressed() => GetTree().ChangeScene("scenes/MainScene.tscn");

    private void _on_BtnCreate_pressed()
    {
        Hero createHero = new Hero();

        try
        {
            createHero = GameState.AllHeroes.Find(hero => hero.Name == TxtHeroName.Text);
        }
        catch (ArgumentNullException)
        {
        }
        if (!string.IsNullOrWhiteSpace(createHero?.Name))
        {
            LblError.Text = "This username has already been taken. Please choose another.";
            TxtHeroName.GrabFocus();
        }
        else
        {
            if (TxtHeroName.Text.Length >= 4 && PswdPassword.Text.Length >= 4)
                if (PswdPassword.Text.Trim() == PswdConfirm.Text.Trim())
                {
                    Hero newHero = new Hero(
                    TxtHeroName.Text.Trim(),
                    PBKDF2.HashPassword(PswdPassword.Text.Trim()),
                    _selectedClass,
                    1,
                    0,
                    0,
                    250,
                    new Attributes(
                    _selectedClass.Strength,
                    _selectedClass.Vitality,
                    _selectedClass.Dexterity,
                    _selectedClass.Wisdom),
                    new Statistics(
                    _selectedClass.CurrentHealth,
                    _selectedClass.MaximumHealth,
                    _selectedClass.CurrentMagic,
                    _selectedClass.MaximumMagic),
                    new Equipment(
                    new Weapon(),
                    new HeadArmor(),
                    new BodyArmor(),
                    new HandArmor(),
                    new LegArmor(),
                    new FeetArmor(),
                    new Ring(),
                    new Ring()),
                    new Spellbook(),
                    new List<Item>(),
                    new Bank(0, 0, 250),
                    new Progression(),
                    ChkHardcore.IsPressed());

                    GameState.NewHero(newHero);
                    GameState.CurrentHero = GameState.AllHeroes.Find(hero => hero.Name == newHero.Name);
                    GetTree().ChangeScene("scenes/city/CityScene.tscn");
                }
                else
                    LblError.Text = "Please ensure that the passwords match.";
            else
                LblError.Text = "Names and passwords have to be at least 4 characters.";
        }
    }

    private void _on_BtnReset_pressed() => Clear();

    private void _on_ItemList_item_selected(int index)
    {
        _compareClass = new HeroClass(GameState.AllClasses[index]);
        _selectedClass = new HeroClass(GameState.AllClasses[index]);
        CheckSkillPoints();
        TxtDescription.Text = _selectedClass.Description;
    }

    #region Plus/Minus Buttons Click

    private void _on_BtnStrengthMinus_pressed()
    {
        _selectedClass.Strength = DecreaseAttribute(_selectedClass.Strength, _compareClass.Strength);
        BtnStrengthMinus.Disabled = _selectedClass.Strength == _compareClass.Strength;
        CheckSkillPoints();
    }

    private void _on_BtnStrengthPlus_pressed()
    {
        _selectedClass.Strength = IncreaseAttribute(_selectedClass.Strength);
        BtnStrengthMinus.Disabled = false;
        CheckSkillPoints();
    }

    private void _on_BtnVitalityMinus_pressed()
    {
        _selectedClass.Vitality = DecreaseAttribute(_selectedClass.Vitality, _compareClass.Vitality);
        BtnVitalityMinus.Disabled = _selectedClass.Vitality == _compareClass.Vitality;
        CheckSkillPoints();
    }

    private void _on_BtnVitalityPlus_pressed()
    {
        _selectedClass.Vitality = IncreaseAttribute(_selectedClass.Vitality);
        BtnVitalityMinus.Disabled = false;
        CheckSkillPoints();
    }

    private void _on_BtDexterityMinus_pressed()
    {
        _selectedClass.Dexterity = DecreaseAttribute(_selectedClass.Dexterity, _compareClass.Dexterity);
        BtnDexterityMinus.Disabled = _selectedClass.Dexterity == _compareClass.Dexterity;
        CheckSkillPoints();
    }

    private void _on_BtnDexterityPlus_pressed()
    {
        _selectedClass.Dexterity = IncreaseAttribute(_selectedClass.Dexterity);
        BtnDexterityMinus.Disabled = false;
        CheckSkillPoints();
    }

    private void _on_BtnWisdomMinus_pressed()
    {
        _selectedClass.Wisdom = DecreaseAttribute(_selectedClass.Wisdom, _compareClass.Wisdom);
        BtnWisdomMinus.Disabled = _selectedClass.Wisdom == _compareClass.Wisdom;
        CheckSkillPoints();
    }

    private void _on_BtnWisdomPlus_pressed()
    {
        _selectedClass.Wisdom = IncreaseAttribute(_selectedClass.Wisdom);
        BtnWisdomMinus.Disabled = false;
        CheckSkillPoints();
    }

    #endregion Plus/Minus Buttons Click

    #endregion Button Click

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}