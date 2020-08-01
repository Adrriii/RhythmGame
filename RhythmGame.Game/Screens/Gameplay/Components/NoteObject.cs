using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using RhythmGame.Domain.Beatmap;

namespace RhythmGame.Game.Screens.Gameplay.Components
{
    public class NoteObject : Container
    {
        public Note Note;
        public Box Image;

        public NoteObject(Note note)
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            Note = note;
            //Hide();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            int AlignLeft = -200;
            int KeyWidth = 100;
            int KeyHeight = 40;
            int KeyPadd = 0;

            Children = new Drawable[]
            {
                Image = new Box
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Size = new Vector2(KeyWidth, KeyHeight),
                    Colour = new Color4(12, 22, 165, 255),
                    Position = new Vector2(0, -Note.Ms)
                }
            };
        }

        public void Update(StopwatchClock StopwatchClock)
        {
            Image.Position = new Vector2(0, -Note.Ms + StopwatchClock.ElapsedMilliseconds);
        }
    }
}
