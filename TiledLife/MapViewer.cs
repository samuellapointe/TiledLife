using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLife.World;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TiledLife
{
    class MapViewer : GameElement
        {
        Map map;
        
        // Inputs
        MouseState mouseState;
        Point oldMousePosition;
        int mouseScrollWheelValue;
        int oldMouseScrollWheelValue;

        // Controls
        float zoomLevel;
        Vector2 offset;

        public MapViewer(Map map)
        {
            this.map = map;
            zoomLevel = 2;
            offset = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float scale = 1 / (float)Math.Pow(2, zoomLevel);
            Matrix translationMatrix = Matrix.CreateTranslation(-offset.X, -offset.Y, 0);
            Matrix scaleMatrix = Matrix.CreateScale(scale);
            Matrix transformMatrix = Matrix.Multiply(scaleMatrix, translationMatrix);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transformMatrix);
            map.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            // Delta position mouse
            Point mousePosition = mouseState.Position;
            Point deltaPosition = Point.Zero;
            if (oldMousePosition != null)
            {
                deltaPosition = oldMousePosition - mousePosition;
            }
            oldMousePosition = mousePosition;

            // Drag mouse (panning)
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                offset = new Vector2(offset.X + deltaPosition.X, offset.Y + deltaPosition.Y);
            }

            // Delta scroll zoom
            mouseScrollWheelValue = mouseState.ScrollWheelValue;
            int deltaScrollWheelValue = oldMouseScrollWheelValue - mouseScrollWheelValue;
            if (deltaScrollWheelValue != 0)
            {
                oldMouseScrollWheelValue = mouseScrollWheelValue;

                float oldScale = (float)Math.Pow(2, zoomLevel);
                float virtualMouseX = (mousePosition.X + offset.X) * oldScale;
                float virtualMouseY = (mousePosition.Y + offset.Y) * oldScale;

                zoomLevel += (float)deltaScrollWheelValue / 256;
                float newScale = (float)Math.Pow(2, zoomLevel);

                float offsetX = (virtualMouseX / newScale) - mousePosition.X;
                float offsetY = (virtualMouseY / newScale) - mousePosition.Y;

                offset = new Vector2(offsetX, offsetY);
            }
        }

        public void UpdateMouseState(MouseState mouseState)
        {
            this.mouseState = mouseState; 
        }
    }
}
