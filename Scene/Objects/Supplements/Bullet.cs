using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumperLibrary;

public class Bullet
{
    private readonly float _accelertion;
    private readonly List<Texture2D> _shootList;
    private readonly int _textureRnd;

    private Rectangle _position;
    private Vector2 _speed;

    public Bullet(int rnd, Dictionary<string, Texture2D> textures)
    {
        _textureRnd = rnd;
        _shootList = new List<Texture2D> { textures["assets/tir"], textures["assets/virus"] };
        _accelertion = 0.5f;
        _speed = new Vector2(0, 0);
        Radius = 10;
        Initialize();
    }

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

    public bool IsCheck { get; set; }
    
    public int Radius { get; }

    public void Initialize()
    {
        // int diameter = 20;
        // int radius = diameter / 2;
        // Rectangle circleRect = new Rectangle(200 - radius, 200 - radius, diameter, diameter);
        IsCheck = false;
        _position = new Rectangle(-50, -50, 20, 20);
    }

    public void Draw(SpriteBatch s, ClassEnums.GameStateEnum gameState)
    {
        if (gameState == ClassEnums.GameStateEnum.GameRunning)
            s.Draw(_shootList[_textureRnd], _position, Color.White);
    }

    public void Move()
    {
        _speed.Y += _accelertion;
        _position.Y += (int)_speed.Y;
        _position.X += (int)_speed.X;
    }

    public bool BulletCollision(Bullet bullet)
    {
        var distance = Vector2.Distance(new Vector2(_position.X, _position.Y), new Vector2(bullet._position.X, bullet.Position.Y));
        return distance <= Radius + bullet.Radius;
    }
}