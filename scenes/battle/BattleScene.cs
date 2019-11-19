using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;
using System.Linq;
using Character = Sulimn.Classes.Entities.Character;

namespace Sulimn.Scenes.Battle
{
    public class BattleScene : Control
    {
        private BattleAction _enemyAction, _heroAction;
        public bool BattleEnded;
        private bool _blnHardcoreDeath;
        private bool _progress;
        private Button BtnAttack, BtnCastSpell, BtnEnemyDetails, BtnFlee, BtnLootBody, BtnReturn;
        private ItemList LstSpells;
        private Label LblHeroName, LblHeroHealth, LblHeroMagic, LblHeroShield, LblEnemyName, LblEnemyHealth, LblEnemyMagic, LblEnemyShield, LblSpellTypeAmount, LblSpellCost;
        private RichTextLabel TxtBattle;
        private string _previousPage;

        // TODO Lose gold on death.

        #region Modifying Properties

        /// <summary>The <see cref="Hero"/>'s current magical shield value.</summary>
        public int HeroShield { get; set; }

        /// <summary>The <see cref="Enemy"/>'s current magical shield value.</summary>
        public int EnemyShield { get; set; }

        #endregion Modifying Properties

        #region Helper Properties

        /// <summary>The <see cref="Hero"/>'s current magical shield value, formatted.</summary>
        public string HeroShieldToString => $"Shield: {HeroShield}";

        /// <summary>The <see cref="Enemy"/>'s current magical shield value, formatted.</summary>
        public string EnemyShieldToString => $"Shield: {EnemyShield}";

        #endregion Helper Properties

        #region Load

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            if (@event.Pressed && @event.Scancode == (int)KeyList.Escape)
                _on_BtnReturn_pressed();
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (GameState.BattleScene != null)
                BattleEnded = GameState.BattleScene.BattleEnded;
            AssignControls();
            UpdateStats();
        }

        /// <summary>Assigns all controls to modifiable values.</summary>
        private void AssignControls()
        {
            BtnAttack = (Button)GetNode("BtnAttack");
            BtnCastSpell = (Button)GetNode("BtnCastSpell");
            BtnEnemyDetails = (Button)GetNode("Enemy/CC/VB/BtnEnemyDetails");
            BtnFlee = (Button)GetNode("BtnFlee");
            BtnLootBody = (Button)GetNode("Enemy/CC/VB/BtnLootBody");
            BtnReturn = (Button)GetNode("BtnReturn");
            LstSpells = (ItemList)GetNode("LstSpells");
            LblHeroName = (Label)GetNode("Hero/LblName");
            LblHeroHealth = (Label)GetNode("Hero/LblHealth");
            LblHeroMagic = (Label)GetNode("Hero/LblMagic");
            LblHeroShield = (Label)GetNode("Hero/LblShield");
            LblEnemyName = (Label)GetNode("Enemy/LblName");
            LblEnemyHealth = (Label)GetNode("Enemy/LblHealth");
            LblEnemyMagic = (Label)GetNode("Enemy/LblMagic");
            LblEnemyShield = (Label)GetNode("Enemy/LblShield");
            LblSpellTypeAmount = (Label)GetNode("LblSpellTypeAmount");
            LblSpellCost = (Label)GetNode("LblSpellCost");
            TxtBattle = (RichTextLabel)GetNode("TxtBattle");

            LstSpells.Clear();
            foreach (Spell spl in GameState.CurrentHero.Spellbook.Spells)
                LstSpells.AddItem(spl.Name);
            if (GameState.CurrentHero.CurrentSpell != new Spell())
                LstSpells.Select(GameState.CurrentHero.Spellbook.Spells.IndexOf(GameState.CurrentHero.CurrentSpell));
        }

        /// <summary>Update labels to current values.</summary>
        private void UpdateStats()
        {
            GameState.Info.DisplayStats();
            LblHeroName.Text = GameState.CurrentHero.Name;
            LblHeroHealth.Text = GameState.CurrentHero.Statistics.HealthToStringWithText;
            LblHeroMagic.Text = GameState.CurrentHero.Statistics.MagicToStringWithText;
            LblHeroShield.Text = HeroShieldToString;
            LblEnemyName.Text = GameState.CurrentEnemy.Name;
            LblEnemyHealth.Text = GameState.CurrentEnemy.Statistics.HealthToStringWithText;
            LblEnemyMagic.Text = GameState.CurrentEnemy.Statistics.MagicToStringWithText;
            LblEnemyShield.Text = EnemyShieldToString;
            LblSpellTypeAmount.Text = GameState.CurrentHero.CurrentSpell.TypeAmount;
            LblSpellCost.Text = GameState.CurrentHero.CurrentSpell.MagicCostToString;
            DisplaySpell();
            CheckButtons();
        }

        private void DisplaySpell()
        {
            if (GameState.CurrentHero.CurrentSpell != null)
            {
                LblSpellTypeAmount.Text = GameState.CurrentHero.CurrentSpell.TypeAmount;
                LblSpellCost.Text = GameState.CurrentHero.CurrentSpell.MagicCostToString;
                BtnCastSpell.Disabled = GameState.CurrentHero.CurrentSpell == new Spell() || GameState.CurrentHero.Statistics.CurrentMagic < GameState.CurrentHero.CurrentSpell.MagicCost;
            }
        }

        #endregion Load

        /// <summary>Adds text to the TxtBattle RichTextLabel.</summary>
        /// <param name="newText">Text to be added</param>
        private void AddTextToTextBox(string newText)
        {
            TxtBattle.Text += TxtBattle.Text.Length > 0 ? "\n\n" + newText : newText;
            TxtBattle.ScrollFollowing = true;
        }

        /// <summary>Represents an action taken in a battle.</summary>
        private enum BattleAction
        {
            Attack,
            Cast,
            Flee
        }

        #region Battle Management

        /// <summary>Sets up the battle engine.</summary>
        /// <param name="prevPage">Previous Page</param>
        /// <param name="progress">Will this battle be for progression?</param>
        internal void PrepareBattle(string prevPage, bool progress = false)
        {
            _previousPage = prevPage;
            _progress = progress;
            TxtBattle.Text = progress ? $"{GameState.CurrentEnemy.Name} rushes at you in the {_previousPage}. You defend yourself. " : $"You encounter an enemy. The {GameState.CurrentEnemy.Name} seems openly hostile to you. Prepare to defend yourself.";
        }

        /// <summary>Ends the battle and allows the user to exit the Page.</summary>
        private void EndBattle(bool win)
        {
            BattleEnded = true;
            if (win && GameState.CurrentEnemy.Type != "Animal")
                BtnLootBody.Disabled = false;

            BtnReturn.Disabled = false;
        }

        #endregion Battle Management

        #region Battle Logic

        /// <summary>Starts a new round of battle.</summary>
        /// <param name="heroAction">Action the hero chose to perform this round</param>
        private void NewRound(BattleAction heroAction)
        {
            ToggleButtons(true);
            _heroAction = heroAction;

            // if Hero Dexterity is greater
            // chance to attack first is between 51 and 90%
            // chance to hit is 50 + Hero Dexterity - Enemy Dexterity
            // chance for Enemy to hit is 50 - Hero Dexterity + Enemy Dexterity
            // 10% chance to hit/miss no matter how big the difference is between the two

            int chanceHeroAttacksFirst = GameState.CurrentHero.TotalDexterity > GameState.CurrentEnemy.TotalDexterity ? Functions.GenerateRandomNumber(51, 90) : Functions.GenerateRandomNumber(10, 49);
            int attacksFirst = Functions.GenerateRandomNumber(1, 100);

            if (attacksFirst <= chanceHeroAttacksFirst)
            {
                HeroTurn();
                if (GameState.CurrentEnemy.Statistics.CurrentHealth > 0 && !BattleEnded)
                {
                    EnemyTurn();
                    if (GameState.CurrentHero.Statistics.CurrentHealth <= 0)
                        Death();
                }
            }
            else
            {
                EnemyTurn();
                if (GameState.CurrentHero.Statistics.CurrentHealth > 0 && !BattleEnded)
                    HeroTurn();
                else if (GameState.CurrentHero.Statistics.CurrentHealth <= 0)
                    Death();
            }
            GameState.Info.DisplayStats();
            UpdateStats();
            CheckButtons();
        }

        /// <summary>The Hero's turn this round of battle.</summary>
        private void HeroTurn()
        {
            switch (_heroAction)
            {
                case BattleAction.Attack:
                    HeroAttack();
                    break;

                case BattleAction.Cast:

                    AddTextToTextBox($"You cast {GameState.CurrentHero.CurrentSpell.Name}.");

                    switch (GameState.CurrentHero.CurrentSpell.Type)
                    {
                        case SpellType.Damage:
                            HeroAttack(true);
                            break;

                        case SpellType.Healing:
                            AddTextToTextBox(GameState.CurrentHero.Heal(GameState.CurrentHero.CurrentSpell.Amount));
                            break;

                        case SpellType.Shield:
                            HeroShield = GameState.CurrentHero.CurrentSpell.Amount;
                            AddTextToTextBox($"You now have a magical shield which will help protect you from {HeroShield} damage.");
                            break;
                    }

                    GameState.CurrentHero.Statistics.CurrentMagic -= GameState.CurrentHero.CurrentSpell.MagicCost;
                    break;

                case BattleAction.Flee:

                    if (FleeAttempt(GameState.CurrentHero.TotalDexterity, GameState.CurrentEnemy.TotalDexterity))
                    {
                        EndBattle(false);
                        AddTextToTextBox($"You successfully fled from the {GameState.CurrentEnemy.Name}.");
                    }
                    else
                        AddTextToTextBox($"The {GameState.CurrentEnemy.Name} blocked your attempt to flee.");
                    break;
            }
        }

        /// <summary>The <see cref="Hero"/> attacks the <see cref="Enemy"/>.</summary>
        /// <param name="castSpell">Is the <see cref="Hero"/> attacking with a <see cref="Spell"/>?</param>
        private void HeroAttack(bool castSpell = false)
        {
            //The Hero's chance to hit depends on whether or not they're overweight and their Dexterity compared to the enemy.
            int chanceHeroHits = !GameState.CurrentHero.Overweight
                ? Functions.GenerateRandomNumber(50 + GameState.CurrentEnemy.Attributes.Dexterity - GameState.CurrentHero.Attributes.Dexterity, 90, 10, 90)
                : Functions.GenerateRandomNumber(Int32Helper.Parse(50 + GameState.CurrentEnemy.Attributes.Dexterity - (GameState.CurrentHero.Attributes.Dexterity * GameState.CurrentHero.StrengthWeightRatio)), 90, 10, 90);

            // If the number generated by this method call is less than the chance that the Hero hits, the Hero successfully hits.
            int success = Functions.GenerateRandomNumber(10, 90);

            if (success <= chanceHeroHits)
            {
                // If successfully hitting, set up
                int statModifier = GetStatModifier(GameState.CurrentHero, castSpell);

                int damage = !castSpell
                    ? GameState.CurrentHero.Equipment.Weapon.Damage
                    : GameState.CurrentHero.CurrentSpell.Amount;

                // Maximum damage is 20% of the statistic used for their primary + damage from the weapon.
                int maximumDamage = Int32Helper.Parse((statModifier * 0.2) + damage);

                // If overweight, multiply by the strength/weight ratio.
                if (GameState.CurrentHero.Overweight)
                    maximumDamage = Int32Helper.Parse(maximumDamage * GameState.CurrentHero.StrengthWeightRatio);

                // Choose the Item which is going to be hit.
                Item itemGettingHit = ChooseItemToHit(GameState.CurrentEnemy.Equipment);

                // Actual defense is maximum defense multiplied by the current durability of the item being hit.
                // If there is no durability left, bypass it completely for determining defense.
                int enemyDefense = itemGettingHit.CurrentDurability > 0 ? Int32Helper.Parse(itemGettingHit.Defense * itemGettingHit.DurabilityRatio) : 0;

                // Actual damage is 10-100% of the maximum damage.
                int actualDamage = Functions.GenerateRandomNumber(maximumDamage / 10, maximumDamage, 1);

                // Maximum shield absorb is 10-100% of the shield currently being attacked.
                int maximumShieldAbsorb = Functions.GenerateRandomNumber(HeroShield / 10, HeroShield);

                // Maximum armor absorb is 10-100% of the Item currently being attacked.
                int maximumArmorAbsorb = enemyDefense > 0 ? Functions.GenerateRandomNumber(enemyDefense / 10, enemyDefense, 1) : 0;

                int actualArmorAbsorb = 0;

                // Shield absorbs actualDamage up to maxShieldAbsorb.
                int actualShieldAbsorb = maximumShieldAbsorb >= actualDamage ? actualDamage : maximumShieldAbsorb;

                HeroShield -= actualShieldAbsorb;

                // If shield absorbs all damage, actualArmorAbsorb is 0, otherwise check actualDamage - maxShieldAbsorb.
                if (actualShieldAbsorb < actualDamage)
                    actualArmorAbsorb = maximumArmorAbsorb >= actualDamage - actualShieldAbsorb ? actualDamage - actualShieldAbsorb : maximumArmorAbsorb;

                string absorb = "";
                string shield = "";

                if (actualShieldAbsorb > 0)
                    shield = $" Its magical shield absorbs {actualShieldAbsorb} damage.";
                if (actualArmorAbsorb > 0)
                    absorb = $" Its armor absorbs {actualArmorAbsorb} damage. ";

                if (actualDamage > actualShieldAbsorb + actualArmorAbsorb) //the enemy actually takes damage
                {
                    //the attacking weapon and item being hit take durability damage
                    itemGettingHit.CurrentDurability -= actualDamage / 10;
                    if (itemGettingHit.CurrentDurability < 0)
                        itemGettingHit.CurrentDurability = 0;
                    if (!castSpell && GameState.CurrentHero.Equipment.Weapon.Name != GameState.DefaultWeapon.Name)
                        GameState.CurrentHero.Equipment.Weapon.CurrentDurability -= actualArmorAbsorb / 10;
                    AddTextToTextBox($"You attack the {GameState.CurrentEnemy.Name} for {actualDamage} damage. {shield}{absorb}{GameState.CurrentEnemy.TakeDamage(actualDamage - actualShieldAbsorb - actualArmorAbsorb)}");
                    if (GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                    {
                        EndBattle(true);
                        AddTextToTextBox(GameState.CurrentHero.GainExperience(GameState.CurrentEnemy.Experience));
                    }
                }
                else if (actualShieldAbsorb > 0 && actualArmorAbsorb > 0)
                    AddTextToTextBox($"You attack the {GameState.CurrentEnemy.Name} for {actualDamage}, but {shield.ToLower()}{absorb.ToLower()}");
                else if (actualDamage <= actualShieldAbsorb)
                    AddTextToTextBox($"You attack the {GameState.CurrentEnemy.Name} for {actualDamage}, but its shield absorbed all of it.");
                else
                    AddTextToTextBox($"You attack the {GameState.CurrentEnemy.Name} for {actualDamage}, but its armor absorbed all of it.");
            }
            else
                AddTextToTextBox("You miss.");
        }

        /// <summary>Sets the <see cref="Enemy"/>'s action for the round.</summary>
        private void SetEnemyAction()
        {
            if (GameState.CurrentEnemy.Spellbook.Spells.Count > 0 && GameState.CurrentEnemy.Statistics.CurrentMagic > 0)
            {
                int action = Functions.GenerateRandomNumber(0, 1);
                _enemyAction = action == 0 ? BattleAction.Attack : BattleAction.Cast;
            }
            else
                _enemyAction = BattleAction.Attack;

            if (!_progress)
            {
                if (GameState.CurrentHero.Level - GameState.CurrentEnemy.Level >= 20)
                {
                    int flee = Functions.GenerateRandomNumber(1, 100);
                    if (flee <= 5)
                        _enemyAction = BattleAction.Flee;
                }
                else if (GameState.CurrentHero.Level - GameState.CurrentEnemy.Level >= 10)
                {
                    int flee = Functions.GenerateRandomNumber(1, 100);
                    if (flee <= 2)
                        _enemyAction = BattleAction.Flee;
                }

                if (_enemyAction != BattleAction.Flee)
                {
                    int result = Functions.GenerateRandomNumber(1, 100);

                    if (GameState.CurrentEnemy.Statistics.CurrentHealth > GameState.CurrentEnemy.Statistics.MaximumHealth / 5) //20% or more health, 2% chance to want to flee.
                        _enemyAction = result >= 98 ? BattleAction.Flee : _enemyAction;
                    else //20% or less health, 5% chance to want to flee.
                        _enemyAction = result >= 95 ? BattleAction.Flee : _enemyAction;
                }
            }
        }

        /// <summary>The <see cref="Enemy"/>'s turn this round of battle.</summary>
        private void EnemyTurn()
        {
            SetEnemyAction();
            switch (_enemyAction)
            {
                case BattleAction.Attack:
                    EnemyAttack();
                    break;

                case BattleAction.Cast:

                    List<Spell> availableSpells = GameState.CurrentEnemy.Spellbook.Spells.Where(spl => spl.MagicCost <= GameState.CurrentEnemy.Statistics.CurrentMagic).ToList();
                    List<Spell> healingSpells = availableSpells.Where(spl => spl.Type == SpellType.Healing).ToList();
                    List<Spell> attackSpells = availableSpells.Where(spl => spl.Type == SpellType.Damage).ToList();
                    List<Spell> shieldSpells = availableSpells.Where(spl => spl.Type == SpellType.Shield).ToList();

                    if (availableSpells.Count > 0)
                    {
                        // Choose Spell
                        if (attackSpells.Count > 0 && healingSpells.Count > 0 && GameState.CurrentEnemy.Statistics.HealthRatio < 0.5m)
                            GameState.CurrentEnemy.CurrentSpell = Functions.GenerateRandomNumber(0, 1) == 0
                                ? attackSpells[Functions.GenerateRandomNumber(0, attackSpells.Count - 1)]
                                : healingSpells[Functions.GenerateRandomNumber(0, healingSpells.Count - 1)];
                        else if (attackSpells.Count > 0 && shieldSpells.Count > 0 && EnemyShield > 0)
                            GameState.CurrentEnemy.CurrentSpell = attackSpells[Functions.GenerateRandomNumber(0, attackSpells.Count - 1)];
                        else if (attackSpells.Count > 0 && shieldSpells.Count > 0 && EnemyShield == 0)
                            GameState.CurrentEnemy.CurrentSpell = Functions.GenerateRandomNumber(0, 1) == 0
                                ? attackSpells[Functions.GenerateRandomNumber(0, attackSpells.Count - 1)]
                                : shieldSpells[Functions.GenerateRandomNumber(0, shieldSpells.Count - 1)];
                        else if (attackSpells.Count > 0)
                            GameState.CurrentEnemy.CurrentSpell = attackSpells[Functions.GenerateRandomNumber(0, attackSpells.Count - 1)];
                        else if (shieldSpells.Count > 0)
                            GameState.CurrentEnemy.CurrentSpell = shieldSpells[Functions.GenerateRandomNumber(0, shieldSpells.Count - 1)];
                        else if (healingSpells.Count > 0)
                            GameState.CurrentEnemy.CurrentSpell = healingSpells[Functions.GenerateRandomNumber(0, healingSpells.Count - 1)];

                        // Cast Spell
                        AddTextToTextBox($"The {GameState.CurrentEnemy.Name} casts {GameState.CurrentEnemy.CurrentSpell.Name}.");

                        switch (GameState.CurrentEnemy.CurrentSpell.Type)
                        {
                            case SpellType.Damage:
                                EnemyAttack(true);
                                break;

                            case SpellType.Healing:
                                AddTextToTextBox(GameState.CurrentEnemy.Heal(GameState.CurrentEnemy.CurrentSpell.Amount));
                                break;

                            case SpellType.Shield:
                                EnemyShield = GameState.CurrentEnemy.CurrentSpell.Amount;
                                AddTextToTextBox($"The {GameState.CurrentEnemy.Name} now has a magical shield which will help protect it from {EnemyShield} damage.");
                                break;
                        }

                        GameState.CurrentEnemy.Statistics.CurrentMagic -= GameState.CurrentEnemy.CurrentSpell.MagicCost;
                    }
                    else
                        EnemyAttack();
                    break;

                case BattleAction.Flee:
                    if (FleeAttempt(GameState.CurrentEnemy.TotalDexterity, GameState.CurrentHero.TotalDexterity))
                    {
                        EndBattle(false);
                        AddTextToTextBox($"The {GameState.CurrentEnemy.Name} fled from the battle.");
                        AddTextToTextBox(GameState.CurrentHero.GainExperience(GameState.CurrentEnemy.Experience / 2));
                    }
                    else
                        AddTextToTextBox($"You block the {GameState.CurrentEnemy.Name}'s attempt to flee.");
                    break;
            }
        }

        /// <summary>The <see cref="Enemy"/> attacks the <see cref="Hero"/>.</summary>
        private void EnemyAttack(bool castSpell = false)
        {
            //The enemy's chance to hit depends on whether or not they're overweight and their Dexterity compared to the Hero.
            int chanceEnemyHits = !GameState.CurrentEnemy.Overweight
                ? Functions.GenerateRandomNumber(50 + GameState.CurrentHero.Attributes.Dexterity - GameState.CurrentEnemy.Attributes.Dexterity, 90, 10, 90)
                : Functions.GenerateRandomNumber(Int32Helper.Parse(50 + GameState.CurrentHero.Attributes.Dexterity - (GameState.CurrentEnemy.Attributes.Dexterity * GameState.CurrentEnemy.StrengthWeightRatio)), 90, 10, 90);

            // If the number generated by this method call is less than the chance that the enemy hits, the enemy successfully hits.
            int success = Functions.GenerateRandomNumber(10, 90);

            if (success <= chanceEnemyHits)
            {
                // If successfully hitting, set up
                int statModifier = GetStatModifier(GameState.CurrentEnemy, castSpell);

                int damage = !castSpell
                    ? GameState.CurrentEnemy.Equipment.Weapon.Damage
                    : GameState.CurrentEnemy.CurrentSpell.Amount;

                // Maximum damage is 20% of the statistic used for their primary + damage from the weapon.
                int maximumDamage = Int32Helper.Parse((statModifier * 0.2) + damage);

                // If overweight, multiply by the strength/weight ratio.
                if (GameState.CurrentEnemy.Overweight)
                    maximumDamage = Int32Helper.Parse(maximumDamage * GameState.CurrentEnemy.StrengthWeightRatio);

                // Choose the Item which is going to be hit.
                Item itemGettingHit = ChooseItemToHit(GameState.CurrentHero.Equipment);

                // Actual defense is maximum defense multiplied by the current durability of the item being hit.
                // If there is no durability left, bypass it completely for determining defense.
                int heroDefense = itemGettingHit.CurrentDurability > 0 ? Int32Helper.Parse(itemGettingHit.Defense * itemGettingHit.DurabilityRatio) : 0;

                // Actual damage is 10-100% of the maximum damage.
                int actualDamage = Functions.GenerateRandomNumber(maximumDamage / 10, maximumDamage, 1);

                // Maximum shield absorb is 10-100% of the shield currently being attacked.
                int maximumShieldAbsorb = Functions.GenerateRandomNumber(HeroShield / 10, HeroShield);

                // Maximum armor absorb is 10-100% of the Item currently being attacked.
                int maximumArmorAbsorb = heroDefense > 0 ? Functions.GenerateRandomNumber(heroDefense / 10, heroDefense, 1) : 0;

                // Shield absorbs actualDamage up to maxShieldAbsorb.
                int actualShieldAbsorb = maximumShieldAbsorb >= actualDamage ? actualDamage : maximumShieldAbsorb;

                HeroShield -= actualShieldAbsorb;

                // If shield absorbs all damage, actualArmorAbsorb is 0, otherwise check actualDamage - actualShieldAbsorb.
                int actualArmorAbsorb = 0;
                if (actualShieldAbsorb < actualDamage)
                    actualArmorAbsorb = maximumArmorAbsorb >= actualDamage - actualShieldAbsorb ? actualDamage - actualShieldAbsorb : maximumArmorAbsorb;

                string absorb = "";
                string shield = "";

                if (actualShieldAbsorb > 0)
                    shield = $" Your magical shield absorbs {actualShieldAbsorb} damage.";
                if (actualArmorAbsorb > 0)
                    absorb = $" Your armor absorbs {actualArmorAbsorb} damage. ";

                if (actualDamage > (actualShieldAbsorb + actualArmorAbsorb)) //the player actually takes damage
                {
                    itemGettingHit.CurrentDurability -= actualDamage / 10;
                    if (itemGettingHit.CurrentDurability < 0)
                        itemGettingHit.CurrentDurability = 0;
                    if (!castSpell)
                        GameState.CurrentEnemy.Equipment.Weapon.CurrentDurability -= actualArmorAbsorb / 10;
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage} damage. {shield}{absorb}{GameState.CurrentHero.TakeDamage(actualDamage - actualShieldAbsorb - actualArmorAbsorb)}");
                }
                else if (actualShieldAbsorb > 0 && actualArmorAbsorb > 0)
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but {shield.ToLower()}{absorb.ToLower()}");
                else if (actualDamage <= actualShieldAbsorb)
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but your shield absorbed all of it.");
                else
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but your armor absorbed all of it.");
            }
            else
                AddTextToTextBox($"The {GameState.CurrentEnemy.Name} misses.");
        }

        private int GetStatModifier(Character character, bool castSpell) => !castSpell
                    ? character.Equipment.Weapon.Type == ItemType.MeleeWeapon
                        ? character.Attributes.Strength
                        : character.Attributes.Dexterity
                    : character.Attributes.Wisdom;

        /// <summary>Picks an <see cref="Item"/> to be targeted in an attack.</summary>
        /// <param name="equipment"><see cref="Equipment"/> where the items are to be picked from</param>
        /// <returns><see cref="Item"/> selected</returns>
        private Item ChooseItemToHit(Equipment equipment)
        {
            // 2% chance to hit rings
            // 10% chance to hit hands
            // 10% chance to hit feet
            // 11% chance to hit head
            // 10% chance to hit legs
            // 15% chance to hit weapon
            // 40% chance to hit body

            int item = Functions.GenerateRandomNumber(1, 100);
            if (item <= 2)
                return equipment.LeftRing;
            else if (item <= 4)
                return equipment.RightRing;
            else if (item <= 14)
                return equipment.Hands;
            else if (item <= 24)
                return equipment.Feet;
            else if (item <= 35)
                return equipment.Head;
            else if (item <= 45)
                return equipment.Legs;
            else if (item <= 60)
                return equipment.Weapon;
            else
                return equipment.Body;
        }

        /// <summary>Determines whether a flight attempt is successful.</summary>
        /// <param name="fleeAttemptDexterity">Whoever is attempting to flee's Dexterity</param>
        /// <param name="blockAttemptDexterity">Whoever is not attempting to flee's Dexterity</param>
        /// <returns>Returns true if the flight attempt is successful</returns>
        private static bool FleeAttempt(int fleeAttemptDexterity, int blockAttemptDexterity) => Functions.GenerateRandomNumber(1, 100) <= Functions.GenerateRandomNumber(1, 20 + fleeAttemptDexterity - blockAttemptDexterity, 1, 90);

        /// <summary>A fairy is summoned to resurrect the Hero.</summary>
        private void Death()
        {
            EndBattle(false);
            if (!GameState.CurrentHero.Hardcore)
            {
                // If you were killed by an animal, your equipment will take damage.
                // If you were killed by a human, you lose 5-10% of your gold, and up to one item with a value <= 200.
                string text = "";
                if (GameState.CurrentEnemy.Type == "Animal")
                {
                    List<Item> equipment = GameState.CurrentHero.Equipment.AllEquipment.FindAll(itm => itm != new Item());
                    int itemsDamaged = Functions.GenerateRandomNumber(1, equipment.Count);

                    if (itemsDamaged > 0)
                    {
                        for (int i = 0; i < itemsDamaged; i++)
                        {
                            Functions.GenerateRandomNumber(0, equipment.Count);
                            equipment[i].CurrentDurability -= Functions.GenerateRandomNumber(1, equipment[i].MaximumDurability / 2);
                            if (equipment[i].CurrentDurability < 0)
                                equipment[i].CurrentDurability = 0;
                        }
                    }
                    text = $"You have died. The {GameState.CurrentEnemy.Name} mauls your dead corpse and your equipment has taken damage. ";
                }
                else
                {
                    text = $"You have died. The {GameState.CurrentEnemy.Name} frisks your dead corpse and has stolen some of your gold. ";
                    List<Item> items = GameState.CurrentHero.Inventory.FindAll(itm => itm.Value <= 200);
                    Item itemStolen = new Item();
                    if (items.Count > 0)
                        itemStolen = items[Functions.GenerateRandomNumber(0, items.Count - 1)];
                    else
                    {
                        List<Item> equipment = GameState.CurrentHero.Equipment.AllEquipment.FindAll(itm => itm != new Item() && itm.Value <= 200);
                        if (equipment.Count > 0)
                        {
                            itemStolen = equipment[Functions.GenerateRandomNumber(0, equipment.Count - 1)];
                            if (itemStolen != null && itemStolen != new Item())
                                GameState.CurrentHero.Unequip(itemStolen);
                        }
                    }

                    if (itemStolen != null && itemStolen != new Item())
                    {
                        GameState.CurrentHero.RemoveItem(itemStolen);
                        text += $"Your {itemStolen.Name} has also been stolen. ";
                    }

                    GameState.CurrentHero.Gold -= GameState.CurrentHero.Gold / 10;
                }
                AddTextToTextBox(text + "A mysterious fairy appears, and, seeing your crumpled body on the ground, resurrects you. You have just enough health to make it back to town.");
                GameState.CurrentHero.Statistics.CurrentHealth = 1;
            }
            else
            {
                AddTextToTextBox("Your Hardcore Hero has been killed. This character will be deleted when you click Return.");
                _blnHardcoreDeath = true;
            }
        }

        #endregion Battle Logic

        #region Button Management

        /// <summary>Checks whether to enable/disable battle buttons.</summary>
        private void CheckButtons() => ToggleButtons(BattleEnded || GameState.CurrentEnemy.Statistics.CurrentHealth <= 0);

        /// <summary>Toggles whether the Page's Buttons are disabled.</summary>
        /// <param name="disabled">Are the buttons disabled?</param>
        private void ToggleButtons(bool disabled)
        {
            BtnAttack.Disabled = disabled;
            BtnCastSpell.Disabled = GameState.CurrentHero.CurrentSpell == null || GameState.CurrentHero.CurrentSpell == new Spell() ? true : disabled;
            BtnFlee.Disabled = disabled;
            if (BattleEnded && GameState.CurrentEnemy.Statistics.CurrentHealth > 0)
            {
                BtnEnemyDetails.Disabled = true;
                BtnLootBody.Disabled = true;
            }
        }

        #endregion Button Management

        #region Button-Click Methods

        private void _on_BtnAttack_pressed() => NewRound(BattleAction.Attack);

        private void _on_BtnCastSpell_pressed() => NewRound(BattleAction.Cast);

        private void _on_BtnEnemyDetails_pressed()
        {
            GameState.AddSceneToHistory(GetTree().CurrentScene);
            GameState.BattleScene = this;
            GetTree().ChangeScene("res://scenes/battle/EnemyDetailsScene.tscn");
        }

        private void _on_BtnFlee_pressed() => NewRound(BattleAction.Flee);

        private void _on_BtnLootBody_pressed()
        {
            GameState.AddSceneToHistory(GetTree().CurrentScene);
            GameState.BattleScene = this;
            GetTree().ChangeScene("res://scenes/battle/LootBodyScene.tscn");
        }

        private void _on_BtnReturn_pressed()
        {
            //if the battle is over, return to where you came from
            //if you were fight a progression battle and you killed the enemy,
            //add progression to the character and set to display after progression screens

            // TODO Implement Progression
            if (BattleEnded)
            {
                GameState.BattleScene = null;
                switch (_previousPage)
                {
                    case "Fields":
                        if (_progress && GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                        {
                            GameState.CurrentHero.Progression.Fields = true;
                            GameState.EventFindGold(250, 250);
                        }
                        break;

                    case "Forest":
                        if (_progress && GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                        {
                            GameState.CurrentHero.Progression.Forest = true;
                            GameState.EventFindGold(1000, 1000);
                        }
                        break;

                    case "Cathedral":
                        if (_progress && GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                        {
                            GameState.CurrentHero.Progression.Cathedral = true;
                            GameState.EventFindItem(1, 5000, ItemType.Ring);
                        }
                        break;

                    case "Mines":
                        if (_progress && GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                        {
                            GameState.CurrentHero.Progression.Mines = true;
                            GameState.EventFindGold(5000, 5000);
                        }
                        break;

                    case "Catacombs":
                        if (_progress && GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                        {
                            GameState.CurrentHero.Progression.Catacombs = true;
                            GameState.EventFindItem(15000, 20000, ItemType.Ring);
                        }
                        break;
                }
                if (_blnHardcoreDeath)
                {
                    GetTree().ChangeScene("res://scenes/MainScene.tscn");
                    GameState.DeleteHero(GameState.CurrentHero);
                    GameState.CurrentHero = new Hero();
                    GameState.History = new List<PackedScene>();
                }
                else
                    GetTree().ChangeSceneTo(GameState.GoBack());
            }
        }

        private void _on_LstSpells_item_selected(int index)
        {
            if (index >= 0)
                GameState.CurrentHero.CurrentSpell = GameState.CurrentHero.Spellbook.Spells[index];
            UpdateStats();
        }

        #endregion Button-Click Methods

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //
        //  }
    }
}