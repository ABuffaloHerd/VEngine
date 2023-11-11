using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Character
{
    public struct CharacterStats
    {
        int ATK { get; set; } // attack stat added to weapon stats
        int DEF { get; set; } // defense stats added to armour pieces
        int RES { get; set; } // magic resistance %
        int HP  { get; set; } // healthy
        int MP  { get; set; } // magical
        int SP  { get; set; } // spell card
        int SPD { get; set; } // sort by turn order
        int ACT { get; set; } // actions per turn
    }
    public class Character
    {
        public Character(string name, char glyph) 
        { 

        }
    }
}
