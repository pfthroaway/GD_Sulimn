using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Enums;
using Sulimn.Classes.HeroParts;

namespace Sulimn.Scenes.CharacterScenes
{
    public class CastSpellScene : Control
    {
        private Button BtnCastSpell;
        private ItemList LstSpells;
        private Label LblName, LblTypeAmount, LblMagicCost, LblCost, LblRequiredLevel, LblDescription, LblError;

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventKey eventKey && eventKey.Pressed && eventKey.Scancode == (int)KeyList.Escape)
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        /// <summary>Assigns all controls.</summary>
        private void AssignControls()
        {
            BtnCastSpell = (Button)GetNode("BtnCastSpell");
            LblName = (Label)GetNode("SpellInfo/LblName");
            LblTypeAmount = (Label)GetNode("SpellInfo/LblTypeAmount");
            LblMagicCost = (Label)GetNode("SpellInfo/LblMagicCost");
            LblCost = (Label)GetNode("SpellInfo/LblCost");
            LblRequiredLevel = (Label)GetNode("SpellInfo/LblRequiredLevel");
            LblDescription = (Label)GetNode("SpellInfo/LblDescription");
            LblError = (Label)GetNode("LblError");
            LstSpells = (ItemList)GetNode("LstSpells");
        }

        /// <summary>Displays information about the currently selected <see cref="Spell"/>.</summary>
        private void DisplaySpell()
        {
            if (GameState.CurrentHero.CurrentSpell != null)
            {
                LblName.Text = GameState.CurrentHero.CurrentSpell.Name;
                LblTypeAmount.Text = GameState.CurrentHero.CurrentSpell.TypeAmount;
                LblMagicCost.Text = GameState.CurrentHero.CurrentSpell.MagicCostToString;
                LblCost.Text = GameState.CurrentHero.CurrentSpell.ValueToStringWithText;
                LblRequiredLevel.Text = GameState.CurrentHero.CurrentSpell.RequiredLevelToString;
                LblDescription.Text = GameState.CurrentHero.CurrentSpell.Description;
                BtnCastSpell.Disabled = string.IsNullOrWhiteSpace(GameState.CurrentHero.CurrentSpell.Name) || GameState.CurrentHero.Statistics.CurrentMagic < GameState.CurrentHero.CurrentSpell.MagicCost;
            }
        }

        /// <summary>Loads all <see cref="Spell"/>s not currently known by the <see cref="Hero"/>.</summary>
        private void LoadSpells()
        {
            foreach (Spell spl in GameState.CurrentHero.Spellbook.Spells)
                LstSpells.AddItem(spl.Name);
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
            SpellType type = GameState.CurrentHero.CurrentSpell.Type;
            if (type == SpellType.Damage || type == SpellType.Shield)
                LblError.Text = "You are not currently in a battle, therefore you are unable to cast this spell.";
            else if (type == SpellType.Healing)
            {
                GameState.CurrentHero.Heal(GameState.CurrentHero.CurrentSpell.Amount);
                GameState.CurrentHero.Statistics.CurrentMagic -= GameState.CurrentHero.CurrentSpell.MagicCost;
                GameState.Info.DisplayStats();
            }
        }

        private void _on_BtnReturn_pressed() => GetTree().ChangeSceneTo(GameState.GoBack());

        private void _on_LstSpells_item_selected(int index)
        {
            LblError.Text = "";
            if (index >= 0)
                GameState.CurrentHero.CurrentSpell = GameState.CurrentHero.Spellbook.Spells[index];
            DisplaySpell();
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }

        #endregion Click
    }
}