using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JumperLibrary;

public class ScorClass
{
    private readonly Vector2 _position;

    public ScorClass(Dictionary<string, SpriteFont> spriteFontsLoad)
    {
        Score = 0;
        _position = new Vector2(15f, 4f);
        SFont = spriteFontsLoad["assets/SpriteFont1"];
    }

    public SpriteFont SFont { get; set; }

    public int Score { get; set; }

    public void Draw(SpriteBatch sp, ClassEnums.GameStateEnum gameState)
    {
        sp.DrawString(SFont, Score.ToString(), _position, Color.White, 0f, new Vector2(0, 0), new Vector2(1, 1),
            SpriteEffects.None, 0f);
    }
}