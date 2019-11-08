using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WeWereBound.Engine;

namespace WeWereBound.Bound {
    class GameStartScene : Scene {
        Camera camera;

        public GameStartScene() : base() { }

        public override void Begin() {
            base.Begin();

            EverythingRenderer er = new EverythingRenderer();
            Add(er);

            camera = new Camera(640, 360);
            camera.CenterOrigin();
            camera.Zoom = 1.5f;
            er.Camera = camera;
        }
    }
}
