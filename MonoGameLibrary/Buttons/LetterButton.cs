using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Buttons
{
    public class LetterButton
    {
        public char Letter { get; }
        public Vector2 Position { get; }
        public Rectangle Bounds { get; }

        private SpriteFont _font;

        public LetterButton(char letter, Vector2 pos, int size, SpriteFont font)
        {
            Letter=letter;
            Position=pos;
            _font=font;
            Bounds = new Rectangle((int)pos.X, (int)pos.Y, size, size);
        }
    }
}