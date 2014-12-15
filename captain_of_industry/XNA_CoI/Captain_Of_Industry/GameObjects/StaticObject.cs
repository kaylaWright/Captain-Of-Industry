using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public class StaticObject : GameObject
    {
        // Store the width and height of this object, for collision check purposes
        private int width;
        private int height;

        public StaticObject(OBJECT_TYPE _type) : base(_type)
        { }
        public virtual void Initialize(Vector2 _position, int _width, int _height)
        {
            position = _position;
            width = _width;
            height = _height;
        }
        // Collision checks should prevent movement through static objects
        public override void OnHit(GameObject _obj) { }

        public override void Draw(SpriteBatch _spriteBatch) { }

        // Get the width of the animation
        public override int GetWidth() { return width; }
        // Get the height of the animation
        public override int GetHeight() { return height; }
    }
}
