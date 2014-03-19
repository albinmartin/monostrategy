using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoStrategy.GameStates
{
    interface IGameState
    {
        void Update(float elapsedTime);
        void Draw(SpriteBatch spritebatch);
    }
}
