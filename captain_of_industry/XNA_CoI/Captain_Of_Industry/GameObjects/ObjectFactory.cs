// Comment out to disable collider draws
//#define DEBUG_COLLIDERS

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    // Used for index access to all animations
    public enum ANIMATIONS
    {
        PLAYER = 0,
        PEDESTRIAN1,
        PEDESTRIAN2,
        PEDESTRIAN3,
        ANIMATION_COUNT
    };
    // Used for index access to all textures
    public enum TEXTURES
    {
        DUMPSTER = 0,
        INTERACT_BOX,
        DEBUG_BOX,
        TEXTURE_COUNT
    };

    class ObjectFactory
    {
        // Structure of all objects loaded into factory
        private List<GameObject> objects;
        private Player player;
        private List<Animation> animations;
        private List<Texture2D> textures;
        private Animation mapAnimation;
        private MapInfo mapInfo;
        // How often player stats should decrement
        private TimeSpan lastStatUpdate;
        // Incremented by delta time, once it hits gameTick we do a stat update
        private TimeSpan timeBetweenUpdates;
        private static int objectIDGen;
        // Called in all object constructors to create a unique object ID
        public static int GetID()
        {
            objectIDGen++;
            return objectIDGen;
        }
        // Values for tracking total objects in array, and max objects allowed
        private int totalObjects;

        // Load NPC sprites

        //npc spawn locations. 
        private List<Vector2> npcPositions;

        // private sprites

        // Constructor only needs the spritesheet for the game world
        public ObjectFactory()
        { }
        public void UpdateCollision(GraphicsDevice _graphics, SpriteBatch _spriteBatch, bool _debug)
        {
            player.CheckInput();
            player.MovePlayer();
            // Use the Rectangle's built-in intersect functionto 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.position.X,
            (int)player.position.Y,
            player.GetWidth() - 6,
            player.GetHeight() - 6);
            // Reset last collision overlap
            player.SetLastCollisionOverlap(null);
            // Do the collision between the player and the enemies
            for (int i = 0; i < objects.Count; i++)
            {
                rectangle2 = new Rectangle((int)objects[i].position.X,
                (int)objects[i].position.Y,
                objects[i].GetWidth() - 4,
                objects[i].GetHeight() - 4);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // If it is a static object, call collision on player
                    if (objects[i].GetEntityType() == OBJECT_TYPE.STATIC)
                    {
                        player.OnHit(objects[i]);
                    }
                    // Otherwise, player is overlapping this object - pass it into player
                    else
                    {
                        player.SetLastCollisionOverlap(objects[i]);
                    }
                }
            }
        }

        /* Interact with the object at this position on the map, the params passed in should be 
        *  relative to the map grid. Meaning an x,y array reference (this can be calculated
        *  by dividing x and y position by 32 to get array x and y values of that position. 
        *  For the time being it is assumed the player is triggering this event! */
        static public void InteractWithObject(/*int _x, int _y*/)
        {
            //DynamicObject temp = (DynamicObject)currentOverlap;
        }
        // Create lists for objects, and init counter variable
        public void Initialize()
        {
            objects = new List<GameObject>();
            animations = new List<Animation>((int)ANIMATIONS.ANIMATION_COUNT);
            textures = new List<Texture2D>((int)TEXTURES.TEXTURE_COUNT);
            objectIDGen = 1;
            timeBetweenUpdates = TimeSpan.FromSeconds(3.0);

            //npc positions
            npcPositions = new List<Vector2>();
            npcPositions.Add(new Vector2(19 * 32, 5 * 32));
            npcPositions.Add(new Vector2(7 * 32, 11 * 32));
            npcPositions.Add(new Vector2(12 * 32, 15 * 32));
            npcPositions.Add(new Vector2(5 * 32, 20 * 32));
            //{ Vector2(19 * 32, 5 * 32) , Vector2(7 * 32, 11 * 32) , Vector2(12 * 32, 15 * 32) , Vector2(5 * 32, 20 * 32) };  
        }

        // Setup map based on width height - this should be changed to read in values from file eventually
        public void LoadMap(int _width, int _height)
        {
            mapInfo = new MapInfo(_width, _height, 32);
            mapInfo.Initialize(this);
        }

        // Load all animations/textures for objects
        // Revamp to use an array of string to load all content
        public void LoadContent(ContentManager _content)
        {
            // Init and load in animations
            Animation tempAnimation = new Animation();
            Texture2D tempTexture = _content.Load<Texture2D>("Art Assets/Player");
            tempAnimation.Initialize(tempTexture, 32, 32, 4, 30, Color.White, 1, true, false);
            animations.Add(tempAnimation);

            tempAnimation = new Animation();
            tempTexture = _content.Load<Texture2D>("Art Assets/AndrewNPC");
            tempAnimation.Initialize(tempTexture, 32, 32, 4, 30, Color.White, 1, true, false);
            animations.Add(tempAnimation);

            tempAnimation = new Animation();
            tempTexture = _content.Load<Texture2D>("Art Assets/MattNPC");
            tempAnimation.Initialize(tempTexture, 32, 32, 4, 30, Color.White, 1, true, false);
            animations.Add(tempAnimation);

            tempAnimation = new Animation();
            tempTexture = _content.Load<Texture2D>("Art Assets/SeanNPC");
            tempAnimation.Initialize(tempTexture, 32, 32, 4, 30, Color.White, 1, true, false);
            animations.Add(tempAnimation);

            tempAnimation = new Animation();
            tempTexture = _content.Load<Texture2D>("Art Assets/TileSpriteSheet");
            tempAnimation.Initialize(tempTexture, 16, 16, 78, 30, Color.White, 2, true, false);
            mapAnimation = tempAnimation;

            // Init and load in textures
            tempTexture = _content.Load<Texture2D>("Art Assets/Dumpster");
            textures.Add(tempTexture);

            tempTexture = _content.Load<Texture2D>("Art Assets/InteractionBox");
            textures.Add(tempTexture);

            tempTexture = _content.Load<Texture2D>("Art Assets/CollisionBox");
            textures.Add(tempTexture);
        }

        // Add the specified object to the factory
        public void AddObject(GameObject _newObject)
        {
            // If total objects has reached negative value, reset to 0 (debug line as this should not happen)
            if (totalObjects == -1)
                totalObjects = 0;
            // Add object to highest position, then increment total objects
            objects.Add(_newObject);
            totalObjects++;
        }

        // Specifify the type of object you want the factory to create
        // and the position you want it at
        public void AddObjectOfTypeAtPosition(OBJECT_TYPE _type, Vector2 _pos)
        {
            switch (_type)
            {
                case OBJECT_TYPE.STORE:
                    Store tempStore = new Store(textures[(int)TEXTURES.INTERACT_BOX], _pos);
                    AddObject(tempStore);
                    break;

                case OBJECT_TYPE.DUMPSTER:
                    Dumpster tempDump = new Dumpster(textures[(int)TEXTURES.DUMPSTER], _pos);
                    AddObject(tempDump);
                    break;

                case OBJECT_TYPE.NPC:

                    break;

                case OBJECT_TYPE.STATIC:
                default:
                    StaticObject tempStatic = new StaticObject(OBJECT_TYPE.STATIC);
                    tempStatic.Initialize(_pos, mapInfo.GetTileSize(), mapInfo.GetTileSize());
                    AddObject(tempStatic);
                    break;
            }
        }

        // Remove all objects from the list
        public void Clear()
        {
            objects.Clear();
            totalObjects = 0;
        }

        // Update all objects in the factory
        public void UpdateAll(GameTime _gameTime)
        {
            if (_gameTime.TotalGameTime - lastStatUpdate > timeBetweenUpdates)
            {
                lastStatUpdate = _gameTime.TotalGameTime;
                // Update all time based spawns
                player.StatUpdate();
            }
            int j, k;
            int tileSize = mapInfo.GetTileSize();
            for (int i = 0; i < totalObjects; i++)
            {
                objects[i].Update(_gameTime);
                // Determine what this object is standing on
                if (objects[i].GetEntityType() == OBJECT_TYPE.NPC)
                {
                    j = (int)((objects[i].position.X + 8) / tileSize);
                    k = (int)((objects[i].position.Y - 26) / tileSize);
                    objects[i].SetTileType(mapInfo.tileType[j, k]);
                }
            }
            player.Update(_gameTime);
            // Determine and update what the player is standing on
            j = (int)((player.position.X + 8) / tileSize);
            k = (int)((player.position.Y - 26) / tileSize);
            player.SetTileType(mapInfo.tileType[j, k]);
        }

        // Update all visuals
        public void RenderAll(SpriteBatch _spriteBatch)
        {
            // Iterate through map array and draw per frame of map animation
            for (int i = mapInfo.GetWidth() - 1; i >= 0; i--)
            {
                for (int j = mapInfo.GetHeight() - 1; j >= 0; j--)
                {
                    mapAnimation.DrawFrame(_spriteBatch, i, j, mapInfo.tile[i, j]);
                }
            }

            // Iterate through objects and do draw calls
            for (int i = 0; i < totalObjects; i++)
            {
                objects[i].Draw(_spriteBatch);
                if (objects[i].GetEntityType() == OBJECT_TYPE.DUMPSTER)
                {

                }
            }

            player.Draw(_spriteBatch);

#if DEBUG_COLLIDERS
            {
                Rectangle rect = new Rectangle();
                // Iterate through objects and do draw calls
                for (int i = 0; i < totalObjects; i++)
                {
                    rect = new Rectangle((int)objects[i].position.X, (int)objects[i].position.Y, 32, 32);
                    _spriteBatch.Draw(textures[(int)TEXTURES.DEBUG_BOX], rect, Color.Pink);
                }

                rect = new Rectangle((int)player.position.X, (int)player.position.Y, 32, 32);
                _spriteBatch.Draw(textures[(int)TEXTURES.DEBUG_BOX], rect, Color.Pink);
            }
#endif
        }

        // Remove the specified GameObject from the factory
        public bool Destroy(GameObject _target)
        {
            // Iterate through and find matching object
            for (int i = 0; i < totalObjects; i++)
            {
                // When match is found call Destroy with index
                if (objects[i] == _target)
                    return DestroyAtIndex(i);
            }
            // Returns false if target was not found
            return false;
        }

        // Remove the first GameObject with matching name from the factory
        public bool Destroy(string _name)
        {
            // Iterate through and find matching object
            for (int i = 0; i < totalObjects; i++)
            {
                // When match is found call Destroy with index
                if (_name == objects[i].GetName())
                    return DestroyAtIndex(i);
            }
            // Returns false if target was not found
            return false;
        }

        // Remove an GameObject with matching x and y position from the factory
        public bool Destroy(int _targetX, int _targetY)
        {
            // Iterate through and find matching object
            for (int i = 0; i < totalObjects; i++)
            {
                // When match is found call Destroy with index
                if (objects[i].position.X == _targetX)
                    if (objects[i].position.Y == _targetY)
                        return DestroyAtIndex(i);
            }
            // Returns false if target was not found
            return false;
        }

        // Destroys target index
        // For effeciency, swaps target index with last index then pops back
        public bool DestroyAtIndex(int _index)
        {
            // Ensure index requested is within bounds of exisiting objects
            if (_index > totalObjects || totalObjects <= 0)
                return false;

            // Swap given index with last object in array, and decrement size
            totalObjects--;
            objects[_index] = objects[totalObjects];
            objects.RemoveAt(totalObjects);
            return true;
        }

        // Get a game object, based on the supplied index
        public GameObject GetObject(int _index)
        {
            if (_index > -1 && _index < totalObjects)
                return objects[_index];
            return null;
        }

        // Get a game object, based on target objects name
        public GameObject GetObject(string _name)
        {
            for (int i = 0; i < totalObjects; i++)
            {
                if (objects[i].GetName() == _name)
                    return objects[i];
            }
            return null;
        }

        // Get a game object based on its x and y position
        public GameObject GetObject(int _x, int _y)
        {
            for (int i = 0; i < totalObjects; i++)
            {
                if (objects[i].position.X == _x && objects[i].position.Y == _y)
                    return objects[i];
            }
            return null;
        }

        // Create player and NPCs, place onto map - should be called after content is loaded
        public void SetupLevel()
        {
            LoadMap(32, 23);

            player = new Player(animations[(int)ANIMATIONS.PLAYER], new Vector2(160, 160));

            //Add some npcs to map. 
            Random rnd = new Random();
            int rndNum = rnd.Next(0, npcPositions.Count);
            Pedestrian pTemp = new Pedestrian(animations[(int)ANIMATIONS.PEDESTRIAN1], npcPositions[rndNum]);
            npcPositions.RemoveAt(rndNum);
            AddObject(pTemp);

            pTemp = new Pedestrian(animations[(int)ANIMATIONS.PEDESTRIAN2], npcPositions[rndNum]);
            npcPositions.RemoveAt(rndNum);
            AddObject(pTemp);

            pTemp = new Pedestrian(animations[(int)ANIMATIONS.PEDESTRIAN3], npcPositions[rndNum]);
            npcPositions.RemoveAt(rndNum);
            AddObject(pTemp);

            // Add some dumpsters to the map
            Dumpster temp = new Dumpster(textures[(int)TEXTURES.DUMPSTER], new Vector2(320, 64));
            AddObject(temp);
            temp = new Dumpster(textures[(int)TEXTURES.DUMPSTER], new Vector2(32, 608));
            AddObject(temp);
            temp = new Dumpster(textures[(int)TEXTURES.DUMPSTER], new Vector2(768, 704));
            AddObject(temp);
            temp = new Dumpster(textures[(int)TEXTURES.DUMPSTER], new Vector2(928, 64));
            AddObject(temp);
        }

        public float GetPlayerHealth() { return player.GetHealth(); }
        public float GetPlayerSanity() { return player.GetSanity(); }
        public float GetPlayerWealth() { return player.GetWealth(); }

    }
}
// MD