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
        internal static void WriteAll(List<HeroClass> classes, List<Item> headArmor, List<Item> bodyArmor, List<Item> handArmor, List<Item> legArmor, List<Item> feetArmor, List<Item> rings, List<Item> weapons, List<Item> drinks, List<Item> food, List<Item> potions, List<Spell> spells, List<Enemy> enemies)
        {
            Write(classes, "user://classes.json");
            Write(headArmor, "user://head_armor.json");
            Write(bodyArmor, "user://body_armor.json");
            Write(handArmor, "user://hand_armor.json");
            Write(legArmor, "user://leg_armor.json");
            Write(feetArmor, "user://feet_armor.json");
            Write(rings, "user://rings.json");
            Write(weapons, "user://weapons.json");
            Write(drinks, "user://drinks.json");
            Write(food, "user://food.json");
            Write(potions, "user://potions.json");
            Write(spells, "user://spells.json");
            Write(enemies, "user://enemies.json");
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
                newFile.Open(path, File.ModeFlags.Write);
                newFile.StoreLine(JsonConvert.SerializeObject(list, Formatting.Indented));
                newFile.Close();
            }
        }

        /// <summary>Write's a user's scene history to file.</summary>
        /// <param name="history">List of all previous scenes in their history</param>
        /// <param name="username">Name of user whose scene history is being written</param>
        internal static void WriteSceneHistory(List<PackedScene> history, string username)
        {
            File newFile = new File();
            newFile.Open($"user://save/{username}History.json", File.ModeFlags.Write);
            newFile.StoreLine(JsonConvert.SerializeObject(history, Formatting.Indented));
            newFile.Close();
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
                jsonFile.Open(path, File.ModeFlags.Read);
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

        /// <summary>Loads all Armor of specified type.</summary>
        /// <typeparam name="T">Type of Armor</typeparam>
        /// <param name="type">Type of Armor</param>
        /// <returns>List of Armor of specified type</returns>
        internal static List<T> LoadArmor<T>(string type) => LoadJsonFromFile<T>($"res://data/{type}_armor.json");

        /// <summary>Loads all Rings from disk.</summary>
        /// <returns>List of Rings</returns>
        internal static List<Item> LoadRings() => LoadJsonFromFile<Item>("res://data/rings.json");

        /// <summary>Loads all Drinks from disk.</summary>
        /// <returns>List of Drinks</returns>
        internal static List<Item> LoadDrinks() => LoadJsonFromFile<Item>("res://data/drinks.json");

        /// <summary>Loads all Food from disk.</summary>
        /// <returns>List of Food/></returns>
        internal static List<Item> LoadFood() => LoadJsonFromFile<Item>("res://data/food.json");

        /// <summary>Loads all Potions from disk.</summary>
        /// <returns>List of Potions</returns>
        internal static List<Item> LoadPotions() => LoadJsonFromFile<Item>("res://data/potions.json");

        /// <summary>Loads all <see cref="Spell"/>s from disk.</summary>
        /// <returns>List of <see cref="Spell"/>s</returns>
        internal static List<Spell> LoadSpells() => LoadJsonFromFile<Spell>("res://data/spells.json");

        /// <summary>Loads all <see cref="Weapon"/>s from disk.</summary>
        /// <returns>List of <see cref="Weapon"/>s</returns>
        internal static List<Item> LoadWeapons() => LoadJsonFromFile<Item>("res://data/weapons.json");

        /// <summary>Loads all <see cref="Enemy"/>s from disk.</summary>
        /// <returns>List of <see cref="Enemy"/>s</returns>
        internal static List<Enemy> LoadEnemies() => LoadJsonFromFile<Enemy>("res://data/enemies.json");

        #endregion Load

        #region Hero Manipulation

        /// <summary>Deletes a <see cref="Hero"/> from disk.</summary>
        /// <param name="deleteHero"><see cref="Hero"/> to be deleted</param>
        /// <returns>True if file no longer exists</returns>
        internal static bool DeleteHero(Hero deleteHero)
        {
            string path = $"user://save/{deleteHero.Name}.json";
            Directory userDir = new Directory();
            if (userDir.FileExists(path))
            {
                userDir.Remove(path);
                return !userDir.FileExists(path);
            }
            else
                return false;
        }

        /// <summary>Loads all <see cref="Hero"/>es from disk.</summary>
        /// <returns>List of <see cref="Hero"/>es</returns>
        internal static List<Hero> LoadHeroes()
        {
            string dirPath = "user://save/";
            List<Hero> heroes = new List<Hero>();
            Directory dir = new Directory();
            if (!dir.DirExists(dirPath))
                dir.MakeDir(dirPath);
            if (dir.Open(dirPath) != Error.CantOpen)
            {
                dir.ListDirBegin();
                string file = dir.GetNext();
                while (!string.IsNullOrWhiteSpace(file))
                {
                    if (!dir.CurrentIsDir())
                    {
                        File newFile = new File();
                        newFile.Open(dirPath + file, File.ModeFlags.Read);
                        string contents = newFile.GetAsText();
                        newFile.Close();
                        heroes.Add(JsonConvert.DeserializeObject<Hero>(contents));
                    }
                    file = dir.GetNext();
                }
            }

            return heroes;
        }

        /// <summary>Saves a <see cref="Hero"/> to disk.</summary>
        /// <param name="saveHero"><see cref="Hero"/> to be saved to disk</param>
        internal static void SaveHero(Hero saveHero)
        {
            Directory dir = new Directory();
            if (!dir.DirExists("user://save/"))
                dir.MakeDir("user://save/");
            File newFile = new File();
            newFile.Open($"user://save/{saveHero.Name}.json", File.ModeFlags.Write);
            string text = JsonConvert.SerializeObject(saveHero, Formatting.Indented);
            newFile.StoreLine(text);
            newFile.Close();
        }

        #endregion Hero Manipulation
    }
}