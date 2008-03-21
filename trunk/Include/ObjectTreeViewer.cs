using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using log4net;
using System.ComponentModel;
using System.Collections.Generic;

namespace Laan.Library.ObjectTree
{
	public enum NodeType { Class, Property }

	public class NodeDefinition: System.Object
	{
		private Object   _instance;
		private NodeType _type;
        private string   _property;

		public NodeDefinition(NodeType type, Object instance, String property)
		{
			_type = type;
			_instance = instance;
			_property = property;
		}

		public Object Instance
		{
			get { return _instance; }
		}

		public String Property
		{
			get { return _property; }
		}

		public NodeType Type
		{
			get { return _type; }
		}
	}

	public class ObjectTreeViewer
	{
		public ObjectTreeViewer(TreeView tree, object rootObject)
		{
			_tree = tree;
			_object = rootObject;
		}

		private object   _object;
		private TreeView _tree;

		private void AddNode(TreeNode parent, TreeNode newNode)
		{
            if (parent != null)
                parent.Nodes.Add(newNode);
			else
				_tree.Nodes.Add(newNode);
		}

		private void AddObject(object item, TreeNode parent)
		{
//			Debug.WriteLine(item.GetType().Name);

			TreeNode newNode = new TreeNode(item.GetType().Name);
			newNode.Tag = new NodeDefinition(NodeType.Class, item, "");

//			Debug.WriteLine("Add Node");
			AddNode(parent, newNode);
//			Debug.WriteLine("Add Properties");
			AddProperties(item, newNode);
		}

		private void AddObjectList(IEnumerable list, TreeNode parent)
		{
			Type t = list.GetType();
			TreeNode rootNode = new TreeNode(t.Name);
			rootNode.Tag = new NodeDefinition(NodeType.Class, list, "");

			AddNode(parent, rootNode);

			foreach(object item in list)
				Add(item, rootNode);

			AddProperties(list, rootNode);
		}

		private void Add(object item, TreeNode parent)
		{
			if (item is IEnumerable)
				AddObjectList(item as IEnumerable, parent);
			else
				AddObject(item, parent);
		}

		private void AddProperties(object item, TreeNode parent)
		{
			foreach(PropertyInfo info in item.GetType().GetProperties())
			{
//				Log.Debug("{0}: {1}", item.GetType(), info.ToString());

                if(info.Name == "Item")
                    continue;

				if(info.PropertyType.IsNotPublic)
					continue;

                object[] browsable = info.GetCustomAttributes(typeof(BrowsableAttribute), true);
                if (browsable.Length > 0 && !((BrowsableAttribute)browsable[0]).Browsable)
                    continue;

				// store the current property's value
				object value = info.GetValue(item, new object[] {});

                if (value != null)
                {
                    // strings are treated as objects, but here they should be
                    // treated like value types (ie. as a simple leaf node)
                    Type t = value.GetType();
                    if (t.IsValueType || t.Name == "String")
                    {
                        string text = String.Format("{0}: {1}", info.Name, value);
                        TreeNode newNode = parent.Nodes.Add(text);
                        newNode.Tag = new NodeDefinition(NodeType.Property, item, info.Name);
                    }
                    else
                        Add(value, parent);
                }
			}
		}

		public void Update()
		{
			_tree.BeginUpdate();
			try
			{
				try
				{
					_tree.Nodes.Clear();
					Add(_object, _tree.TopNode);

					_tree.ExpandAll();
					_tree.Update();
				}
				catch (Exception ex)
				{
                    Debug.WriteLine("Error: " + ex.ToString());
					throw;
				}
			}
			finally
			{
				_tree.EndUpdate();
			}
		}

		public NodeDefinition SelectedObject
		{
			get {
				return (NodeDefinition)_tree.SelectedNode.Tag;
			}
		}

		public object Object
		{
			get { return _object; }
			set { _object = value; }
		}

	}

}