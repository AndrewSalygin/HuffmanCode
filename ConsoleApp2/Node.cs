using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCode
{
    public class Node : IComparable
    {
        private Node _leftChild = null;
        private Node _rightChild = null;
        private char _inf;
        private int _height;
        private int _frequency;

        public char Inf
        {
            get
            {
                return _inf;
            }
            set
            {
                _inf = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        public int Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
            }
        }

        public Node LeftChild
        {
            get
            {
                return _leftChild;
            }
            set
            {
                _leftChild = value;
            }
        }

        public Node RightChild
        {
            get
            {
                return _rightChild;
            }
            set
            {
                _rightChild = value;
            }
        }

        // конструктор для объединяющей ноды
        public Node(Node leftChild, Node rightChild)
        {
            _inf = leftChild.Inf;
            _leftChild = leftChild;
            _rightChild = rightChild;
            _height = Math.Max(leftChild.Inf, rightChild.Inf) + 1;
            _frequency = leftChild.Frequency + rightChild.Frequency;
        }

        // конструктор для листа
        public Node(char inf, int height, int frequency)
        {
            _inf = inf;
            _height = height;
            _frequency = frequency;
        }

        public static Node joinSubtree(Node leftChild, Node rightChild)
        {
            Node rootOfSubtree = new Node(leftChild, rightChild);
            return rootOfSubtree;
        }

        public int CompareTo(object obj)
        {
            Node other = obj as Node;
            // сравнение по частоте
            if (_frequency == other.Frequency)
            {
                // сравнение по высоте
                if (_height == other.Height)
                    // сравнение по лексикографическому порядку
                    return _inf.CompareTo(other.Inf);
                else
                    return _height.CompareTo(other.Height);
            }
            else
                return _frequency.CompareTo(other.Frequency);
        }

        public static void PreorderSetCode(Node r, ref StringBuilder code, ref Dictionary<char, string> table) // прямой обход поддерева
        {
            if (r != null)
            {
                // если это лист
                if (r.LeftChild == null)
                {
                    table.Add(r.Inf, code.ToString());
                }
                // есть элементы снизу слева
                else
                {
                    code.Insert(code.Length, "0");
                    PreorderSetCode(r.LeftChild, ref code, ref table);
                    code.Remove(code.Length - 1, 1);
                }
                // есть элементы снизу справа
                if (r.RightChild != null)
                {
                    code.Insert(code.Length, "1");
                    PreorderSetCode(r.RightChild, ref code, ref table);
                    code.Remove(code.Length - 1, 1);
                }

            }
        }
        // посмотреть для каких полей убрать свойства

    }
}