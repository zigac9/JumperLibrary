using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class EasyMovingEnemy
{
    private readonly List<Texture2D> _enemyList;
    private bool _mecollision;
    private Texture2D _movingEnemy;
    private Rectangle _position;
    private Vector2 _speed;

    public EasyMovingEnemy(IReadOnlyDictionary<string, Texture2D> textures)
    {
        TextureRand = 0;
        _enemyList = new List<Texture2D>
        {
            textures["assets/dva"], textures["assets/sest"], textures["assets/tri"], textures["assets/stiri"],
            textures["assets/pet"]
        };
        Initialize();
    }

    private int TextureRand { get; }

    public Rectangle Position
    {
        get => _position;
        set => _position = value;
    }

    public Vector2 Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public bool Visible { get; set; }

    public int StRand { get; set; }

    public void Initialize()
    {
        StRand = -1;
        _position = new Rectangle(20, 750, 80, 60);
        _speed = new Vector2(3, 0);
        Visible = false;
        _mecollision = false;
    }

    public void Draw(SpriteBatch s)
    {
        s.Draw(_enemyList[TextureRand], _position, null, Color.White, 0f, Vector2.Zero,
            _speed.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
    }

    public void Move()
    {
        _position.X += (int)_speed.X;
        if (_position.X > 410)
            _speed.X *= -1;
        if (_position.X < 10)
            _speed.X *= -1;
    }

    public void Update(bool gameover, Player player, Bullet bullet)
    {
        Move();

        if (Collision(player) == 0 && !gameover)
        {
            player.Speed = new Vector2(player.Speed.X, -15);
            _mecollision = true;
        }

        else if (Collision(player) == 1)
        {
            player.Speed = new Vector2(player.Speed.X, 0);
        }

        if (Position.Y < 780 && _mecollision)
            Position = new Rectangle(Position.X, Position.Y + 11, Position.Width, Position.Height);
        if (Position.Y > 785 || BulletCollision(bullet))
        {
            _mecollision = false;
            Position = new Rectangle(20, 800, Position.Width, Position.Height);
        }
    }

    public int Collision(Player player)
    {
        if (_position.Y - player.PlayerPosition.Y - 45 < 5 && _position.Y - player.PlayerPosition.Y - 45 > -15 &&
            player.Speed.Y > 0 &&
            ((player.PlayerPosition.X + 15 > _position.X &&
              player.PlayerPosition.X + 15 < _position.X + player.PlayerPosition.Width) ||
             (player.PlayerPosition.X + 45 > _position.X &&
              player.PlayerPosition.X + 45 <
              _position.X + player.PlayerPosition.Width))) return 0; // our enemy is dead!

        if (player.PlayerPosition.Intersects(_position) ||
            _position.Intersects(player.PlayerPosition)) return 1; // you are dead!

        return 2;
    }

    public bool BulletCollision(Bullet bullet)
    {
        var closest = new Vector2(
            MathHelper.Clamp(bullet.Position.X, Position.Left, Position.Right),
            MathHelper.Clamp(bullet.Position.Y, Position.Top, Position.Bottom)
        );
        var distance = Vector2.Distance(new Vector2(bullet.Position.X, bullet.Position.Y), closest);

        return distance <= bullet.Radius;
    }
}