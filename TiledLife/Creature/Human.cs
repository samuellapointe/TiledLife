using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TiledLife.Creature
{
    class Human : AbstractCreature
    {
        private Texture2D texture;
        private float maxThirst = 100;

        public Human(Vector2 position)
        {
            thirst = 0f;
            this.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, null, null, 0, new Vector2(0.10f, 0.10f));
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Human");
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
