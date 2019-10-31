using Godot;
using Sulimn.Classes;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Enums;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;

public class BattleScene : Control
{
    private BattleAction _enemyAction, _heroAction;
    private bool _battleEnded;
    private bool _blnHardcoreDeath;
    private bool _progress;
    private Button BtnCastSpell;
    private Info info;
    private string _previousPage;
    private TextEdit TxtBattle;

    #region Modifying Properties

    /// <summary>The <see cref="Hero"/>'s currently selected <see cref="Spell"/>.</summary>
    public Spell CurrentHeroSpell { get; set; }

    /// <summary>The <see cref="Enemy"/>'s currently selected <see cref="Spell"/>.</summary>
    public Spell CurrentEnemySpell { get; set; }

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AssignControls();
        UpdateStats();
    }

    /// <summary>Assigns all controls to modifiable values.</summary>
    private void AssignControls()
    {
        info = (Info)GetNode("/root/Info");
        BtnCastSpell = null;
        TxtBattle = null;
    }

    /// <summary>Update labels to current values.</summary>
    private void UpdateStats()
    {
        info.DisplayStats();
    }

    #endregion Load

    /// <summary>Adds text to the TxtBattle TextEdit.</summary>
    /// <param name="newText">Text to be added</param>
    private void AddTextToTextBox(string newText)
    {
        TxtBattle.Text += "\n\n" + newText;
        TxtBattle.CursorSetLine(TxtBattle.GetLineCount());
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
        //_previousPage = prevPage;
        _progress = progress;
        TxtBattle.Text = progress ? $"{GameState.CurrentEnemy.Name} rushes at you in the {_previousPage}. You defend yourself. " : $"You encounter an enemy. The {GameState.CurrentEnemy.Name} seems openly hostile to you. Prepare to defend yourself.";
    }

    /// <summary>Ends the battle and allows the user to exit the Page.</summary>
    private void EndBattle()
    {
        _battleEnded = true;
        //BtnReturn.IsEnabled = true;
    }

    #endregion Battle Management

    #region Battle Logic

    /// <summary>Starts a new round of battle.</summary>
    /// <param name="heroAction">Action the hero chose to perform this round</param>
    private void NewRound(BattleAction heroAction)
    {
        ToggleButtons(false);
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
            if (GameState.CurrentEnemy.Statistics.CurrentHealth > 0 && !_battleEnded)
            {
                EnemyTurn();
                if (GameState.CurrentHero.Statistics.CurrentHealth <= 0)
                    Death();
            }
        }
        else
        {
            EnemyTurn();
            if (GameState.CurrentHero.Statistics.CurrentHealth > 0 && !_battleEnded)
                HeroTurn();
            else if (GameState.CurrentHero.Statistics.CurrentHealth <= 0)
                Death();
        }

        CheckButtons();
    }

    /// <summary>The Hero's turn this round of battle.</summary>
    private void HeroTurn()
    {
        switch (_heroAction)
        {
            case BattleAction.Attack:
                if (GameState.CurrentHero.Equipment.Weapon.Type == ItemType.MeleeWeapon)
                    HeroAttack(GameState.CurrentHero.TotalStrength, GameState.CurrentHero.Equipment.TotalDamage);
                else if (GameState.CurrentHero.Equipment.Weapon.Type == ItemType.RangedWeapon)
                    HeroAttack(GameState.CurrentHero.TotalDexterity, GameState.CurrentHero.Equipment.TotalDamage);
                break;

            case BattleAction.Cast:

                AddTextToTextBox($"You cast {CurrentHeroSpell.Name}.");

                switch (CurrentHeroSpell.Type)
                {
                    case SpellType.Damage:
                        HeroAttack(GameState.CurrentHero.TotalWisdom, CurrentHeroSpell.Amount);
                        break;

                    case SpellType.Healing:
                        AddTextToTextBox(GameState.CurrentHero.Heal(CurrentHeroSpell.Amount));
                        break;

                    case SpellType.Shield:
                        HeroShield += CurrentHeroSpell.Amount;
                        AddTextToTextBox($"You now have a magical shield which will help protect you from {HeroShield} damage.");
                        break;
                }

                GameState.CurrentHero.Statistics.CurrentMagic -= CurrentHeroSpell.MagicCost;
                break;

            case BattleAction.Flee:

                if (FleeAttempt(GameState.CurrentHero.TotalDexterity, GameState.CurrentEnemy.TotalDexterity))
                {
                    EndBattle();
                    AddTextToTextBox($"You successfully fled from the {GameState.CurrentEnemy.Name}.");
                }
                else
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} blocked your attempt to flee.");
                break;
        }
    }

    /// <summary>The Hero attacks the Enemy.</summary>
    /// <param name="statModifier">Stat to be given 20% bonus to damage</param>
    /// <param name="damage">Damage</param>
    private void HeroAttack(int statModifier, int damage)
    {
        //TODO Implement Hero attacking the same way I did Enemy attacking.

        int chanceHeroHits =
        Functions.GenerateRandomNumber(
        50 + GameState.CurrentHero.Attributes.Dexterity - GameState.CurrentEnemy.Attributes.Dexterity, 90,
        10, 90);
        int heroHits = Functions.GenerateRandomNumber(10, 90);

        if (heroHits <= chanceHeroHits)
        {
            int maximumHeroDamage = Int32Helper.Parse((statModifier * 0.2) + damage);
            int maximumEnemyAbsorb = GameState.CurrentEnemy.Equipment.TotalDefense;
            int actualDamage = Functions.GenerateRandomNumber(maximumHeroDamage / 10, maximumHeroDamage, 1);
            int actualAbsorb = Functions.GenerateRandomNumber(maximumEnemyAbsorb / 10, maximumEnemyAbsorb);

            string absorb = "";
            if (actualAbsorb > 0)
                absorb = $"The {GameState.CurrentEnemy.Name}'s armor absorbed {actualAbsorb} damage. ";

            if (actualDamage > actualAbsorb)
            {
                AddTextToTextBox($"You attack for {actualDamage} damage. {absorb}{GameState.CurrentEnemy.TakeDamage(actualDamage - actualAbsorb)}");
                if (GameState.CurrentEnemy.Statistics.CurrentHealth <= 0)
                {
                    EndBattle();
                    AddTextToTextBox(GameState.CurrentHero.GainExperience(GameState.CurrentEnemy.Experience));
                    if (GameState.CurrentEnemy.Gold > 0)
                        AddTextToTextBox($"You find {GameState.CurrentEnemy.Gold} gold on the body.");

                    GameState.CurrentHero.Gold += GameState.CurrentEnemy.Gold;
                }
            }
            else
                AddTextToTextBox($"You attack for {actualDamage}, but its armor absorbs all of it.");
        }
        else
            AddTextToTextBox("You miss.");
    }

    /// <summary>Sets the current Spell.</summary>
    /// <param name="spell">Spell to be set</param>
    internal void SetSpell(Spell spell)
    {
        CurrentHeroSpell = spell;
        BtnCastSpell.Disabled = false;
    }

    /// <summary>Sets the Enemy's action for the round.</summary>
    private void SetEnemyAction()
    {
        if (GameState.CurrentEnemy.Spellbook.Spells.Count > 0)
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

    /// <summary>The Enemy's turn this round of battle.</summary>
    private void EnemyTurn()
    {
        SetEnemyAction();
        switch (_enemyAction)
        {
            case BattleAction.Attack:
                EnemyAttack();
                break;

            case BattleAction.Cast:
                //TODO Implement the ability for Enemies to cast spells.
                break;

            case BattleAction.Flee:
                if (FleeAttempt(GameState.CurrentEnemy.TotalDexterity, GameState.CurrentHero.TotalDexterity))
                {
                    EndBattle();
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} fled from the battle.");
                    AddTextToTextBox(GameState.CurrentHero.GainExperience(GameState.CurrentEnemy.Experience / 2));
                }
                //else
                AddTextToTextBox($"You block the {GameState.CurrentEnemy.Name}'s attempt to flee.");
                break;
        }
    }

    /// <summary>The Enemy attacks the Hero.</summary>
    private void EnemyAttack(bool castSpell = false)
    {
        // TODO I'm rewriting the attack methods to implement doing damage to an Item's durability when struck with a weapon or spell.
        // TODO I'm also implementing an overweight debuff to hit chances with weapons and spells and damage output when using a weapon.
        // TODO Make it possible to loot enemy corpses instead of automatically giving the Hero gold.

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
                : CurrentEnemySpell.Amount;

            // Maximum damage is 20% of the statistic used for their primary + damage from the weapon.
            int maximumDamage = Int32Helper.Parse((statModifier * 0.2) + damage);

            // If overweight, multiply by the strength/weight ratio.
            if (GameState.CurrentEnemy.Overweight)
                maximumDamage = Int32Helper.Parse(maximumDamage * GameState.CurrentEnemy.StrengthWeightRatio);

            // Choose the Item which is going to be hit.
            Item itemGettingHit = ChooseItemToHit(GameState.CurrentHero.Equipment);

            // Actual defense is maximum defense multiplied by the current durability of the item being hit.
            int heroDefense = itemGettingHit.CurrentDurability > 0 ? Int32Helper.Parse(itemGettingHit.Defense * itemGettingHit.DurabilityRatio) : 0;

            // Actual damage is 10-100% of the maximum damage.
            int actualDamage = Functions.GenerateRandomNumber(maximumDamage / 10, maximumDamage, 1);

            // Maximum shield absorb is 10-100% of the shield currently being attacked.
            int maximumShieldAbsorb = Functions.GenerateRandomNumber(HeroShield / 10, HeroShield);

            // Maximum armor absorb is 10-100% of the Item currently being attacked.
            int maximumArmorAbsorb = Functions.GenerateRandomNumber(heroDefense / 10, heroDefense);

            int actualArmorAbsorb = 0;

            // Shield absorbs actualDamage up to maxShieldAbsorb.
            int actualShieldAbsorb = maximumShieldAbsorb >= actualDamage ? actualDamage : maximumShieldAbsorb;

            HeroShield -= actualShieldAbsorb;

            // If shield absorbs all damage, actualArmorAbsorb is 0, otherwise check actualDamage - maxShieldAbsorb.
            if (actualShieldAbsorb < actualDamage)
                if (maximumArmorAbsorb >= actualDamage - actualShieldAbsorb)
                    actualArmorAbsorb = actualDamage - actualShieldAbsorb;
                else
                    actualArmorAbsorb = maximumArmorAbsorb;

            string absorb = "";
            string shield = "";

            if (actualShieldAbsorb > 0)
                shield = $" Your magical shield absorbs {actualShieldAbsorb} damage.";
            if (actualArmorAbsorb > 0)
                absorb = $" Your armor absorbs {actualArmorAbsorb} damage. ";

            if (actualDamage > actualShieldAbsorb + actualArmorAbsorb) //the player actually takes damage
            {
                itemGettingHit.CurrentDurability -= actualDamage / 10;
                if (!castSpell)
                    GameState.CurrentEnemy.Equipment.Weapon.CurrentDurability -= actualArmorAbsorb / 10;
                AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage} damage. {shield}{absorb}{GameState.CurrentHero.TakeDamage(actualDamage - actualShieldAbsorb - actualArmorAbsorb)}");
            }
            else
            {
                if (actualShieldAbsorb > 0 && actualArmorAbsorb > 0)
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but {shield.ToLower()}{absorb.ToLower()}");
                else if (actualDamage == actualShieldAbsorb)
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but your shield absorbed all of it.");
                else
                    AddTextToTextBox($"The {GameState.CurrentEnemy.Name} attacks you for {actualDamage}, but your armor absorbed all of it.");
            }
        }
        //else
        AddTextToTextBox($"The {GameState.CurrentEnemy.Name} misses.");
    }

    private int GetStatModifier(Character character, bool castSpell) => !castSpell
                ? character.Equipment.Weapon.Type == ItemType.MeleeWeapon
                    ? character.Attributes.Strength
                    : character.Attributes.Dexterity
                : character.Attributes.Wisdom;

    /// <summary>Picks an <see cref="Item"/> to be targeted in an attack.</summary>
    /// <param name="equipment"><see cref="Equipment"/> where the items are to be picked from</param>
    /// <returns>Item selected</returns>
    private Item ChooseItemToHit(Equipment equipment)
    {
        switch (Functions.GenerateRandomNumber(1, 8))
        {
            case 1:
                return equipment.Weapon;

            case 2:
                return equipment.Head;

            case 3:
                return equipment.Body;

            case 4:
                return equipment.Hands;

            case 5:
                return equipment.Legs;

            case 6:
                return equipment.Feet;

            case 7:
                return equipment.LeftRing;

            case 8:
                return equipment.RightRing;

            default:
                return equipment.Body;
        }
    }

    /// <summary>Determines whether a flight attempt is successful.</summary>
    /// <param name="fleeAttemptDexterity">Whoever is attempting to flee's Dexterity</param>
    /// <param name="blockAttemptDexterity">Whoever is not attempting to flee's Dexterity</param>
    /// <returns>Returns true if the flight attempt is successful</returns>
    private static bool FleeAttempt(int fleeAttemptDexterity, int blockAttemptDexterity) => Functions.GenerateRandomNumber(1, 100) <= Functions.GenerateRandomNumber(1, 20 + fleeAttemptDexterity - blockAttemptDexterity, 1, 90);

    /// <summary>A fairy is summoned to resurrect the Hero.</summary>
    private void Death()
    {
        EndBattle();
        if (!GameState.CurrentHero.Hardcore)
        {
            AddTextToTextBox("A mysterious fairy appears, and, seeing your crumpled body on the ground, resurrects you. You have just enough health to make it back to town.");
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
    private void CheckButtons() => ToggleButtons(!_battleEnded);

    /// <summary>Toggles whether the Page's Buttons are enabled.</summary>
    /// <param name="enabled">Are the buttons enabled?</param>
    private void ToggleButtons(bool enabled)
    {
        //BtnAttack.IsEnabled = !enabled;
        BtnCastSpell.Disabled = !enabled && CurrentHeroSpell != new Spell();
        //BtnChooseSpell.IsEnabled = !enabled;
        //BtnFlee.IsEnabled = !enabled;
    }

    #endregion Button Management

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}