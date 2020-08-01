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
    public class Gameplay : GameplayInputContainer, IKeyBindingHandler<GameplayInput>
    {
        private Bindable<Beatmap> Beatmap;
        private Playfield Playfield;

        private Dictionary<GameplayInput,Box> keys;

        private StopwatchClock StopwatchClock;

        public Gameplay(Bindable<Beatmap> beatmap)
        {
            Beatmap = beatmap;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
        }
        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            keys = new Dictionary<GameplayInput, Box>();

            int AlignLeft = -200;
            int KeyWidth = 100;
            int KeyHeight = 40;
            int KeyPadd = 0;
            int nk = 0;

            foreach(GameplayInput key in Enum.GetValues(typeof(GameplayInput)))
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

                nk++;
            }

            List<Drawable> elements = new List<Drawable>();

            elements.AddRange(keys.Values.ToList());
            elements.Add(Playfield = new Playfield(Beatmap));

            Children = elements;

            StopwatchClock = new StopwatchClock(true);
        }
        protected override void Update()
        {
            Playfield.Update(StopwatchClock);
            base.Update();
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
