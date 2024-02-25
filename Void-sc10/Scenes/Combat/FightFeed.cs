using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Scenes.Combat
{
    internal class FightFeed : Console
    {
        private DoubleEndedStack<string> buffer;

        private readonly Point defaultPosition = (98, 33);
        private const int defaultSize = 12;
        public FightFeed(int width, int height) : base(width, height)
        {
            buffer = new();
            Surface.UsePrintProcessor = true;

            FontSize = ((int)(defaultSize / 1.5), defaultSize);

            Resize((int)(width * 1.5f), height, true);

            Position = ((int)(defaultPosition.X * 1.5), defaultPosition.Y);
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            Surface.Clear();

            int y = this.Height;
            foreach(string str in buffer) 
            {
                if (y == this.Height)
                    Surface.Print(0, --y, "[c:g b:Orange:Black:10]" + str);
                else
                    Surface.Print(0, --y, str);
            }
        }

        public void Print(string str)
        {
            buffer.Push(str);

            if (buffer.Size > Height)
            {
                  buffer.PopHead();
            }
        }
        
    }

    /// <summary>
    /// Since the builtin stack class uses an array, removing the head is going to be a nightmare of array shuffling and will take forever.
    /// This implementation will use a single linked list
    /// </summary>
    /// <typeparam name="T">A type</typeparam>
    internal class DoubleEndedStack<T> : IEnumerable<T>
    {
        private Node? head; // First item added
        private Node? tail; // Last item added
        public uint Size { get; private set; }

        public DoubleEndedStack()
        {
            head = null;
            tail = null;
            Size = 0;
        }

        public void Push(T val)
        {
            Node node = new(val);
            if(Size == 0)
            {
                head = node;
                tail = head;
            }
            else
            {
                tail.Next = node; // Link the current tail's next to the new node
                node.Prev = tail; // Set the new node's previous to the current tail
                tail = node;

                if (Size == 1)
                    head.Next = node;
            }

            Size++;
        }

        public T PopHead()
        {
            if (Size == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            Node node = head;
            head = node.Next;

            if (head != null)
            {
                head.Prev = null;
            }
            else
            {
                // If head is null after popping, it means the stack is now empty
                tail = null;
            }

            Size--;
            return node.Data;
        }


        public T PopTail()
        {
            if (Size == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }

            Node node = tail;
            tail = node.Prev;

            if (tail != null)
            {
                tail.Next = null;
            }

            Size--;
            return node.Data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DoubleStackEnumerator(tail);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DoubleStackEnumerator : IEnumerator<T>
        {
            Node? current;
            Node tail;

            public DoubleStackEnumerator(Node tail)
            {
                current = null;
                this.tail = tail;
            }

            public T Current => current.Data;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // nothing to dispose of
            }

            public bool MoveNext()
            {
                if (current == null)
                {
                    current = tail;
                    return true;
                }    

                if (current.Prev == null) return false;
                current = current.Prev;
                

                return true;
            }

            public void Reset()
            {
                current = null;
            }
        }

        private class Node
        {
            public T Data;
            public Node? Next { get; set; }
            public Node? Prev { get; set; }

            public Node(T data)
            {
                Data = data;
            }
        }
    }
}
