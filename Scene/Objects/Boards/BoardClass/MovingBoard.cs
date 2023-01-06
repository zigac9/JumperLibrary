using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class MovingBoard
{
    private Rectangle _position;

    private int _speed;
    private readonly Texture2D _texture;

    public MovingBoard(Texture2D texture, Rectangle position, int speed)
    {
        _texture = texture;
        _position = position;
        _speed = speed;
        DrawVisible = true;
    }
    
    public bool DrawVisible { get; set; }

    public Rectangle Position
    {
        get => _position;
        set => _position = value;
    }

    public void DrawSprite(SpriteBatch s)
    {
        s.Draw(_texture, _position, Color.White);
    }

    public void Move()
    {
        _position.X += _speed;
        if (_position.X > 420 || _position.X < 0) _speed *= -1;
    }

    public bool Collision(Player player)
    {
        if ((player.PlayerPosition.X + 15 > _position.X && player.PlayerPosition.X + 15 < _position.X + 60) ||
            (player.PlayerPosition.X + 45 > _position.X && player.PlayerPosition.X + 45 < _position.X + 60))
            if (_position.Y - player.PlayerPosition.Y - 60 < 5 && _position.Y - player.PlayerPosition.Y - 60 > -20 &&
                player.Speed.Y > 0)
                return true;
            else return false;
        return false;
    }
}