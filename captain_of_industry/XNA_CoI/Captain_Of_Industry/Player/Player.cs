using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    class Player : DynamicObject
    {
        private float health, sanity, wealth;
        private InventoryManager inventoryManager = new InventoryManager();
        // Flag to toggle inventory visibility
        private bool inventoryOpen;
        // Store the last collided object, for purposes of dumpsters and stores
        private GameObject lastCollisionObject;
        // Flags for movement key presses
        private struct MOVE_FLAGS
        {
            public bool moveUp;
            public bool moveDown;
            public bool moveLeft;
            public bool moveRight;
            public MOVE_FLAGS(bool _set)
            {
                moveUp = _set;
                moveDown = _set;
                moveLeft = _set;
                moveRight = _set;
            }
        };
        private MOVE_FLAGS moveFlags;

        public Player(Animation _animation, Vector2 _position)
            : base(OBJECT_TYPE.PLAYER)
        {
            Initialize(_animation, _position);
        }

        // Setup player variables
        public override void Initialize(Animation _animation, Vector2 _position)
        {
            health = 100;
            sanity = 100;
            wealth = 0;
            base.Initialize(_animation, _position);
        }

        // Decrement/increase player stats, based on items
        // weather, conditions, etc
        public void StatUpdate()
        {
            CheckValues();
        }
        public void UseItem(int slot)
        {/*
            if (inventory[slot].type == ITEM_TYPE.EMPTY)
            {
                // do nothing for an empty slot, maybe feedback?
            }
            else if (inventory[slot].type == ITEM_TYPE.CONSUMABLE)
            {
                tempConsumable = (ConsumableItem)inventoryManager.itemList[slot];
                health += tempConsumable.healthVal * tempConsumable.quality;
                sanity += tempConsumable.sanityVal * tempConsumable.quality;
                wealth += tempConsumable.wealthVal * tempConsumable.quality;
                CheckValues();
            }
            else if (inventory[slot].type == ITEM_TYPE.USABLE)
            {

            }*/
        }
        public override void OnHit(GameObject _obj)
        {
            // If this, colliding object is above
            moveFlags.moveUp = false;
            moveFlags.moveDown = false;
            moveFlags.moveRight = false;
            moveFlags.moveLeft = false;
            lastCollisionObject = _obj;
            base.OnHit(_obj);
        }

        public override void Update(GameTime _gameTime)
        {
            if (inventoryOpen == false)
            {
                MovePlayer();
            }
            
            base.Update(_gameTime);
        }
        // Check registered keys and set flags as necessary
        // Should be called by factory, prior to checking collision
        public void CheckInput()
        {
            moveFlags = new MOVE_FLAGS();
            // Check direction movement flags
            if (InputManager.IsKeyDown(InputManager.IA.DOWN) == true)
                moveFlags.moveDown = true;
            if (InputManager.IsKeyDown(InputManager.IA.UP) == true)
                moveFlags.moveUp = true;   
            if (InputManager.IsKeyDown(InputManager.IA.LEFT) == true)
                moveFlags.moveLeft = true;
            if (InputManager.IsKeyDown(InputManager.IA.RIGHT) == true)
                moveFlags.moveRight = true;
            // Player hit interact button
            if (InputManager.WasKeyReleased(InputManager.IA.USE_INTERACT) == true)
            {
                // If they did so with the inventory closed, then check what they are standing on for interactable
                if (inventoryOpen == false)
                {
                    // Ensure their has been a collision object of some sort
                    if (lastCollisionObject != null)
                    {
                        // Trigger event for searching a store
                        if (lastCollisionObject.GetEntityType() == OBJECT_TYPE.STORE)
                        {
                            // open store window
                        }
                        // Trigger event for visiting a dumpster
                        else if (lastCollisionObject.GetEntityType() == OBJECT_TYPE.DUMPSTER)
                        {
                            // pass item from dumpster to player
                            sanity -= 3;
                        }
                    }
                }
            }
            if (InputManager.WasKeyReleased(InputManager.IA.TOGGLE_INVENTORY) == true)
            {
                inventoryOpen = !inventoryOpen;
            }
        }
        // Move player based on movement flags
        public void MovePlayer()
        {
            if (moveFlags.moveUp == true)
                position.Y -= 1;
            if (moveFlags.moveDown == true)
                position.Y += 1;
            if (moveFlags.moveLeft == true)
                position.X -= 1;
            if (moveFlags.moveRight == true)
                position.X += 1;
        }
        // Check values for health and sanity
        // Decreasing as need be
        public void CheckValues()
        {
            DecrementHealth(1.0f);
            if (health > 100)
            {
                health = 100;
            }
            else if (health < 50)
            {
                DecrementSanity(2.0f);
            }
            else if (health < 0)
            {
                // game over
            }
            if (sanity > 100)
            {
                sanity = 100;
            }
            else if (sanity < 50)
            {
                DecrementHealth(0.5f);
            }
            else if (sanity < 0)
            {
                // crazy mode
                sanity = 0;
            }
            // realistically, should never trigger
            // MD - ^Not true. I know lots of people with negative net worth
            if (wealth < 0)
            {
                wealth = 0;
            }
        }

        void DecrementHealth(float _baseVal)
        {
            health -= _baseVal;
        }

        void DecrementSanity(float _baseVal)
        {
            sanity -= _baseVal;
        }
        // Set the object the player last collided with
        public void SetLastCollisionOverlap(GameObject _obj) { lastCollisionObject = _obj; }
        // How many gets passed to the player
        public void GrowRicher(float _f) { wealth += _f; }
        // Getting player values
        public float GetHealth() { return health; }
        public float GetSanity() { return sanity; }
        public float GetWealth() { return wealth; }
    }
}
