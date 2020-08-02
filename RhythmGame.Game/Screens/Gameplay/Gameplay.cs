using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;
using RhythmGame.Domain.Beatmap;

namespace RhythmGame.Game.Screens.Gameplay
{
    public class Gameplay : Screen
    {
        public Bindable<Beatmap> Beatmap;

        public Gameplay(Bindable<Beatmap> Beatmap)
        {
            this.Beatmap = Beatmap;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(
                new GameplayContainer(Beatmap)
            );
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
            {
                this.Exit();
            }

            return true;
        }
    }
}
