using System;

namespace ParticlesTraining
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ParticlesMain game = new ParticlesMain())
            {
                game.Run();
            }
        }
    }
#endif
}

