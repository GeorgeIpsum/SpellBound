using System;
using Microsoft.Xna.Framework;
using Monocle;

namespace SpellBound.Scenes {
  class Dungeon : Scene {
    Camera camera;

    public Dungeon() : base() { }

    public override void Begin() {
      base.Begin();

      EverythingRenderer renderer = new EverythingRenderer();
      Add(renderer);

      camera = new Camera();
      camera.CenterOrigin();
    }

    public override void BeforeUpdate() {
      base.BeforeUpdate();
    }

    public override void Update() {
      base.Update();
    }

    public override void AfterUpdate() {
      base.AfterUpdate();
    }

    public override void BeforeRender() {
      base.BeforeRender();
    }

    public override void Render() {
      base.Render();
    }

    public override void AfterRender() {
      base.AfterRender();
    }
  }
}
