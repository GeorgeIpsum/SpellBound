using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WeWereBound
{
    public class Entity : IEnumerable<Component>, IEnumerable
    {
        public bool Active = true;
        public bool Visible = true;
        public bool Collidable = true;
        public Vector2 Position;

        public Scene Scene { get; private set; }
        public ComponentList Components { get; private set; }

        private int tag;
        private Collider collider;
        internal int depth = 0;
        internal double actualDepth = 0;

        public Entity(Vector2 position)
        {
            Position = position;
            Components = new ComponentList(this);
        }

        public Entity() : this(Vector2.Zero)
        {

        }

        public virtual void SceneBegin(Scene scene)
        {

        }

        public virtual void SceneEnd(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components) c.SceneEnd(scene);
        }

        public virtual void Awake(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components) c.EntityAwake();
        }

        public virtual void Added(Scene scene)
        {
            Scene = scene;
            if (Components != null)
                foreach (var c in Components) c.EntityAdded(scene);

            Scene.SetActualDepth(this);
        }

        public virtual void Removed(Scene scene)
        {
            if (Components != null)
                foreach (var c in Components) c.EntityRemoved(scene);

            Scene = null;
        }

        public virtual void Update()
        {
            Components.Update();
        }

        public virtual void Render()
        {
            Components.Render();
        }

        public virtual void DebugRender(Camera camera)
        {
            if (Collider != null) Collider.Render(camera, Collidable ? Color.Red : Color.DarkRed);
        }

        public virtual void HandleGraphicsReset()
        {
            Components.HandleGraphicsReset();
        }

        public void RemoveSelf()
        {
            if (Scene != null) Scene.Entities.Remove(this);
        }

        public int Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    if (Scene != null) Scene.SetActualDepth(this);
                }
            }
        }

        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }

        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }

        #region Collider

        public Collider Collider
        {
            get { return collider; }
            set
            {
                if (value == collider) return;
#if DEBUG
                if (value.Entity != null) throw new Exception("Setting an Entity's Collider to a Collider already in use by another object");
#endif
                if (collider != null) collider.Removed();
                collider = value;
                if (collider != null) collider.Added(this);
            }
        }

        public float Width
        {
            get
            {
                if (collider == null) return 0;
                else return collider.Width;
            }
        }

        public float Height
        {
            get
            {
                if (collider == null) return 0;
                else return collider.Height;
            }
        }

        public float Left
        {
            get
            {
                if (collider == null) return X;
                else return Position.X + collider.Left;
            }
            set
            {
                if (collider == null) Position.X = value;
                else Position.X = value - collider.Left;
            }
        }

        public float Right
        {
            get
            {
                if (collider == null) return Position.X;
                else return Position.X + collider.Right;
            }
            set
            {
                if (collider == null) Position.X = value;
                else Position.X = value - collider.Right;
            }
        }

        public float Top
        {
            get
            {
                if (collider == null) return Position.Y;
                else return Position.Y + collider.Top;
            }
            set
            {
                if (collider == null) Position.Y = value;
                else Position.Y = value - collider.Top;
            }
        }

        public float Bottom
        {
            get
            {
                if (collider == null) return Position.Y;
                else return Position.Y + collider.Bottom;
            }
            set
            {
                if (collider == null) Position.Y = value;
                else Position.Y = value - collider.Bottom;
            }
        }

        public float CenterX
        {
            get
            {
                if (collider == null) return Position.X;
                else return Position.X + collider.CenterX;
            }
            set
            {
                if (collider == null) Position.X = value;
                else Position.X = value - collider.CenterX;
            }
        }

        public float CenterY
        {
            get
            {
                if (collider == null) return Position.Y;
                else return Position.Y + collider.CenterY;
            }
            set
            {
                if (collider == null) Position.Y = value;
                else Position.Y = value - collider.CenterY;
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Right, Top);
            }
            set
            {
                Right = value.X;
                Top = value.Y;
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Left, Bottom);
            }
            set
            {
                Left = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Right, Bottom);
            }
            set
            {
                Right = value.X;
                Bottom = value.Y;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(CenterX, CenterY);
            }
            set
            {
                CenterX = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 CenterLeft
        {
            get
            {
                return new Vector2(Left, CenterY);
            }
            set
            {
                Left = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 CenterRight
        {
            get
            {
                return new Vector2(Right, CenterY);
            }
            set
            {
                Right = value.X;
                CenterY = value.Y;
            }
        }

        public Vector2 TopCenter
        {
            get
            {
                return new Vector2(CenterX, Top);
            }
            set
            {
                CenterX = value.X;
                Top = value.Y;
            }
        }

        public Vector2 BottomCenter
        {
            get
            {
                return new Vector2(CenterY, Bottom);
            }
            set
            {
                CenterX = value.X;
                Bottom = value.Y;
            }
        }

        #endregion

        #region Tag

        public int Tag
        {
            get
            {
                return tag;
            }
            set
            {
                if (tag != value)
                {
                    if (Scene != null)
                    {
                        for (int i = 0; i < WeWereBound.BitTag.TotalTags; i++)
                        {
                            int check = 1 << i;
                            bool add = (value & check) != 0;
                            bool has = (Tag & check) != 0;

                            if (has != add)
                            {
                                if (add) Scene.TagLists[i].Add(this);
                                else Scene.TagLists[i].Remove(this);
                            }
                        }
                    }

                    tag = value;
                }
            }
        }

        public bool TagFullCheck(int tag)
        {
            return (this.tag & tag) == tag;
        }

        public bool TagCheck(int tag)
        {
            return (this.tag & tag) != 0;
        }

        public void AddTag(int tag)
        {
            Tag |= tag;
        }

        public void RemoveTag(int tag)
        {
            Tag &= -tag;
        }

        #endregion

        #region Collision Shortcuts

        #region Collide Check

        public bool CollideCheck(Entity other)
        {
            return Collide.Check(this, other);
        }

        public bool CollideCheck(Entity other, Vector2 at)
        {
            return Collide.Check(this, other, at);
        }

        public bool CollideCheck(CollidableComponent other)
        {
            return Collide.Check(this, other);
        }

        public bool CollideCheck(CollidableComponent other, Vector2 at)
        {
            return Collide.Check(this, other, at);
        }

        public bool CollideCheck(BitTag tag)
        {
#if DEBUG
            if (Scene == null) throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.Check(this, Scene[tag]);
        }

        public bool CollideCheck(BitTag tag, Vector2 at)
        {
#if DEBUG
            if (Scene == null) throw new Exception("Can't collide check an Entity against a tag list when it is not a member of a Scene");
#endif
            return Collide.Check(this, Scene[tag], at);
        }

        public bool CollideCheck<T>() where T : Entity
        {
#if DEBUG
            if (Scene == null) throw new Exception("Can't collide check an Entity against tracked Entities when it is not a member of a Scene");
            else if (!Scene.Tracker.Entities.ContainsKey(typeof(T))) throw new Exception("Can't collide check an Entity against an untracked Entity type");
#endif
            return Collide.Check(this, Scene.Tracker.Entities[typeof(T)]);
        }

        #endregion

        #endregion
    }
}
