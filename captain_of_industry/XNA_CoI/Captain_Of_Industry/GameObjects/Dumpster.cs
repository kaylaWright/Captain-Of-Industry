using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public class Dumpster : VisualStaticObject
    {
        public Dumpster(Texture2D _texture, Vector2 _position)
            : base(_texture, _position, OBJECT_TYPE.DUMPSTER)
        {
            
        }

        // Override to increment necessary elements on game update call
        public virtual void Update(GameTime gameTime) 
        { 
        
        }
    }
}
