using Godot;
using Newtonsoft.Json;
using Sulimn.Classes.Entities;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Classes.Database
{
    /// <summary>Implements functionality of reading and writing game data from JSON files.</summary>
    public static class JSONInteraction
    {
        #region Write

        /// <summary>Writes all important files to disk</summary>
        internal static void WriteAll(List<HeroClass> classes, List<HeadArmor> headArmor, List<BodyArmor> bodyArmor, List<HandArmor> handArmor, List<LegArmor> legArmor, List<FeetArmor> feetArmor, List<Ring> rings, List<Weapon> weapons, List<Drink> drinks, List<Food> food, List<Potion> potions, List<Spell> spells, List<Enemy> enemies)
        {
            Write(classes, "user://classes.json");
            Write(headArmor, "user://headArmor.json");
            Write(bodyArmor, "user://bodyArmor.json");
            Write(handArmor, "user://handArmor.json");
            Write(legArmor, "user://legArmor.json");
            Write(feetArmor, "user://feetArmor.json");
            Write(rings, "user://rings.json");
            Write(weapons, "user://weapons.json");
            Write(drinks, "user://drinks.json");
            Write(food, "user://food.json");
            Write(potions, "user://potions.json");
            Write(spells, "user://spells.json");
            Write(enemies, "user://enemies.json");
        }

        /// <summary>Saves a <see cref="Hero"/> to disk.</summary>
        /// <param name="saveHero"><see cref="Hero"/> to be saved to disk</param>
        internal static void SaveHero(Hero saveHero)
        {
            GD.Print("Attemping to save Hero.");
            Directory dir = new Directory();
            if (!dir.DirExists("user://save/"))
                dir.MakeDir("user://save/");
            File newFile = new File();
            newFile.Open($"user://save/{saveHero.Name}.json", 2);
            string text = JsonConvert.SerializeObject(saveHero, Formatting.Indented);
            newFile.StoreLine(text);
            newFile.Close();
        }

        /// <summary>Writes a List of any type to disk.</summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="list">List of object</param>
        /// <param name="path">Path to where it will be written</param>
        private static void Write<T>(List<T> list, string path)
        {
            if (list.Count > 0)
            {
                File newFile = new File();
                newFile.Open(path, 2);
                newFile.StoreLine(JsonConvert.SerializeObject(list, Formatting.Indented));
                newFile.Close();
            }
        }

        #endregion Write

        #region Load

        /// <summary>Loads JSON data from a file.</summary>
        /// <param name="path">Path to the file to be loaded</param>
        /// <returns>JSON data from a file</returns>
        private static List<T> LoadJsonFromFile<T>(string path)
        {
            string data = "";
            File jsonFile = new File();
            if (jsonFile.FileExists(path))
            {
                jsonFile.Open(path, 1);
                data = jsonFile.GetAsText();
                jsonFile.Close();
            }
            else
                GD.Print($"{path} does not exist.");

            return !string.IsNullOrWhiteSpace(data) ? JsonConvert.DeserializeObject<List<T>>(data) : new List<T>();
        }

        /// <summary>Loads all <see cref="HeroClass"/>es from disk.</summary>
        /// <returns>List of <see cref="HeroClass"/>es</returns>
        internal static List<HeroClass> LoadClasses() => LoadJsonFromFile<HeroClass>("res://data/classes.json");

        /// <summary>Loads all <see cref="Armor"/> of specified type.</summary>
        /// <typeparam name="T">Type of <see cref="Armor"/></typeparam>
        /// <param name="type">Type of <see cref="Armor"/></param>
        /// <returns>List of <see cref="Armor"/> of specified type</returns>
        internal static List<T> LoadArmor<T>(string type) => LoadJsonFromFile<T>($"res://data/{type}_armor.json");

        /// <summary>Loads all <see cref="Ring"/>s from disk.</summary>
        /// <returns>List of <see cref="Ring"/>s</returns>
        internal static List<Ring> LoadRings() => LoadJsonFromFile<Ring>("res://data/rings.json");

        /// <summary>Loads all <see cref="Drink"/>s from disk.</summary>
        /// <returns>List of <see cref="Drink"/>s</returns>
        internal static List<Drink> LoadDrinks() => LoadJsonFromFile<Drink>("res://data/drinks.json");

        /// <summary>Loads all <see cref="Food"/> from disk.</summary>
        /// <returns>List of <see cref="Food"/></returns>
        internal static List<Food> LoadFood() => LoadJsonFromFile<Food>("res://data/food.json");

        /// <summary>Loads all <see cref="Potion"/>s from disk.</summary>
        /// <returns>List of <see cref="Potion"/>s</returns>
        internal static List<Potion> LoadPotions() => LoadJsonFromFile<Potion>("res://data/potions.json");

        /// <summary>Loads all <see cref="Spell"/>s from disk.</summary>
        /// <returns>List of <see cref="Spell"/>s</returns>
        internal static List<Spell> LoadSpells() => LoadJsonFromFile<Spell>("res://data/spells.json");

        /// <summary>Loads all <see cref="Weapon"/>s from disk.</summary>
        /// <returns>List of <see cref="Weapon"/>s</returns>
        internal static List<Weapon> LoadWeapons() => LoadJsonFromFile<Weapon>("res://data/weapons.json");

        /// <summary>Loads all <see cref="Enemy"/>s from disk.</summary>
        /// <returns>List of <see cref="Enemy"/>s</returns>
        internal static List<Enemy> LoadEnemies() => LoadJsonFromFile<Enemy>("res://data/enemies.json");

        /// <summary>Loads all <see cref="Hero"/>es from disk.</summary>
        /// <returns>List of <see cref="Hero"/>es</returns>
        internal static List<Hero> LoadHeroes()
        {
            List<Hero> heroes = new List<Hero>();
            Directory dir = new Directory();
            if (dir.Open("user://save/") != Error.CantOpen)
            {
                dir.ListDirBegin();
                string file = dir.GetNext();
                while (!string.IsNullOrWhiteSpace(file))
                {
                    if (!dir.CurrentIsDir())
                    {
                        File newFile = new File();
                        newFile.Open($"user://save/{file}", 1);
                        string contents = newFile.GetAsText();
                        newFile.Close();
                        heroes.Add(JsonConvert.DeserializeObject<Hero>(contents));
                    }
                    file = dir.GetNext();
                }
            }

            return heroes;
        }

        #endregion Load
    }
}