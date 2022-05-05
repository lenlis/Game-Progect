using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    class Level
    {
        static public int windowWidth;
        static public int windowHeight;
        static public Vector2 roomPos;
        static public int MaxRoomInd = 50;
        static private int HeroInRoom;
        public static GraphicsDevice graphicsDevice;
        public static int[,] levelMap = new int[10,10];
        public static readonly List<Room> rooms = new List<Room>();
        public static Queue<NotInitRoom> queue = new Queue<NotInitRoom>();
        public static void InitLevel(GraphicsDevice graphicsDevice, int Width, int Height)
        {
            
            Level.graphicsDevice = graphicsDevice;
            windowWidth = Width;
            windowHeight = Height;       
            for (int t = 0; t < 10; t++)
            {
                for (int u = 0; u < 10; u++)
                {
                    levelMap[u, t] = int.MaxValue;
                }
            }
            levelMap[3, 0] = 0;
            rooms.Add(new Room());
            rooms[HeroInRoom].InitFirst();
            while(queue.Count != 0)
            {
                var room = queue.Dequeue();
                room.Init();
            }
            foreach (var room in rooms)
            {
                foreach(var dr in room.doors)
                {
                    dr.wayToRoomNumChange(levelMap[Room.GetPosibleWay(room.roomMapCord, (int)dr.GetWall()).X, Room.GetPosibleWay(room.roomMapCord, (int)dr.GetWall()).Y]);
                }
            }
            

            roomPos = new Vector2(rooms[HeroInRoom].GetPos().X, rooms[HeroInRoom].GetPos().Y);
            Hero1.UpdatePos(rooms[HeroInRoom].HeroStartPosition);

        }

        public static int GetHeroInRoomValue()
        {
            return HeroInRoom;
        }

        public static void AddRoom(Point mapId, int doorId)
        {
            
            rooms.Add(new Room());
            if (levelMap[mapId.X, mapId.Y] == int.MaxValue)
                levelMap[mapId.X, mapId.Y] = rooms.Count - 1;
            rooms[^1].InitRoom(rooms.Count - 1, mapId);         
                if (doorId % 2 == 0)
                    rooms[levelMap[mapId.X, mapId.Y]].AddDoor(doorId + 1);
                else
                    if (doorId % 2 == 1)
                    rooms[levelMap[mapId.X, mapId.Y]].AddDoor(doorId - 1);
        }

        public static void ChangeHeroInRoomValue(int id)
        {
            HeroInRoom = id;
        }

        public static void Update()
        {
            foreach(var ch in rooms[HeroInRoom].chests)
            {
                if (ch.GetCollusion().Intersects(Hero1.CountCollusion()) && !ch.GetCondition() && !ch.closed)
                    ch.Open(rooms[HeroInRoom].Rand);
            }
            foreach (var dr in rooms[HeroInRoom].doors)
            {
                if (dr.GetCollusion().Intersects(Hero1.CountCollusion()) && !dr.GetCondition())
                {
                    var id = dr.GetWayToRoomNum();
                    Level.ChangeHeroInRoomValue(id);
                    Level.roomPos = new Vector2(Level.rooms[id].GetPos().X, Level.rooms[id].GetPos().Y);
                    Hero1.UpdatePos(new Vector2(900, 500));
                }
            }
        }

        public static void Draw(GraphicsDevice graphicsDevice, EffectPassCollection effectPassCollection)
        {
            rooms[HeroInRoom].Draw(graphicsDevice, effectPassCollection, HeroInRoom);
        }
    }

    class Room
    {
        public Point roomMapCord;
        VertexPositionColor[] vertexPositionColorsHorisontal;
        VertexPositionColor[] vertexPositionColorsVertical;
        Vector2 Position;
        Vector2 FirstDoorPos;
        List<Coin> coins = new List<Coin>();
        public List<Chest> chests = new List<Chest>();
        public List<Door> doors = new List<Door>();
        public Random Rand = new Random();
        public bool Initialized;
        public Vector2 HeroStartPosition;
        public static Texture2D DoorTexture { get; set; }

        public Vector2 GetPos()
        {
            return Position;
        }

        public void InitFirst()
        {
            roomMapCord = new Point(3, 0);
            CountRoomSize(0);
            HeroStartPosition = new Vector2(Level.windowWidth / 2 - Hero1.Texture2D.Width / 2, Level.windowHeight / 2 - Hero1.Texture2D.Height / 2);
            doors.Add(new Door());
            Level.AddRoom(GetPosibleWay(roomMapCord, 0), 0);
            doors[0].InitDoor(0, Position);
            doors[0].wayToRoomNumChange(1);
        }

        public void AddDoor(int wall)
        {
            doors.Add(new Door());
            doors[^1].InitDoor(wall, Position);
        }

        public void InitRoom(int roomInd, Point mapCord)
        {
            roomMapCord = mapCord;
            CountRoomSize(roomInd);
            var flag = false;
            for (int i = 0; i < Rand.Next(2, 4); i++)
            {
                flag = false;
                foreach (var dr in doors)
                {
                    if (dr.GetWall() == (Door.WallId)i || 
                        GetPosibleWay(mapCord, i).X < 0 || 
                        GetPosibleWay(mapCord, i).Y < 0 || 
                        Level.levelMap[GetPosibleWay(mapCord, i).X, GetPosibleWay(mapCord, i).Y] != int.MaxValue)
                    {
                        flag = true;
                    }
                }
                if (flag == true) continue;
                if (Level.rooms.Count + Level.queue.Count < 50)
                {
                    doors.Add(new Door());
                    doors[doors.Count - 1].InitDoor(i, Position);
                    Level.queue.Enqueue(new NotInitRoom(i , GetPosibleWay(mapCord, i)));               
                }
            }
            GenerateObjAndEnem(roomInd);
        }

        public void GenerateObjAndEnem(int roomInd)
        {
            if (Position.X >= 300 && Position.Y >= 200 && roomInd != Level.MaxRoomInd)
            {
                chests.Add(new Chest());
                if (Rand.Next(1, 100) > 90)
                    chests[0].closed = false;
                else
                    chests[0].closed = true;
                chests[0].UpdatePos(new Vector2(Level.windowWidth / 2 - Chest.Texture2D.Width, Level.windowHeight / 2 - Chest.Texture2D.Height));
            }
            //else if (roomInd != Level.MaxRoomInd)

        }

        public void CountRoomSize(int roomInd) 
        {
            if (roomInd == 0 || roomInd == Level.MaxRoomInd )
                Position = new Vector2(600,300);
            else
                Position = new Vector2(Rand.Next(100, 600), Rand.Next(100, 300));
            vertexPositionColorsHorisontal = new[]
            {
                new VertexPositionColor(new Vector3(Position.X, Position.Y, 0), Color.White),
                new VertexPositionColor(new Vector3(Level.windowWidth - Position.X, Position.Y, 0), Color.White),
                new VertexPositionColor(new Vector3(Position.X, Level.windowHeight-Position.Y, 0), Color.White),
                new VertexPositionColor(new Vector3(Level.windowWidth - Position.X, Level.windowHeight-Position.Y, 0), Color.White)
            };

            vertexPositionColorsVertical = new[]
            {
                vertexPositionColorsHorisontal[0],
                vertexPositionColorsHorisontal[2],
                vertexPositionColorsHorisontal[1],
                vertexPositionColorsHorisontal[3]
            };
           
            FirstDoorPos = new Vector2(Position.X, (Level.windowHeight - Position.Y - Position.Y) / 2 + Position.Y - 50);
            Initialized = true;
        }

        public void Draw(GraphicsDevice graphicsDevice, EffectPassCollection effectPassCollection, int roomInd)
        {
            foreach (EffectPass pass in effectPassCollection)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsHorisontal, 0, 2);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsVertical, 0, 2);   
            }
            foreach (var dr in doors)
            {
                dr.Draw();
            }
            foreach (var ch in chests)
            {
                ch.Draw();
            }
        }
        
        static public Point GetPosibleWay(Point mapId, int doorId)
        {
            if (doorId == 0 && mapId.Y < 10)
                return new Point(mapId.X, mapId.Y + 1);
            if (doorId == 1 && mapId.Y > 0)
                return new Point(mapId.X, mapId.Y - 1);
            if (doorId == 2 && mapId.X > 0)
                return new Point(mapId.X - 1, mapId.Y);
            if (doorId == 3 && mapId.X < 10)
                return new Point(mapId.X + 1, mapId.Y);
            else
                return new Point(-1,-1);
        }
    }

    class Door : GameObject
    {
        private static Point TextureSize;
        private static Point TextureSizeUD;
        public static Texture2D Texture2D { get; set; }
        public static Texture2D SecTexture2D { get; set; }

        int wayToRoomNum;
        public enum WallId
        {            
            right,
            left,
            up,
            down
        }
        private WallId Wall;

        public static void CountTextureSize(Point size, Point sizeUD)
        {
            TextureSize = size;
            TextureSizeUD = sizeUD;
        }

        public void wayToRoomNumChange(int wayID)
        {
            wayToRoomNum = wayID;
        }
        public void InitDoor(int wall, Vector2 roomPos)
        {
            Wall = (WallId)wall;
            if (Wall == WallId.left)
                this.UpdatePos(new Vector2(roomPos.X + 5, (Level.windowHeight - roomPos.Y - roomPos.Y) / 2 + roomPos.Y - 50));
            if (Wall == WallId.right)
                this.UpdatePos(new Vector2(Level.windowWidth - roomPos.X - 5, (Level.windowHeight - roomPos.Y - roomPos.Y) / 2 + roomPos.Y - 50));
            if (Wall == WallId.up)
                this.UpdatePos(new Vector2((Level.windowWidth) / 2 - TextureSizeUD.X + 10, roomPos.Y - TextureSizeUD.Y));
            if (Wall == WallId.down)
                this.UpdatePos(new Vector2((Level.windowWidth) / 2 - TextureSizeUD.X + 10, Level.windowHeight - roomPos.Y));
        }

        public WallId GetWall()
        {
            return Wall;
        }
        
        public int GetWayToRoomNum()
        {
            return wayToRoomNum;
        }
        public void Draw()
        {
            if (Wall == WallId.right)
                Objects.spriteBatch.Draw(Texture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            if (Wall == WallId.left)
                Objects.spriteBatch.Draw(Texture2D,new Vector2(this.GetPos().X, this.GetPos().Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            if (Wall == WallId.up)
                Objects.spriteBatch.Draw(SecTexture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            if (Wall == WallId.down)
                Objects.spriteBatch.Draw(SecTexture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 1);
        }

        public Rectangle GetCollusion()
        {
            if((int)Wall == 2)
                return new Rectangle((int)this.GetPos().X + (int)(TextureSize.Y/1.5f),
                (int)this.GetPos().Y - TextureSize.X/4 + 5, TextureSize.X/4, TextureSize.Y/4);
            else if ((int)Wall == 3)
                return new Rectangle((int)this.GetPos().X,
                (int)this.GetPos().Y - 10, TextureSize.Y, TextureSize.X);
            else
                return new Rectangle((int)this.GetPos().X ,
                (int)this.GetPos().Y + TextureSize.X / 2, TextureSize.X, TextureSize.Y / 4);
        }
    }
    public struct NotInitRoom
    {
        public int doorId;

        public Point mapId;

        public NotInitRoom(int doorId, Point mapId)
        {
            this.doorId = doorId;
            this.mapId = mapId;
        }

        public void Init()
        {
            Level.AddRoom(mapId, doorId);
        }
    }
}
