using System.Collections.Generic;
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
    private Rectangle _sPosise1;
    private Rectangle _sPosise2;
    private readonly Dictionary<string, Texture2D> _textures;

    public Background(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        _introMenuposize = new Rectangle(0, 0, 480, 720);
        _optionposize = new Rectangle(0, 0, 480, 720);
        _sOnposize = new Rectangle(85, 400, 200, 60);
        _sOffposize = new Rectangle(85, 400, 200, 60);
        _notifposize = new Rectangle(0, 0, 480, 60);
        _pauseposize = new Rectangle(0, 0, 480, 720);
        _gameOverposize = new Rectangle(0, 0, 480, 720);
        _hScoreposize = new Rectangle(0, 0, 480, 720);
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

    public bool GameStateCheck { get; set; }

    private int HScore1 { get; set; }

    private int HScore2 { get; set; }

    private int HScore3 { get; set; }

    private int HScore4 { get; set; }

    private int HScore5 { get; set; }

    private int Bests { get; set; }

    public void Initialize()
    {
        _bPosize = new Rectangle(0, -6480, 480, 7200);
        _kPosize = new Rectangle(0, 0, 480, 720);
        _sPosise1 = new Rectangle(0, -2880, 480, 3600);
        _sPosise2 = new Rectangle(0, -6480, 480, 3600);
        _bPosize = new Rectangle(_bPosize.X, -7200 + 720, _bPosize.Width, _bPosize.Height);
        SoundCheck = true;
        GameStateCheck = true;
    }

    public void Draw(SpriteBatch s, ClassEnums.GameStateEnum gameState, ScorClass score)
    {
        s.Draw(_textures["assets/gradient"], _bPosize, Color.White);
        s.Draw(_textures["assets/kooh"], _kPosize, Color.White);
        s.Draw(_textures["assets/sides"], _sPosise1, Color.White);
        s.Draw(_textures["assets/sides"], _sPosise2, Color.White);
        if (gameState == ClassEnums.GameStateEnum.IntroMenu)
            s.Draw(_textures["assets/mainMenu1"], _introMenuposize, Color.White);
        if (gameState == ClassEnums.GameStateEnum.Pause)
            s.Draw(_textures["assets/pause"], _pauseposize, Color.White);
        if (gameState == ClassEnums.GameStateEnum.Option)
        {
            s.Draw(_textures["assets/option"], _optionposize, Color.White);
            if (SoundCheck)
                s.Draw(_textures["assets/sOn"], _sOnposize, Color.White);
            else
                s.Draw(_textures["assets/sOff"], _sOffposize, Color.White);
        }

        if (gameState == ClassEnums.GameStateEnum.GameOver)
        {
            s.Draw(_textures["assets/gameOver"], _gameOverposize, Color.White);
            s.DrawString(score.SFont, score.Score.ToString(), new Vector2(325f, 228f), Color.Black);
            s.DrawString(score.SFont, Bests.ToString(), new Vector2(325f, 290f), Color.Black);
        }

        if (gameState == ClassEnums.GameStateEnum.HScore)
        {
            s.Draw(_textures["assets/highscore"], _hScoreposize, Color.White);
            s.DrawString(score.SFont, HScore1.ToString(), new Vector2(150f, 295f), Color.Black);
            s.DrawString(score.SFont, HScore2.ToString(), new Vector2(150f, 345f), Color.Black);
            s.DrawString(score.SFont, HScore3.ToString(), new Vector2(150f, 400f), Color.Black);
            s.DrawString(score.SFont, HScore4.ToString(), new Vector2(150f, 450f), Color.Black);
            s.DrawString(score.SFont, HScore5.ToString(), new Vector2(150f, 500f), Color.Black);
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
    }
}