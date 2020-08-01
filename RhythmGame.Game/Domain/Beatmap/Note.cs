using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmGame.Domain.Beatmap
{
    public class Note
    {
        public int Ms;
        public int Col;

        public Note(int msTime, int colNumber)
        {
            Ms = msTime;
            Col = colNumber;
        }

        public override string ToString()
        {
            return $"{Ms},{Col}";
        }
    }
}
