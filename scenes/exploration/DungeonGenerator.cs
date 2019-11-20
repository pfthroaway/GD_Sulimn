using Godot;
using Sulimn.Classes.Extensions;
using Sulimn.Classes.Extensions.DataTypeHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sulimn.Scenes.Exploration
{
    internal class DungeonGenerator : Node2D
    {
        private TileMap map;
        private const int FLOOR_WIDTH = 30;
        private const int FLOOR_HEIGHT = 25;

        private const int MAX_ROOM_WIDTH = 5;
        private const int MAX_ROOM_HEIGHT = 5;

        private const int MIN_ROOM_WIDTH = 3;
        private const int MIN_ROOM_HEIGHT = 3;

        private const int MAX_ROOM_COUNT = 20;

        public override void _Ready()
        {
            map = (TileMap)GetNode("TileMap");

            for (int y = 0; y < FLOOR_HEIGHT; y++)
                for (int x = 0; y < FLOOR_WIDTH; x++)
                    map.SetCell(x, y, 1);

            List<Rect> rooms = new List<Rect>();

            for (int i = 0; i < MAX_ROOM_COUNT; i++)
            {
                Rect r = CreateRandomRect(FLOOR_WIDTH, FLOOR_HEIGHT);
                var failed = false;
                foreach (Rect j in rooms)
                    if (r.Intersect(j))
                        failed = true;
                if (!failed && rooms.Count > 0)
                {
                    Vector2 nr = r.Center();
                    Vector2 pr = new Vector2(rooms.Last().Center());

                    RandomNumberGenerator ran = new RandomNumberGenerator();
                    if (ran.Randf() > 0.5)
                    {
                        H_Tunnel(Int32Helper.Parse(pr.x), Int32Helper.Parse(nr.x), Int32Helper.Parse(pr.y));
                        V_Tunnel(Int32Helper.Parse(pr.y), Int32Helper.Parse(nr.y), Int32Helper.Parse(nr.x));
                    }
                    else
                    {
                        V_Tunnel(Int32Helper.Parse(pr.y), Int32Helper.Parse(nr.y), Int32Helper.Parse(nr.x));
                        H_Tunnel(Int32Helper.Parse(pr.x), Int32Helper.Parse(nr.x), Int32Helper.Parse(pr.y));
                    }
                }
            }
            foreach (Rect room in rooms)
                CreateRoom(room);
        }

        internal void CreateRoom(Rect room)
        {
            for (int y = Int32Helper.Parse(room.Position.y); y < room.Height; y++)
                for (int x = Int32Helper.Parse(room.Position.x); x < room.Width; x++)
                    map.SetCell(x, y, 0);
        }

        internal Rect CreateRandomRect(int mapWidth, int mapHeight)
        {
            int w = Functions.GenerateRandomNumber(MIN_ROOM_WIDTH, MAX_ROOM_WIDTH);
            int h = Functions.GenerateRandomNumber(MIN_ROOM_HEIGHT, MAX_ROOM_HEIGHT);
            int x = Functions.GenerateRandomNumber(1, mapWidth - w);
            int y = Functions.GenerateRandomNumber(1, mapHeight - h);
            return new Rect(x, y, w, h);
        }

        internal void V_Tunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y < Math.Max(y1, y2); y++)
                map.SetCell(x, y, 0);
        }

        internal void H_Tunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x < Math.Max(x1, x2); x++)
                map.SetCell(x, y, 0);
        }
    }
}