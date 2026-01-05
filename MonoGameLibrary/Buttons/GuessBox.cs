using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace MonoGameLibrary.Buttons
{
    public enum GuessBoxState
    {
        Correct,
        Present,
        Absent,
        Empty
    }

    public class GuessBox
    {
        public char Letter { get; set;}
        public Color Color => State switch
        {
            GuessBoxState.Correct => Color.Green,
            GuessBoxState.Present => Color.Yellow,
            GuessBoxState.Absent => Color.Gray,
            _ => Color.White,
        };
        public GuessBoxState State { get; set; } = GuessBoxState.Empty;
    }
}