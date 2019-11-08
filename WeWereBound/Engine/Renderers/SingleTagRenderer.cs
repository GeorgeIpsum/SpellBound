using Microsoft.Xna.Framework.Graphics;

namespace WeWereBound.Engine {
    public class SingleTagRenderer : Renderer {
        public BitTag Tag;
        public BlendState BlendState;
        public SamplerState SamplerState;
        public Effect Effect;
        public Camera Camera;

        public SingleTagRenderer(BitTag tag) {
            Tag = tag;
            BlendState = BlendState.AlphaBlend;
            SamplerState = SamplerState.LinearClamp;
            Camera = new Camera();
        }

        public override void BeforeRender(Scene scene) {

        }

        public override void Render(Scene scene) {
            Draw.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState, SamplerState, DepthStencilState.None, RasterizerState.CullNone, Effect, Camera.Matrix * GameEngine.ScreenMatrix);

            foreach (var entity in scene[Tag])
                if (entity.Visible)
                    entity.Render();

            if (GameEngine.Commands.Open)
                foreach (var entity in scene[Tag])
                    entity.DebugRender(Camera);

            Draw.SpriteBatch.End();
        }

        public override void AfterRender(Scene scene) {

        }
    }
}
