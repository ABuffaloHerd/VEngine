using SadRogue.Primitives.GridViews;
using System;
using System.Collections;
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

            // If the list is empty or the new node has a speed greater than the head
            if (head == null || newNode.obj.Speed > head.obj.Speed)
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                Node current = head;

                // Find the node after which the new node is to be inserted
                while (current.Next != null && current.Next.obj.Speed >= newNode.obj.Speed)
                {
                    current = current.Next;
                }

                // Insert the new node
                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }

        /// <summary>
        /// Sort using merge sort
        /// </summary>
        public void Sort()
        {
            if (Size < 2) return;


        }

        private Node MergeSort(Node node)
        {
            // Guard case
            if (head == null || head.Next == null)
                return head;

            // recursive halve
            Node half = GetMiddle(node);
            Node mid = half.Next;
            half.Next = null;

            Node left = MergeSort(head);
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

        // Fast pointer tech
        private Node GetMiddle(Node head)
        {
            if (head.Next == null) return head;

            Node slow = head, fast = head;

            while (fast.Next != null && fast.Next.Next != null)
            {
                slow = slow.Next;
                fast = fast.Next.Next;
            }

            return slow;
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
                current = head;
                this.head = head;
            }

            public void Dispose()
            {
                // do nothing
            }

            public bool MoveNext()
            {
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
