using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK.Input;
using RhythmGame.Domain.Beatmap;
using RhythmGame.Domain.Beatmap.Converters;
using RhythmGame.Game.Screens.Gameplay;
using RhythmGame.Utils;

namespace RhythmGame.Game.Screens.MainScreen
{
    public class MainScreen : Screen
    {
        public MainScreen()
        {
            DataManager.Initialize();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.F5)
            {
                BeatmapHelper.RescanBeatmaps();
            }
            if (e.Key == Key.F6)
            {
                new Mania2RG().Save("R:\\osu!\\Songs\\934993 PE - Central DELAY");
                BeatmapHelper.RescanBeatmaps();

                this.Push(new Gameplay.Gameplay(new Bindable<Beatmap>(BeatmapHelper.Beatmaps[0])));                    
            }

            return true;
        }
    }
}
