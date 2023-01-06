using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace JumperLibrary;

public class Pointer
{
    public Pointer(Dictionary<string, SpriteSheet> spriteSheetDictionary)
    {
        Position = new Vector2(0, 0);
        var spriteSheet = spriteSheetDictionary["assets/shoot.sf"];
        GetAnimatedSprite = new AnimatedSprite(spriteSheet);
    }

    public Vector2 Position { get; set; }

    public AnimatedSprite GetAnimatedSprite { get; }

    public void Draw(SpriteBatch sp)
    {
        GetAnimatedSprite.Draw(sp, Position, 0f, new Vector2(0.5f, 0.5f));
    }
}