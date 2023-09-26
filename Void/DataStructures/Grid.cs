using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Battle;

namespace Void.DataStructures
{
    /*
     * This class represents the arena as a two dimensional list. By placing and updating 
     * objects here it makes it easy to find and edit them.
     * Objects cannot overlap.
     *     */
    public class Grid
    {
        // this contains references to all objects for easy tracking
        private HashSet<GameObject> allObjects;
        private GameObject?[,] objects;

        private readonly int x, y;

        public GameObject? this[int i, int j]
        {
            get
            {
                return objects[i, j];
            }

            set
            {
                objects[i, j] = value;

                if (value != null) allObjects.Add(value);
            }
        }

        public GameObject? this[Point p]
        {
            get
            {
                return objects[p.X, p.Y];
            }

            set
            {
                objects[p.X, p.Y] = value;

                if (value != null) allObjects.Add(value);
            }
        }

        public Grid(int x, int y)
        {
            this.x = x;
            this.y = y;
            objects = new GameObject[x, y];

            allObjects = new();
        }

        public Grid(List<GameObject> input, int x, int y) : this(x, y) 
        {
            foreach(var gameobj in input)
            {
                objects[gameobj.Position.X, gameobj.Position.Y] = gameobj;
            }
        }

        // Returns true if the entity was moved, false if it was blocked
        public bool Move(Point here, Point there)
        {
            // check to make sure that it is possible to move
            if (this[there] != null) return false;

            // save here if not null
            if (this[here] != null)
            { 
                this[there] = this[here];
                this[here] = null;

                return true;
            }

            return false;
        }

        public void Clear()
        {
            objects = new GameObject[x, y];
            allObjects = new();
        }

        public bool Add(GameObject obj)
        {
            if (this[obj.Position] != null) return false;
            if (allObjects.Contains(obj)) return false;

            this[obj.Position] = obj;
            return allObjects.Add(obj);
        }

        /// <summary>
        /// This method indexes all objects in a collection to this grid.
        /// </summary>
        /// <param name="input">Input collection</param>
        /// <returns>Number of objects indexed</returns>
        public int Index(IEnumerable<GameObject> input)
        {
            int count = 0;
            foreach(GameObject obj in input)
            {
                // look at the object's position and put it down on the grid
                this[obj.Position] = obj;
                count++;
            }

            return count;
        }
    }
}
