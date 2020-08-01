using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Input.Bindings;

namespace RhythmGame.Game
{
    public class GameplayInputContainer : KeyBindingContainer<GameplayInput>
    {
        public GameplayInputContainer(KeyCombinationMatchingMode keyCombinationMatchingMode = KeyCombinationMatchingMode.Any,
                                      SimultaneousBindingMode simultaneousBindingMode = SimultaneousBindingMode.All)
                    : base(simultaneousBindingMode, keyCombinationMatchingMode) { }
        public override IEnumerable<KeyBinding> DefaultKeyBindings => new[]
        {
            new KeyBinding(new[] { InputKey.D }, GameplayInput.Left),
            new KeyBinding(new[] { InputKey.F }, GameplayInput.Up),
            new KeyBinding(new[] { InputKey.J }, GameplayInput.Down),
            new KeyBinding(new[] { InputKey.K }, GameplayInput.Right)
        };
    }
}
