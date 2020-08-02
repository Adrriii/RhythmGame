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
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using RhythmGame.Domain.Beatmap;

namespace RhythmGame.Game.Screens.Gameplay.Components
{
    public class Playfield : GameplayInputContainer, IKeyBindingHandler<GameplayInput>
    {
        public Bindable<Beatmap> Beatmap;
        public List<Column> Columns;

        private Dictionary<GameplayInput, Box> keys;

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

            int AlignLeft = -200;
            int KeyWidth = 100;
            int KeyHeight = 40;
            int KeyPadd = 0;
            int nk = 0;

            Columns = new List<Column>();
            keys = new Dictionary<GameplayInput, Box>();

            foreach (GameplayInput key in Enum.GetValues(typeof(GameplayInput)))
            {
                keys.Add(key,
                    new Box
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Size = new Vector2(KeyWidth, KeyHeight),
                        Colour = new Color4(255, 255, 255, 255),
                        Position = new Vector2(AlignLeft + nk * KeyWidth + nk * KeyPadd, 0)
                    }
                );
                Columns.Add(new Column(nk, Beatmap.Value.GetColumnNotes(nk + 1)));

                nk++;
            }

            List<Drawable> elements = new List<Drawable>();

            elements.AddRange(keys.Values.ToList());
            elements.AddRange(Columns);

            Children = new Drawable[]
            {
                new Container
                {
                    Children = elements
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

        public bool OnPressed(GameplayInput input)
        {
            keys[input].Colour = new Color4(255, 0, 0, 255);

            return true;
        }

        public void OnReleased(GameplayInput input)
        {
            keys[input].Colour = new Color4(255, 255, 255, 255);
        }
    }
}
