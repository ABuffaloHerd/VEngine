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

// TODO: fix funny fart variable names
namespace Void.Battle
{
    // The arena object keeps track of all entities and their states. 
    // The parent scene handles the rendering of entities and controls.
    public class Arena : ScreenSurface
    {
        private HashSet<GameObject> objects; // All objects
        private Stack<GameObject> turnStack; // whose turn is it?
        private Grid tracker; // The position tracker

        public GameObject Selected { get; protected set; }

        private EntityManager entityManager;

        public Arena(int width, int height, Color background) : base(width, height)
        {
            objects = new();
            tracker = new(width, height);

            _ = Surface.Fill(background: background);

            Selected = null;

            entityManager = new();
            SadComponents.Add(entityManager);
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
                Point posfinal = p + Selected.Position;

                if (tracker[posfinal] == null) continue;
                if (tracker[posfinal].ID == Selected.ID) continue;

                tracker[posfinal].SetBlinker(true);
            }
        }


        public void Add(GameObject obj)
        {
            entityManager.Add(obj);
            objects.Add(obj);

            // when objects are added build the turn stack. TODO: This is another feature entirely.
            List<GameObject> temp = new(objects);
            temp.Sort(new GameObject.SpeedComparer());

            // Sort out the tracker
            tracker.Index(objects);

            turnStack = new(temp);
            Selected = turnStack.Pop();

            if (Selected is ControllableGameObject)
            {
                // Get controls
                System.Console.WriteLine("Controllable game object selected. Get controls here!");
            }
            else
            {
                System.Console.WriteLine("NPC selected");
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
        }

        public void Move(Point position)
        {
            // Move selected to position
            Selected.Position += position;

            // Update the grid
            tracker.Index(objects);
        }

        /// <summary>
        /// Returns a list of all entites in the currently selected object's range
        /// </summary>
        /// <param name="controlledobject">Includes the currently selected object if true</param>
        /// <returns>List of entities in object's range.</returns>
        public List<GameObject> InRange(bool controlledobject = false)
        {
            List<GameObject> returnme = new();
            Pattern p = Selected.GetRange();

            foreach(Point point in p.Points)
            {
                // Adjust point so that it is relative to selected object's position
                Point cp = point + Selected.Position;

                // Search the grid
                if (tracker[cp.X, cp.Y] != null)
                {
                    System.Console.WriteLine("Found " + tracker[cp.X, cp.Y]);
                    returnme.Add(tracker[cp.X, cp.Y]);
                }
            }

            if (!controlledobject) returnme.Remove(Selected);

            return returnme;
        }

        /// <summary>
        /// HOW OVERRIDE EXTENSION METHOD
        /// </summary>
        public void ResetObjects()
        {
            foreach(GameObject obj in objects)
            {
                obj.SetBlinker(false);
            }
        }
    }
}
