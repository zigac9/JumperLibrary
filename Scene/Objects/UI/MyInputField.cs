using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumperLibrary;

public class MyInputField
{
    // Used to leave some space for text when drawing the rectangle for the Input Field
    private const int WidthPadding = 4;
    private const int HeightPadding = 4;
    private readonly SpriteFont _font; // Font used for text rendering
    private readonly Rectangle _rectangle; // The hitbox for the Input Field
    private readonly int _textLengthCap; // The maximum length the Input Field can store
    private readonly Texture2D _whiteTexture; // Used to  draw a rectangle for the Input Field
    public readonly bool AllowSpaces; // Allow the use of the spacebar (default is false)
    public Color BackgroundColor; // Input Field background color (Default is white)
    public Color BorderColor; // Input Field border color (Default is black)
    public readonly int BorderThickness;
    private bool _finishedWriting; // Triggered when Input Field loses focus
    private bool _hasFocus; // Does the Input Field have focus right now?
    private KeyboardState _lastKeyboardState; // This is the keyboard state for last frame, used for keyboard events
    private Vector2 _position; // Stores the position of Input Field
    public readonly float Scale; // Scale of the Input Field (default is 1)
    private int _startIndexToDraw; // Start drwaing from this index
    public StringBuilder Text { get; set; } // Actual text for the Input Field
    public Color TextColor; // Color of the main text (Default is black)

    public MyInputField(GraphicsDevice device, SpriteFont font, Vector2 position, string hintText,
        int maxLength = int.MaxValue)
    {
        _font = font;
        HintText = hintText;
        _position = position;

        AllowSpaces = true;
        _textLengthCap = maxLength;
        _hasFocus = false;
        TextColor = Color.Black;
        Scale = 1;
        Text = new StringBuilder();
        _rectangle = new Rectangle(position.ToPoint(),
            new Point(WidthPadding * 2, HeightPadding * 2) + font.MeasureString(hintText).ToPoint());
        _whiteTexture = new Texture2D(device, 1, 1);
        _whiteTexture.SetData(new[] { Color.White });
        BorderColor = Color.Black;
        BackgroundColor = Color.White;
        BorderThickness = 2;
        _finishedWriting = false;
    }

    public string HintText { get; set; }

    public void Update(KeyboardState keyboardState, MouseState mouseState)
    {
        switch (mouseState)
        {
            case { LeftButton: ButtonState.Pressed, X: > 100 and < 388, Y: > 358 and < 412 }:
                _hasFocus = true;
                BorderColor = Color.LightGreen;
                BackgroundColor = Color.WhiteSmoke;
                HintText = "Typing ...             ";
                break;
            case { LeftButton: ButtonState.Pressed }:
                _hasFocus = false;
                BorderColor = Color.Black;
                BackgroundColor = Color.White;
                HintText = "Enter your name ...    ";
                break;
        }
        
        // Keys pressed being recorded here
        foreach (var key in keyboardState.GetPressedKeys())
            if (KeyJustPressed(key, keyboardState))
            {
                if (_hasFocus && key is Keys.Back or Keys.Delete) // User wants to erase a character
                {
                    if (Text.Length > 0)
                    {
                        Text.Remove(Text.Length - 1, 1);
                        _startIndexToDraw = Math.Clamp(_startIndexToDraw - 1, 0, int.MaxValue);
                    }
                }
                // else if (key == Keys.Enter) // User wants to finish writing to the Input Field
                // {
                //     var prevHasFocus = _hasFocus;
                //
                //     _hasFocus = !_hasFocus;
                //
                //     BorderColor = Color.Green;
                //
                //     if (prevHasFocus != _hasFocus && !_hasFocus)
                //     {
                //         BorderColor = Color.Black;
                //     }
                // }
                else if (_hasFocus && Text.Length < _textLengthCap) // If user can add more characters
                {
                    if (key == Keys.Space && !AllowSpaces)
                        continue;

                    if (key is >= Keys.D0 and <= Keys.D9 or >= Keys.NumPad0 and <= Keys.NumPad9
                        or >= Keys.A and <= Keys.Z or Keys.Space)
                    {
                        var characterToDraw = keyboardState.CapsLock || keyboardState.IsKeyDown(Keys.LeftShift)
                            ? key.ToString()
                            : key.ToString().ToLower();
                        if (key is (Keys.D0 or Keys.D1 or Keys.D2 or Keys.D3 or Keys.D4 or Keys.D5 or Keys.D6 or Keys.D7
                            or Keys.D8 or Keys.D9))
                            Text.Append(characterToDraw[1..]);
                        else
                            Text.Append(key == Keys.Space ? ' ' : characterToDraw);
                    }

                    if (_font.MeasureString(Text).X > _rectangle.Width - 2 * WidthPadding)
                        _startIndexToDraw += 1;
                }
            }
        _lastKeyboardState = keyboardState;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var scaledRectangle = new Rectangle(_position.ToPoint(),
            new Point((int)(_rectangle.Width * Scale), (int)(_rectangle.Height * Scale)));

        // Draw Background
        spriteBatch.Draw(_whiteTexture, scaledRectangle, BackgroundColor);

        // Draw border on top
        DrawBorder(spriteBatch, scaledRectangle);

        // Draw text
        if (GetLength() > 0)
            spriteBatch.DrawString(_font, Text.ToString().Substring(_startIndexToDraw),
                _position + new Vector2(WidthPadding, HeightPadding), TextColor, 0, Vector2.Zero, Scale,
                SpriteEffects.None, 0);
        else
            spriteBatch.DrawString(_font, HintText, _position + new Vector2(WidthPadding, HeightPadding), Color.Gray, 0,
                Vector2.Zero, Scale, SpriteEffects.None, 0);
    }

    // Does the Input Field have any text inside?
    public bool HasValue()
    {
        return Text.Length > 0;
    }

    // Get the length if the text inside the Input Field
    public int GetLength()
    {
        return Text.Length;
    }

    public bool FinishedEditing()
    {
        return _finishedWriting;
    }

    public void DrawName(SpriteBatch sp)
    {
        sp.DrawString(_font, "Hello " + Text, new Vector2(30, 675), Color.Black, 0f, new Vector2(0, 0),
            new Vector2(1, 1),
            SpriteEffects.None, 0f);
    }

    #region HelperFunctions

    // Is the given key just pressed during this frame?
    private bool KeyJustPressed(Keys key, KeyboardState state)
    {
        return state.IsKeyDown(key) && _lastKeyboardState.IsKeyUp(key);
    }

    private void DrawBorder(SpriteBatch spriteBatch, Rectangle rect)
    {
        spriteBatch.Draw(_whiteTexture, new Rectangle(rect.Left, rect.Top, rect.Width, BorderThickness),
            BorderColor); // Top
        spriteBatch.Draw(_whiteTexture,
            new Rectangle(rect.Left, rect.Bottom - BorderThickness, rect.Width, BorderThickness),
            BorderColor); // Bottom
        spriteBatch.Draw(_whiteTexture, new Rectangle(rect.Left, rect.Top, BorderThickness, rect.Height),
            BorderColor); // Left
        spriteBatch.Draw(_whiteTexture,
            new Rectangle(rect.Right - BorderThickness, rect.Top, BorderThickness, rect.Height), BorderColor); // Right
    }

    #endregion
}