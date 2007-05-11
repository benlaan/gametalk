using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Forms = System.Windows.Forms;
using System.Collections;

namespace RiskClient
{

    interface IComponent
    {
        void Initialise();
        void Update();
        void Draw();

        bool Enabled { get ; set ; }
    }

    public class ComponentCollection
    {

        private GraphicsDevice _device;
        private ArrayList _components;

        public ComponentCollection(GraphicsDevice device)
        {
            _device = device;
            _components = new ArrayList();
        }

        internal void Draw()
        {
            _device.Clear(Color.CornflowerBlue);

            foreach (IComponent component in _components)
                if (component.Enabled)
                    component.Draw();

            _device.Present();
        }

        internal void Update()
        {
            foreach (IComponent component in _components)
                if (component.Enabled)
                    component.Update();
        }

        internal void Add(IComponent component)
        {
            _components.Add(component);
            component.Initialise();
        }

        public IEnumerator GetEnumerator()
        {
            return _components.GetEnumerator();
        }
    }

    public abstract class Component : IComponent
    {
        static    int            gID = 0;

        private   bool           _enabled;
        private   GraphicsDevice _device;
        private   Vector2        _offset;
        protected Rectangle      _boundingBox;

        protected Forms.Control  Owner;

        public    int            ID;

        public Component(Forms.Control owner, GraphicsDevice device)
        {
            Enabled = false;

            ID = gID++;
            _device = device;
            System.Drawing.Point p = owner.Parent.PointToScreen(owner.Location);
            _offset = new Vector2(p.X, p.Y);

            _boundingBox = new Rectangle(0, 0, 0, 0);

            Owner = owner;
        }

        public virtual void Initialise()
        {
            Enabled = true;
        }

        public abstract void Update();

        public abstract void Draw();

        public GraphicsDevice Device
        {
            get {
                return _device;
            }
        }

        public Vector2 Offset
        {
            get {
                return _offset;
            }
        }

        public bool Enabled
        {
            get {
                return _enabled;
            }
            set{
                _enabled = value;
            }
        }

        protected virtual Rectangle GetBoundingBox()
        {
            return _boundingBox;
        }

        public Rectangle BoundingBox
        {
            get {
                return GetBoundingBox();
            }
        }
    }
}
