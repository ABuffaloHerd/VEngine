using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;

namespace VEngine.Character
{
    // Represents the maximum stats the character can have
    public struct CharacterStats
    {
        int ATK { get; set; } // attack stat added to weapon stats
        int DEF { get; set; } // defense stat
        int RES { get; set; } // magic resistance %
        int HP  { get; set; } // healthy
        int MP  { get; set; } // magical
        int SP  { get; set; } // spell card
        int SPD { get; set; } // sort by turn order
        int ACT { get; set; } // actions per turn
    }
    public class Character
    {
        public CharacterStats Stats { get; set; }
        public string Name { get; set; }
        public Color Colour { get; set; }
        public char Glyph { get; set; }
        public Character(string name, char glyph, Color colour, CharacterStats stats) 
        {
            Stats = stats;

            Name = name;
            Colour = colour;
            Glyph = glyph;
        }

        public GameObject ToGameObject()
        {
            return null;
        }
    }
}
