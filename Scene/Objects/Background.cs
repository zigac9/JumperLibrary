﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Background
{
    private Rectangle _bPosize;
    private readonly Rectangle _gameOverposize;
    private readonly Rectangle _hScoreposize;
    private readonly Rectangle _introMenuposize;
    private Rectangle _kPosize;
    private readonly Rectangle _notifposize;
    private readonly Rectangle _optionposize;
    private readonly Rectangle _pauseposize;
    private readonly Rectangle _sOffposize;
    private readonly Rectangle _sOnposize;
    private readonly Rectangle _sOffposizeMusic;
    private readonly Rectangle _sOnposizeMusic;
    private Rectangle _sPosise1;
    private Rectangle _sPosise2;
    private readonly Rectangle _gameModeEasyPos;
    private readonly Rectangle _gameModeHardPos;
    private readonly Dictionary<string, Texture2D> _textures;

    public Background(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        _introMenuposize = new Rectangle(0, 0, 480, 720);
        _optionposize = new Rectangle(0, 0, 480, 720);
        _sOnposize = new Rectangle(150, 360, 200, 60);
        _sOffposize = new Rectangle(150, 360, 200, 60);
        _sOnposizeMusic = new Rectangle(150, 480, 200, 60);
        _sOffposizeMusic = new Rectangle(150, 480, 200, 60);
        _notifposize = new Rectangle(0, 0, 480, 60);
        _pauseposize = new Rectangle(0, 0, 480, 720);
        _gameOverposize = new Rectangle(0, 0, 480, 720);
        _hScoreposize = new Rectangle(0, 0, 480, 720);
        _gameModeEasyPos = new Rectangle(200, 285, 220, 60);
        _gameModeHardPos = new Rectangle(200, 285, 220, 60);
        SoundCheck = true;
        SoundEffectCheck = true;
        Initialize();
    }

    public Rectangle BPosize
    {
        get => _bPosize;
        set => _bPosize = value;
    }

    public Rectangle SPosise1
    {
        get => _sPosise1;
        set => _sPosise1 = value;
    }

    public Rectangle SPosise2
    {
        get => _sPosise2;
        set => _sPosise2 = value;
    }

    public bool SoundCheck { get; set; }
    
    public bool SoundEffectCheck { get; set; }

    public bool GameStateCheck { get; set; }

    private int HScore1 { get; set; }

    private int HScore2 { get; set; }

    private int HScore3 { get; set; }

    private int HScore4 { get; set; }

    private int HScore5 { get; set; }
    
    private string HScore1Name { get; set; }

    private string HScore2Name { get; set; }

    private string HScore3Name { get; set; }

    private string HScore4Name { get; set; }

    private string HScore5Name { get; set; }


    private int Bests { get; set; }

    public void Initialize()
    {
        HScore1Name = "";HScore2Name = "";HScore3Name = "";HScore4Name = "";HScore5Name = "";
        _bPosize = new Rectangle(0, -6480, 480, 7200);
        _kPosize = new Rectangle(0, 0, 480, 720);
        _sPosise1 = new Rectangle(0, -2880, 480, 3600);
        _sPosise2 = new Rectangle(0, -6480, 480, 3600);
        _bPosize = new Rectangle(_bPosize.X, -7200 + 720, _bPosize.Width, _bPosize.Height);
        GameStateCheck = true;
    }

    public void Draw(SpriteBatch s, ClassEnums.GameStateEnum gameState, ScorClass score, ClassEnums.GameModeEnum gameModeEnum)
    {
        if (gameState == ClassEnums.GameStateEnum.InputName)
        {
            s.Draw(_textures["assets/input"], _introMenuposize, Color.White);
        }
        else
        {
            if (gameState == ClassEnums.GameStateEnum.IntroMenu)
            {
                s.Draw(_textures["assets/mainMenu1"], _introMenuposize, Color.White);
                if (gameModeEnum == ClassEnums.GameModeEnum.Easy)
                {
                    s.Draw(_textures["assets/easy"], _gameModeEasyPos, Color.White);
                }
                else if (gameModeEnum == ClassEnums.GameModeEnum.Hard)
                {
                    s.Draw(_textures["assets/hard"], _gameModeHardPos, Color.White);
                }
            }
            else if (gameState == ClassEnums.GameStateEnum.About)
            {
                s.Draw(_textures["assets/about"], _introMenuposize, Color.White);
            }
            else
            {
                s.Draw(_textures["assets/gradient"], _bPosize, Color.White);
                s.Draw(_textures["assets/kooh"], _kPosize, Color.White);
                s.Draw(_textures["assets/sides"], _sPosise1, Color.White);
                s.Draw(_textures["assets/sides"], _sPosise2, Color.White);
            }
            if (gameState == ClassEnums.GameStateEnum.Pause)
                s.Draw(_textures["assets/pause"], _pauseposize, Color.White);
            if (gameState == ClassEnums.GameStateEnum.Option)
            {
                s.Draw(_textures["assets/option"], _optionposize, Color.White);
                if (SoundCheck)
                    s.Draw(_textures["assets/sOn"], _sOnposize, Color.White);
                else
                    s.Draw(_textures["assets/sOff"], _sOffposize, Color.White);
                
                if(SoundEffectCheck)    
                    s.Draw(_textures["assets/sOn"], _sOnposizeMusic, Color.White);
                else
                    s.Draw(_textures["assets/sOff"], _sOffposizeMusic, Color.White);

            }

            if (gameState == ClassEnums.GameStateEnum.GameOver)
            {
                s.Draw(_textures["assets/gameOver"], _gameOverposize, Color.White);
                s.DrawString(score.SFont, score.Score.ToString(), new Vector2(325f, 228f), Color.Black);
                s.DrawString(score.SFont, Bests.ToString(), new Vector2(325f, 290f), Color.Black);
            }

            if (gameState == ClassEnums.GameStateEnum.HScore)
            {
                if (gameModeEnum == ClassEnums.GameModeEnum.Easy)
                {
                    s.Draw(_textures["assets/Hscore"], _hScoreposize, Color.White);
                }
                else if (gameModeEnum == ClassEnums.GameModeEnum.Hard)
                {
                    s.Draw(_textures["assets/HscoreHard"], _hScoreposize, Color.White);
                }
                s.DrawString(score.SFont, HScore1Name + ": " + HScore1, new Vector2(150f, 295f), Color.Black);
                s.DrawString(score.SFont, HScore2Name + ": " + HScore2, new Vector2(150f, 345f), Color.Black);
                s.DrawString(score.SFont, HScore3Name + ": " + HScore3, new Vector2(150f, 400f), Color.Black);
                s.DrawString(score.SFont, HScore4Name + ": " + HScore4, new Vector2(150f, 450f), Color.Black);
                s.DrawString(score.SFont, HScore5Name + ": " + HScore5, new Vector2(150f, 500f), Color.Black);
            }
        }
    }

    public void ScoreDraw(SpriteBatch s, ClassEnums.GameStateEnum gameState)
    {
        s.Draw(_textures["assets/notif"], _notifposize, Color.White);
    }

    public void SideCheck()
    {
        if (_sPosise1.Y > 720)
            _sPosise1.Y = _sPosise2.Y - 3600;
        if (_sPosise2.Y > 720)
            _sPosise2.Y = _sPosise1.Y - 3600;
    }

    public void UpdateScores(ScoreManager scoreManager, string playerName)
    {
        Bests = scoreManager.BestOfYou(playerName);
        HScore1 = scoreManager.Highscores[0];
        HScore2 = scoreManager.Highscores[1];
        HScore3 = scoreManager.Highscores[2];
        HScore4 = scoreManager.Highscores[3];
        HScore5 = scoreManager.Highscores[4];

        if (HScore1 != 0)
            HScore1Name = scoreManager.Scores[0].PlayerName;
        if (HScore2 != 0)
            HScore2Name = scoreManager.Scores[1].PlayerName;
        if (HScore3 != 0)
            HScore3Name = scoreManager.Scores[2].PlayerName;
        if (HScore4 != 0)
            HScore4Name = scoreManager.Scores[3].PlayerName;
        if (HScore5 != 0)
            HScore5Name = scoreManager.Scores[4].PlayerName;
    }
}