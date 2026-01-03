using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Buttons;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;


namespace Zoordle.Scenes;

public class GameScene : Scene
{   
    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = false;

        _currentGuess = new GuessBox[5];
        _guessIndex = 0;

        for (int i = 0; i < _currentGuess.Length; i++)
        {
            _currentGuess[i] = new GuessBox
            {
                Letter = '\0',
                Color = Color.White
            };
        }
    }

    public override void LoadContent()
    {
        _font = Core.Content.Load<SpriteFont>("fonts/fonts");
        _letterTileset = Core.Content.Load<Texture2D>("images/letters");
        _guessBoxTileset = Core.Content.Load<Texture2D>("images/empty_box");
        _animalBg = Core.Content.Load<Texture2D>("images/animal_bg");

        _animalTextures = new Dictionary<string, Texture2D>
        {
            { "chaki", Core.Content.Load<Texture2D>("images/chaki")},
            { "drago", Core.Content.Load<Texture2D>("images/drago")},
            { "helga", Core.Content.Load<Texture2D>("images/helga")},
            { "lucek", Core.Content.Load<Texture2D>("images/lucek")},
            { "pyrka", Core.Content.Load<Texture2D>("images/pyrka")},
            { "rambo", Core.Content.Load<Texture2D>("images/rambo")},
            { "ronan", Core.Content.Load<Texture2D>("images/ronan")},
            { "sopel", Core.Content.Load<Texture2D>("images/sopel")},
            { "wigor", Core.Content.Load<Texture2D>("images/wigor")},
            { "zelda", Core.Content.Load<Texture2D>("images/zelda")},
        };

        _keyboardButtons = new List<LetterButton>();
        DrawButtonGrid();

        StartMainLoop();
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
            return;
        }

        var mouse = Core.Input.Mouse;

        foreach(var button in _keyboardButtons)
        {
            if(button.Bounds.Contains(mouse.Position) && mouse.WasButtonJustPressed(MouseButton.Left))
            {
                OnLetterPressed(button.Letter);
                break;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawAnimals();
        DrawGuessBoxes();
        DrawAlphabetGrid();

        Core.SpriteBatch.End();
    }


    private void OnLetterPressed(char letter)
    {
        if (_guessIndex >= _currentGuess.Length)
            return;

        _currentGuess[_guessIndex].Letter = letter;
        _guessIndex++;
    }

    private void DrawAnimals()
    {
        if (_currentAnimalTexture == null)
            return;

        Vector2 position = new Vector2(
            1050,
            _alphabetStart.Y - ANIMAL_TARGET_HEIGHT - 40
        );

        // Background scale
        Vector2 bgScale = GetScaleToFit(
            _animalBg,
            ANIMAL_TARGET_WIDTH,
            ANIMAL_TARGET_HEIGHT
        );

        // Animal scale
        Vector2 animalScale = GetScaleToFit(
            _currentAnimalTexture,
            ANIMAL_TARGET_WIDTH,
            ANIMAL_TARGET_HEIGHT
        );

        // Draw background frame
        Core.SpriteBatch.Draw(
            _animalBg,
            position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            bgScale,
            SpriteEffects.None,
            0f
        );

        // Draw animal on top
        Core.SpriteBatch.Draw(
            _currentAnimalTexture,
            position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            animalScale,
            SpriteEffects.None,
            0f
        );
    }

    private void DrawGuessBoxes()
    {
        Vector2 scale = Vector2.One * LETTER_SCALE;

        for (int i = 0; i < _currentGuess.Length; i++)
        {
            var box = _currentGuess[i];

            Vector2 position = new Vector2(
                100 + i * (LETTER_SIZE * LETTER_SCALE + SPACING),
                400
            );

            // Draw empty box background
            Core.SpriteBatch.Draw(
                _guessBoxTileset,
                position,
                null,
                box.Color,     // tint for Wordle logic later
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );

            // Draw letter if present
            if (box.Letter != '\0')
            {
                string letter = box.Letter.ToString();

                Vector2 textSize = _font.MeasureString(letter);

                Vector2 textPosition = position + new Vector2(
                    (LETTER_SIZE * LETTER_SCALE - textSize.X) * 0.5f,
                    (LETTER_SIZE * LETTER_SCALE - textSize.Y) * 0.5f
                );

                Core.SpriteBatch.DrawString(
                    _font,
                    letter,
                    textPosition,
                    Color.Black
                );
            }
        }
    }

    private void DrawButtonGrid()
    {
        float tile = LETTER_SIZE * LETTER_SCALE;
        float spacing = SPACING * LETTER_SCALE;

        int totalRows = (int)Math.Ceiling(ALPHABET.Length / (float)LETTER_COLUMNS);

        for (int i = 0; i < ALPHABET.Length; i++)
        {
            Rectangle source = new Rectangle(
                i * LETTER_SIZE, 0, LETTER_SIZE, LETTER_SIZE
            );

            int row = i / LETTER_COLUMNS;
            int col = i % LETTER_COLUMNS;

            int lettersInRow = LETTER_COLUMNS;

            if (row == totalRows - 1)
            {
                int remaining = ALPHABET.Length % LETTER_COLUMNS;
                if (remaining != 0)
                    lettersInRow = remaining;
            }

            float rowWidth =
                lettersInRow * tile +
                (lettersInRow - 1) * spacing;

            float fullRowWidth =
                LETTER_COLUMNS * tile +
                (LETTER_COLUMNS - 1) * spacing;

            float centerOffsetX = (fullRowWidth - rowWidth) * 0.5f;

            Vector2 position = new Vector2(
                _alphabetStart.X + centerOffsetX + col * (tile + spacing),
                _alphabetStart.Y + row * (tile + spacing)
            );

            Rectangle bounds = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)tile,
                (int)tile
            );

            _keyboardButtons.Add(new LetterButton
            {
                Letter = ALPHABET[i],
                SourceRect = source,
                Bounds = bounds
            });
        }
    }

    private void StartMainLoop()
    {
        var keys = new List<string>(_animalTextures.Keys);
        _currentAnswer = keys[_rng.Next(keys.Count)];
        _currentAnimalTexture = _animalTextures[_currentAnswer];

        _guessIndex = 0;
        _currentGuess = new GuessBox[_currentAnswer.Length];

        for (int i = 0; i < _currentGuess.Length; i++)
        {
            _currentGuess[i] = new GuessBox
            {
                Letter = '\0',
                Color = Color.White
            };
        }

    }

    private void DrawAlphabetGrid()
    {
        foreach (var button in _keyboardButtons)
        {
            Core.SpriteBatch.Draw(
                _letterTileset,
                new Vector2(button.Bounds.X, button.Bounds.Y),
                button.SourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                LETTER_SCALE,
                SpriteEffects.None,
                0f
            );
        }
    }

    private Vector2 GetScaleToFit(Texture2D texture, int targetWidth, int targetHeight)
    {
        float scaleX = (float)targetWidth / texture.Width;
        float scaleY = (float)targetHeight / texture.Height;
        return new Vector2(scaleX, scaleY);
    }

        private const int LETTER_SIZE = 64;
        
        private const int LETTER_COLUMNS = 9;

        private const int SPACING = 6;

        private const float LETTER_SCALE = 2.0f;

        private const int ANIMAL_TARGET_WIDTH = 300;

        private const int ANIMAL_TARGET_HEIGHT = 500;

        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private SpriteFont _font;

        private Texture2D _letterTileset;

        private Texture2D _guessBoxTileset;

        private Dictionary<string, Texture2D> _animalTextures;   

        private Texture2D _animalBg;    

        private Texture2D _currentAnimalTexture;

        private string _currentAnswer;

        private Random _rng = new Random();

        private Vector2 _alphabetStart = new Vector2(350, 650);

        private List<LetterButton> _keyboardButtons;
        
        private GuessBox[] _currentGuess;

        private int _guessIndex;

        
}
