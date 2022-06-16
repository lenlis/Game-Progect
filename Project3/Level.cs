using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BulletHell
{
    class Level
    {
        static public int windowWidth;
        static public int windowHeight;
        static bool transportFlag;
        static public Texture2D Marker { get; set; }
        static public Texture2D Torch { get; set; }
        static public Texture2D Floor { get; set; }
        static public Texture2D Wall { get; set; }
        static public Vector2 roomPos;
        public const int MaxRoomInd = 30;
        static private int HeroInRoom;
        static public int LevelCount = 0;
        const int VerticalMapSize = 10;
        const int HorisontalMapSize = 10;
        public static GraphicsDevice graphicsDevice;
        public static int[,] initLevelMap = new int[VerticalMapSize, HorisontalMapSize];
        public static readonly List<MapRoom> map = new List<MapRoom>();
        public static readonly List<Room> rooms = new List<Room>();
        public static readonly List<int> posibleEndRooms = new List<int>();
        public static Queue<NotInitRoom> queue = new Queue<NotInitRoom>();

        public static void ReInitLevel(GraphicsDevice graphicsDevice, int Width, int Height)
        {
            Hero1.ChengeHP(10);
            for (int i = 0; i < rooms.Count; i++)
            {
                map.RemoveAt(i);
                rooms.RemoveAt(i);
                i--;
            }
            InitLevel(graphicsDevice, Width, Height);
        }
        public static void InitLevel(GraphicsDevice graphicsDevice, int Width, int Height)
        {
            LevelCount++;
            HeroInRoom = 0;
            Level.graphicsDevice = graphicsDevice;
            windowWidth = Width;
            windowHeight = Height;
            for (int t = 0; t < VerticalMapSize; t++)
            {
                for (int u = 0; u < HorisontalMapSize; u++)
                {
                    initLevelMap[u, t] = int.MaxValue;
                }
            }
            initLevelMap[5, 5] = 0;
            rooms.Add(new Room());
            rooms[HeroInRoom].InitFirst();
            roomPos = new Vector2(rooms[HeroInRoom].GetPos().X, rooms[HeroInRoom].GetPos().Y);
            Hero1.UpdatePos(new Vector2(windowWidth / 2 - Hero1.Texture2D.Width / 2, windowHeight / 2 - Hero1.Texture2D.Height / 2));
            BuildLevel();
            DebugDoors();
            var ID = Room.Rand.Next(1, posibleEndRooms.Count) - 1;
            rooms[posibleEndRooms[ID]].CountRoomSize(-1);
            rooms[posibleEndRooms[ID]].doors[0].ChengeDoorPos(rooms[posibleEndRooms[ID]].GetPos());
            for (int i = 0; i < rooms[posibleEndRooms[ID]].chests.Count; i++)
            {
                rooms[posibleEndRooms[ID]].chests.RemoveAt(i);
            }
            for (int i = 0; i < rooms[posibleEndRooms[ID]].enemies.Count; i++)
            {
                rooms[posibleEndRooms[ID]].enemies.RemoveAt(i);
            };
            rooms[posibleEndRooms[ID]].portals.Add(new Portal());
            rooms[posibleEndRooms[ID]].portals[^1].UpdatePos(
                new Vector2(Level.windowWidth / 2 - Portal.Texture2D.Width * 0.3f / 2, 
                Level.windowHeight / 2 - Portal.Texture2D.Height * 0.3f / 2));
        }

        private static void BuildLevel()
        {
            while (queue.Count != 0)
            {
                var room = queue.Dequeue();
                if (initLevelMap[room.mapId.X, room.mapId.Y] == int.MaxValue || initLevelMap[room.mapId.X, room.mapId.Y] == -1)
                    room.Init();
                map.Add(new MapRoom());
                map[^1].CountPos(room.realMapPos, room.doorId);
            }
        }

        private static void DebugDoors()
        {
            foreach (var room in rooms)
            {
                foreach (var dr in room.doors)
                {
                    var x = Room.GetPosibleWay(room.roomMapCord, (int)dr.GetWall()).X;
                    var y = Room.GetPosibleWay(room.roomMapCord, (int)dr.GetWall()).Y;
                    dr.WayToRoomNumChange(initLevelMap[x, y]);
                }
                if (room.doors.Count == 1 && initLevelMap[room.roomMapCord.X, room.roomMapCord.Y] !=0)
                {
                    posibleEndRooms.Add(initLevelMap[room.roomMapCord.X, room.roomMapCord.Y]);
                }
            }
        }

        public static int GetHeroInRoomValue()
        {
            return HeroInRoom;
        }

        public static void AddRoom(Point mapId, int doorWallId, Vector2 realMapPos)
        {

            rooms.Add(new Room());
            if (initLevelMap[mapId.X, mapId.Y] == int.MaxValue ||
                initLevelMap[mapId.X, mapId.Y] == -1)
                initLevelMap[mapId.X, mapId.Y] = rooms.Count - 1;
            rooms[^1].InitRoom(rooms.Count - 1, mapId, doorWallId, realMapPos);

        }

        public static void ChangeHeroInRoomValue(int id)
        {
            HeroInRoom = id;
        }

        public static void InteractWithItemsAndObjects()
        {

            for (int i = 0; i < rooms[HeroInRoom].items.Count; i++)
                {
                    var item = rooms[HeroInRoom].items[i];
                    if (item.GetCollusion().Intersects(Hero1.CountCollusion()))
                    {
                        item.UseItem();
                        rooms[HeroInRoom].items.RemoveAt(i);
                        i--;
                        return;
                    }
                    
                }


            foreach (var sh in rooms[HeroInRoom].showcases)
                {
                    if (sh.GetCollusion().Intersects(Hero1.CountCollusion()) && Hero1.GetCoinsValue() >= sh.Price && !sh.GetCondition())
                    {
                        sh.Buy();
                        return;
                    }
                }


                foreach (var prt in rooms[HeroInRoom].portals)
                {
                    if (prt.GetCollusion().Intersects(Hero1.CountCollusion()))
                    {
                        prt.GoToNextLevel();
                        return;
                    }
                }
            


                    foreach (var ch in rooms[HeroInRoom].chests)
                    {
                        if (ch.GetCollusion().Intersects(Hero1.CountCollusion()) && !ch.GetCondition() && ch.closed && Hero1.GetKeysValue() > 0)
                        {
                            Hero1.ChengeKeysValue(-1);
                            ch.Open(Room.Rand);
                            return;
                        }
                    }
        }

        public static void Update()
        {
            Objects.Update(rooms[HeroInRoom].projectiles, rooms[HeroInRoom].enProjectiles, rooms[HeroInRoom].coins, rooms[HeroInRoom].keys);
            foreach (var ch in rooms[HeroInRoom].chests)
            {
                if (ch.GetCollusion().Intersects(Hero1.CountCollusion()) && !ch.GetCondition() && !ch.closed)
                    ch.Open(Room.Rand);
            }
            for (int i = 0; i < rooms[HeroInRoom].enemies.Count; i++)
            {
                var en = rooms[HeroInRoom].enemies[i];
                if (en.GetHP() <= 0)
                {
                    var random = Room.Rand.Next(0, 100);
                    if (random <= 70)
                        Objects.AddCoins(en.GetPos(), rooms[HeroInRoom].coins);
                    else if (random <= 95)
                        Loot.AddLoot(4, en.GetPos(), rooms[HeroInRoom].items);
                    else
                        Objects.AddKey(en.GetPos(), rooms[HeroInRoom].keys);
                    rooms[HeroInRoom].enemies.RemoveAt(i);
                    i--;
                }
                en.Update();
                for (int j = 0; j < rooms[HeroInRoom].projectiles.Count; j++)
                {
                    if (rooms[HeroInRoom].projectiles[j].GetCollusion().Intersects(en.GetCollusion()))
                    {
                        rooms[HeroInRoom].projectiles[j].Texture = Game1.empty;
                        en.UpdatePos(en.GetPos() + rooms[HeroInRoom].projectiles[j].CalcDir()* rooms[HeroInRoom].projectiles[j].speed);
                        if(!rooms[HeroInRoom].projectiles[j].damageGained) 
                        {
                            rooms[HeroInRoom].projectiles[j].damageGained = true;
                            en.ChengeHP(-rooms[HeroInRoom].projectiles[j].damage);
                            j--;
                        }
                    }
                }
            }
            if(rooms[HeroInRoom].enemies.Count == 0)
            foreach (var dr in rooms[HeroInRoom].doors)
            {
                    if (dr.GetCollusion().Intersects(Hero1.CountCollusion()) && !dr.GetCondition())
                    {
                        if(Game1.CheckDoorOpening((int)dr.GetWall()))
                        if (!transportFlag)
                        {
                            transportFlag = true;
                            var id = dr.GetWayToRoomNum();
                            roomPos = new Vector2(rooms[id].GetPos().X, rooms[id].GetPos().Y);
                            foreach (var door in rooms[id].doors)
                            {
                                map[door.GetWayToRoomNum()].mayVisited = true;
                                if ((((int)dr.GetWall() % 2 == 0) && ((int)dr.GetWall() + 1 == (int)door.GetWall())) ||
                                    (((int)dr.GetWall() % 2 == 1) && ((int)dr.GetWall() - 1 == (int)door.GetWall())))
                                {
                                    if (door.GetEnterPos().X != 0)
                                        Hero1.UpdatePos(new Vector2(door.GetEnterPos().X, Hero1.GetPos().Y));
                                    if (door.GetEnterPos().Y != 0)
                                        Hero1.UpdatePos(new Vector2(Hero1.GetPos().X, door.GetEnterPos().Y));
                                }
                            }
                            ChangeHeroInRoomValue(id);
                            if (map[id].visited == false)
                                map[id].visited = true;
                        }
                    }
                    else
                        transportFlag = false;
            }
        }   
        public static void DrawMap(GraphicsDevice graphicsDevice, BasicEffect effect)
        {
            for(int i = 0; i < map.Count; i++) 
            {
                if (map[i].visited)
                    map[i].Draw(graphicsDevice, effect);
                else if (map[i].mayVisited)
                    map[i].DrawCoridor(graphicsDevice, effect);
                if (i == HeroInRoom)
                {
                    Objects.spriteBatch.Begin();
                    Objects.spriteBatch.Draw(Marker, new Vector2(map[i].GetPos().X - 15, map[i].GetPos().Y - 15),
                        null, Color.White, 0, Vector2.Zero, 0.25f, SpriteEffects.None, 1);
                    Objects.spriteBatch.End();
                }
            }
        }
        public static void Draw(GraphicsDevice graphicsDevice, EffectPassCollection effectPassCollection)
        {
            rooms[HeroInRoom].Draw(graphicsDevice, effectPassCollection);
        }
    }

    class Room
    {
        public Point roomMapCord;
        VertexPositionColor[] vertexPositionColorsHorisontal;
        VertexPositionColor[] vertexPositionColorsVertical;
        VertexPositionColor[] vertexPositionColorsVerticalWall;
        VertexPositionColor[] vertexPositionColorsHorisontalWall;
        private readonly List<Vector2> EnemyPositions = new List<Vector2>();
        Vector2 Position;
        Vector2 realMapPosition;
        public readonly List<IItem> items = new List<IItem>();
        public readonly List<Projectile> projectiles = new List<Projectile>();
        public readonly List<Projectile> enProjectiles = new List<Projectile>();
        public readonly List<Coin> coins = new List<Coin>();
        public readonly List<Key> keys = new List<Key>();
        public List<Chest> chests = new List<Chest>();
        public List<SimpleEnem> enemies = new List<SimpleEnem>();
        public List<Showcase> showcases = new List<Showcase>();
        public List<Portal> portals = new List<Portal>();
        public List<Door> doors = new List<Door>();
        public static Random Rand = new Random();
        public static Texture2D DoorTexture { get; set; }

        public Vector2 GetPos()
        {
            return Position;
        }

        public void InitFirst()
        {
            realMapPosition = new Vector2(890, 470);
            Level.map.Add(new MapRoom());
            Level.map[^1].visited = true;
            Level.map[^1].CountPos(realMapPosition, -1);
            roomMapCord = new Point(5, 5);
            CountRoomSize(0);
            doors.Add(new Door());
            var door = Rand.Next(0, 3);
            Level.AddRoom(GetPosibleWay(roomMapCord, door), door, GetMapWay(door, realMapPosition));
            Level.map.Add(new MapRoom());
            Level.map[^1].CountPos(GetMapWay(door, realMapPosition), door);
            Level.map[^1].mayVisited = true;
            doors[0].InitDoor(door, Position);
            doors[0].WayToRoomNumChange(1);
        }

        public void AddDoor(int wall)
        {
            doors.Add(new Door());
            doors[^1].InitDoor(wall, Position);
        }

        public void InitRoom(int roomInd, Point mapCord, int doorWallId, Vector2 realMapPos)
        {
            roomMapCord = mapCord;
            realMapPosition = realMapPos;
            CountRoomSize(roomInd);

            if (doorWallId % 2 == 0)
                Level.rooms[Level.initLevelMap[mapCord.X, mapCord.Y]].AddDoor(doorWallId + 1);
            else if (doorWallId % 2 == 1)
                Level.rooms[Level.initLevelMap[mapCord.X, mapCord.Y]].AddDoor(doorWallId - 1);

            var flag = false;
            var doorCount = Math.Min(Rand.Next(2, 6), 4);
            for (int i = 0; i < doorCount; i++)
            {
                flag = false;
                foreach (var dr in doors)
                {
                    if
                    (
                        (dr.GetWall() == (Door.WallId)i) ||
                        (GetPosibleWay(mapCord, i).X < 0) ||
                        (GetPosibleWay(mapCord, i).Y < 0) ||
                        (Level.initLevelMap[GetPosibleWay(mapCord, i).X, GetPosibleWay(mapCord, i).Y] != int.MaxValue) ||
                        (Level.initLevelMap[GetPosibleWay(mapCord, i).X, GetPosibleWay(mapCord, i).Y] == -1)
                    )
                    {
                        flag = true;
                    }
                }
                if (flag == true) continue;
                if (Level.rooms.Count + Level.queue.Count < Level.MaxRoomInd)
                {
                    doors.Add(new Door());
                    Level.queue.Enqueue(new NotInitRoom(i, GetPosibleWay(mapCord, i), GetMapWay(i, realMapPosition)));
                    Level.initLevelMap[GetPosibleWay(mapCord, i).X, GetPosibleWay(mapCord, i).Y] = -1;

                    doors[^1].InitDoor(i, Position);
                }
            }
            GenerateObjAndEnem(roomInd);
        }

        public void GenerateObjAndEnem(int roomInd)
        {
            if (Position.X >= 500 && Position.Y >= 200)
            {
                chests.Add(new Chest());
                if (Rand.Next(1, 100) > 90)
                    chests[0].closed = false;
                else
                    chests[0].closed = true;
                chests[0].UpdatePos(new Vector2(Level.windowWidth / 2 - Chest.Texture2D.Width, Level.windowHeight / 2 - Chest.Texture2D.Height));
            }
            else if (Position.X >= 450 && Position.Y >= 200)
            {
                for (int i = 1; i <= 3; i++)
                {
                    showcases.Add(new Showcase() { ItemInCase = Loot.AddLootInCase(Rand.Next(0, 17)), Price = Rand.Next(20, 50) * Level.LevelCount });
                    showcases[^1].UpdatePos(new Vector2(Position.X + Position.X * i / 2, Level.windowHeight / 2 - Showcase.Texture2D.Height));
                }
            }
            else if (roomInd != 0)
            {
                var enemMaxAmount = Rand.Next(2, 4 * Level.LevelCount);
                for (var i = 1; i <= enemMaxAmount; i++)
                {
                    if (EnemyPositions.Count == 0)
                        return;
                    enemies.Add(new SimpleEnem());
                    var posNum = Rand.Next(0, EnemyPositions.Count - 1);
                    enemies[^1].UpdatePos(EnemyPositions[posNum]);
                    EnemyPositions.RemoveAt(posNum);
                }
            }
        }

        public void CountRoomSize(int roomInd)
        {
            if (roomInd == 0 || roomInd == -1)
                Position = new Vector2(600, 300);
            else
                Position = new Vector2(Rand.Next(100, 600), Rand.Next(100, 300));
            vertexPositionColorsHorisontal = new[]
            {
                new VertexPositionColor(new Vector3(Position.X, Position.Y, 0), Color.Black),
                new VertexPositionColor(new Vector3(Level.windowWidth - Position.X, Position.Y, 0), Color.Black),
                new VertexPositionColor(new Vector3(Position.X, Level.windowHeight-Position.Y, 0), Color.Black),
                new VertexPositionColor(new Vector3(Level.windowWidth - Position.X, Level.windowHeight-Position.Y, 0), Color.Black)
            };

            vertexPositionColorsVertical = new[]
            {
                vertexPositionColorsHorisontal[0],
                vertexPositionColorsHorisontal[2],
                vertexPositionColorsHorisontal[1],
                vertexPositionColorsHorisontal[3]
            };
            vertexPositionColorsHorisontalWall = new[]
            {
                 new VertexPositionColor(new Vector3(Position.X, Position.Y - Door.SecTexture2D.Height-2, 0), Color.Black),
                new VertexPositionColor(new Vector3(Level.windowWidth - Position.X, Position.Y - Door.SecTexture2D.Height-2, 0), Color.Black),
                vertexPositionColorsHorisontal[2],
                vertexPositionColorsHorisontal[3]
            };
            vertexPositionColorsVerticalWall = new[]
            {
                vertexPositionColorsHorisontalWall[0],
                vertexPositionColorsHorisontalWall[2],
                vertexPositionColorsHorisontalWall[1],
                vertexPositionColorsHorisontalWall[3]
            };

            for(var i = Position.X; i <= Level.windowWidth - Position.X * 2;i += Position.X)
                for (var j = Position.Y; j <= Level.windowHeight - Position.Y * 2; j += Position.Y)
                    EnemyPositions.Add(new Vector2(i, j));
        }

        public void Draw(GraphicsDevice graphicsDevice, EffectPassCollection effectPassCollection)
        {
            foreach (EffectPass pass in effectPassCollection)
            {
                var floorBorders = new Rectangle((int)this.Position.X, (int)this.Position.Y, 
                    Level.windowWidth - (int)Position.X * 2, Level.windowHeight - (int)Position.Y * 2);
                var wallBorders = new Rectangle((int)this.Position.X, (int)this.Position.Y - Door.SecTexture2D.Height, 
                    Level.windowWidth - (int)Position.X * 2, Door.SecTexture2D.Height - 1);
                pass.Apply();
                Objects.spriteBatch.Draw(Level.Torch, new Vector2(Position.X, Position.Y - Door.SecTexture2D.Height), 
                    null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.9f);
                Objects.spriteBatch.Draw(Level.Floor, Position, floorBorders, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                Objects.spriteBatch.Draw(Level.Wall, new Vector2(Position.X, Position.Y - Door.SecTexture2D.Height),
                    wallBorders, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsHorisontal, 0, 2);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsVertical, 0, 2);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsHorisontalWall, 0, 2);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsVerticalWall, 0, 2);
            }
            foreach (var dr in doors)
            {
                dr.Draw();
            }
            foreach (var ch in chests)
            {
                ch.Draw();
            }
            foreach (var en in enemies)
            {
                en.Draw();
            }
            foreach (var coin in coins)
            {
                coin.Draw();
            }
            foreach (var key in keys)
            {
                key.Draw();
            }
            foreach (var proj in projectiles)
            {
                proj.Draw();
            }
            foreach (var proj in enProjectiles)
            {
                proj.Draw();
            }
            foreach (var item in items)
            {
                Objects.spriteBatch.Draw(item.GetTexture(), item.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.7f);
            }
            foreach (var portal in portals)
            {
                portal.Draw();
            }
            foreach (var showCase in showcases)
            {
                showCase.Draw();
            }
        }

        static public Vector2 GetMapWay(int doorId, Vector2 position)
        {
            if (doorId == 0)
                return new Vector2(position.X + 80, position.Y);
            if (doorId == 1)
                return new Vector2(position.X - 80, position.Y);
            if (doorId == 2)
                return new Vector2(position.X, position.Y - 80);
            if (doorId == 3)
                return new Vector2(position.X, position.Y + 80);
            else
                return new Vector2(position.X, position.Y);
        }

        static public Point GetPosibleWay(Point mapId, int doorId)
        {
            if (doorId == 0 && mapId.Y < 9)
                return new Point(mapId.X, mapId.Y + 1);
            if (doorId == 1 && mapId.Y > 0)
                return new Point(mapId.X, mapId.Y - 1);
            if (doorId == 2 && mapId.X > 0)
                return new Point(mapId.X - 1, mapId.Y);
            if (doorId == 3 && mapId.X < 9)
                return new Point(mapId.X + 1, mapId.Y);
            else
                return new Point(-1, -1);
        }


    }

    class Door : GameObject
    {
        private static Point TextureSize;
        private static Point TextureSizeUD;
        Vector2 EnterPosition;
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
        public Vector2 GetEnterPos()
        {
            return EnterPosition;
        }

        public static void CountTextureSize(Point size, Point sizeUD)
        {
            TextureSize = size;
            TextureSizeUD = sizeUD;
        }

        public void WayToRoomNumChange(int wayID)
        {
            wayToRoomNum = wayID;
        }
        public void InitDoor(int wall, Vector2 roomPos)
        {
            Wall = (WallId)wall;
            ChengeDoorPos(roomPos);
        }

        public void ChengeDoorPos(Vector2 roomPos)
        {
            if (Wall == WallId.left)
            {
                this.UpdatePos(new Vector2(roomPos.X - 18, (Level.windowHeight - roomPos.Y - roomPos.Y) / 2 + roomPos.Y - 50));
                EnterPosition = new Vector2(roomPos.X + 30, 0);
            }
            if (Wall == WallId.right)
            {
                this.UpdatePos(new Vector2(Level.windowWidth - roomPos.X - 5, (Level.windowHeight - roomPos.Y - roomPos.Y) / 2 + roomPos.Y - 50));
                EnterPosition = new Vector2(Level.windowWidth - roomPos.X - 30 - Hero1.Texture2D.Width, 0);
            }
            if (Wall == WallId.up)
            {
                this.UpdatePos(new Vector2((Level.windowWidth) / 2 - TextureSizeUD.X + 15, roomPos.Y - TextureSizeUD.Y));
                EnterPosition = new Vector2(0, roomPos.Y + 30);
            }
            if (Wall == WallId.down)
            {
                this.UpdatePos(new Vector2((Level.windowWidth) / 2 - TextureSizeUD.X + 15, Level.windowHeight - roomPos.Y));
                EnterPosition = new Vector2(0, Level.windowHeight - roomPos.Y - 50 - Hero1.Texture2D.Height);
            }
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
                Objects.spriteBatch.Draw(Texture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            if (Wall == WallId.left)
                Objects.spriteBatch.Draw(Texture2D, new Vector2(this.GetPos().X, this.GetPos().Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            if (Wall == WallId.up)
                Objects.spriteBatch.Draw(SecTexture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            if (Wall == WallId.down)
                Objects.spriteBatch.Draw(SecTexture2D, this.GetPos(), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0.9f);
        }

        public Rectangle GetCollusion()
        {
            if ((int)Wall == 2)
                return new Rectangle((int)this.GetPos().X + (int)(TextureSize.Y / 1.5f),
                (int)this.GetPos().Y - TextureSize.X / 4 + 5, TextureSize.X / 4, TextureSize.Y / 4);
            else if ((int)Wall == 3)
                return new Rectangle((int)this.GetPos().X,
                (int)this.GetPos().Y - 10, TextureSize.Y, TextureSize.X);
            else
                return new Rectangle((int)this.GetPos().X,
                (int)this.GetPos().Y + TextureSize.X / 2, TextureSize.X, TextureSize.Y / 4);
        }
    }
    public struct NotInitRoom
    {
        public int doorId;
        public Vector2 realMapPos;
        public Point mapId;

        public NotInitRoom(int doorId, Point mapId, Vector2 realMapPos)
        {
            this.doorId = doorId;
            this.mapId = mapId;
            this.realMapPos = realMapPos;
        }

        public void Init()
        {
            Level.AddRoom(mapId, doorId, realMapPos);
        }
    }

    class MapRoom 
    {
        VertexPositionColor[] vertexPositionColorsHorisontal;
        VertexPositionColor[] vertexPositionColorsVertical;
        VertexPositionColor[] vertexPositionColorsCoridor;
        public bool visited;
        public bool mayVisited;
        Vector2 position;

        public Vector2 GetPos()
        {
            return position;
        }
        public void CountPos(Vector2 position, int doorID)
        {
            this.position = position;
            vertexPositionColorsHorisontal = new[]
            {
                new VertexPositionColor(new Vector3(position.X, position.Y, 0), Color.Black),
                new VertexPositionColor(new Vector3(position.X + 70, position.Y, 0), Color.Black),
                new VertexPositionColor(new Vector3(position.X, position.Y + 70, 0), Color.Black),
                new VertexPositionColor(new Vector3(position.X + 70, position.Y + 70, 0), Color.Black)
            };

            vertexPositionColorsVertical = new[]
            {
                vertexPositionColorsHorisontal[0],
                vertexPositionColorsHorisontal[2],
                vertexPositionColorsHorisontal[1],
                vertexPositionColorsHorisontal[3]
            };
            if (doorID % 2 == 0)
                doorID++;
            else
                doorID--;
            vertexPositionColorsCoridor = new[]
            {
                new VertexPositionColor(GetCorPos(doorID, position)[0], Color.Black),
                new VertexPositionColor(GetCorPos(doorID, position)[1], Color.Black)
            };
        }

        private Vector3[] GetCorPos(int doorID, Vector2 position)
        {
            Vector3 start = Vector3.Zero;
            Vector3 end = Vector3.Zero;
            if (doorID == 0) 
            {
                start = new Vector3(position.X + 70, position.Y + 35, 0);
                end = new Vector3(position.X + 80, position.Y + 35, 0);
            }              
            if (doorID == 1) 
            {
                start = new Vector3(position.X , position.Y + 35, 0);
                end = new Vector3(position.X - 10, position.Y + 35, 0);
            }             
            if (doorID == 2) 
            {
                start = new Vector3(position.X + 35, position.Y, 0);
                end = new Vector3(position.X + 35, position.Y - 10, 0);
            }               
            if (doorID == 3) 
            {
                start = new Vector3(position.X + 35, position.Y + 70, 0);
                end = new Vector3(position.X + 35, position.Y + 80, 0);
            }             
            return new Vector3[2] { start, end };
        }

        public void Draw(GraphicsDevice graphicsDevice, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsHorisontal, 0, 2);
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsVertical, 0, 2);
                DrawCoridor(graphicsDevice, effect);
            }
        }

        public void DrawCoridor(GraphicsDevice graphicsDevice, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertexPositionColorsCoridor, 0, 1);
            }
        }
    }
}
