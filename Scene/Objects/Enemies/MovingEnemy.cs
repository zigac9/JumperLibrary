using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Sprites;

namespace JumperLibrary;

public class MovingEnemy
{
    private readonly List<Texture2D> _enemyList;
    private readonly List<Texture2D> _bossList;
    private Vector2 _bullFirePosition;
    private bool _drawFire;

    private Rectangle _position;
    private Vector2 _speed;

    public MovingEnemy(IReadOnlyDictionary<string, Texture2D> textures,
        IReadOnlyDictionary<string, SpriteSheet> spriteSheets, IReadOnlyDictionary<string, SpriteFont> spriteFontsLoad)
    {
        TextureRand = 0;
        SFont = spriteFontsLoad["assets/SpriteFont1"];
        _enemyList = new List<Texture2D> { textures["assets/tri"], textures["assets/stiri"], textures["assets/pet"] };
        _bossList = new List<Texture2D> { textures["assets/dva"], textures["assets/sest"] };
        GetAnimatedSprite = new AnimatedSprite(spriteSheets["assets/fire.sf"]);
        Initialize();
    }

    public Rectangle Position
    {
        get => _position;
        set => _position = value;
    }

    public AnimatedSprite GetAnimatedSprite { get; }
    private SpriteFont SFont { get; }

    public bool Visible { get; set; }

    public int Start { get; set; }

    public int End { get; set; }

    public int View { get; set; }

    public int Step { get; set; }

    private float Degree { get; set; }

    public int TextureRand { get; set; }
    
    public int MaxLife { get; set; }
    
    public int Life { get; set; }
    
    public bool NotDie { get; set; }

    public void Initialize()
    {
        _bullFirePosition = new Vector2(0, 0);
        _drawFire = false;
        Degree = 0;
        Start = 100;
        End = 1500;
        View = 500;
        Step = 1000;
        _position = new Rectangle(20, -200, 80, 60);
        _speed = new Vector2(3, 0);
        Visible = false;
        MaxLife = 2;
        Life = MaxLife;
        NotDie = false;
    }

    public void Draw(SpriteBatch s)
    {
        List<Texture2D> active = new List<Texture2D>();
        if (NotDie)
        {
            active = _bossList;
        }
        else
        {
            active = _enemyList;
        }

        s.Draw(active[TextureRand], _position, null, Color.White, 0f, Vector2.Zero,
            _speed.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

        if (_drawFire)
            GetAnimatedSprite.Draw(s, _bullFirePosition, 10f, new Vector2(3, 3));
    }

    public void Move()
    {
        //Debug.WriteLine("Speed:" + _speed);
        _position.X += (int)_speed.X;
        if (_position.X > 410)
            _speed.X *= -1;
        if (_position.X < 10)
            _speed.X *= -1;
        // Debug.WriteLine("Speed po metodi:" + _speed);
    }

    public void Update(Bullet bullet, Bullet bulletEnemy, Sound sound, Player player,
        ClassEnums.GameStateEnum currentGameState, ref bool collisionCheck, bool SoundEffectCheck)
    {
        if (Life > 0 && bullet.IsCheck && BulletCloseCollision(bullet) && Visible)
        {
            //Debug.WriteLine(Life);
            if (!NotDie)
            {
                switch (_position.X)
                {
                    case >= 0 and < 160:
                        _position = new Rectangle(240, _position.Y, _position.Width, _position.Height);
                        break;
                    case >= 160 and < 320 when _speed.X > 0:
                        _position = new Rectangle(400, _position.Y, _position.Width, _position.Height);
                        break;
                    case >= 160 and < 320 when _speed.X < 0:
                    {
                        _position = new Rectangle(80, _position.Y, _position.Width, _position.Height);
                        break;
                    }
                    case >= 320 and < 480:
                        _position = new Rectangle(240, _position.Y, _position.Width, _position.Height);
                        break;
                }
                Move();
                Life--;
            }
            else
            {
                bullet.IsCheck = false;
                GetAnimatedSprite.Play("fire");
                _drawFire = true;
                _bullFirePosition = new Vector2(bullet.Position.X, bullet.Position.Y);
            }
        }
        else
        {
            _drawFire = false;
            if (BulletCollision(bullet) && Visible)
            {
                Visible = false;
                End = Start + View;
                Life = MaxLife;
            }
            else
            {
                if (Math.Abs(Position.X - player.PlayerPosition.X) < 10 && Position.Y > 0 && Visible && collisionCheck)
                {
                    if (!bulletEnemy.IsCheck)
                    {
                        Degree =
                            // ReSharper disable once PossibleLossOfFraction
                            (float)Math.Atan(-(player.PlayerPosition.Y - 30 - Position.Y) /
                                             (player.PlayerPosition.X - 30 - Position.X));
                        bulletEnemy.Position = new Rectangle(Position.X + 30,
                            Position.Y + 30, bulletEnemy.Position.Width, bulletEnemy.Position.Height);
                        if (player.PlayerPosition.X < Position.X + 30)
                            bulletEnemy.Speed = new Vector2(-1 * (float)Math.Cos(Degree), (float)
                                0.5 * (float)Math.Sin(Degree));
                        else
                            bulletEnemy.Speed = new Vector2(1 * (float)Math.Cos(Degree), (float)
                                0.5 * (float)Math.Sin(Degree));

                        bulletEnemy.IsCheck = true;
                        if (SoundEffectCheck) sound.EnemyShoot.Play();
                    }
                }
            }
        }

        if (bulletEnemy.Position.Y > 740 || bulletEnemy.Position.X is < -20 or > 500 ||
            bulletEnemy.Position.Y < -20)
            bulletEnemy.IsCheck = false;
        if (bulletEnemy.IsCheck && currentGameState == ClassEnums.GameStateEnum.GameRunning)
            bulletEnemy.Move();

        if (bulletEnemy.IsCheck && player.BulletCollision(bulletEnemy))
        {
            player.Speed = new Vector2(player.Speed.X, 0);
            MediaPlayer.Stop();
            if (SoundEffectCheck) sound.Dead.Play();
            bulletEnemy.IsCheck = false;
            collisionCheck = false;
            Life = MaxLife;
        }
    }
    
    public void LifeDraw(SpriteBatch sp)
    {
        String currentLife = "";
        if (NotDie)
        {
            currentLife = "Enemy life: " +(Life+1);
        }
        else
        {
            currentLife = "BOSS life: infinity";
        }
        sp.DrawString(SFont, "Enemy life: " + currentLife, new Vector2(0,40), Color.White, 0f, new Vector2(0, 0), new Vector2(1, 1),
            SpriteEffects.None, 0f);
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

    private bool BulletCloseCollision(Bullet bullet)
    {
        if (bullet.Position.X > _position.X - 30 &&
            bullet.Position.X + bullet.Position.Width < _position.X + _position.Width + 30 &&
            bullet.Position.Y > _position.Y &&
            bullet.Position.Y < _position.Y + _position.Height + 50) return true;
        return false;
    }
}