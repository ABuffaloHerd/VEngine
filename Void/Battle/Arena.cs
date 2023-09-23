using SadConsole;
using SadConsole.Effects;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Void.DataStructures;
using Void.UI;

namespace Void.Battle
{
    // The arena object keeps track of all entities and their states. 
    // The parent scene handles the rendering of entities and controls.
    public class Arena : ScreenSurface
    {
        public List<GameObject> neutralObjects; // Neutral objects.
        public List<GameObject> playerObjects; // player allied objects.
        public List<GameObject> enemyObjects; // enemy objects.
        public List<GameObject> objects; // All objects

        public GameObject Selected { get; protected set; }

        private Renderer renderer;

        public Arena(int width, int height, Color background) : base(width, height)
        {
            objects = new();
            playerObjects = new();
            enemyObjects = new();

            _ = Surface.Fill(background: background);

            Selected = null;

            renderer = new();
            SadComponents.Add(renderer);
        }

        public void Mark()
        {
            // highlight the current object's range
            Blinker blinkingEffect = new Blinker
            {
                // Blink forever
                Duration = System.TimeSpan.MaxValue,
                BlinkOutForegroundColor = Color.Black,
                // Every half a second
                BlinkSpeed = TimeSpan.FromMilliseconds(500),
                RunEffectOnApply = true
            };

            foreach (Point p in Selected.GetRange())
            {
                Surface.SetGlyph(p.X + Selected.Position.X, p.Y + Selected.Position.Y, 'X', Color.Yellow);
                Surface.SetEffect(p.X + Selected.Position.X, p.Y + Selected.Position.Y, blinkingEffect);

                // If there's an entity at this point, set it to blink
                foreach (var obj in objects)
                {
                    if (obj.Position == p + Selected.Position)
                    {
                        obj.SetVisualEffect(blinkingEffect);
                    }
                }
            }
        }


        public void Add(GameObject obj)
        {
            renderer.Add(obj);
            objects.Add(obj);

            // Figure out which bucket to put this in
            switch(obj.Alignment)
            {
                case Alignment.PLAYER:
                    playerObjects.Add(obj);
                    break;
                case Alignment.ENEMY:
                    enemyObjects.Add(obj);
                    break;
                default:
                    neutralObjects.Add(obj);
                    break;
                
            }
        }

        public void Move(Guid objID, Point position)
        {
            // move object to position.
            // Find object in objects list using given ID
            foreach(var obj in objects)
            {
                if(obj.ID == objID)
                {
                    obj.Position += position;
                }
            }

            System.Console.WriteLine("Arena move");
            Update();
        }

        // Returns a list of all entites in the currently selected object's range
        public List<GameObject> InRange()
        {
            System.Console.WriteLine("arena range check");
            List<GameObject> returnme = new();
            Pattern p = Selected.GetRange();

            foreach(Point point in p.Points)
            {
                // Adjust point so that it is relative to selected object's position
                Point cp = point + Selected.Position;

                System.Console.WriteLine("Checking " + cp);

                // Search all objects list
                foreach(var gameobj in objects)
                {
                    if (gameobj.Position == cp)
                    {
                        // Make sure this isn't the current selected object
                        if (gameobj.ID != Selected.ID)
                        {
                            System.Console.WriteLine("Rangecheck got a hit");
                            returnme.Add(gameobj);
                        }
                    }
                }
            }

            return returnme;
        }

        public void Update()
        {
            // TODO: Cycle player objects when the turn is ticked
            Selected = playerObjects[0];
        }
    }
}
