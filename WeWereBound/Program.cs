using System;
using WeWereBound.Bound;

namespace WeWereBound {
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            using (var game = new SpellBound())
                game.Run();
        }
    }
}
