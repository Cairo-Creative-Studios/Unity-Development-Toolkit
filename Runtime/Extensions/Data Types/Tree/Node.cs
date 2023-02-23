//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace UDT.Data
{
    [Serializable]
    public class Node<T> : ISerializationCallbackReceiver
    {
        [NonSerialized]
        public T value;

        public string Value = "";
        [NonSerialized] public Tree<T> tree;
        [NonSerialized] public Node<T> parent;
        [HideInInspector] public int[] index;
        public List<Node<T>> children;

        public Node(Tree<T> tree, T value, int[] index, Node<T> parent = null)
        {
            this.tree = tree;
            this.value = value;
            this.parent = parent;
            this.index = index;
        }

        public Node<T>[] GetHiearchy()
        {
            List<Node<T>> hiearchy = new List<Node<T>>();
            Node<T> lastNode = this;

            foreach (int i in index)
            {
                hiearchy.Add(lastNode);
                if (lastNode.parent != null)
                    lastNode = lastNode.parent;
            }

            return hiearchy.ToArray();
        }

        public T[] GetValuesInHiearchy()
        {
            Node<T>[] hiearchy = GetHiearchy();
            List<T> values = new List<T>();

            for (int i = hiearchy.Length - 1; i > -1; i--)
            {
                values.Add(hiearchy[i].value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Returns the Node within this Node at the given Inex
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="index">The index.</param>
        public Node<T> GetNode(int index)
        {
            return children[index];
        }

        /// <summary>
        /// Adds a Node as a Child to this Node
        /// </summary>
        /// <param name="node">Node.</param>
        public void Add(Node<T> node)
        {
            if (children == null) children = new List<Node<T>>();
            children.Add(node);
        }

        public void OnBeforeSerialize()
        {
            if(value != null)
                Value = value.ToString();
        }

        public void OnAfterDeserialize()
        {
            if(value != null)
                Value = value.ToString();
        }
    }
}