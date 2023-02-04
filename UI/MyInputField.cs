using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System;
using MonoGame.Extended;

namespace JumperLibrary
{
    public class MyInputField
    {
        public Color textColor; // Color of the main text (Default is black)
        public float scale; // Scale of the Input Field (default is 1)
        public bool allowSpaces; // Allow the use of the spacebar (default is false)
        public Color borderColor; // Input Field border color (Default is black)
        public Color backgroundColor; // Input Field background color (Default is white)
        public int borderThickness;

        private bool finishedWriting; // Triggered when Input Field loses focus
        private Vector2 position; // Stores the position of Input Field
        private bool hasFocus; // Does the Input Field have focus right now?
        private SpriteFont font; // Font used for text rendering
        private StringBuilder text; // Actual text for the Input Field
        private string hintText; // Text shown if input field is empty
        private KeyboardState lastKeyboardState; // This is the keyboard state for last frame, used for keyboard events
        private int textLengthCap; // The maximum length the Input Field can store
        private Rectangle rectangle; // The hitbox for the Input Field
        private Texture2D whiteTexture; // Used to  draw a rectangle for the Input Field
        private int startIndexToDraw; // Start drwaing from this index

        // Used to leave some space for text when drawing the rectangle for the Input Field
        private const int widthPadding = 4;
        private const int heightPadding = 4;

        public MyInputField(GraphicsDevice device, SpriteFont font, Vector2 position, string hintText, int maxLength = int.MaxValue)
        {
            this.font = font;
            this.hintText = hintText;
            this.position = position;

            allowSpaces = true;
            textLengthCap = maxLength;
            hasFocus = false;
            textColor = Color.Black;
            scale = 1;
            text = new StringBuilder();
            rectangle = new Rectangle(position.ToPoint(), new Point(widthPadding * 2, heightPadding * 2) + font.MeasureString(hintText).ToPoint());
            whiteTexture = new Texture2D(device, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
            borderColor = Color.Black;
            backgroundColor = Color.White;
            borderThickness = 2;
            finishedWriting = false;
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            // Keys pressed being recorded here
            foreach(Keys key in keyboardState.GetPressedKeys())
            {
                if (KeyJustPressed(key, keyboardState))
                {
                    if (hasFocus && (key == Keys.Back || key == Keys.Delete)) // User wants to erase a character
                    {
                        if (text.Length > 0)
                        {
                            text.Remove(text.Length - 1, 1);
                            startIndexToDraw = Math.Clamp(startIndexToDraw - 1, 0, int.MaxValue);
                        }
                    }
                    else if (key == Keys.Enter) // User wants to finish writing to the Input Field
                    {
                        bool prevHasFocus = hasFocus;

                        hasFocus = !hasFocus;
                        finishedWriting = true;

                        borderColor = Color.Green;

                        if (prevHasFocus != hasFocus && !hasFocus)
                        {
                            borderColor = Color.Black;
                            finishedWriting = true;
                        }
                    }
                    else if (hasFocus && text.Length < textLengthCap) // If user can add more characters
                    {
                        string characterToDraw = keyboardState.CapsLock || keyboardState.IsKeyDown(Keys.LeftShift) ? key.ToString() : key.ToString().ToLower();

                        if (key == Keys.Space && !allowSpaces)
                            continue;

                        text.Append(key == Keys.Space? ' ' : key.ToString());

                        if (font.MeasureString(text).X > rectangle.Width - 2 * widthPadding)
                            startIndexToDraw += 1;
                    }
                }    
            }

            lastKeyboardState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle scaledRectangle = new Rectangle(position.ToPoint(), new Point((int)(rectangle.Width * scale), (int)(rectangle.Height * scale)));

            // Draw Background
            spriteBatch.Draw(whiteTexture, scaledRectangle, backgroundColor);

            // Draw border on top
            DrawBorder(spriteBatch, scaledRectangle);

            // Draw text
            if (GetLength() > 0)
                spriteBatch.DrawString(font, text.ToString().Substring(startIndexToDraw), position + new Vector2(widthPadding, heightPadding), textColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(font, hintText, position + new Vector2(widthPadding, heightPadding), Color.Gray, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        // Does the Input Field have any text inside?
        public bool HasValue()
        {
            return text.Length > 0;
        }

        // Get the length if the text inside the Input Field
        public int GetLength()
        {
            return text.Length;
        }

        public bool FinishedEditing()
        {
            return finishedWriting;
        }

        #region HelperFunctions

        // Is the given key just pressed during this frame?
        private bool KeyJustPressed(Keys key, KeyboardState state)
        {
            return state.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Rectangle rect)
        {
            spriteBatch.Draw(whiteTexture, new Rectangle(rect.Left, rect.Top, rect.Width, borderThickness), borderColor); // Top
            spriteBatch.Draw(whiteTexture, new Rectangle(rect.Left, rect.Bottom - borderThickness, rect.Width, borderThickness), borderColor); // Bottom
            spriteBatch.Draw(whiteTexture, new Rectangle(rect.Left, rect.Top, borderThickness, rect.Height), borderColor); // Left
            spriteBatch.Draw(whiteTexture, new Rectangle(rect.Right - borderThickness, rect.Top, borderThickness, rect.Height), borderColor); // Right
        }

        #endregion
    }
}
