using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public class DynamicObject : GameObject
    {
        public enum FACING_DIRECTION
        {
            DOWN,
            LEFT,
            RIGHT,
            UP
        };

        // Store the last non-colliding position this object was at
        protected Vector2 lastSafePosition;
        // Animation representing the player
        private Animation animation;
        // Direction this object is facing
        private FACING_DIRECTION direction;

        public DynamicObject(OBJECT_TYPE _type) : base(_type)
        { 
            
        }
        public virtual void Initialize(Animation _animation, Vector2 _position)
        {
            animation = _animation;
            position = _position;
        }
        // Collision checks should prevent movement through static objects
        public override void OnHit(GameObject _obj)
        {
            position = lastSafePosition;
        }
        // Draw to screen based on frame/direction
        public override void Draw(SpriteBatch _spriteBatch)
        {
            animation.Draw(_spriteBatch);
        }
        // Override to increment necessary elements on game update call
        public override void Update(GameTime _gameTime)
        {
            lastSafePosition = position;

            animation.Update(_gameTime, position);
        }
        // Get the width of the animation
        public override int GetWidth() { return animation.frameWidth; }
        // Get the height of the animation
        public override int GetHeight() { return animation.frameHeight; }
        // Get/set direction object is facing
        public FACING_DIRECTION GetDirection() { return direction; }
        public void SetDirection(FACING_DIRECTION _d) { direction = _d; }

    }
}
