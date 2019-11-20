using Godot;
using Sulimn.Classes;
using Sulimn.Classes.HeroParts;
using System.Collections.Generic;

namespace Sulimn.Scenes.Shopping
{
    public class MagickShoppeScene : Control
    {
        private Button BtnLearnSpell;
        private ItemList LstSpells;
        private Label LblName, LblTypeAmount, LblMagicCost, LblCost, LblRequiredLevel, LblDescription;
        private List<Spell> AvailableSpells = new List<Spell>();
        private Spell SelectedSpell = new Spell();

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        private void AssignControls()
        {
            BtnLearnSpell = (Button)GetNode("BtnLearnSpell");
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
                BtnLearnSpell.Disabled = string.IsNullOrWhiteSpace(SelectedSpell.Name) || GameState.CurrentHero.Gold < SelectedSpell.Value;
            }
        }

        /// <summary>Loads all <see cref="Spell"/>s not currently known by the <see cref="Hero"/>.</summary>
        private void LoadSpells()
        {
            LstSpells.Clear();
            AvailableSpells.Clear();
            foreach (Spell spl in GameState.AllSpells)
            {
                if (!GameState.CurrentHero.Spellbook.Spells.Contains(spl) && (spl.AllowedClasses.Count == 0 || spl.AllowedClasses.Contains(GameState.CurrentHero.Class)))
                    AvailableSpells.Add(spl);
            }

            if (AvailableSpells.Count > 0)
                AvailableSpells.ForEach(spl => LstSpells.AddItem(spl.Name));
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
            LoadSpells();
        }

        #region Click

        private void _on_BtnLearnSpell_pressed()
        {
            GameState.CurrentHero.Gold -= SelectedSpell.Value;
            GameState.CurrentHero.Spellbook.LearnSpell(SelectedSpell);
            GameState.SaveHero(GameState.CurrentHero);
            SelectedSpell = new Spell();
            DisplaySpell();
            LoadSpells();
            GameState.Info.DisplayStats();
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
                SelectedSpell = AvailableSpells[index];
            DisplaySpell();
        }

        #endregion Click
    }
}