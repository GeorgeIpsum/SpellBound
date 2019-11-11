using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WeWereBound.Engine;

namespace WeWereBound.Bound {
    public class SpellBound : GameEngine {
        public SpellBound() : base(1280, 720, 640, 360, "Spellbound", false) {
            GameEngine.ClearColor = Color.Chocolate;
            Content.RootDirectory = "Bound\\Content";
            ViewPadding = -32;
            ExitOnEscapeKeypress = false;
        }

        protected override void Initialize() {

            {
                int Wall = TagsHandler.TAG_Wall.ID;
                GraphicsHandler graphics = GraphicsHandler.Instance;
            }

            base.Initialize();
            GameStartScene gameStart = new GameStartScene();
            Scene = gameStart;
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        protected override void UnloadContent() {

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
