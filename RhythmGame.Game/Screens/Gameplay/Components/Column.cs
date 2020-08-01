using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Column : Container
    {
        public int N;
        public LinkedList<NoteObject> Notes;
        public LinkedListNode<NoteObject> NotesPointer;
        public NoteObject[] DrawableNotes;

        public Column(int n, List<Note> notes)
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            N = n;
            Notes = new LinkedList<NoteObject>();

            notes = notes.OrderBy(note => note.Ms).ToList();

            foreach (Note note in notes)
            {
                Notes.AddLast(new NoteObject(note));
            }
        }

        public void SetNotesPointerToClosestMs(int ms, int direction = 0)
        {
            if(NotesPointer.Previous != null && direction != 2 && NotesPointer.Value.Note.Ms > ms)
            {
                NotesPointer = NotesPointer.Previous;
                SetNotesPointerToClosestMs(ms, 1);
            }
            else if (NotesPointer.Next != null && direction != 1 && NotesPointer.Value.Note.Ms < ms)
            {
                NotesPointer = NotesPointer.Next;
                SetNotesPointerToClosestMs(ms, 2);
            } else
            {
                return;
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textureStore)
        {
            int AlignLeft = -200;
            int KeyWidth = 100;
            int KeyHeight = 40;
            int KeyPadd = 0;

            Position = new Vector2(AlignLeft + N * KeyWidth + N * KeyPadd, 0);

            Children = new Drawable[]
            {
                new Box
                {
                    RelativePositionAxes = Axes.Y,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(KeyWidth, 1f),
                    Colour = new Color4(0, 0, 0, 255),
                },
                new Container
                {
                    Children = DrawableNotes = Notes.ToArray()
                }
            };
        }

        public void Activate()
        {

        }

        public void Disable()
        {

        }

        public void Update(StopwatchClock StopwatchClock)
        {
            foreach(NoteObject note in DrawableNotes)
            {
                note.Update(StopwatchClock);
            }
        }
    }
}
