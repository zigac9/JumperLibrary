using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Trampo
{
    private readonly Dictionary<string, Texture2D> _textures;
    private Rectangle _position;

    public Trampo(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        Initialize();
    }

    public Rectangle TrampoPosition
    {
        get => _position;
        set => _position = value;
    }

    public int TRand { get; set; }

    public bool Visible { get; set; }

    public bool Check { get; set; }

    public int ScoreToMove { get; set; }

    public int ScoreMoveStep { get; set; }
    
    public bool DrawVisible { get; set; }

    public void Initialize()
    {
        ScoreToMove = 1000;
        ScoreMoveStep = 700;
        TRand = -1;
        Visible = false;
        DrawVisible = true;
        Check = false;
        _position = new Rectangle(-100, 730, 40, 18);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_textures["assets/toshak"], _position, Color.White);
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
                player.Speed.Y > 0)
            {
                if (collisionCheck)
                    return true;
                return false;
            }

            return false;
        }

        return false;
    }

    public void Update(ScorClass score, Spring spring, Jetpack jetpack, BoardsList boardsList, Player player, bool collisionCheck, bool thingsCollisionCheck)
    {
        if (score.Score > ScoreToMove && !Visible)
        {
            ScoreToMove += ScoreMoveStep;
            do
            {
                var rnd = new Random();
                TRand = rnd.Next(0, boardsList.BoardList.Length - 1);
            } while (boardsList.BoardList[TRand].Position.Y > 0 ||
                     boardsList.BoardList[TRand].Visible == false ||
                     (spring.SRand == TRand && spring.SRand != -1 && TRand != -1) ||
                     (spring.SRand == jetpack.JRand && spring.SRand != -1 && jetpack.JRand != -1) ||
                     (TRand == jetpack.JRand && TRand != -1 && jetpack.JRand != -1));

            Visible = true;
        }

        if (TRand != -1)
        {
            TrampoPosition = new Rectangle(boardsList.BoardList[TRand].Position.X + 10,
                boardsList.BoardList[TRand].Position.Y - 15, TrampoPosition.Width,
                TrampoPosition.Height);
            Visible = true;
        }

        if (Visible) Check = Collision(player, collisionCheck);

        if (Check && thingsCollisionCheck)
        {
            player.Speed = new Vector2(player.Speed.X, -32);
            TRand = -1;
            Visible = false;
            Check = false;
        }

        if (TrampoPosition.Y > 690)
        {
            TRand = -1;
            Visible = false;
            Check = false;
        }
    }
}