using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NeedleAssets.Console.Core.Registry.TreeTri
{
    public class TreeNode<T>
    {
        private readonly string _key;
        private readonly List<T> _values = new List<T>();
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();
        private TreeNode<T> _parent = null;
        
        public TreeNode(string key) => _key = key;
        
        public TreeNode(T value, string key)
        {
            _values.Add(value);
            _key = key;
        }
        
        public void AddChildCommand(T value, string key)
        {
            if (key.Length - _key.Length == 0) _values.Add(value);
            else if (AddToChildWithPrefix(value, key)) return;
            CreatePathToValue(value, key);
        }

        private bool AddToChildWithPrefix(T value, string key)
        {
            // try to find path in children, if there are no equivalent paths we return false.
            foreach (var child in _children)
            {
                if (child._key[_key.Length] > key[_key.Length]) return false;
                if (child._key[_key.Length] < key[_key.Length]) continue;
                
                child.AddChildCommand(value, key);
                return true;
            }
            return false;
        }

        private void CreatePathToValue(T value, string key)
        {
            // if there are no equal keys, we need to create path to them!
            var pNode = this; 
            for (int i = _key.Length + 1; i <= key.Length; i++) 
            { 
                var node = new TreeNode<T>(key[..i]) 
                {
                    _parent = pNode
                }; 
                pNode.AddChildNode(node); 
                pNode = node;
            } 
            pNode._values.Add(value);
        }

        public bool RemoveValue(T value) => _values.Remove(value);
        
        public TreeNode<T> Find(char c)
        {
            foreach (var child in _children)
            {
                if (child.GetCharKey() > c) return null;
                if (child.GetCharKey() == c) return child;
            }
            return null;
        }

        private void AddChildNode(TreeNode<T> node)
        {
            int i = 0;
            for (; i < _children.Count && node._key[_key.Length] > _children[i]._key[_key.Length]; i++);
            _children.Insert(i, node);
        }
        
        public string GetKey() => _key;
        private char GetCharKey() => _key[^1];
        
        public TreeNode<T> Parent => _parent;
        public List<T> Values => _values;
        public List<TreeNode<T>> Children => _children;
    }
}