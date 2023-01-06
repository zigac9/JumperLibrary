using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumperLibrary
{
    public abstract class ClassEnums
    {
        public enum GameStateEnum
        {
            IntroMenu = 0,
            GameRunning,
            Pause,
            Option,
            GameOver,
            HScore
        }

        public enum PlayerOrientEnum
        {
            Left = 1,
            Right
        }
    }
}
