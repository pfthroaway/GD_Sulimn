using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Enums;
using Sulimn.Classes.HeroParts;
using System.Collections.Generic;

namespace Sulimn.Scenes.Character
{
    public class CastSpellScene : Control
    {
        private bool Battle;
        private Button BtnCastSpell;
        private ItemList LstSpells;
        private Label LblName, LblTypeAmount, LblMagicCost, LblCost, LblRequiredLevel, LblDescription;
        private Spell SelectedSpell = new Spell();

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        /// <summary>Assigns all controls.</summary>
        private void AssignControls()
        {
            BtnCastSpell = (Button)GetNode("BtnCastSpell");
            if (GameState.PreviousScene == "BattleScene")
                Battle = true;
            if (Battle)
                BtnCastSpell.Text = "Choose Spell";
            LblName = (Label)GetNode("SpellInfo/LblName");
            LblTypeAmount = (Label)GetNode("SpellInfo/LblTypeAmount");
            LblMagicCost = (Label)GetNode("SpellInfo/LblMagicCost");
            LblCost = (Label)GetNode("SpellInfo/LblCost");
            LblRequiredLevel = (Label)GetNode("SpellInfo/LblRequiredLevel");
            LblDescription = (Label)GetNode("SpellInfo/LblDescription");
            LstSpells = (ItemList)GetNode("LstSpells");
        }

        /// <summary>Displays information about the currently selected <see cref="Spell"/>.</summary>
        private void DisplaySpell()
        {
            if (SelectedSpell != null)
            {
                LblName.Text = SelectedSpell.Name;
                LblTypeAmount.Text = SelectedSpell.TypeAmount;
                LblMagicCost.Text = SelectedSpell.MagicCostToString;
                LblCost.Text = SelectedSpell.ValueToStringWithText;
                LblRequiredLevel.Text = SelectedSpell.RequiredLevelToString;
                LblDescription.Text = SelectedSpell.Description;
                BtnCastSpell.Disabled = string.IsNullOrWhiteSpace(SelectedSpell.Name) || GameState.CurrentHero.Statistics.CurrentMagic < SelectedSpell.MagicCost;
            }
        }

        /// <summary>Loads all <see cref="Spell"/>s not currently known by the <see cref="Hero"/>.</summary>
        private void LoadSpells()
        {
            if (GameState.CurrentHero.Spellbook.Spells.Count > 0)
            {
                foreach (Spell spl in GameState.CurrentHero.Spellbook.Spells)
                {
                    if (!Battle)
                    {
                        if (spl.Type != SpellType.Damage && spl.Type != SpellType.Shield)
                            LstSpells.AddItem(spl.Name);
                    }
                    else
                        LstSpells.AddItem(spl.Name);
                }
            }
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            LoadSpells();
        }

        #region Click

        private void _on_BtnCastSpell_pressed()
        {
            GameState.CurrentHero.CurrentSpell = SelectedSpell;
            if (Battle)
                GetTree().ChangeSceneTo(GameState.GoBack());
            else
            {
                if (GameState.CurrentHero.CurrentSpell.Type == SpellType.Healing)
                    GameState.CurrentHero.Heal(GameState.CurrentHero.CurrentSpell.Amount);
                GameState.CurrentHero.Statistics.CurrentMagic -= GameState.CurrentHero.CurrentSpell.MagicCost;
                GameState.Info.DisplayStats();
            }
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }

        private void _on_LstSpells_item_selected(int index)
        {
            if (index >= 0)
                SelectedSpell = GameState.AllSpells.Find(spl => spl.Name == LstSpells.Items[index].ToString());
            DisplaySpell();
        }

        #endregion Click
    }
}