using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class FakeBoard
{
    private Rectangle _position;
    private readonly Texture2D _texture;

    public FakeBoard(Texture2D texture, Rectangle position)
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
        if ((player.PlayerPosition.X + 15 > _position.X && player.PlayerPosition.X + 15 < _position.X + 60) ||
            (player.PlayerPosition.X + 45 > _position.X && player.PlayerPosition.X + 45 < _position.X + 60))
            if (_position.Y - player.PlayerPosition.Y - 60 < 5 && _position.Y - player.PlayerPosition.Y - 60 > -20 &&
                player.Speed.Y > 0)
                return true;
            else return false;
        return false;
    }
}