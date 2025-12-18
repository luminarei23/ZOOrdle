using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;


namespace Zoordle.Scenes
{
    public class GameScene : Scene
    {

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // Initialize list for sprites spliced later
        _letterSprites = new List<Texture2D> {};
        _letterSourceRect = new Rectangle(0, 0, LETTER_SIZE, LETTER_SIZE);

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

    }

    public override void LoadContent()
    {
        // Load the font.
        _font = Core.Content.Load<SpriteFont>("fonts/fonts");

        // Set up the letter tileset sprite.
        _letterTileset = Core.Content.Load<Texture2D>("images/letters");


    }

    public override void Update(GameTime gameTime)
    {
        // Check for keyboard input and handle it.
        CheckKeyboardInput();
    }

    private void CheckKeyboardInput()
    {
        // Get a reference to the keyboard inof
        KeyboardInfo keyboard = Core.Input.Keyboard;
        
        // If the escape key is pressed, return to the title screen.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
        }
    }

    private void DisplayAllLetters()
    {
        //var screen = Core.GraphicsDevice.PresentationParameters.Bounds;

        float scaledTile = LETTER_SIZE * LETTER_SCALE;
        float scaledSpacing = SPACING * LETTER_SCALE;

        int totalRows = (int)Math.Ceiling(_lettersInPlay.Length / (float)LETTER_COLUMNS);

        Vector2 startPosition = new Vector2(250,650);

        for (int i = 0; i < _lettersInPlay.Length; i++)
        {
            Rectangle sourceRect = new Rectangle(
                i * LETTER_SIZE,
                0,
                LETTER_SIZE,
                LETTER_SIZE
            );

            int row = i / LETTER_COLUMNS;
            int col = i % LETTER_COLUMNS;

            int lettersInRow = LETTER_COLUMNS;

            if (row == totalRows -1)
            {
                int remaining = _lettersInPlay.Length % LETTER_COLUMNS;
                if (remaining != 0)
                    {
                        lettersInRow = remaining;
                    }        
            }

            float rowWidth = lettersInRow * scaledTile + (lettersInRow -1) * scaledSpacing;
            float fullRowWidth = LETTER_COLUMNS * scaledTile + (LETTER_COLUMNS - 1 ) * scaledSpacing;
            float centerOffsetX = (fullRowWidth - rowWidth) * 0.5f;

            Vector2 position = new Vector2(
                startPosition.X + centerOffsetX + col * (scaledTile + scaledSpacing),
                startPosition.Y + row * (scaledTile + scaledSpacing)
            );

            Core.SpriteBatch.Draw(
                _letterTileset,
                position,
                sourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                2f,
                SpriteEffects.None,
                0f
            );
        }
    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the score.
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

        DisplayAllLetters();

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
    private const int LETTER_SIZE = 64;

    private const int LETTER_COLUMNS = 9;

    private const int SPACING = 8;

    private const float LETTER_SCALE = 1.8f;
    
    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    // The Sprite2d used for the all letter tiles.
    private Texture2D _letterTileset;

    private List<Texture2D> _letterSprites;

    private Rectangle _letterSourceRect;

    // The letters currently in play. A equal to 65, B = 66, etc.
    private Enum[] _lettersInPlay = {
        Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H,
        Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q,
        Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z
        };
    }
}