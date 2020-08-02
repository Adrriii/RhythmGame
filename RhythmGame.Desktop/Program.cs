using osu.Framework.Platform;
using osu.Framework;
using RhythmGame.Game;

namespace RhythmGame.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"RhythmGame"))
            using (osu.Framework.Game game = new Game.RhythmGame())
                host.Run(game);
        }
    }
}
