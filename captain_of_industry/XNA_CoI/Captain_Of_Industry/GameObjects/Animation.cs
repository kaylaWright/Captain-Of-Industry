// Animation.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        // The scale used to display the sprite strip
        float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int frameWidth;

        // Height of a given frame
        public int frameHeight;

        // The state of the Animation
        public bool isActive;

        // Determines if the animation will keep playing or deactivate after one run
        public bool isLooping;

        public void Initialize(Texture2D _texture, int _frameWidth, int _frameHeight, int _frameCount,
                                int _frametime, Color _color, float _scale, bool _looping, bool _startPlaying)
        {
            // Keep a local copy of the values passed in
            this.color = _color;
            this.frameWidth = _frameWidth;
            this.frameHeight = _frameHeight;
            this.frameCount = _frameCount;
            this.frameTime = _frametime;
            this.scale = _scale;

            isLooping = _looping;
            spriteStrip = _texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            isActive = _startPlaying;
        }

        // Should move to the next frame of animation, based on gametime
        public void Update(GameTime _gameTime, Vector2 _position)
        {
            // Do not alter frame if this is not active
            if (isActive == true)
            {
                // Update the elapsed time
                elapsedTime += (int)_gameTime.ElapsedGameTime.TotalMilliseconds;

                // If the elapsed time is larger than the frame time
                // we need to switch frames
                if (elapsedTime > frameTime)
                {
                    // Move to the next frame
                    currentFrame++;

                    // If the currentFrame is equal to frameCount reset currentFrame to zero
                    if (currentFrame == frameCount)
                    {
                        currentFrame = 0;
                        // If we are not looping deactivate the animation
                        if (isLooping == false)
                            isActive = false;
                    }

                    // Reset the elapsed time to zero
                    elapsedTime = 0;
                }
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)_position.X, (int)_position.Y,
            (int)(frameWidth * scale),
            (int)(frameHeight * scale));
        }

        // Draw the Animation Strip
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
        }

        // Draw the Animation Strip
        public void DrawFrame(SpriteBatch _spriteBatch, int _x, int _y, int _frameNumber)
        {
            // Grab image from correct frame and then draw to screen
            sourceRect = new Rectangle(_frameNumber * frameWidth, 0, frameWidth, frameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle(_x * (int)(frameWidth * scale),
            _y * (int)(frameHeight * scale) + (32),
            (int)(frameWidth * scale),
            (int)(frameHeight * scale));

            _spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
        }
    }
}
