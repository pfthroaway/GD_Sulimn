using Godot;
using Newtonsoft.Json;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Classes.Database
{
    /// <summary>Implements functionality of reading and writing game data from JSON files.</summary>
    public class JSONInteraction
    {
        /// <summary>Loads all <see cref="HeroClass"/>es from the database.</summary>
        /// <returns>List of <see cref="HeroClass"/>es</returns>
        internal static List<HeroClass> LoadClasses()
        {
            List<HeroClass> allClasses = new List<HeroClass>();
            File classesFile = new File();
            if (classesFile.FileExists("res://data/classes/classes.json"))
            {
                classesFile.Open("res://data/classes/classes.json", 1);
                allClasses.AddRange(JsonConvert.DeserializeObject<List<HeroClass>>(classesFile.GetAsText()));
            }
            return allClasses;
        }

        /// <summary>Loads all <see cref="Armor"/> of specified type.</summary>
        /// <typeparam name="T">Type of <see cref="Armor"/></typeparam>
        /// <param name="type"></param>
        /// <returns>List of <see cref="Armor"/> of specified type</returns>
        internal static List<T> LoadArmor<T>(string type)
        {
            List<T> allArmor = new List<T>();
            File armorFile = new File();
            if (armorFile.FileExists($"res://data/armor/{type}_armor.json"))
            {
                armorFile.Open($"res://data/armor/{type}_armor.json", 1);
                allArmor.AddRange(JsonConvert.DeserializeObject<List<T>>(armorFile.GetAsText()));
                armorFile.Close();
            }
            return allArmor;
        }
    }
}