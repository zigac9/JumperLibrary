using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Spring
{
    private readonly Dictionary<string, Texture2D> _textures;
    private Rectangle _position;

    public Spring(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        Initialize();
    }

    public Rectangle SpringPosition
    {
        get => _position;
        set => _position = value;
    }

    public bool Visible { get; set; }

    public bool SCheck { get; set; }

    public int SRand { get; set; }

    public int ScoreToMove { get; set; }

    public int ScoreMoveStep { get; set; }
    
    public bool DrawVisible { get; set; }

    public void Initialize()
    {
        Visible = false;
        ScoreToMove = 200;
        ScoreMoveStep = 500;
        DrawVisible = true;
        _position = new Rectangle(-100, 730, 20, 30);
        SRand = -1;
        SCheck = false;
    }

    public void Draw(SpriteBatch s)
    {
        s.Draw(_textures["assets/fanar"], _position, Color.White);
    }

    public bool Collision(Player player, bool collisionCheck)
    {
        if ((player.PlayerPosition.X + 10 > _position.X &&
             player.PlayerPosition.X + 10 < _position.X + player.PlayerPosition.Width) ||
            (player.PlayerPosition.X + player.PlayerPosition.Width - 10 > _position.X &&
             player.PlayerPosition.X + player.PlayerPosition.Width - 10 < _position.X + player.PlayerPosition.Width))
        {
            if (_position.Y + _position.Height - player.PlayerPosition.Y - player.PlayerPosition.Height < 5 &&
                _position.Y + _position.Height - player.PlayerPosition.Y - player.PlayerPosition.Height > -15 &&
                player.Speed.Y > 0) return collisionCheck;
            return false;
        }

        return false;
    }

    public void Update(ScorClass score, Trampo trampo, Jetpack jetpack, BoardsList boardsList, Player player, bool collisionCheck, bool thingsCollisionCheck)
    {
        if (score.Score > ScoreToMove && !Visible)
        {
            ScoreToMove += ScoreMoveStep;
            do
            {
                var rnd = new Random();
                SRand = rnd.Next(0, boardsList.BoardList.Length - 1);
            } while (boardsList.BoardList[SRand].Position.Y > 0 ||
                     boardsList.BoardList[SRand].Visible == false ||
                     (SRand == trampo.TRand && SRand != -1 && trampo.TRand != -1) ||
                     (SRand == jetpack.JRand && SRand != -1 && jetpack.JRand != -1) ||
                     (trampo.TRand == jetpack.JRand && trampo.TRand != -1 && jetpack.JRand != -1));

            Visible = true;
        }

        if (SRand != -1 && Visible)
            SpringPosition = new Rectangle(boardsList.BoardList[SRand].Position.X + 10,
                boardsList.BoardList[SRand].Position.Y - 30, SpringPosition.Width,
                SpringPosition.Height);

        if (Visible) SCheck = Collision(player, collisionCheck);

        if (SCheck && thingsCollisionCheck)
        {
            player.Speed = new Vector2(player.Speed.X, -23);
            SRand = -1;
            SCheck = false;
            Visible = false;
        }

        if (SpringPosition.Y > 690)
        {
            SRand = -1;
            Visible = false;
            SCheck = false;
        }
    }
}