using Microsoft.Xna.Framework;
using WeWereBound.Engine;

namespace WeWereBound.Bound {
    class Player : Entity {
        Sprite Sprite;

        public Player() {
            Add(Sprite = GraphicsHandler.Sprites.Create("pika"));
            Collider = new Hitbox(32, 32, -16, -16);
        }

        public void Move(Vector2 delta) {
            if(delta.X != 0 || delta.Y != 0) {
                Sprite.Play("move", false);

                if(CollideCheck(TagsHandler.TAG_Wall, new Vector2(X + delta.X, Y))) {
                    while (!CollideCheck(TagsHandler.TAG_Wall, new Vector2(X + Calc.Sign(delta).X, Y))) X += Calc.Sign(delta).X;
                } else {
                    X += delta.X * 60f * GameEngine.DeltaTime;
                }

                if(CollideCheck(TagsHandler.TAG_Wall, new Vector2(X, Y + delta.Y))) {
                    while (!CollideCheck(TagsHandler.TAG_Wall, new Vector2(X, Y + Calc.Sign(delta).Y))) Y += Calc.Sign(delta).Y;
                } else {
                    Y += delta.Y * 60f * GameEngine.DeltaTime;
                }
            }
        }
    }
}
