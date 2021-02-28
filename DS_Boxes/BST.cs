using System;

namespace DS_Boxes
{
    public class BST<T> where T : IComparable<T>
    {
        public Node root = null;

        public bool IsRootEmpty()
        {
            return root == null;
        }

        public T ReturnNode(T value)
        {
            if (root == null)
            {
                return value;
            }
            else
            {
                Node temp = root;
                while (temp != null)
                {
                    if (value.CompareTo(temp.data) == 0)
                    {
                        return temp.data;
                    }
                    else if (value.CompareTo(temp.data) > 0)
                    {
                        temp = temp.right;
                    }
                    else
                    {
                        temp = temp.left;
                    }
                }
            }
            return default(T);
        }

        public void Add(T value)
        {
            if (root == null)
            {
                root = new Node(value);
                return;
            }

            Node temp = root;
            while (temp != null)
            {
                if (value.CompareTo(temp.data) < 0) //value < tmp.data
                {
                    if (temp.left == null)//add value to left and return
                    {
                        temp.left = new Node(value);
                        break;
                    }
                    else
                    {
                        temp = temp.left;
                    }
                }
                else
                {
                    if (temp.right == null)//add value to right and return
                    {
                        temp.right = new Node(value);
                        break;
                    }
                    else
                    {
                        temp = temp.right;
                    }
                }
            }
        }

        public bool FindValue(T searchedValue, out T foundValue)
        {
            foundValue = default(T);
            if (root == null)
            {
                return false;
            }
            else
            {
                Node temp = root;
                while (temp != null)
                {
                    if (searchedValue.CompareTo(temp.data) == 0)
                    {
                        foundValue = temp.data;
                        return true;
                    }
                    else if (searchedValue.CompareTo(temp.data) > 0)
                    {
                        temp = temp.right;
                    }
                    else
                    {
                        temp = temp.left;
                    }
                }
            }
            return false;
        }
        public void ScanInOrder(Action<T> action)
        {
            ScanInOrder(root, action);
        }
        private void ScanInOrder(Node root, Action<T> action)
        {
            if (root == null) return;
            ScanInOrder(root.left, action);
            action(root.data);
            ScanInOrder(root.right, action);
        }
        private bool FindToRemove(T valueToSearch, ref Node Parent, ref Node current)
        {
            int res;
            while (current != null)
            {
                res = valueToSearch.CompareTo(current.data);
                if (res == 0)
                {
                    return true;
                }
                Parent = current;
                if (res < 0)
                {
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }
            return false;
        }
        public bool Remove(T value)
        {
            Node parent = root;
            Node current = root;
            if (!FindToRemove(value, ref parent, ref current))
            {
                return false;
            }

            if (current.left == null && current.right == null)
            {
                if (current == root)
                {
                    root = null;
                }
                if (current.data.CompareTo(parent.data) < 0)
                {
                    parent.left = null;
                }
                else
                {
                    parent.right = null;
                }
                return true;
            }

            if (!(current.left != null && current.right != null))
            {
                if (current.data.CompareTo(parent.data) < 0)
                {
                    if (current.left != null)
                    {
                        if (current == root)
                        {
                            root = current.left;
                        }
                        else
                        {
                            parent.left = current.left;
                        }

                    }
                    else
                    {
                        if (current == root)
                        {
                            root = current.right;
                        }
                        else
                        {
                            parent.left = current.right;
                        }

                    }
                }
                else
                {
                    if (current.left != null)
                    {
                        if (current == root)
                        {
                            root = current.left;
                        }
                        else
                        {
                            parent.right = current.left;
                        }
                    }
                    else
                    {
                        if (current == root)
                        {
                            root = current.right;
                        }
                        else
                        {
                            parent.right = current.right;
                        }
                    }

                }
                return true;
            }
            else
            {
                Node smallest = current.right;
                Node BeforeSmallest = current;
                while (smallest.left != null)
                {
                    BeforeSmallest = smallest;
                    smallest = smallest.left;
                }

                current.data = smallest.data;

                if (BeforeSmallest == current)
                {
                    current.right = smallest.right;
                }
                else if (smallest.right != null)
                {
                    BeforeSmallest.left = smallest.right;
                }
                else
                {
                    BeforeSmallest.left = null;
                }
                return true;
            }



        }
        public Node Search(T findThis)
        {
            return Search(root, findThis);
        }
        private Node Search(Node root, T findThis)
        {
            if (root == null) return root;
            if (root.data.CompareTo(findThis) < 0) return Search(root.right, findThis); // root.data < findthis || this.root is smaller than value we need to find go right
            else if (root.data.CompareTo(findThis) == 0) return root;
            else
            {
                Node tmp = Search(root.left, findThis);
                if (tmp == null) return root; return tmp;
            }
        }
        public bool FindValueOrClosest(T searchedValue, out T foundValue, out Node res)
        {
            Node temp = root;
            res = default;
            while (temp != null)
            {
                if (searchedValue.CompareTo(temp.data) == 0)
                {
                    foundValue = temp.data;
                    res = temp;
                    return true;
                }
                if (searchedValue.CompareTo(temp.data) < 0)
                {
                    if (res == null || res.data.CompareTo(temp.data) > 0)
                    {
                        res = temp;
                    }
                    temp = temp.left;
                }
                else
                {
                    temp = temp.right;
                }
            }
            if (res == null)
            {
                foundValue = default(T);
                return false;
            }
            else
            {
                foundValue = res.data;
                return true;
            }
        }
        public bool FindValueOrClosest(T searchedValue, out T foundValue)
        {
            Node temp = root;
            Node res = default;
            while (temp != null)
            {
                if (searchedValue.CompareTo(temp.data) == 0)
                {
                    foundValue = temp.data;
                    return true;
                }
                if (searchedValue.CompareTo(temp.data) < 0)
                {
                    if (res == null || res.data.CompareTo(temp.data) > 0)
                    {
                        res = temp;
                    }
                    temp = temp.left;
                }
                else
                {
                    temp = temp.right;
                }
            }
            if (res == null)
            {
                foundValue = default(T);
                return false;
            }
            else
            {
                foundValue = res.data;
                return true;
            }
        }
        public bool Find_Next_Size(Node currentNode, out T nextNode, out Node newcurrentnode)
        {
            newcurrentnode = default;

            if (currentNode.right != null && currentNode.right.left.data.CompareTo(currentNode.data) > 0)//nop es mayor
            {
                currentNode = currentNode.right;
                nextNode = currentNode.data;
                newcurrentnode = currentNode;
                return true;
            }
            else
            {
                if (currentNode != root)
                {
                    FindDad(currentNode.data, out newcurrentnode);
                    if (newcurrentnode.data.CompareTo(currentNode.data) > 0)//if the dad node is greater 
                    {
                        nextNode = newcurrentnode.data;
                        return true;
                    }
                }
                // if the dad node is lower // heigth is root
                newcurrentnode = default;
                nextNode = default;
                return false;
            }
        }
        public bool Find_Next_Size(T searchedValue, out T foundValue)
        {
            Node temp = root;
            Node res = default;
            while (temp != null)
            {
                if (searchedValue.CompareTo(temp.data) < 0)
                {
                    if (res == null || res.data.CompareTo(temp.data) > 0)
                    {
                        res = temp;
                    }
                    temp = temp.left;
                }
                else
                {
                    temp = temp.right;
                }
            }
            if (res == null)
            {
                foundValue = default(T);
                return false;
            }
            else
            {
                foundValue = res.data;
                return true;
            }
        }
        private bool FindDad(T valueToSearch, out Node Parent)
        {
            Node tmp = root;
            Parent = default;
            int res;
            while (tmp != null)
            {
                res = valueToSearch.CompareTo(tmp.data);
                if (res == 0)
                {

                    return true;
                }
                Parent = tmp;
                if (res < 0)
                {
                    tmp = tmp.left;
                }
                else
                {
                    tmp = tmp.right;
                }
            }
            return false;
        }
        public int GetDepth()
        {
            return GetDepth(root);
        }
        private int GetDepth(Node root)
        {
            if (root == null) return 0;
            int leftDepth = GetDepth(root.left);
            int rightDepth = GetDepth(root.right);

            return Math.Max(leftDepth, rightDepth) + 1;
        }
        enum Direction
        {
            unassigned = 0,
            left = 1,
            right = 2
        }
        public class Node
        {
            public T data { get; set; }
            public Node left { get; set; }
            public Node right { get; set; }
            public Node(T data) => this.data = data;
        }
       
    }
}
