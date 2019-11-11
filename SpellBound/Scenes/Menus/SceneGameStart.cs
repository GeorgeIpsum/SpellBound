using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using SpellBound.Entities.Actors;
using SpellBound.Entities.Environment;

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

      genWalls();
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

    private void genWalls() {
      for (int i = 0; i < 3; i++) {
        Vector2 position = new Vector2(player.X, player.Y - 32 * (i+2));
        Vector2 position2 = new Vector2(player.X, player.Y + 32 * (i+2));
        Wall w = new Wall(position);
        Wall w2 = new Wall(position2);
        Wall[] a = new Wall[2];
        a[0] = w; a[1] = w2;
        Add(a);
      }

      for (int i = 0; i < 5; i++) {
        Vector2 pos = new Vector2(player.X - 32 * (i+3), player.Y);
        Vector2 pos2 = new Vector2(player.X + 32*(i+3), player.Y);
        Wall w = new Wall(pos);
        Wall w2 = new Wall(pos2);
        Add(w);
        Add(w2);
      }

      for (int i = 0; i < 30; i++) {
        Random rand = new Random(i);
        float x = Calc.Range(rand, player.X - 500, player.X + 500);
        float y = Calc.Range(rand, player.Y - 300, player.Y + 300);
        Vector2 pos = new Vector2(x,y);
        Wall w = new Wall(pos);
        Add(w);
      }
    }
  }
}
