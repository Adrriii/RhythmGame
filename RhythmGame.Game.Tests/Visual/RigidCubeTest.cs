using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Physics;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace RhythmGame.Game.Tests.Visual
{
    [TestFixture]
    public class RigidCubeTest : TestScene
    {
        private RigidBodySimulation sim;

        [BackgroundDependencyLoader]
        private void load()
        {
            // Set up the simulation once before any tests are ran
            Child = sim = new RigidBodySimulation { RelativeSizeAxes = Axes.Both };
        }

        [Test]
        public void AwesomeTestName()
        {
            AddStep("Drop a cube", performDropCube);
        }

        private void performDropCube()
        {
            // Add a new cube to the simulation
            RigidBodyContainer<Drawable> rbc = new RigidBodyContainer<Drawable>
            {
                Child = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(150, 150),
                    Colour = Color4.Tomato,
                },
                Position = new Vector2(500, 500),
                Size = new Vector2(200, 200),
                Rotation = 45,
                Colour = Color4.Tomato,
                Masking = true,
            };

            sim.Add(rbc);
        }
    }
}
