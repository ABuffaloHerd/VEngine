using System;
using System.Collections.Generic;
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
        public HashSet<GameObject> AllObjects;
        private GameObject?[,] objects;

        private int x, y;

        public GameObject? this[int i, int j]
        {
            get
            {
                return objects[i, j];
            }

            set
            {
                objects[i, j] = value;

                if (value != null) AllObjects.Add(value);
                else AllObjects.Remove(value);
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

                if (value != null) AllObjects.Add(value);
                else AllObjects.Remove(value);
            }
        }

        public Grid(int x, int y)
        {
            this.x = x;
            this.y = y;
            objects = new GameObject[x, y];

            AllObjects = new();
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
        }
    }
}
