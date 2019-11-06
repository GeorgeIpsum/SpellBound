using System;

namespace WeWereBound
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new GameEngine(800, 600, 800, 600, "we were bound", false))
                game.Run();
        }
    }
}
