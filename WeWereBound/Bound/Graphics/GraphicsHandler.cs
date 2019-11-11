using WeWereBound.Engine;
using System;
using System.Collections.Generic;

namespace WeWereBound.Bound {
  public class GraphicsHandler {
    private static SpriteBank _SpriteBank = null;
    List<Atlas> Atlases;

    public static SpriteBank Sprites {
      get { return _SpriteBank; }
    }

    ///<summary>The singleton instance for the graphics handler</summary>
    private static GraphicsHandler _Instance = null;

    public static GraphicsHandler Instance {
      get {
        if (_Instance == null) _Instance = new GraphicsHandler();
        return _Instance;
      }
    }

    private GraphicsHandler() {
      Initialize();
    }

    private void Initialize() {
      Atlases = new List<Atlas>();
      Atlases.Add(Atlas.FromAtlas("Atlases\\atlas", Atlas.AtlasDataFormat.CrunchXmlOrBinary));

      _SpriteBank = new SpriteBank(Atlases[0], "SpriteData\\Sprites.xml");
    }

    public void RefreshInstance() {
      Initialize();
    }

  }
}
