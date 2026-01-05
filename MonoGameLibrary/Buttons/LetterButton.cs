using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Buttons
{
    public class LetterButton
    {
        public char Letter { get; set; }
        public Rectangle Bounds { get; set;}
        public Rectangle SourceRect { get; set;} 
        public bool Enabled = true;
        public GuessBoxState State { get; set; } = GuessBoxState.Empty;
        public Color Color => State switch
        {
            GuessBoxState.Correct => Color.Green,
            GuessBoxState.Present => Color.Yellow,
            GuessBoxState.Absent => Color.Gray,
            _ => Color.White,
        };
    }
}