using Godot;
using Sulimn.Actors;
using Sulimn.Classes;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Items;
using System.Collections.Generic;

namespace Sulimn.Scenes.Shopping
{
    public class MarketScene : Node2D
    {
        private Player Player;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Player = (Player)GetTree().CurrentScene.FindNode("Player");
            GameState.Info.Scale = new Vector2(1, 1);
            GameState.Info.DisplayStats();
        }

        /// <summary>Loads the ItemMerchantScene.</summary>
        /// <param name="items">Items to be loaded</param>
        private void EnterMerchantArea(params List<Item>[] items)
        {
            Player.Move("down");
            GameState.MerchantInventory.Clear();
            GameState.MerchantInventory.AddRanges(items);
            GameState.AddSceneToHistory(GetTree().CurrentScene);
            GetTree().ChangeScene("res://scenes/shopping/ItemMerchantScene.tscn");
        }

        #region Areas Entered

        private void _on_ArmouryArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                EnterMerchantArea(GameState.AllHeadArmor, GameState.AllBodyArmor, GameState.AllHandArmor, GameState.AllLegArmor, GameState.AllFeetArmor);
        }

        private void _on_CityArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                GetTree().ChangeSceneTo(GameState.GoBack());
        }

        private void _on_GeneralStoreArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                EnterMerchantArea(GameState.AllFood, GameState.AllDrinks, GameState.AllPotions);
        }

        private void _on_MagickShoppeArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/shopping/MagickShoppeScene.tscn");
            }
        }

        private void _on_SilverEmpireArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                EnterMerchantArea(GameState.AllRings);
        }

        private void _on_SmithyArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
            {
                Player.Move("down");
                GameState.AddSceneToHistory(GetTree().CurrentScene);
                GetTree().ChangeScene("res://scenes/shopping/SmithScene.tscn");
            }
        }

        private void _on_WeaponsArea_area_shape_entered(int area_id, object area, int area_shape, int self_shape)
        {
            if (area is Node player && player.IsInGroup("Player"))
                EnterMerchantArea(GameState.AllWeapons);
        }

        #endregion Areas Entered

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        //public override void _PhysicsProcess(float delta)
        //{
        //}
    }
}