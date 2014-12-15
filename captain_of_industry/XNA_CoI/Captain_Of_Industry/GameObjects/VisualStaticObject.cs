using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public class VisualStaticObject : StaticObject
    {
        // Stores the texture for this object
        Texture2D texture;
        // Stores the target rectangle to draw to the map
        Rectangle destinationRect;
        // Stores the source rectangle in spritesheet
        Rectangle sourceRect;

        public VisualStaticObject(Texture2D _texture, Vector2 _position, OBJECT_TYPE _type)
            : base(_type)
        {
            texture = _texture;
            Initialize(_position, texture.Width, texture.Height);
        }

        public override void Initialize(Vector2 _position, int _width, int _height)
        {
            destinationRect = new Rectangle((int)_position.X,
                (int)_position.Y,
                texture.Width,
                texture.Height);
            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            base.Initialize(_position, _width, _height);
        }
        public override void Draw(SpriteBatch _spriteBatch) 
        {
            _spriteBatch.Draw(texture, destinationRect, sourceRect, Color.White);
        }
        // Override to increment necessary elements on game update call
        public override void Update(GameTime gameTime) 
        { 
        
        }
        // Override for collision checks
        public override void OnHit(GameObject _obj)
        {

        }
    }
}
