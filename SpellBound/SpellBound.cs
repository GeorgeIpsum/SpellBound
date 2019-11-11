using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using SpellBound.Scenes;
using SpellBound.Singletons;

namespace SpellBound {
  public class SpellBound : Engine {
    public SpellBound() : base(1280, 720, 640, 360, "SpellBound Test", false) {
      Engine.ClearColor = Color.CornflowerBlue;
      Content.RootDirectory = "Content";
      ViewPadding = -32;
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize() {
      // TODO: Add your initialization logic here
      {
        int Wall = TagsSingleton.TAG_Wall.ID;
        GraphicsSingleton gfx = GraphicsSingleton.Instance;
      }

      base.Initialize();
      SceneGameStart gameStart = new SceneGameStart();
      Scene = gameStart;
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      base.LoadContent();

      // TODO: use this.Content to load your game content here
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// game-specific content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

      // TODO: Add your update logic here

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      // TODO: Add your drawing code here

      base.Draw(gameTime);
    }
  }
}
