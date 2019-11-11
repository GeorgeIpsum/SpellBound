using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using SpellBound.Entities.Actors;

namespace SpellBound.Scenes {
  class SceneGameStart : Scene {
    Camera camera;
    Player player;

    public SceneGameStart() : base() { }

    public override void Begin() {
      base.Begin();

      EverythingRenderer renderer = new EverythingRenderer();
      Add(renderer);

      camera = new Camera(640, 360);
      camera.CenterOrigin();
      camera.Zoom = 1.5f;
      renderer.Camera = camera;

      player = new Player();
      Vector2 screenHalf = (new Vector2(Engine.Width, Engine.Height)) / 2;
      player.Position = screenHalf;
      Add(player);
    }

    public override void Update() {
      base.Update();

      int dx = 4 * MInput.Keyboard.AxisCheck(Keys.Left, Keys.Right);
      int dy = 4 * MInput.Keyboard.AxisCheck(Keys.Up, Keys.Down);

      player.Move(new Vector2(dx, dy));
    }

    public override void AfterUpdate() {
      base.AfterUpdate();

      Vector2 tresh = new Vector2(120, 90); // Padding (horizontal,vertical)
      Vector2 npos = new Vector2(player.X - Engine.ViewWidth / 4, player.Y - Engine.ViewHeight / 4);

      // Clamp camera position
      if (npos.X > Engine.Width / 2 + tresh.X)
        npos.X = Math.Min(npos.X, Engine.Width / 2 + tresh.X);
      else if (npos.X < Engine.Width / 2 - tresh.X)
        npos.X = Math.Max(npos.X, tresh.X / 2);

      if (npos.Y > Engine.Height / 2 + tresh.Y)
        npos.Y = Math.Min(npos.Y, Engine.Height / 2 + tresh.Y);
      else if (npos.Y < Engine.Height / 2 - tresh.Y)
        npos.Y = Math.Max(npos.Y, tresh.Y / 2);

      camera.Position = npos;
    }
  }
}
