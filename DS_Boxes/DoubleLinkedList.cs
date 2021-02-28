
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DS_Boxes
{
    public class DoubleLinkedList<T> : IEnumerable<T>
    {
        private Node head;
        public Node First
        {
            get { return head; }
        }

        private Node tail;
        public Node Last
        {
            get { return tail; }
        }
        public int Length { get; private set; }
        public void AddLast(T data)
        {
            Node newNode = new Node(data);
            if (tail == null)
            {
                head = newNode;
            }
            else
            {
                newNode.Previous = tail;
                tail.Next = newNode;
            }

            tail = newNode;
            Length++;
        }
        public void AddFirst(T data)
        {
            Node newNode = new Node(data);
            newNode.Next = head;

            if (head == null)
            {
                tail = newNode;
            }
            else
            {
                head.Previous = newNode;
            }
            head = newNode;
            Length++;
        }
        public bool AddAt(T value, int position)
        {
            if (position < 0)
            {
                throw new ArgumentException("position must be positive or zero");
            }
            if (position == 0)
            {
                AddFirst(value);
                return true;
            }
            if (head == null || position > Length)
            {
                return false;
            }

            Node tmp = head;
            for (int i = 1; tmp != null && i < position; i++)
            {
                tmp = tmp.Next;
            }
            if (tmp == null)
            {
                return false;
            }

            Node newNode = new Node(value);
            newNode.Next = tmp.Next;
            newNode.Previous = tmp;
            newNode.Next.Previous = newNode;
            tmp.Next = newNode;
            Length++;
            return true;
        }
        public void RemoveFirst()
        {
            if (head != null)
            {
                head = head.Next;
                if (head == null)
                {
                    tail = null;
                }
                Length--;
            }
        }
        public void RemoveLast()
        {
            if (tail != null)
            {
                tail = tail.Previous;
                if (tail == null)
                {
                    head = null;
                }
                Length--;
            }
        }
        public void DeleteNode(Node nodeToDelete)
        {
            if (head == null || nodeToDelete == null)
            {
                return;
            }

            if (head == nodeToDelete)
            {
                head = nodeToDelete.Next;
            }

            if (nodeToDelete.Next != null)
            {
                nodeToDelete.Next.Previous = nodeToDelete.Previous;
            }

            if (nodeToDelete.Previous != null)
            {
                nodeToDelete.Previous.Next = nodeToDelete.Next;
            }

            return;
        }
        public bool GetAt(out T value, int position = 0)
        {
            value = default(T);
            if (position >= Length)
            {
                return false;
            }
            Node tmp = head;
            for (int i = 0; i < position; i++)
            {
                tmp = tmp.Next;
            }
            value = tmp.Data;
            return true;
        }
        public IEnumerator<Node> GetEnumerator()
        {
            Node current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return (IEnumerator<T>)GetEnumerator();
        }
        public override string ToString()
        {
            Node tmp = head;
            StringBuilder sb = new StringBuilder();

            while (tmp != null)
            {
                sb.Append($"{tmp.Data}, ");
                tmp = tmp.Next;
            }

            return sb.ToString();
        }
        public class Node
        {
            private T _data;
            public T Data
            {
                get { return _data; }
                set { _data = value; }
            }

            private Node _next;
            public Node Next
            {
                get { return _next; }
                set { _next = value; }
            }

            private Node _previous;
            public Node Previous
            {
                get { return _previous; }
                set { _previous = value; }
            }

            public Node(T data)
            {
                Data = data;
            }

            public override string ToString()
            {
                return $"{Data}, ";
            }

          
        }
        public void ReorderNode(Node node)
        {
            RemoveNodeFromDoubleList(node);
            AddFirst(node.Data);
        }
        public void RemoveNodeFromDoubleList(Node node)
        {
            if (node.Data.Equals(First.Data)) //if the node is equal to the end
                RemoveFirst();
            else if (node.Data.Equals(Last.Data))//if the node is equal to the start
                RemoveLast();
            else
            {  ///the node is deleted from the queue
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
                Length--;
            }
        }
    }  
}

