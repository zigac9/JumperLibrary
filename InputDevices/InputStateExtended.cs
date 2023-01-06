//(c) Jesper Niedermann 2010
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace JumperLibrary
{
    /// <summary>
    /// </summary>
    /// <typeparam name="S">The State (MouseState, KeyboardState, GamePadState)</typeparam>
    public class InputStateExtended<S> where S : struct
    {
        public InputStateExtended(GameTime gameTime, S state)
        {
            StateTime = gameTime.TotalGameTime;
            State = state;
        }
        public TimeSpan StateTime { get; set; }
        public S State { get; set; }
    }
}
