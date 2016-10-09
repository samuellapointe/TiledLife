using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife
{
    // Game elements can be rendered and updated
    interface GameElement
    {
        void LoadContent(ContentManager content);
        void UnloadContent();
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        
    }
}
