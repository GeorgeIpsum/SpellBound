using System.Collections.Generic;
using Monocle;

namespace SpellBound.Singletons {
  class GraphicsSingleton {
    private static SpriteBank _SpriteBank = null;
    List<Atlas> Atlases;

    public static SpriteBank Sprites {
      get { return _SpriteBank; }
    }

    private static GraphicsSingleton _Instance = null;

    public static GraphicsSingleton Instance {
      get {
        if (_Instance == null) _Instance = new GraphicsSingleton();
        return _Instance;
      }
    }

    private GraphicsSingleton() => Initialize();

    private void Initialize() {
      Atlases = new List<Atlas>();
      Atlases.Add(Atlas.FromAtlas("Atlases\\atlas", Atlas.AtlasDataFormat.CrunchXmlOrBinary));

      _SpriteBank = new SpriteBank(Atlases[0], "SpriteData\\Sprites.xml");
    }

    //avoid using
    public void RefreshInstance() => Initialize();
  }
}
