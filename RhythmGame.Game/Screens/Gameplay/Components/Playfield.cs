using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Timing;
using osuTK;
using RhythmGame.Domain.Beatmap;

namespace RhythmGame.Game.Screens.Gameplay.Components
{
    class Playfield : Container
    {
        Bindable<Beatmap> Beatmap;
        public List<Column> Columns;

        public Playfield(Bindable<Beatmap> beatmap)
        {
            RelativePositionAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(0.5f, 0.5f);
            Beatmap = beatmap;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            Columns = new List<Column>();

            for (int k = 0; k < Beatmap.Value.keycount; k++)
            {
                Columns.Add(new Column(k, Beatmap.Value.GetColumnNotes(k+1)));
            }

            Children = new Drawable[]
            {
                new Container
                {
                    Children = Columns
                },
            };
        }

        public void Update(StopwatchClock StopwatchClock)
        {
            foreach(Column col in Columns)
            {
                col.Update(StopwatchClock);
            }
        }
    }
}
