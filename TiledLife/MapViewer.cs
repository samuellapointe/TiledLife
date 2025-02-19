﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledLife.World;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace TiledLife
{
    class MapViewer : GameElement
        {
        Map map;
        
        // Inputs
        MouseState mouseState;
        MouseState lastMouseState;
        Point oldMousePosition;
        int mouseScrollWheelValue;
        int oldMouseScrollWheelValue;

        double lastClickTime = 0;

        // Controls
        float zoomLevel;
        Vector2 offset;

        public MapViewer(Map map)
        {
            this.map = map;
            zoomLevel = 0.25f;
            offset = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float scale = 1 / (float)Math.Pow(2, zoomLevel);
            Matrix translationMatrix = Matrix.CreateTranslation(-offset.X, -offset.Y, 0);
            Matrix scaleMatrix = Matrix.CreateScale(scale);
            Matrix transformMatrix = Matrix.Multiply(scaleMatrix, translationMatrix);

            SamplerState samplerState;

            if (zoomLevel < -0.5)
            {
                samplerState = SamplerState.PointClamp;
            } else
            {
                samplerState = SamplerState.LinearClamp;
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, null, samplerState, null, null, null, transformMatrix);
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

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
            {
                lastClickTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            else if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                double clickLength = currentTime - lastClickTime;

                if (clickLength < 150)
                {
                    Creature.AbstractCreature creature = Map.GetInstance().GetCreatureAt(mousePosition.X, mousePosition.Y);
                    if (creature != null)
                    {
                        Debug.Print(creature.position.ToString());
                    }
                }
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
            lastMouseState = this.mouseState;
            this.mouseState = mouseState; 
        }
    }
}
