using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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

    public int TextureRand { get; set; }

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
    
    public void Initialize()
    {
        _position = new Rectangle(20, 750, 80, 55);
        _speed = new Vector2(2, 0);
        Visible = false;
        _mecollision = false;
    }

    public void Draw(SpriteBatch s)
    {
        s.Draw(_enemyList[TextureRand], _position, null, Color.White, 0f, Vector2.Zero,
            _speed.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
    }

    private void Move()
    {
        _position.X += (int)_speed.X;
        if (_position.X > 410)
            _speed.X *= -1;
        if (_position.X < 10)
            _speed.X *= -1;
    }

    public void Update(Bullet bullet, Sound sound, Player player,
        ref bool gameOver, ref bool collisionCheck, bool thingsCollisionCheck, bool SoundEffectCheck)
    {
        Move();

        if (Collision(player) == 0 && !gameOver && thingsCollisionCheck)
        {
            player.Speed = new Vector2(player.Speed.X, -15);
            _mecollision = true;
        }

        else if (Collision(player) == 1 && !gameOver &&
                 thingsCollisionCheck && collisionCheck)
        {
            player.Speed = new Vector2(player.Speed.X, 0);
            MediaPlayer.Stop();
            if (SoundEffectCheck) sound.Dead.Play();
            collisionCheck = false;
            gameOver = true;
        }

        if (Position.Y < 780 && _mecollision)
            Position = new Rectangle(Position.X, Position.Y + 11, Position.Width, Position.Height);
        if (Position.Y > 785 || BulletCollision(bullet))
        {
            _mecollision = false;
            Position = new Rectangle(20, 800, Position.Width, Position.Height);
        }
    }

    private int Collision(Player player)
    {
        //enemy dead
        if (player.PlayerPosition.Bottom > _position.Top && player.PlayerPosition.Top < _position.Top &&
            player.PlayerPosition.Left < _position.Right && player.PlayerPosition.Right > _position.Left && player.Speed.Y > 0)
        {
            return 0;
        }
        if (player.PlayerPosition.Intersects(_position) || _position.Intersects(player.PlayerPosition) && player.Speed.Y > 0)
        {
            return 1;

        }
        return 2;
    }

    private bool BulletCollision(Bullet bullet)
    {
        var closest = new Vector2(
            MathHelper.Clamp(bullet.Position.X, Position.Left, Position.Right),
            MathHelper.Clamp(bullet.Position.Y, Position.Top, Position.Bottom)
        );
        var distance = Vector2.Distance(new Vector2(bullet.Position.X, bullet.Position.Y), closest);

        return distance <= bullet.Radius;
    }
}