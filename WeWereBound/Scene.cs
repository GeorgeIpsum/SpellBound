using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeWereBound
{
    public class Scene : IEnumerable<Entity>, IEnumerable
    {
        public bool Paused;
        public float TimeActive;
        public float RawTimeActive;
        public bool Focused { get; private set; }
        public EntityList Entities { get; private set; }
        public TagLists TagLists { get; private set; }
        public RendererList RendererList { get; private set; }
        public Entity HelperEntity { get; private set; }
        public Tracker Tracker { get; private set; }

        private Dictionary<int, double> actualDepthLookup;

        public event Action OnEndOfFrame;

        public Scene()
        {
            Tracker = new Tracker();
            Entities = new EntityList(this);
            TagLists = new TagLists();
            RendererList = new RendererList(this);

            actualDepthLookup = new Dictionary<int, double>();

            HelperEntity = new Entity();
            Entities.Add(HelperEntity);
        }

        public virtual void Begin()
        {
            Focused = true;
            foreach (var entity in Entities) entity.SceneBegin(this);
        }

        public virtual void End()
        {
            Focused = false;
            foreach (var entity in Entities) entity.SceneEnd(this);
        }

        public virtual void BeforeUpdate()
        {
            if (!Paused) TimeActive += GameEngine.DeltaTime;
            RawTimeActive += GameEngine.RawDeltaTime;

            Entities.UpdateLists();
            TagLists.UpdateLists();
            RendererList.UpdateLists();
        }

        public virtual void Update()
        {
            if (!Paused)
            {
                Entities.Update();
                RendererList.Update();
            }
        }

        public virtual void AfterUpdate()
        {
            if (OnEndOfFrame != null)
            {
                OnEndOfFrame();
                OnEndOfFrame = null;
            }
        }

        public virtual void BeforeRender()
        {
            RendererList.BeforeRender();
        }

        public virtual void Render()
        {
            RendererList.Render();
        }

        public virtual void AfterRender()
        {
            RendererList.AfterRender();
        }

        public virtual void HandleGraphicsReset()
        {
            Entities.HandleGraphicsReset();
        }

        public virtual void HandleGraphicsCreate()
        {
            Entities.HandleGraphicsCreate();
        }

        public virtual void GainFocus() { }

        public virtual void LoseFocus() { }

        #region Interval

        public bool OnInterval(float interval)
        {
            return (int)((TimeActive - GameEngine.DeltaTime) / interval) < (int)(TimeActive / interval);
        }

        public bool BetweenInterval(float interval)
        {
            return Calc.BetweenInterval(TimeActive, interval);
        }

        public bool OnRawInterval(float interval)
        {
            return (int)((RawTimeActive - GameEngine.RawDeltaTime) / interval) < Math.Floor((RawTimeActive) / interval);
        }

        public bool OnRawInterval(float interval, float offset)
        {
            return (RawTimeActive - offset - GameEngine.RawDeltaTime) / interval < Math.Floor((RawTimeActive - offset) / interval);
        }

        public bool BetweenRawInterval(float interval)
        {
            return Calc.BetweenInterval(RawTimeActive, interval);
        }

        #endregion

        #region Collisions v Tags

        public bool CollideCheck(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) return true;

            return false;
        }

        public bool CollideCheck(Vector2 from, Vector2 to, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) return true;

            return false;
        }

        public bool CollideCheck(Rectangle rect, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) return true;

            return false;
        }

        public bool CollideCheck(Rectangle rect, Entity entity)
        {
            return (entity.Collidable && entity.CollideRect(rect));
        }

        public Entity CollideFirst(Vector2 point, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point))
                    return list[i];
            return null;
        }

        public Entity CollideFirst(Vector2 from, Vector2 to, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to))
                    return list[i];
            return null;
        }

        public Entity CollideFirst(Rectangle rect, int tag)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect))
                    return list[i];
            return null;
        }

        public void CollideInto(Vector2 point, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) hits.Add(list[i]);
        }

        public void CollideInto(Vector2 from, Vector2 to, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) hits.Add(list[i]);
        }

        public void CollideInto(Rectangle rect, int tag, List<Entity> hits)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) list.Add(list[i]);
        }

        public List<Entity> CollideAll(Vector2 point, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(point, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Vector2 from, Vector2 to, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(from, to, tag, list);
            return list;
        }

        public List<Entity> CollideAll(Rectangle rect, int tag)
        {
            List<Entity> list = new List<Entity>();
            CollideInto(rect, tag, list);
            return list;
        }

        public void CollideDo(Vector2 point, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) action(list[i]);
        }

        public void CollideDo(Vector2 from, Vector2 to, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) action(list[i]);
        }

        public void CollideDo(Rectangle rect, int tag, Action<Entity> action)
        {
            var list = TagLists[(int)tag];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) action(list[i]);
        }

        public Vector2 LineWalkCheck(Vector2 from, Vector2 to, int tag, float precision)
        {
            Vector2 add = to - from;
            add.Normalize();
            add *= precision;

            int amount = (int)Math.Floor((from - to).Length() / precision);
            Vector2 prev = from;
            Vector2 at = from + add;

            for (int i = 0; i <= amount; i++)
            {
                if (CollideCheck(at, tag)) return prev;
                prev = at;
                at += add;
            }

            return to;
        }

        #endregion

        #region Collisions v Tracked List Entities

        public bool CollideCheck<T>(Vector2 point) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) return true;
            return false;
        }

        public bool CollideCheck<T>(Vector2 from, Vector2 to) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) return true;
            return false;
        }

        public bool CollideCheck<T>(Rectangle rect) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) return true;
            return false;
        }

        public T CollideFirst<T>(Vector2 point) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) return list[i] as T;
            return null;
        }

        public T CollideFirst<T>(Vector2 from, Vector2 to) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) return list[i] as T;
            return null;
        }

        public T CollideFirst<T>(Rectangle rect) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) return list[i] as T;
            return null;
        }

        public void CollideInto<T>(Vector2 point, List<Entity> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) hits.Add(list[i]);
        }

        public void CollideInto<T>(Vector2 from, Vector2 to, List<Entity> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) hits.Add(list[i]);
        }

        public void CollideInto<T>(Rectangle rect, List<Entity> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) list.Add(list[i]);
        }

        public void CollideInto<T>(Vector2 point, List<T> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) hits.Add(list[i] as T);
        }

        public void CollideInto<T>(Vector2 from, Vector2 to, List<T> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) hits.Add(list[i] as T);
        }

        public void CollideInto<T>(Rectangle rect, List<T> hits) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) hits.Add(list[i] as T);
        }

        public List<T> CollideAll<T>(Vector2 point) where T : Entity
        {
            List<T> list = new List<T>();
            CollideInto<T>(point, list);
            return list;
        }

        public List<T> CollideAll<T>(Vector2 from, Vector2 to) where T : Entity
        {
            List<T> list = new List<T>();
            CollideInto<T>(from, to, list);
            return list;
        }

        public List<T> CollideAll<T>(Rectangle rect) where T : Entity
        {
            List<T> list = new List<T>();
            CollideInto<T>(rect, list);
            return list;
        }

        public void CollideDo<T>(Vector2 point, Action<T> action) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollidePoint(point)) action(list[i] as T);
        }

        public void CollideDo<T>(Vector2 from, Vector2 to, Action<T> action) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideLine(from, to)) action(list[i] as T);
        }

        public void CollideDo<T>(Rectangle rect, Action<T> action) where T : Entity
        {
            var list = Tracker.Entities[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Collidable && list[i].CollideRect(rect)) action(list[i] as T);
        }

        public Vector2 LineWalkCheck<T>(Vector2 from, Vector2 to, float precision) where T : Entity
        {
            Vector2 add = to - from;
            add.Normalize();
            add *= precision;

            int amount = (int)Math.Floor((from - to).Length() / precision);
            Vector2 prev = from;
            Vector2 at = from + add;

            for (int i = 0; i <= amount; i++)
            {
                if (CollideCheck<T>(at)) return prev;
                prev = at;
                at += add;
            }

            return to;
        }

        #endregion

        #region Collisions v Tracked List Components

        public bool CollideCheckByComponent<T>(Vector2 point) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point)) return true;
            return false;
        }

        public bool CollideCheckByComponent<T>(Vector2 from, Vector2 to) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideLine(from, to)) return true;
            return false;
        }

        public bool CollideCheckByComponent<T>(Rectangle rect) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideRect(rect)) return true;
            return false;
        }

        public T CollideFirstByComponent<T>(Vector2 point) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point)) return list[i] as T;
            return null;
        }

        public T CollideFirstByComponent<T>(Vector2 from, Vector2 to) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideLine(from, to)) return list[i] as T;
            return null;
        }

        public T CollideFirstByComponent<T>(Rectangle rect) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideRect(rect)) return list[i] as T;
            return null;
        }

        public void CollideIntoByComponent<T>(Vector2 point, List<Component> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point)) hits.Add(list[i]);
        }

        public void CollideIntoByComponent<T>(Vector2 from, Vector2 to, List<Component> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideLine(from, to)) hits.Add(list[i]);
        }

        public void CollideIntoByComponent<T>(Rectangle rect, List<Component> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideRect(rect)) list.Add(list[i]);
        }

        public void CollideIntoByComponent<T>(Vector2 point, List<T> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point)) hits.Add(list[i] as T);
        }

        public void CollideIntoByComponent<T>(Vector2 from, Vector2 to, List<T> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideLine(from, to)) hits.Add(list[i] as T);
        }

        public void CollideIntoByComponent<T>(Rectangle rect, List<T> hits) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideRect(rect)) list.Add(list[i] as T);
        }

        public List<T> CollideAllByComponent<T>(Vector2 point) where T : Component
        {
            List<T> list = new List<T>();
            CollideIntoByComponent<T>(point, list);
            return list;
        }

        public List<T> CollideAllByComponent<T>(Vector2 from, Vector2 to) where T : Component
        {
            List<T> list = new List<T>();
            CollideIntoByComponent<T>(from, to, list);
            return list;
        }

        public List<T> CollideAllByComponent<T>(Rectangle rect) where T : Component
        {
            List<T> list = new List<T>();
            CollideIntoByComponent<T>(rect, list);
            return list;
        }

        public void CollideDoByComponent<T>(Vector2 point, Action<T> action) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollidePoint(point)) action(list[i] as T);
        }

        public void CollideDoByComponent<T>(Vector2 from, Vector2 to, Action<T> action) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideLine(from, to)) action(list[i] as T);
        }

        public void CollideDoByComponent<T>(Rectangle rect, Action<T> action) where T : Component
        {
            var list = Tracker.Components[typeof(T)];

            for (int i = 0; i < list.Count; i++)
                if (list[i].Entity.Collidable && list[i].Entity.CollideRect(rect)) action(list[i] as T);
        }

        public Vector2 LineWalkCheckByComponent<T>(Vector2 from, Vector2 to, float precision) where T : Component
        {
            Vector2 add = to - from;
            add.Normalize();
            add *= precision;

            int amount = (int)Math.Floor((from - to).Length() / precision);
            Vector2 prev = from;
            Vector2 at = from + add;

            for (int i = 0; i <= amount; i++)
            {
                if (CollideCheckByComponent<T>(at)) return prev;
                prev = at;
                at += add;
            }

            return to;
        }

        #endregion

        #region Utils

        internal void SetActualDepth(Entity entity)
        {
            const double theta = .000001f;

            double add = 0;
            if (actualDepthLookup.TryGetValue(entity.depth, out add)) actualDepthLookup[entity.depth] += theta;
            else actualDepthLookup.Add(entity.depth, theta);

            entity.actualDepth = entity.depth - add;

            Entities.MarkUnsorted();
            for (int i = 0; i < BitTag.TotalTags; i++)
                if (entity.TagCheck(1 << i)) TagLists.MarkUnsorted(i);
        }

        #endregion

        #region Entity Shortcuts

        public T CreateAndAdd<T>() where T : Entity, new()
        {
            var entity = GameEngine.Pooler.Create<T>();
            Add(entity);
            return entity;
        }

        public List<Entity> this[BitTag tag]
        {
            get
            {
                return TagLists[tag.ID];
            }
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void Add(IEnumerable<Entity> entities)
        {
            Entities.Add(entities);
        }

        public void Remove(IEnumerable<Entity> entities)
        {
            Entities.Remove(entities);
        }

        public void Add(params Entity[] entities)
        {
            Entities.Add(entities);
        }

        public void Remove(params Entity[] entities)
        {
            Entities.Remove(entities);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Entity> GetEntititesByTagMask(int mask)
        {
            List<Entity> list = new List<Entity>();
            foreach (var entity in Entities)
                if ((entity.Tag & mask) != 0) list.Add(entity);

            return list;
        }

        public List<Entity> GetEntititesByExcludingTagMask(int mask)
        {
            List<Entity> list = new List<Entity>();
            foreach (var entity in Entities)
                if ((entity.Tag & mask) == 0) list.Add(entity);

            return list;
        }

        #endregion

        #region Renderer Shortcuts

        public void Add(Renderer renderer)
        {
            RendererList.Add(renderer);
        }

        public void Remove(Renderer renderer)
        {
            RendererList.Remove(renderer);
        }

        #endregion

    }
}
