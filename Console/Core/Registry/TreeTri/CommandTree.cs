using System.Collections.Generic;
using NeedleAssets.Console.Core.Command;
using UnityEngine;

namespace NeedleAssets.Console.Core.Registry.TreeTri
{
    public class CommandTree
    {
        private readonly TreeNode<ConsoleCommand> _root = new TreeNode<ConsoleCommand>("");

        public void AddNode(ConsoleCommand command)
        {
            _root.AddChildCommand(command, command.Name);   
        }

        private TreeNode<ConsoleCommand> NodeByName(string name)
        {
            var n = _root;
            foreach (var c in name)
            {
                n = n.Find(c);
                if (n == null) return null;
            }
            return n;
        }
        
        public bool? RemoveNode(ConsoleCommand command) => NodeByName(command.Name)?.RemoveValue(command);
        
        public List<ConsoleCommand> CommandsByName(string name) => NodeByName(name)?.Values;

        // Returns dictionary with keys in alphabetical order
        public Dictionary<string, List<ConsoleCommand>> AlphabeticalCommands() => AlphabeticalCommandsFromNode(_root);

        private Dictionary<string, List<ConsoleCommand>> AlphabeticalCommandsFromNode(TreeNode<ConsoleCommand> node)
        {
            Dictionary<string, List<ConsoleCommand>> alphabeticalCommands = new Dictionary<string, List<ConsoleCommand>>();
            if(node.Values != null && node.Values.Count > 0) alphabeticalCommands.Add(node.GetKey(), node.Values);
            foreach (var child in node.Children)
            {
                alphabeticalCommands = Utilities.Utils.MergeDictionaries(alphabeticalCommands, AlphabeticalCommandsFromNode(child));
            }
            return alphabeticalCommands;
        }
    }
}