using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace WeWereBound
{
    public class Draw
    {
        public static Renderer Renderer { get; internal set; }
        public static SpriteBatch SpriteBatch { get; private set; }
        public static SpriteFont DefaultFont { get; private set; }
        public static MTexture Particle;
        public static MTexture Pixel;

        internal static void Initialize(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            DefaultFont = GameEngine.Instance.Content.Load<SpriteFont>(@"WeWereBound\WeWereBoundDefault");
            UseDebugPixelTexture();
        }

        public static void UseDebugPixelTexture()
        {
            MTexture texture = new MTexture(2, 2, Color.White);
            Pixel = new MTexture(texture, 0, 0, 1, 1);
            Particle = new MTexture(texutre, 0, 0, 2, 2);
        }
    }
}
