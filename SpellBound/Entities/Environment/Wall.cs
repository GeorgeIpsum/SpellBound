using Microsoft.Xna.Framework;
using Monocle;
using SpellBound.Singletons;

namespace SpellBound.Entities.Environment {
  class Wall : Entity {
    Sprite Sprite;

    public Wall() {
      Add(Sprite = GraphicsSingleton.Sprites.Create("pika"));
      Collider = new Hitbox(32, 32, -16, -16);
      Tag = TagsSingleton.TAG_Wall;
    }

    public Wall(Vector2 position) : this()  {
      Position = position;
    }
  }
}
