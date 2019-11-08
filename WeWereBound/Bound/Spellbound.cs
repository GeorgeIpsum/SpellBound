using Microsoft.Xna.Framework;
using WeWereBound.Engine;

namespace WeWereBound.Bound {
    public class SpellBound : GameEngine {
        public SpellBound() : base(1280, 720, 640, 360, "Spellbound", false) {
            GameEngine.ClearColor = Color.AliceBlue;
            Content.RootDirectory = "Content";
            ViewPadding = -32;
        }

        protected override void Initialize() {
            base.Initialize();
            GameStartScene gameStart = new GameStartScene();
            Scene = gameStart;
        }
    }
}
