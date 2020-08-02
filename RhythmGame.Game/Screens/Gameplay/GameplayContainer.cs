using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Logging;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using RhythmGame.Domain.Beatmap;
using RhythmGame.Game.Screens.Gameplay.Components;

namespace RhythmGame.Game.Screens.Gameplay
{
    public class GameplayContainer : Container
    {
        public Bindable<Beatmap> Beatmap;
        public Playfield Playfield;

        public StopwatchClock StopwatchClock;

        public GameplayContainer(Bindable<Beatmap> beatmap)
        {
            Beatmap = beatmap;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
        }
        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {

            Children = new Drawable[]
            {
                (Playfield = new Playfield(Beatmap))
            };

            StopwatchClock = new StopwatchClock(true);
        }
        protected override void Update()
        {
            Playfield.Update(StopwatchClock);
            base.Update();
        }
    }
}
