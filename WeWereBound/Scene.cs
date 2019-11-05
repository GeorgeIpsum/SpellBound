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
        public bool focused { get; private set; }
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

        #endregion

    }
}
