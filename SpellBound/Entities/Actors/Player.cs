using Microsoft.Xna.Framework;
using Monocle;
using SpellBound.Singletons;

namespace SpellBound.Entities.Actors {
  class Player : Entity {
    Sprite Sprite;

    public Player() {
      Add(Sprite = GraphicsSingleton.Sprites.Create("pika"));
      Collider = new Hitbox(32, 32, -16, -16);
    }

    public void Move(Vector2 delta) {
      if (delta.X != 0 || delta.Y != 0) {
        Sprite.Play("move", false);

        if (CollideCheck(TagsSingleton.TAG_Wall, new Vector2(X + delta.X, Y))) {
          while (!CollideCheck(TagsSingleton.TAG_Wall, new Vector2(X + Calc.Sign(delta).X, Y))) X += Calc.Sign(delta).X;
        } else {
          X += delta.X * 60f * Engine.DeltaTime;
        }

        if (CollideCheck(TagsSingleton.TAG_Wall, new Vector2(X, Y + delta.Y))) {
          while (!CollideCheck(TagsSingleton.TAG_Wall, new Vector2(X, Y + Calc.Sign(delta).Y))) Y += Calc.Sign(delta).Y;
        } else {
          Y += delta.Y * 60f * Engine.DeltaTime;
        }
      }
    }
  }
}
