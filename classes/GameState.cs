using Godot;
using Sulimn.Actors;
using Sulimn.Classes.Database;
using Sulimn.Classes.Entities;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using Sulimn.Scenes;
using Sulimn.Scenes.Battle;
using Sulimn.Scenes.Exploration;
using Sulimn.Scenes.Inventory;
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
        internal static List<Item> AllHeadArmor = new List<Item>();
        internal static List<Item> AllBodyArmor = new List<Item>();
        internal static List<Item> AllHandArmor = new List<Item>();
        internal static List<Item> AllLegArmor = new List<Item>();
        internal static List<Item> AllFeetArmor = new List<Item>();
        internal static List<Item> AllRings = new List<Item>();
        internal static List<Item> AllWeapons = new List<Item>();
        internal static List<Item> AllFood = new List<Item>();
        internal static List<Item> AllDrinks = new List<Item>();
        internal static List<Item> AllPotions = new List<Item>();
        internal static List<Item> MerchantInventory = new List<Item>();
        internal static List<Spell> AllSpells = new List<Spell>();
        internal static List<Hero> AllHeroes = new List<Hero>();
        internal static List<HeroClass> AllClasses = new List<HeroClass>();
        internal static Item DefaultWeapon = new Item();
        internal static Item DefaultHead = new Item();
        internal static Item DefaultBody = new Item();
        internal static Item DefaultHands = new Item();
        internal static Item DefaultLegs = new Item();
        internal static Item DefaultFeet = new Item();

        internal static BattleScene BattleScene;
        internal static string PreviousScene;
        internal static List<PackedScene> History = new List<PackedScene>();
        internal static bool UpdateDisplay { get; set; }
        internal static Info Info { get; set; }

        internal static SceneTree SceneTree { get; set; }
        internal static MainLoop MainLoop { get; set; }

        // TODO Add Tavern
        // TODO Basic quests like go kill 5 wolves for 500 gold from tavern.
        // TODO Make dude walk around tavern. Go up to bar, go to quests tables, blackjack, etc.
        // TODO Gear and ? Buttons on the info bar
        // TODO Set up textures for enemies and players. Use default until getting new.
        // TODO Random generation rooms top left corner of room 1, add stairs entity which calls GameState.GoBack(). Add Player entity one tile to the right.
        // TODO Find a way to get the scenes' variables' values to save. Why does The RichTextLabel keep it's value on leaving and coming back? Public vs private?
        // TODO Player/Enemy editor. Slot interface for editing items one at a time.

        #region Scene Navigation

        /// <summary>Adds the current scene to the history.</summary>
        /// <param name="scene">Scene to be added</param>
        internal static void AddSceneToHistory(Node scene)
        {
            PreviousScene = scene.Name;
            PackedScene packedScene = new PackedScene();
            packedScene.Pack(scene);
            History.Add(packedScene);
            SaveHero(CurrentHero);
        }

        /// <summary>Go back to previous scene.</summary>
        /// <returns>Previous scene</returns>
        internal static PackedScene GoBack()
        {
            PackedScene last = History.Last();
            History.Remove(last);
            SaveHero(CurrentHero);
            return last;
        }

        internal static void WriteSceneHistory() => JSONInteraction.WriteSceneHistory(History, CurrentHero.Name);

        #endregion Scene Navigation

        /// <summary>Sets the current inventory for a given <see cref="GridInventory"/>.</summary>
        /// <param name="inventory"><see cref="GridInventory"/> to set the inventory</param>
        /// <param name="hero">Is this being set for the <see cref="Hero"/>?</param>
        internal static void SetInventoryFromGrid(GridInventory inventory, bool hero = true)
        {
            Godot.Collections.Array allSlots = inventory.GetChild(0).GetChildren();
            List<Item> allItems = new List<Item>();
            foreach (ItemSlot slot in allSlots)
            {
                if (slot?.Item?.Item != new Item() && slot.Item.Item.Name != "Fists")
                    allItems.Add(slot?.Item?.Item);
                else if (slot?.Item?.Item?.Name == "Fists")
                {
                    slot.RemoveChild(slot.GetChild(1));
                    slot.Item = new InventoryItem();
                }
            }
            if (hero)
                CurrentHero.Inventory = allItems;
            else
                CurrentEnemy.Inventory = allItems;
        }

        /// <summary>Sets the current <see cref="Equipment"/> for a given <see cref="GridEquipment"/>.</summary>
        /// <param name="equipment"><see cref="GridEquipment"/> to set the <see cref="Equipment"/></param>
        /// <param name="hero">Is this being set for the <see cref="Hero"/>?</param>
        internal static void SetEquipmentFromGrid(GridEquipment equipment, bool hero = true)
        {
            ItemSlot WeaponSlot = (ItemSlot)equipment.GetNode("WeaponSlot");
            ItemSlot HeadSlot = (ItemSlot)equipment.GetNode("HeadSlot");
            ItemSlot BodySlot = (ItemSlot)equipment.GetNode("BodySlot");
            ItemSlot HandsSlot = (ItemSlot)equipment.GetNode("HandsSlot");
            ItemSlot LegsSlot = (ItemSlot)equipment.GetNode("LegsSlot");
            ItemSlot FeetSlot = (ItemSlot)equipment.GetNode("FeetSlot");
            ItemSlot LeftRingSlot = (ItemSlot)equipment.GetNode("LeftRingSlot");
            ItemSlot RightRingSlot = (ItemSlot)equipment.GetNode("RightRingSlot");
            if (hero)
            {
                if (WeaponSlot.Item != null)
                    CurrentHero.Equipment.Weapon = WeaponSlot.Item.Item;
                if (HeadSlot.Item != null)
                    CurrentHero.Equipment.Head = HeadSlot.Item.Item;
                if (BodySlot.Item != null)
                    CurrentHero.Equipment.Body = BodySlot.Item.Item;
                if (HandsSlot.Item != null)
                    CurrentHero.Equipment.Hands = HandsSlot.Item.Item;
                if (LegsSlot.Item != null)
                    CurrentHero.Equipment.Legs = LegsSlot.Item.Item;
                if (FeetSlot.Item != null)
                    CurrentHero.Equipment.Feet = FeetSlot.Item.Item;
                if (LeftRingSlot.Item != null)
                    CurrentHero.Equipment.LeftRing = LeftRingSlot.Item.Item;
                if (RightRingSlot.Item != null)
                    CurrentHero.Equipment.RightRing = RightRingSlot.Item.Item;
            }
            else
            {
                if (WeaponSlot.Item != null)
                    CurrentEnemy.Equipment.Weapon = WeaponSlot.Item.Item;
                if (HeadSlot.Item != null)
                    CurrentEnemy.Equipment.Head = HeadSlot.Item.Item;
                if (BodySlot.Item != null)
                    CurrentEnemy.Equipment.Body = BodySlot.Item.Item;
                if (HandsSlot.Item != null)
                    CurrentEnemy.Equipment.Hands = HandsSlot.Item.Item;
                if (LegsSlot.Item != null)
                    CurrentEnemy.Equipment.Legs = LegsSlot.Item.Item;
                if (FeetSlot.Item != null)
                    CurrentEnemy.Equipment.Feet = FeetSlot.Item.Item;
                if (LeftRingSlot.Item != null)
                    CurrentEnemy.Equipment.LeftRing = LeftRingSlot.Item.Item;
                if (RightRingSlot.Item != null)
                    CurrentEnemy.Equipment.RightRing = RightRingSlot.Item.Item;
            }
        }

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
            AllHeadArmor = JSONInteraction.LoadArmor<Item>("head").OrderBy(o => o.Value).ToList();
            AllBodyArmor = JSONInteraction.LoadArmor<Item>("body").OrderBy(o => o.Value).ToList();
            AllHandArmor = JSONInteraction.LoadArmor<Item>("hand").OrderBy(o => o.Value).ToList();
            AllLegArmor = JSONInteraction.LoadArmor<Item>("leg").OrderBy(o => o.Value).ToList();
            AllFeetArmor = JSONInteraction.LoadArmor<Item>("feet").OrderBy(o => o.Value).ToList();
            AllRings = JSONInteraction.LoadRings().OrderBy(o => o.Value).ToList();
            AllWeapons = JSONInteraction.LoadWeapons().OrderBy(o => o.Type).ThenBy(o => o.Value).ToList();
            AllDrinks = JSONInteraction.LoadDrinks().OrderBy(o => o.Value).ToList();
            AllFood = JSONInteraction.LoadFood().OrderBy(o => o.Value).ToList();
            AllPotions = JSONInteraction.LoadPotions().OrderBy(o => o.Cures).ThenBy(o => o.RestoreHealth).ThenBy(o => o.RestoreMagic).ToList();
            AllSpells = JSONInteraction.LoadSpells().OrderBy(o => o.Value).ToList();
            AllEnemies = JSONInteraction.LoadEnemies().OrderBy(o => o.Name).ToList();

            //foreach (Enemy enemy in AllEnemies)
            //{
            //    if (enemy.Equipment.Weapon != new Item())
            //        enemy.Equipment.Weapon = AllWeapons.Find(itm => itm.Name == enemy.Equipment.Weapon.Name);
            //    if (enemy.Equipment.Head != new Item())
            //        enemy.Equipment.Head = AllHeadArmor.Find(itm => itm.Name == enemy.Equipment.Head.Name);
            //    if (enemy.Equipment.Body != new Item())
            //        enemy.Equipment.Body = AllBodyArmor.Find(itm => itm.Name == enemy.Equipment.Body.Name);
            //    if (enemy.Equipment.Hands != new Item())
            //        enemy.Equipment.Hands = AllHandArmor.Find(itm => itm.Name == enemy.Equipment.Hands.Name);
            //    if (enemy.Equipment.Legs != new Item())
            //        enemy.Equipment.Legs = AllLegArmor.Find(itm => itm.Name == enemy.Equipment.Legs.Name);
            //    if (enemy.Equipment.Feet != new Item())
            //        enemy.Equipment.Feet = AllFeetArmor.Find(itm => itm.Name == enemy.Equipment.Feet.Name);
            //    if (enemy.Equipment.LeftRing != new Item())
            //        enemy.Equipment.LeftRing = AllRings.Find(itm => itm.Name == enemy.Equipment.LeftRing.Name);
            //    if (enemy.Equipment.RightRing != new Item())
            //        enemy.Equipment.RightRing = AllRings.Find(itm => itm.Name == enemy.Equipment.RightRing.Name);
            //}

            //JSONInteraction.WriteAll(AllClasses, AllHeadArmor, AllBodyArmor, AllHandArmor, AllLegArmor, AllFeetArmor, AllRings, AllWeapons, AllDrinks, AllFood, AllPotions, AllSpells, AllEnemies);

            // TODO Save scene history on application exit and load back when the same player logs in.
            // TODO Display current weight on any relevant scenes: battle/inventory for sure.

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

        /// <summary>Adds an instance of <see cref="Item"/> to an <see cref="ItemSlot"/>.</summary>
        /// <param name="slot"><see cref="ItemSlot"/> where the <see cref="Item"/> is to be added</param>
        /// <param name="item"><see cref="Item"/> to be added</param>
        internal static void AddItemInstanceToSlot(ItemSlot slot, Item item)
        {
            PackedScene scene = (PackedScene)ResourceLoader.Load("res://scenes/inventory/InventoryItem.tscn");
            while (slot.GetChildren().Count > 1)
                slot.RemoveChild(slot.GetChild(1));
            InventoryItem invItem = (InventoryItem)scene.Instance();
            slot.AddChild(invItem);
            slot.Item = invItem;
            slot.Item.SetItem(item);
            invItem.Theme = (Theme)ResourceLoader.Load("res://resources/TextureRect.tres");
        }

        /// <summary>Gets a specific Item based on its name.</summary>
        /// <param name="name">Item name</param>
        /// <returns>Item</returns>
        private static Item GetItem(string name) => AllItems.Find(itm => itm.Name == name);

        /// <summary>Retrieves a List of all Items of specified Type.</summary>
        /// <param name="type">Type</param>
        /// <returns>List of specified Type.</returns>
        public static List<Item> GetItemsOfType(ItemType type) => new List<Item>(AllItems.FindAll(itm => itm.Type == type));

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
            JSONInteraction.DeleteHero(deleteHero);
            AllHeroes.Remove(deleteHero);
        }

        /// <summary>Creates a new Hero and adds it to the database.</summary>
        /// <param name="newHero">New Hero</param>
        internal static void NewHero(Hero newHero)
        {
            if (newHero.Equipment.Head == null || newHero.Equipment.Head == new Item())
                newHero.Equipment.Head = AllHeadArmor.Find(armor => armor.Name == DefaultHead.Name);
            if (newHero.Equipment.Body == null || newHero.Equipment.Body == new Item())
                newHero.Equipment.Body = AllBodyArmor.Find(armor => armor.Name == DefaultBody.Name);
            if (newHero.Equipment.Hands == null || newHero.Equipment.Hands == new Item())
                newHero.Equipment.Hands = AllHandArmor.Find(armor => armor.Name == DefaultHands.Name);
            if (newHero.Equipment.Legs == null || newHero.Equipment.Legs == new Item())
                newHero.Equipment.Legs = AllLegArmor.Find(armor => armor.Name == DefaultLegs.Name);
            if (newHero.Equipment.Feet == null || newHero.Equipment.Feet == new Item())
                newHero.Equipment.Feet = AllFeetArmor.Find(armor => armor.Name == DefaultFeet.Name);
            if (newHero.Equipment.Weapon == null || newHero.Equipment.Weapon == new Item())
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

        /// <summary>Displays a popup next to the Player.</summary>
        /// <param name="acceptDialog">Instance of MyAcceptDialog</param>
        /// <param name="text">Text to be displayed</param>
        /// <param name="displayTime">Duration for the text to be displayed</param>
        internal static void DisplayPopup(MyAcceptDialog acceptDialog, Player player, string text, float displayTime = 0.75f)
        {
            acceptDialog.Popup_();
            acceptDialog.SetExpiration(displayTime);
            acceptDialog.DialogText = text;
            acceptDialog.SetSize(new Vector2(84f, 128f));
            acceptDialog.SetGlobalPosition(new Vector2(player.GetGlobalPosition().x + 32, player.GetGlobalPosition().y - 32));
            Info.DisplayStats();
        }

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

        /// <summary>Event where the <see cref="Hero"/> finds an <see cref="Item"/>.</summary>
        /// <param name="minValue">Minimum value of <see cref="Item"/></param>
        /// <param name="maxValue">Maximum value of <see cref="Item"/></param>
        /// <param name="isSold">Is the <see cref="Item"/> sold?</param>
        /// <returns>Returns text about found <see cref="Item"/></returns>
        internal static string EventFindItem(int minValue, int maxValue, bool isSold = true)
        {
            Item item = GetRandomItem(minValue, maxValue, isSold);
            if (CurrentHero.Inventory.Count < 40)
            {
                CurrentHero.AddItem(item);
                SaveHero(CurrentHero);
                return $"You find a {item.Name}!";
            }
            return $"You find a {item.Name},\nbut your inventory is full.";
        }

        /// <summary>Event where the <see cref="Hero"/> finds an <see cref="Item"/>.</summary>
        /// <param name="minValue">Minimum value of <see cref="Item"/></param>
        /// <param name="maxValue">Maximum value of <see cref="Item"/></param>
        /// <param name="type">Type of <see cref="Item"/></param>
        /// <param name="isSold">Is the <see cref="Item"/> sold?</param>
        /// <returns>Returns text about found <see cref="Item"/></returns>
        internal static string EventFindItem(int minValue, int maxValue, ItemType type, bool isSold = true)
        {
            Item item = GetRandomItem(minValue, maxValue, type, isSold);
            if (CurrentHero.Inventory.Count < 40)
            {
                CurrentHero.AddItem(item);
                SaveHero(CurrentHero);
                return $"You find a {item.Name}!";
            }
            return $"You find a {item.Name},\nbut your inventory is full.";
        }

        /// <summary>Gets a random <see cref="Item"/>.</summary>
        /// <param name="minValue">Minimum value of <see cref="Item"/></param>
        /// <param name="maxValue">Maximum value of <see cref="Item"/></param>
        /// <param name="isSold">Is the <see cref="Item"/> sold?</param>
        /// <returns>Random <see cref="Item"/></returns>
        private static Item GetRandomItem(int minValue, int maxValue, bool isSold = true)
        {
            List<Item> availableItems = new List<Item>(AllItems.FindAll(itm => itm.Value >= minValue && itm.Value <= maxValue && itm.IsSold == isSold));
            return availableItems[Functions.GenerateRandomNumber(0, availableItems.Count - 1)];
        }

        /// <summary>Gets a random <see cref="Item"/> of a given <see cref="ItemType"/>.</summary>
        /// <param name="minValue">Minimum value of <see cref="Item"/></param>
        /// <param name="maxValue">Maximum value of It<see cref="Item"/>em</param>
        /// <param name="type">Type of <see cref="Item"/></param>
        /// <param name="isSold">Is the <see cref="Item"/> sold?</param>
        /// <returns>Random <see cref="Item"/></returns>
        private static Item GetRandomItem(int minValue, int maxValue, ItemType type, bool isSold = true)
        {
            List<Item> availableItems = new List<Item>(AllItems.FindAll(itm => itm.Type == type && itm.Value >= minValue && itm.Value <= maxValue && itm.IsSold == isSold));
            return availableItems[Functions.GenerateRandomNumber(0, availableItems.Count - 1)];
        }

        /// <summary>Event where the <see cref="Hero"/> finds an <see cref="Item"/>.</summary>
        /// <param name="names">List of names of available <see cref="Item"/>s</param>
        /// <returns>Returns text about found <see cref="Item"/></returns>
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
            if (CurrentEnemy.Type == "Human" || CurrentEnemy.Type == "Boss")
            {
                if (Functions.GenerateRandomNumber(1, 100) <= 20)
                    CurrentEnemy.AddItem(GetRandomItem(1, CurrentEnemy.Level * 50));
                if (Functions.GenerateRandomNumber(1, 100) <= 10)
                    CurrentEnemy.AddItem(GetRandomItem(1, CurrentEnemy.Level * 100));
                if (CurrentEnemy.Type == "Boss" && Functions.GenerateRandomNumber(1, 100) <= 10)
                    CurrentEnemy.AddItem(GetRandomItem(1, CurrentEnemy.Level * 200));
            }
        }

        /// <summary>Event where the Hero encounters a water stream and restores health and magic.</summary>
        /// <returns>String saying Hero has been healed</returns>
        internal static string EventEncounterStream()
        {
            CurrentHero.Statistics.CurrentHealth = CurrentHero.Statistics.MaximumHealth;
            CurrentHero.Statistics.CurrentMagic = CurrentHero.Statistics.MaximumMagic;

            return
            "You stumble across a stream.\nYou stop to drink some of\nthe water and rest a while.\nYou feel recharged!";
        }

        #endregion Exploration Events
    }
}