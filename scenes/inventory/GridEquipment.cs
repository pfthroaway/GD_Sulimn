using Godot;
using Sulimn.Classes.HeroParts;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Inventory
{
    public class GridEquipment : Panel
    {
        private ItemSlot WeaponSlot, HeadSlot, BodySlot, HandsSlot, LegsSlot, FeetSlot, LeftRingSlot, RightRingSlot;

        private void AddItemInstanceToSlot(ItemSlot slot, Item item)
        {
            var scene = (PackedScene)ResourceLoader.Load("res://scenes/inventory/InventoryItem.tscn");
            InventoryItem invItem = (InventoryItem)scene.Instance();
            slot.AddChild(invItem);
            slot.Item = invItem;
            slot.Item.SetItem(item);
            invItem.Theme = (Theme)ResourceLoader.Load("res://resources/TextureRect.tres");
        }

        public void SetUpEquipment(Equipment equipment)
        {
            if (equipment.Weapon != new Item())
                AddItemInstanceToSlot(WeaponSlot, equipment.Weapon);
            if (equipment.Head != new Item())
                AddItemInstanceToSlot(HeadSlot, equipment.Head);
            if (equipment.Body != new Item())
                AddItemInstanceToSlot(BodySlot, equipment.Body);
            if (equipment.Hands != new Item())
                AddItemInstanceToSlot(HandsSlot, equipment.Hands);
            if (equipment.Legs != new Item())
                AddItemInstanceToSlot(LegsSlot, equipment.Legs);
            if (equipment.Feet != new Item())
                AddItemInstanceToSlot(FeetSlot, equipment.Feet);
            if (equipment.LeftRing != new Item())
                AddItemInstanceToSlot(LeftRingSlot, equipment.LeftRing);
            if (equipment.RightRing != new Item())
                AddItemInstanceToSlot(RightRingSlot, equipment.RightRing);
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
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            AssignControls();
        }
    }
}