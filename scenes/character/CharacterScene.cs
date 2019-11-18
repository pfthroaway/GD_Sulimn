using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Scenes.Inventory;

namespace Sulimn.Scenes.Character
{
    public class CharacterScene : Control
    {
        private Button BtnStrengthMinus, BtnStrengthPlus, BtnVitalityMinus, BtnVitalityPlus, BtnDexterityMinus, BtnDexterityPlus, BtnWisdomMinus, BtnWisdomPlus;
        private Label LblName, LblLevel, LblExperience, LblSkillPoints, LblHardcore, LblGold, LblStrength, LblVitality, LblDexterity, LblWisdom, LblHealth, LblMagic;
        private GridEquipment GridEquipment;
        private GridInventory GridInventory;
        private Hero _copyOfHero = new Hero();

        // TODO Make it to where you can't add Fists to the inventory.

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
            {
                Save();
                GameState.Info.DisplayStats();
                GameState.UpdateDisplay = false;
                GetTree().ChangeSceneTo(GameState.GoBack());
            }
        }

        #region Load

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            _copyOfHero = new Hero(GameState.CurrentHero);
            UpdateLabels();
            GridInventory.SetUpInventory(GameState.CurrentHero.Inventory);
            GridEquipment.SetUpEquipment(GameState.CurrentHero.Equipment);
            CheckSkillPoints();
        }

        /// <summary>Assigns all controls to something usable in code.</summary>
        private void AssignControls()
        {
            GridInventory = (GridInventory)GetNode("GridInventory");
            GridEquipment = (GridEquipment)GetNode("GridEquipment");

            LblName = (Label)GetNode("Info/LblName");
            LblLevel = (Label)GetNode("Info/LblLevel");
            LblExperience = (Label)GetNode("Info/LblExperience");
            LblHardcore = (Label)GetNode("Info/LblHardcore");

            BtnStrengthMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnStrengthMinus");
            BtnVitalityMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnVitalityMinus");
            BtnDexterityMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnDexterityMinus");
            BtnWisdomMinus = (Button)GetNode("Info/Vitals/Attributes/MinusButtons/BtnWisdomMinus");

            LblStrength = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblStrength");
            LblVitality = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblVitality");
            LblDexterity = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblDexterity");
            LblWisdom = (Label)GetNode("Info/Vitals/Attributes/AttributeValues/LblWisdom");

            BtnStrengthPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnStrengthPlus");
            BtnVitalityPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnVitalityPlus");
            BtnDexterityPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnDexterityPlus");
            BtnWisdomPlus = (Button)GetNode("Info/Vitals/Attributes/PlusButtons/BtnWisdomPlus");

            LblSkillPoints = (Label)GetNode("Info/Vitals/Statistics/TextLabels/LblSkillPoints");
            LblHealth = (Label)GetNode("Info/Vitals/Statistics/TextLabels/LblHealth");
            LblMagic = (Label)GetNode("Info/Vitals/Statistics/TextLabels/LblMagic");

            LblGold = (Label)GetNode("Info/LblGold");
        }

        #endregion Load

        #region Save

        private void Save()
        {
            SaveInventory();
            SaveEquipment();
        }

        private void SaveInventory() => GameState.SetInventoryFromGrid(GridInventory);

        private void SaveEquipment() => GameState.SetEquipmentFromGrid(GridEquipment);

        #endregion Save

        #region Display Manipulation

        /// <summary>Enables/disables buttons based on the Hero's available Skill Points.</summary>
        private void CheckSkillPoints()
        {
            if (GameState.CurrentHero.SkillPoints > 0)
            {
                if (GameState.CurrentHero.SkillPoints >= _copyOfHero.SkillPoints)
                    DisableMinus();
                TogglePlus(true);
            }
            else if (GameState.CurrentHero.SkillPoints == 0)
            {
                TogglePlus(false);
            }
            else if (GameState.CurrentHero.SkillPoints < 0)
            {
                GameState.CurrentHero = new Hero(_copyOfHero);
            }

            GameState.CurrentHero.UpdateStatistics();
            UpdateLabels();
        }

        /// <summary>Updates all the labels.</summary>
        public void UpdateLabels()
        {
            LblName.Text = GameState.CurrentHero.Name;
            LblLevel.Text = GameState.CurrentHero.LevelAndClassToString;
            LblExperience.Text = GameState.CurrentHero.ExperienceToStringWithText;
            LblSkillPoints.Text = GameState.CurrentHero.SkillPointsToString;
            LblHardcore.Text = GameState.CurrentHero.HardcoreToString;
            LblGold.Text = GameState.CurrentHero.GoldToStringWithText;

            UpdateAttributeLabels();

            GameState.Info.DisplayStats();
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

        #endregion Display Manipulation

        #region Attribute Modification

        /// <summary>Increases specified Attribute.</summary>
        /// <param name="attribute">Attribute to be increased.</param>
        /// <returns>Increased attribute</returns>
        private int IncreaseAttribute(int attribute)
        {
            if (Input.IsActionPressed("shift"))
            {
                if (GameState.CurrentHero.SkillPoints >= 5)
                {
                    attribute += 5;
                    GameState.CurrentHero.SkillPoints -= 5;
                }
                else
                {
                    attribute += GameState.CurrentHero.SkillPoints;
                    GameState.CurrentHero.SkillPoints = 0;
                }
            }
            else
            {
                attribute++;
                GameState.CurrentHero.SkillPoints--;
            }
            CheckSkillPoints();
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
                    GameState.CurrentHero.SkillPoints += 5;
                }
                else
                {
                    GameState.CurrentHero.SkillPoints += attribute - original;
                    attribute -= attribute - original;
                }
            }
            else
            {
                attribute--;
                GameState.CurrentHero.SkillPoints++;
            }

            CheckSkillPoints();
            return attribute;
        }

        #endregion Attribute Modification

        #region Buttons

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

        #region Button Click

        private void _on_BtnCastSpell_pressed()
        {
            GameState.AddSceneToHistory(GetTree().CurrentScene);
            GetTree().ChangeScene("res://scenes/character/CastSpellScene.tscn");
        }

        #region Plus/Minus Buttons Click

        private void _on_BtnStrengthMinus_pressed()
        {
            GameState.CurrentHero.Attributes.Strength = DecreaseAttribute(GameState.CurrentHero.Attributes.Strength, _copyOfHero.Attributes.Strength);
            BtnStrengthMinus.Disabled = GameState.CurrentHero.Attributes.Strength == _copyOfHero.Attributes.Strength;
            CheckSkillPoints();
        }

        private void _on_BtnStrengthPlus_pressed()
        {
            GameState.CurrentHero.Attributes.Strength = IncreaseAttribute(GameState.CurrentHero.Attributes.Strength);
            BtnStrengthMinus.Disabled = false;
            CheckSkillPoints();
        }

        private void _on_BtnVitalityMinus_pressed()
        {
            GameState.CurrentHero.Attributes.Vitality = DecreaseAttribute(GameState.CurrentHero.Attributes.Vitality, _copyOfHero.Attributes.Vitality);
            GameState.CurrentHero.Statistics.CurrentHealth -= 5;
            GameState.CurrentHero.Statistics.MaximumHealth -= 5;
            BtnVitalityMinus.Disabled = GameState.CurrentHero.Attributes.Vitality == _copyOfHero.Attributes.Vitality;
            CheckSkillPoints();
        }

        private void _on_BtnVitalityPlus_pressed()
        {
            GameState.CurrentHero.Attributes.Vitality = IncreaseAttribute(GameState.CurrentHero.Attributes.Vitality);
            GameState.CurrentHero.Statistics.CurrentHealth += 5;
            GameState.CurrentHero.Statistics.MaximumHealth += 5;
            BtnVitalityMinus.Disabled = false;
            CheckSkillPoints();
        }

        private void _on_BtnDexterityMinus_pressed()
        {
            GameState.CurrentHero.Attributes.Dexterity = DecreaseAttribute(GameState.CurrentHero.Attributes.Dexterity, _copyOfHero.Attributes.Dexterity);
            BtnDexterityMinus.Disabled = GameState.CurrentHero.Attributes.Dexterity == _copyOfHero.Attributes.Dexterity;
            CheckSkillPoints();
        }

        private void _on_BtnDexterityPlus_pressed()
        {
            GameState.CurrentHero.Attributes.Dexterity = IncreaseAttribute(GameState.CurrentHero.Attributes.Dexterity);
            BtnDexterityMinus.Disabled = false;
            CheckSkillPoints();
        }

        private void _on_BtnWisdomMinus_pressed()
        {
            GameState.CurrentHero.Attributes.Wisdom = DecreaseAttribute(GameState.CurrentHero.Attributes.Wisdom, _copyOfHero.Attributes.Wisdom);
            GameState.CurrentHero.Statistics.CurrentMagic -= 5;
            GameState.CurrentHero.Statistics.MaximumMagic -= 5;

            BtnWisdomMinus.Disabled = GameState.CurrentHero.Attributes.Wisdom == _copyOfHero.Attributes.Wisdom;
            CheckSkillPoints();
        }

        private void _on_BtnWisdomPlus_pressed()
        {
            GameState.CurrentHero.Attributes.Wisdom = IncreaseAttribute(GameState.CurrentHero.Attributes.Wisdom);
            GameState.CurrentHero.Statistics.CurrentMagic += 5;
            GameState.CurrentHero.Statistics.MaximumMagic += 5;
            BtnWisdomMinus.Disabled = false;
            CheckSkillPoints();
        }

        #endregion Plus/Minus Buttons Click

        #endregion Button Click

        #endregion Buttons

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (GameState.UpdateDisplay)
            {
                Save();
                UpdateLabels();
                GameState.UpdateDisplay = false;
            }
        }
    }
}