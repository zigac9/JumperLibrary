using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Board
{
    private readonly Texture2D _texture;
    private Rectangle _position;

    public Board(Texture2D texture, Rectangle position)
    {
        _texture = texture;
        _position = position;
        Visible = true;
        DrawVisible = true;
    }

    public Rectangle Position
    {
        get => _position;
        set => _position = value;
    }

    public bool Visible { get; set; }

    public bool DrawVisible { get; set; }

    public void DrawSprite(SpriteBatch s)
    {
        s.Draw(_texture, _position, Color.White);
    }

    public bool Collision(Player player)
    {
        if (Visible)
        {
            if ((player.PlayerPosition.X + 15 > _position.X &&
                 player.PlayerPosition.X + 15 < _position.X + _position.Width) ||
                (player.PlayerPosition.X + player.PlayerPosition.Width - 15 > _position.X &&
                 player.PlayerPosition.X + player.PlayerPosition.Width - 15 < _position.X + _position.Width))
                if (_position.Y - player.PlayerPosition.Y - player.PlayerPosition.Height < 5 &&
                    _position.Y - player.PlayerPosition.Y - player.PlayerPosition.Height > -20 && player.Speed.Y > 0)
                    return true;
                else return false;
            return false;
        }

        return false;
    }
}