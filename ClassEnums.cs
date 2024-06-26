﻿namespace JumperLibrary
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
            HScore,
            HScoreHard,
            InputName,
            About
        }

        public enum PlayerOrientEnum
        {
            Left = 1,
            Right
        }

        public enum GameModeEnum
        {
            Easy = 0,
            Hard
        }
    }
}
