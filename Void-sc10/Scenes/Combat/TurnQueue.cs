using SadRogue.Primitives.GridViews;
using System;
using System.Collections;
using System.Text;
using VEngine.Logging;
using VEngine.Objects;

namespace VEngine.Scenes.Combat
{
    /// <summary>
    /// Statically implements a queue sorted by the GameObject's speed stat
    /// </summary>
    internal class TurnQueue : IEnumerable<GameObject>
    {
        public int Size { get; private set; }

        private Node head;

        public TurnQueue() 
        {

        }  

        public GameObject Dequeue()
        {
            Node store = head;
            head = head.Next;
            Size--;

            return store.obj;
        }

        /// <summary>
        /// Enqueue this object into its sorted position
        /// </summary>
        /// <param name="obj">New object</param>
        public void Enqueue(GameObject obj)
        {
            Node newNode = new Node(obj);
            Size++;  // Increment size for every new node

            if (Size == 1)
            {
                head = newNode;
            }
            else
            {
                // Go to the end, link, sort
                Node current = head;

                while (current.Next != null)
                    current = current.Next;

                current.Next = newNode;
            }

            Sort();
        }

        public void Enqueue(ICollection<GameObject> collection)
        {
            foreach (var obj in collection)
                Enqueue(obj);
        }

        /// <summary>
        /// Sort using merge sort
        /// </summary>
        public void Sort()
        {
            if (Size < 2) return;

            head = MergeSort(head);
        }

        private Node MergeSort(Node node)
        {

            // Guard case
            if (node == null || node.Next == null)
                return node;

            // recursive halve
            Node half = GetMiddle(node);
            Node mid = half.Next;
            half.Next = null;

            Node left = MergeSort(node);
            Node right = MergeSort(mid);

            return MergeNodes(left, right);
        }

        /// <summary>
        /// Merges two linked lists
        /// </summary>
        /// <param name="left">List 1 head</param>
        /// <param name="right">List 2 head</param>
        /// <returns>Head of merged list</returns>
        private Node MergeNodes(Node left, Node right)
        {
            if (left == null) return right;
            if (right == null) return left;

            Node currentLeft = left;
            Node currentRight = right;
            Node result, tail;

            // figure out who's first
            if (currentLeft.obj.Speed >= currentRight.obj.Speed)
            {
                result = currentLeft;
                currentLeft = currentLeft.Next;
            }
            else
            {
                result = currentRight;
                currentRight = currentRight.Next;
            }

            // Initialize tail to the first element
            tail = result;

            // Merge the remaining elements
            while (currentLeft != null && currentRight != null)
            {
                if (currentLeft.obj.Speed >= currentRight.obj.Speed)
                {
                    tail.Next = currentLeft;
                    currentLeft = currentLeft.Next;
                }
                else
                {
                    tail.Next = currentRight;
                    currentRight = currentRight.Next;
                }

                // Move the tail forward
                tail = tail.Next;
            }

            // Append any remaining elements
            if (currentLeft != null)
            {
                tail.Next = currentLeft;
            }
            else if (currentRight != null)
            {
                tail.Next = currentRight;
            }

            return result;
        }

        // fast pointer tech
        private Node GetMiddle(Node node)
        {
            if (node == null) return node;

            Node slow = node, fast = node.Next;
            while (fast != null)
            {
                fast = fast.Next;
                if (fast != null)
                {
                    slow = slow.Next;
                    fast = fast.Next;
                }
            }

            return slow;
        }

        public override string ToString()
        {
            return "Turn Queue";
        }

        public string Debug()
        {
            if (Size < 1) return "";

            StringBuilder sb = new();

            Node n = head;
            while (n.Next != null)
            {
                sb.Append(n.obj.Name + " : " + n.obj.Speed)
                    .Append(" -> ");
                n = n.Next;
            }

            return sb.ToString();
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return new TurnQueueEnumerator(head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class TurnQueueEnumerator : IEnumerator<GameObject>
        {
            private Node current;
            private Node head;
            public GameObject Current => current.obj;

            object IEnumerator.Current => current.obj;

            public TurnQueueEnumerator(Node head)
            {
                this.head = head;
                current = null;
            }

            public void Dispose()
            {
                // do nothing
            }

            public bool MoveNext()
            {
                if (current == null)
                {
                    // this is the first call, so set current to head
                    current = head;
                    return (current != null);
                }

                if (current.Next == null) return false;
                current = current.Next;
                return true;
            }

            public void Reset()
            {
                current = head;
            }
        }
        private class Node
        {
            public GameObject obj { get; set; }
            public Node? Next;

            public Node(GameObject obj, Node next)
            {
                this.obj = obj;
                Next = next;
            }

            public Node(GameObject obj)
            {
                this.obj = obj;
                Next = null;
            }
        }
    }
}
