using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace JumperLibrary;

public class Player
{
    private readonly Dictionary<string, Texture2D> _textures;
    private int _accelarator;
    private Texture2D _active;
    private int _ch;

    private Vector2 _firePosition;

    private Rectangle _position;
    private Rectangle _shootPosition;
    private Vector2 _speed;

    public Player(Dictionary<string, Texture2D> textures, IReadOnlyDictionary<string, SpriteSheet> spriteSheets)
    {
        _textures = textures;
        GetAnimatedSprite = new AnimatedSprite(spriteSheets["assets/fire.sf"]);
        Initialize();
    }

    public Rectangle PlayerPosition
    {
        get => _position;
        set => _position = value;
    }

    public Rectangle ShootPosition
    {
        get => _shootPosition;
        set => _shootPosition = value;
    }

    public Vector2 Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float Degree { get; set; }

    public float ShootDegree { get; set; }

    public Vector2 FirePosition
    {
        get => _firePosition;
        set => _firePosition = value;
    }

    public bool IsJetpack { get; set; }

    public AnimatedSprite GetAnimatedSprite { get; }

    public void Initialize()
    {
        _accelarator = 1;
        _ch = 0;
        Degree = 0;
        IsJetpack = false;
        _speed = new Vector2(0, -13);
        _position = new Rectangle(230, 560, 60, 60);
        _shootPosition = new Rectangle(_position.X + _position.Width / 2, _position.Y + _position.Height / 2 + 15, 12,
            55);
        _firePosition = new Vector2(_position.X, _position.Y + _position.Height);
    }

    public void Move()
    {
        if (_ch == 0)
        {
            _speed.Y += _accelarator;
            _ch = 1;
        }
        else
        {
            _ch = 0;
        }

        _position.Y += (int)_speed.Y;
        _shootPosition.Y = _position.Y + _position.Height / 2 + 15;
        _firePosition.Y = _position.Y + _position.Height;
    }

    public void Draw(SpriteBatch s, ClassEnums.PlayerOrientEnum name, ClassEnums.GameStateEnum game, bool collisionCheck)
    {
        if (game == ClassEnums.GameStateEnum.GameRunning)
        {
            switch (name)
            {
                case ClassEnums.PlayerOrientEnum.Left:
                    if (IsJetpack)
                    {
                        _active = _textures["assets/manjetpack"];
                        _firePosition = new Vector2(_firePosition.X + _position.Width - 10, _firePosition.Y);
                        GetAnimatedSprite.Draw(s, _firePosition, 0f, new Vector2(2, 2));
                    }
                    else if (!collisionCheck)
                    {
                        _active = _textures["assets/dead"];
                    }
                    else
                    {
                        _active = _textures["assets/DoodleR1"];
                    }

                    s.Draw(_active, _position, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                    break;
                case ClassEnums.PlayerOrientEnum.Right:
                    if (IsJetpack)
                    {
                        _active = _textures["assets/manjetpack"];
                        _firePosition = new Vector2(_firePosition.X + 10, _firePosition.Y);
                        GetAnimatedSprite.Draw(s, _firePosition, 0f, new Vector2(2, 2));
                    }
                    else if (!collisionCheck)
                    {
                        _active = _textures["assets/dead"];
                    }
                    else
                    {
                        _active = _textures["assets/DoodleR1"];
                    }

                    s.Draw(_active, _position, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
                    break;
            }

            s.Draw(_textures["assets/injection"], _shootPosition, null, Color.White, ShootDegree, new Vector2(50, 0),
                SpriteEffects.None, 1f);
        }
        else if (game == ClassEnums.GameStateEnum.IntroMenu)
        {
            s.Draw(_textures["assets/DoodleR1"], _position, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None,
                0f);
        }
    }

    public void Update()
    {
        if (PlayerPosition.X + 10 < 0)
        {
            PlayerPosition = new Rectangle(450, PlayerPosition.Y, PlayerPosition.Width,
                PlayerPosition.Height);
            ShootPosition = new Rectangle(PlayerPosition.X + PlayerPosition.Width / 2,
                PlayerPosition.Y + PlayerPosition.Height / 2 + 15, ShootPosition.Width,
                ShootPosition.Height);
            FirePosition = new Vector2(PlayerPosition.X,
                PlayerPosition.Y + PlayerPosition.Height);
        }

        if (PlayerPosition.X > 451)
        {
            PlayerPosition = new Rectangle(-10, PlayerPosition.Y, PlayerPosition.Width,
                PlayerPosition.Height);
            ShootPosition = new Rectangle(PlayerPosition.X + PlayerPosition.Width / 2,
                PlayerPosition.Y + PlayerPosition.Height / 2 + 15, ShootPosition.Width,
                ShootPosition.Height);
            FirePosition = new Vector2(PlayerPosition.X,
                PlayerPosition.Y + PlayerPosition.Height);
        }
    }

    public bool BulletCollision(Bullet bullet)
    {
        return bullet.Position.Intersects(_position) || _position.Intersects(bullet.Position);
    }
}