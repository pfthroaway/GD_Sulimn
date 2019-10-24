using Godot;
using Sulimn.Classes.Database;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sulimn.Classes
{
    /// <summary>Represents the current state of the game.</summary>
    internal static class GameState
    {
        internal static Hero CurrentHero = new Hero();

        internal static Enemy CurrentEnemy = new Enemy();
        internal static List<Enemy> AllEnemies = new List<Enemy>();
        internal static List<Item> AllItems = new List<Item>();
        internal static List<HeadArmor> AllHeadArmor = new List<HeadArmor>();
        internal static List<BodyArmor> AllBodyArmor = new List<BodyArmor>();
        internal static List<HandArmor> AllHandArmor = new List<HandArmor>();
        internal static List<LegArmor> AllLegArmor = new List<LegArmor>();
        internal static List<FeetArmor> AllFeetArmor = new List<FeetArmor>();
        internal static List<Ring> AllRings = new List<Ring>();
        internal static List<Weapon> AllWeapons = new List<Weapon>();
        internal static List<Food> AllFood = new List<Food>();
        internal static List<Drink> AllDrinks = new List<Drink>();
        internal static List<Potion> AllPotions = new List<Potion>();
        internal static List<Spell> AllSpells = new List<Spell>();
        internal static List<Hero> AllHeroes = new List<Hero>();
        internal static List<HeroClass> AllClasses = new List<HeroClass>();
        internal static Weapon DefaultWeapon = new Weapon();
        internal static HeadArmor DefaultHead = new HeadArmor();
        internal static BodyArmor DefaultBody = new BodyArmor();
        internal static HandArmor DefaultHands = new HandArmor();
        internal static LegArmor DefaultLegs = new LegArmor();
        internal static FeetArmor DefaultFeet = new FeetArmor();

        /// <summary>Determines whether a Hero's credentials are authentic.</summary>
        /// <param name="username">Hero's name</param>
        /// <param name="password">Hero's password</param>
        /// <returns>Returns true if valid login</returns>
        internal static bool CheckLogin(string username, string password)
        {
            Hero checkHero = AllHeroes.Find(hero => string.Equals(hero.Name, username, StringComparison.InvariantCultureIgnoreCase));
            if (checkHero != null && checkHero != new Hero() && PBKDF2.ValidatePassword(password, checkHero.Password))
            {
                CurrentHero = checkHero;
                return true;
            }
            return false;
        }

        /// <summary>Loads almost everything from the database.</summary>
        internal static void LoadAll()
        {
            AllClasses = JSONInteraction.LoadClasses().OrderBy(o => o.Name).ToList();
            AllHeadArmor = JSONInteraction.LoadArmor<HeadArmor>("head").OrderBy(o => o.Name).ToList();
            AllBodyArmor = JSONInteraction.LoadArmor<BodyArmor>("body").OrderBy(o => o.Name).ToList();
            AllHandArmor = JSONInteraction.LoadArmor<HandArmor>("hand").OrderBy(o => o.Name).ToList();
            AllLegArmor = JSONInteraction.LoadArmor<LegArmor>("leg").OrderBy(o => o.Name).ToList();
            AllFeetArmor = JSONInteraction.LoadArmor<FeetArmor>("feet").OrderBy(o => o.Name).ToList();
            AllRings = JSONInteraction.LoadRings().OrderBy(o => o.Name).ToList();
            AllWeapons = JSONInteraction.LoadWeapons().OrderBy(o => o.Name).ToList();
            AllDrinks = JSONInteraction.LoadDrinks().OrderBy(o => o.Name).ToList();
            AllFood = JSONInteraction.LoadFood().OrderBy(o => o.Name).ToList();
            AllPotions = JSONInteraction.LoadPotions().OrderBy(o => o.Name).ToList();
            AllSpells = JSONInteraction.LoadSpells().OrderBy(o => o.Name).ToList();
            AllEnemies = JSONInteraction.LoadEnemies().OrderBy(o => o.Name).ToList();

            //JSONInteraction.WriteAll(AllClasses, AllHeadArmor, AllBodyArmor, AllHandArmor, AllLegArmor, AllFeetArmor, AllRings, AllWeapons, AllDrinks, AllFood, AllPotions, AllSpells, AllEnemies);

            AllItems.AddRanges(AllHeadArmor, AllBodyArmor, AllHandArmor, AllLegArmor, AllFeetArmor, AllRings, AllFood, AllDrinks, AllPotions, AllWeapons);

            DefaultWeapon = AllWeapons.Find(weapon => weapon.Name == "Fists");
            DefaultHead = AllHeadArmor.Find(armor => armor.Name == "Cloth Helmet");
            DefaultBody = AllBodyArmor.Find(armor => armor.Name == "Cloth Shirt");
            DefaultHands = AllHandArmor.Find(armor => armor.Name == "Cloth Gloves");
            DefaultLegs = AllLegArmor.Find(armor => armor.Name == "Cloth Pants");
            DefaultFeet = AllFeetArmor.Find(armor => armor.Name == "Cloth Shoes");

            AllHeroes = JSONInteraction.LoadHeroes().OrderBy(o => o.Name).ToList();
        }

        /// <summary>Gets a specific Enemy based on its name.</summary>
        /// <param name="name">Name of Enemy</param>
        /// <returns>Enemy</returns>
        private static Enemy GetEnemy(string name) => new Enemy(AllEnemies.Find(enemy => enemy.Name == name));

        #region Item Management

        /// <summary>Gets a specific Item based on its name.</summary>
        /// <param name="name">Item name</param>
        /// <returns>Item</returns>
        private static Item GetItem(string name) => AllItems.Find(itm => itm.Name == name);

        /// <summary>Retrieves a List of all Items of specified Type.</summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>List of specified Type.</returns>
        public static List<T> GetItemsOfType<T>() => AllItems.OfType<T>().ToList();

        #endregion Item Management

        #region Hero Management

        /// <summary>Modifies a Hero's details in the database.</summary>
        /// <param name="oldHero">Hero whose details need to be modified</param>
        /// <param name="newHero">Hero with new details</param>
        /// <returns>True if successful</returns>
        internal static void ChangeHeroDetails(Hero oldHero, Hero newHero)
        {
            //JSONInteraction.ChangeHeroDetails(oldHero, newHero);
            AllHeroes.Replace(oldHero, newHero);
        }

        /// <summary>Deletes a Hero from the game and database.</summary>
        /// <param name="deleteHero">Hero to be deleted</param>
        /// <returns>Whether deletion was successful</returns>
        internal static void DeleteHero(Hero deleteHero)
        {
            //JSONInteraction.DeleteHero(deleteHero);
            AllHeroes.Remove(deleteHero);
        }

        /// <summary>Creates a new Hero and adds it to the database.</summary>
        /// <param name="newHero">New Hero</param>
        internal static void NewHero(Hero newHero)
        {
            if (newHero.Equipment.Head == null || newHero.Equipment.Head == new HeadArmor())
                newHero.Equipment.Head = AllHeadArmor.Find(armor => armor.Name == DefaultHead.Name);
            if (newHero.Equipment.Body == null || newHero.Equipment.Body == new BodyArmor())
                newHero.Equipment.Body = AllBodyArmor.Find(armor => armor.Name == DefaultBody.Name);
            if (newHero.Equipment.Hands == null || newHero.Equipment.Hands == new HandArmor())
                newHero.Equipment.Hands = AllHandArmor.Find(armor => armor.Name == DefaultHands.Name);
            if (newHero.Equipment.Legs == null || newHero.Equipment.Legs == new LegArmor())
                newHero.Equipment.Legs = AllLegArmor.Find(armor => armor.Name == DefaultLegs.Name);
            if (newHero.Equipment.Feet == null || newHero.Equipment.Feet == new FeetArmor())
                newHero.Equipment.Feet = AllFeetArmor.Find(armor => armor.Name == DefaultFeet.Name);
            if (newHero.Equipment.Weapon == null || newHero.Equipment.Weapon == new Weapon())
            {
                switch (newHero.Class.Name)
                {
                    case "Wizard":
                        newHero.Equipment.Weapon = AllWeapons.Find(wpn => wpn.Name == "Starter Staff");
                        if (newHero.Spellbook == null || newHero.Spellbook == new Spellbook())
                            newHero.Spellbook?.LearnSpell(AllSpells.Find(spell => spell.Name == "Fire Bolt"));
                        break;

                    case "Cleric":
                        newHero.Equipment.Weapon = AllWeapons.Find(wpn => wpn.Name == "Starter Staff");
                        if (newHero.Spellbook == null || newHero.Spellbook == new Spellbook())
                            newHero.Spellbook?.LearnSpell(AllSpells.Find(spell => spell.Name == "Heal Self"));
                        break;

                    case "Warrior":
                        newHero.Equipment.Weapon = AllWeapons.Find(wpn => wpn.Name == "Stone Dagger");
                        break;

                    case "Rogue":
                        newHero.Equipment.Weapon = AllWeapons.Find(wpn => wpn.Name == "Starter Bow");
                        break;

                    default:
                        newHero.Equipment.Weapon = AllWeapons.Find(wpn => wpn.Name == "Stone Dagger");
                        break;
                }
            }

            newHero.Gold = 250;
            for (int i = 0; i < 3; i++)
                newHero.AddItem(AllPotions.Find(itm => itm.Name == "Minor Healing Potion"));

            JSONInteraction.SaveHero(newHero);
            AllHeroes.Add(newHero);
        }

        /// <summary>Saves Hero to database.</summary>
        /// <param name="saveHero">Hero to be saved</param>
        /// <returns>Returns true if successfully saved</returns>
        internal static bool SaveHero(Hero saveHero)
        {
            JSONInteraction.SaveHero(saveHero);

            int index = AllHeroes.FindIndex(hero => hero.Name == saveHero.Name);
            AllHeroes[index] = saveHero;

            return true;
        }

        #endregion Hero Management

        #region Exploration Events

        /// <summary>Event where the Hero finds gold.</summary>
        /// <param name="minGold">Minimum amount of gold to be found</param>
        /// <param name="maxGold">Maximum amount of gold to be found</param>
        /// <returns>Returns text regarding gold found</returns>
        internal static string EventFindGold(int minGold, int maxGold)
        {
            int foundGold = minGold == maxGold ? minGold : Functions.GenerateRandomNumber(minGold, maxGold);
            CurrentHero.Gold += foundGold;
            SaveHero(CurrentHero);
            return $"You find {foundGold:N0} gold!";
        }

        /// <summary>Event where the Hero finds an item.</summary>
        /// <param name="minValue">Minimum value of Item</param>
        /// <param name="maxValue">Maximum value of Item</param>
        /// <param name="isSold">Is the item sold?</param>
        /// <returns>Returns text about found Item</returns>
        internal static string EventFindItem(int minValue, int maxValue, bool isSold = true)
        {
            List<Item> availableItems = AllItems.Where(itm => itm.Value >= minValue && itm.Value <= maxValue && itm.IsSold == isSold).ToList();
            int item = Functions.GenerateRandomNumber(0, availableItems.Count - 1);

            CurrentHero.AddItem(availableItems[item]);
            SaveHero(CurrentHero);
            return $"You find a {availableItems[item].Name}!";
        }

        /// <summary>Event where the Hero finds an item.</summary>
        /// <param name="names">List of names of available Items</param>
        /// <returns>Returns text about found Item</returns>
        internal static string EventFindItem(params string[] names)
        {
            List<Item> availableItems = new List<Item>();
            foreach (string name in names)
                availableItems.Add(GetItem(name));
            int item = Functions.GenerateRandomNumber(0, availableItems.Count - 1);

            CurrentHero.AddItem(availableItems[item]);

            SaveHero(CurrentHero);
            return $"You find a {availableItems[item].Name}!";
        }

        /// <summary>Event where the Hero finds an item.</summary>
        /// <typeparam name="T">Type of Item to be found</typeparam>
        /// <param name="minValue">Minimum value of Item</param>
        /// <param name="maxValue">Maximum value of Item</param>
        /// <param name="isSold">Is the item sold?</param>
        /// <returns>Returns text about found Item</returns>
        internal static string EventFindItem<T>(int minValue, int maxValue, bool isSold = true) where T : Item
        {
            List<T> availableItems = new List<T>(GetItemsOfType<T>());
            availableItems = availableItems.FindAll(itm => itm.Value >= minValue && itm.Value <= maxValue && itm.IsSold == isSold).ToList();
            int item = Functions.GenerateRandomNumber(0, availableItems.Count - 1);
            CurrentHero.AddItem(availableItems[item]);
            SaveHero(CurrentHero);
            return $"You find a {availableItems[item].Name}!";
        }

        /// <summary>Event where the Hero encounters a hostile animal.</summary>
        /// <param name="minLevel">Minimum level of animal</param>
        /// <param name="maxLevel">Maximum level of animal</param>
        internal static void EventEncounterAnimal(int minLevel, int maxLevel) => EventEncounterEnemy(AllEnemies.Where(enemy => enemy.Level >= minLevel && enemy.Level <= maxLevel && enemy.Type == "Animal").ToList());

        /// <summary>Event where the Hero encounters a hostile Enemy.</summary>
        /// <param name="minLevel">Minimum level of Enemy.</param>
        /// <param name="maxLevel">Maximum level of Enemy.</param>
        internal static void EventEncounterEnemy(int minLevel, int maxLevel) => EventEncounterEnemy(AllEnemies.Where(enemy => enemy.Level >= minLevel && enemy.Level <= maxLevel && enemy.Type != "Boss").ToList());

        /// <summary>Event where the Hero encounters a hostile Enemy.</summary>
        /// <param name="names">Array of names</param>
        internal static void EventEncounterEnemy(params string[] names) =>
            EventEncounterEnemy(names.Select(GetEnemy).ToList());

        internal static void EventEncounterEnemy(List<Enemy> availableEnemies)
        {
            int enemyNum = Functions.GenerateRandomNumber(0, availableEnemies.Count - 1);
            CurrentEnemy = new Enemy(availableEnemies[enemyNum]);
            if (CurrentEnemy.Gold > 0)
                CurrentEnemy.Gold = Functions.GenerateRandomNumber(CurrentEnemy.Gold / 2, CurrentEnemy.Gold);
        }

        /// <summary>Event where the Hero encounters a water stream and restores health and magic.</summary>
        /// <returns>String saying Hero has been healed</returns>
        internal static string EventEncounterStream()
        {
            CurrentHero.Statistics.CurrentHealth = CurrentHero.Statistics.MaximumHealth;
            CurrentHero.Statistics.CurrentMagic = CurrentHero.Statistics.MaximumMagic;

            return
            "You stumble across a stream. You stop to drink some of the water and rest a while. You feel recharged!";
        }

        #endregion Exploration Events
    }
}