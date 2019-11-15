using Godot;
using Sulimn.Classes;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridEquipment : Panel
    {
        private ItemSlot WeaponSlot, HeadSlot, BodySlot, HandsSlot, LegsSlot, FeetSlot, LeftRingSlot, RightRingSlot;

        public void SetUpEquipment(Equipment equipment, bool enemy = false)
        {
            if (enemy)
            {
                WeaponSlot.Enemy = true;
                HeadSlot.Enemy = true;
                BodySlot.Enemy = true;
                HandsSlot.Enemy = true;
                LegsSlot.Enemy = true;
                FeetSlot.Enemy = true;
                LeftRingSlot.Enemy = true;
                RightRingSlot.Enemy = true;
            }

            if (equipment.Weapon != new Item())
                GameState.AddItemInstanceToSlot(WeaponSlot, equipment.Weapon);
            if (equipment.Head != new Item())
                GameState.AddItemInstanceToSlot(HeadSlot, equipment.Head);
            if (equipment.Body != new Item())
                GameState.AddItemInstanceToSlot(BodySlot, equipment.Body);
            if (equipment.Hands != new Item())
                GameState.AddItemInstanceToSlot(HandsSlot, equipment.Hands);
            if (equipment.Legs != new Item())
                GameState.AddItemInstanceToSlot(LegsSlot, equipment.Legs);
            if (equipment.Feet != new Item())
                GameState.AddItemInstanceToSlot(FeetSlot, equipment.Feet);
            if (equipment.LeftRing != new Item())
                GameState.AddItemInstanceToSlot(LeftRingSlot, equipment.LeftRing);
            if (equipment.RightRing != new Item())
                GameState.AddItemInstanceToSlot(RightRingSlot, equipment.RightRing);
        }

        private void AssignControls()
        {
            WeaponSlot = (ItemSlot)GetNode("WeaponSlot");
            HeadSlot = (ItemSlot)GetNode("HeadSlot");
            BodySlot = (ItemSlot)GetNode("BodySlot");
            HandsSlot = (ItemSlot)GetNode("HandsSlot");
            LegsSlot = (ItemSlot)GetNode("LegsSlot");
            FeetSlot = (ItemSlot)GetNode("FeetSlot");
            LeftRingSlot = (ItemSlot)GetNode("LeftRingSlot");
            RightRingSlot = (ItemSlot)GetNode("RightRingSlot");
            WeaponSlot.ItemTypes = new List<ItemType> { ItemType.MeleeWeapon, ItemType.RangedWeapon };
            HeadSlot.ItemTypes = new List<ItemType> { ItemType.HeadArmor };
            BodySlot.ItemTypes = new List<ItemType> { ItemType.BodyArmor };
            HandsSlot.ItemTypes = new List<ItemType> { ItemType.HandArmor };
            LegsSlot.ItemTypes = new List<ItemType> { ItemType.LegArmor };
            FeetSlot.ItemTypes = new List<ItemType> { ItemType.FeetArmor };
            LeftRingSlot.ItemTypes = new List<ItemType> { ItemType.Ring };
            RightRingSlot.ItemTypes = new List<ItemType> { ItemType.Ring };
            WeaponSlot.Equipment = true;
            HeadSlot.Equipment = true;
            BodySlot.Equipment = true;
            HandsSlot.Equipment = true;
            LegsSlot.Equipment = true;
            FeetSlot.Equipment = true;
            LeftRingSlot.Equipment = true;
            RightRingSlot.Equipment = true;
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
        }
    }
}