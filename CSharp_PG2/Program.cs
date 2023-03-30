using System;

namespace CSharp_PG2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                game.Run();
                
            }

        }
    }
}
