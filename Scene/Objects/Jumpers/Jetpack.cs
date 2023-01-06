using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Jetpack
{
    private readonly Dictionary<string, Texture2D> _textures;
    private Rectangle _position;

    public Jetpack(Dictionary<string, Texture2D> textures)
    {
        _textures = textures;
        Initialize();
    }

    public Rectangle JetPosition
    {
        get => _position;
        set => _position = value;
    }

    public bool Visible { get; set; }

    public bool JCheck { get; set; }

    public int JRand { get; set; }

    public int ScoreToMove { get; set; }

    public int ScoreMoveStep { get; set; }
    
    public bool DrawVisible { get; set; }

    public void Initialize()
    {
        Visible = false;
        ScoreToMove = 1000;
        ScoreMoveStep = 2000;
        _position = new Rectangle(-100, 730, 30, 40);
        JRand = -1;
        JCheck = false;
        DrawVisible = true;
    }

    public void Draw(SpriteBatch s)
    {
        s.Draw(_textures["assets/jet"], _position, Color.White);
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
                return collisionCheck;
            return false;
        }

        return false;
    }

    public void Update(ScorClass score, Spring spring, Trampo trampo, BoardsList boardsList, Player player,
        bool collisionCheck, bool thingsCollisionCheck)
    {
        if (score.Score > ScoreToMove && !Visible)
        {
            ScoreToMove += ScoreMoveStep;
            do
            {
                var rnd = new Random();
                JRand = rnd.Next(0, boardsList.BoardList.Length - 1);
            } while (boardsList.BoardList[JRand].Position.Y > 0 ||
                     boardsList.BoardList[JRand].Visible == false ||
                     (spring.SRand == trampo.TRand && spring.SRand != -1 && trampo.TRand != -1) ||
                     (spring.SRand == JRand && spring.SRand != -1 && JRand != -1) ||
                     (trampo.TRand == JRand && trampo.TRand != -1 && JRand != -1));

            Visible = true;
        }

        if (JRand != -1 && Visible)
            JetPosition = new Rectangle(boardsList.BoardList[JRand].Position.X + 10,
                boardsList.BoardList[JRand].Position.Y - JetPosition.Height,
                JetPosition.Width, JetPosition.Height);

        if (Visible) JCheck = Collision(player, collisionCheck);

        if (JCheck && thingsCollisionCheck)
        {
            player.Speed = new Vector2(player.Speed.X, -50);
            JRand = -1;
            Visible = false;
            JCheck = false;
            player.IsJetpack = true;
            player.GetAnimatedSprite.Play("fire");
        }

        if (JetPosition.Y > 690)
        {
            JRand = -1;
            Visible = false;
            JCheck = false;
        }
    }
}