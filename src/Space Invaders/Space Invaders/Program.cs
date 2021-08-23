using System;

namespace Space_Invaders
{
#if WINDOWS || XBOX
    static class Program
    {
        /// The main entry point for the application.
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

