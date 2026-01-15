using System;
using System.Linq;
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
    public GameScene(int maxRounds)
    {
        _maxRounds = maxRounds;
    }
    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = false;

        StartMainLoop();
    }

    public override void LoadContent()
    {
        _font = Core.Content.Load<SpriteFont>("fonts/fonts");
        _letterTileset = Core.Content.Load<Texture2D>("images/letters");
        _guessBoxTileset = Core.Content.Load<Texture2D>("images/empty_box");
        _animalBg = Core.Content.Load<Texture2D>("images/animal_bg");

        _animalTextures = new Dictionary<string, Texture2D>
        {
            { "CHAKI", Core.Content.Load<Texture2D>("images/chaki")},
            { "DRAGO", Core.Content.Load<Texture2D>("images/drago")},
            { "HELGA", Core.Content.Load<Texture2D>("images/helga")},
            { "LUCEK", Core.Content.Load<Texture2D>("images/lucek")},
            { "PYRKA", Core.Content.Load<Texture2D>("images/pyrka")},
            { "RAMBO", Core.Content.Load<Texture2D>("images/rambo")},
            { "RONAN", Core.Content.Load<Texture2D>("images/ronan")},
            { "SOPEL", Core.Content.Load<Texture2D>("images/sopel")},
            { "WIGOR", Core.Content.Load<Texture2D>("images/wigor")},
            { "ZELDA", Core.Content.Load<Texture2D>("images/zelda")},
        };

        _keyboardButtons = new List<LetterButton>();
        DrawButtonGrid();
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
            return;
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter)){
            OnEnterPressed();
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

    private void OnEnterPressed()
    {
        // Only process if current guess is full
        if (_guessIndex < _currentGuess.Length)
            return;

        // Evaluate the current guess and update letter states
        EvaluateGuess();

        // Lock the current guess into past guesses
        GuessBox[] lockedGuess = new GuessBox[_currentGuess.Length];
        for (int i = 0; i < _currentGuess.Length; i++)
        {
            lockedGuess[i] = new GuessBox
            {
                Letter = _currentGuess[i].Letter,
                State = _currentGuess[i].State
            };
        }
        _pastGuesses.Add(lockedGuess);

        // Check if the guess was correct
        if (IsCorrectGuess() || _pastGuesses.Count >= _maxRounds)
        {
            // Reset game: new answer, clear past guesses and keyboard
            StartMainLoop();
            _pastGuesses.Clear();
            foreach (var button in _keyboardButtons)
            {
                button.State = GuessBoxState.Empty;
            }
            return;
        }

        // Prepare next guess
        _guessIndex = 0;
        for (int i = 0; i < _currentGuess.Length; i++)
        {
            _currentGuess[i].Letter = '\0';
            _currentGuess[i].State = GuessBoxState.Empty;
        }
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
        
            // scale background slightly bigger
        bgScale = new Vector2(
            (ANIMAL_TARGET_WIDTH + ANIMAL_BG_PADDING * 2) / (float)_animalBg.Width,
            (ANIMAL_TARGET_HEIGHT + ANIMAL_BG_PADDING * 2) / (float)_animalBg.Height
        );

        // background position (offset so it surrounds animal)
        Vector2 bgPosition = position - new Vector2(
            ANIMAL_BG_PADDING,
            ANIMAL_BG_PADDING
        );
        // Draw background frame
        Core.SpriteBatch.Draw(
            _animalBg,
            bgPosition,
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
        float startY = 50;
        float spacingY = LETTER_SIZE * LETTER_SCALE + SPACING;

        // Draw previous guesses
        for (int row = 0; row < _pastGuesses.Count; row++)
        {
            var guessRow = _pastGuesses[row];

            for (int i = 0; i < Math.Min(guessRow.Length, 5); i++)
            {
                var box = guessRow[i];
                Vector2 position = new Vector2(
                    100 + i * (LETTER_SIZE * LETTER_SCALE + SPACING),
                    startY + row * spacingY
                );

                // Draw box background with color based on state
                Core.SpriteBatch.Draw(
                    _guessBoxTileset,
                    position,
                    null,
                    box.Color,
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

        int currentRow = _pastGuesses.Count;
        for (int i = 0; i < Math.Min(_currentGuess.Length, 5); i++)
        {
            var box = _currentGuess[i];

            Vector2 position = new Vector2(
                100 + i * (LETTER_SIZE * LETTER_SCALE + SPACING),
                startY + currentRow * spacingY
            );

            // Draw empty box background
            Core.SpriteBatch.Draw(
                _guessBoxTileset,
                position,
                null,
                box.Color,
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
    // Logic similar to DrawGuessBoxes 
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

    private void DrawAlphabetGrid()
    {
        foreach (var button in _keyboardButtons)
        {
            Core.SpriteBatch.Draw(
                _letterTileset,
                new Vector2(button.Bounds.X, button.Bounds.Y),
                button.SourceRect,
                button.Color,
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

    private void EvaluateGuess()
    {
        string guess = _currentAnswer;
        int length = guess.Length;

        bool[] letterUsed = new bool[length];

        for (int i = 0; i < length; i++)
        {
            if (_currentGuess[i].Letter == guess[i])
            {
                _currentGuess[i].State = GuessBoxState.Correct;
                letterUsed[i] = true;
            }
        }

        for (int i = 0; i < length; i++)
        {
            if (_currentGuess[i].State == GuessBoxState.Correct)
                continue;

            bool found = false;
            for (int j = 0; j < length; j++)
            {
                if (!letterUsed[j] && _currentGuess[i].Letter == guess[j])
                {
                    found = true;
                    letterUsed[j] = true;
                    break;
                }
            }

            _currentGuess[i].State = found ? GuessBoxState.Present : GuessBoxState.Absent;
        }

        foreach (var button in _keyboardButtons)
        {
            char btnLetter = char.ToUpper(button.Letter);

            // Find all guesses that match this button
            var matchingGuesses = _currentGuess.Where(g => char.ToUpper(g.Letter) == btnLetter);

            foreach (var guessBox in matchingGuesses)
            {
                // Priority: Correct > Present > Absent > Empty
                if (guessBox.State == GuessBoxState.Correct)
                {
                    button.State = GuessBoxState.Correct;
                    break; // Once Correct, no need to check other occurrences
                }
                else if (guessBox.State == GuessBoxState.Present && button.State != GuessBoxState.Correct)
                {
                    button.State = GuessBoxState.Present;
                }
                else if (guessBox.State == GuessBoxState.Absent && button.State == GuessBoxState.Empty)
                {
                    button.State = GuessBoxState.Absent;
                }
            }
        }
        Console.WriteLine("Evaluated guess: " + new string(_currentGuess.Select(g => g.Letter).ToArray()));
    }

    private bool IsCorrectGuess()
    {
        foreach (var box in _currentGuess)
        {
            if (box.State != GuessBoxState.Correct)
                return false;
        }
        return true;
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
                State = GuessBoxState.Empty
            };
        }
    }
    /*
        varibles
    */
    
    // Base pixel size of a single letter tile (before scaling)
    private const int LETTER_SIZE = 64;

    // Number of letters per keyboard row
    private const int LETTER_COLUMNS = 9;

    // Space in pixels between letters/tiles
    private const int SPACING = 6;

    // Scale multiplier applied to letters and boxes
    private const float LETTER_SCALE = 2.0f;

    // Target width for displaying the animal image
    private const int ANIMAL_TARGET_WIDTH = 300;

    // Target height for displaying the animal image
    private const int ANIMAL_TARGET_HEIGHT = 500;

    // Padding around the animal background frame
    private const int ANIMAL_BG_PADDING = 12;

    // All supported keyboard letters
    private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    // Font used for drawing letters and text
    private SpriteFont _font;

    // Tileset texture containing alphabet letter sprites
    private Texture2D _letterTileset;

    // Texture used for the guess box background
    private Texture2D _guessBoxTileset;

    // Stored previous guesses (one array per guess row)
    private List<GuessBox[]> _pastGuesses = new List<GuessBox[]>();

    // Mapping of animal names to their textures
    private Dictionary<string, Texture2D> _animalTextures;

    // Background frame texture behind the animal
    private Texture2D _animalBg;

    // Currently displayed animal texture
    private Texture2D _currentAnimalTexture;

    // Correct answer word for the current round
    private string _currentAnswer;

    // Random generator for selecting animals
    private Random _rng = new Random();

    // Top-left position of the on-screen keyboard
    private Vector2 _alphabetStart = new Vector2(350, 650);

    // All interactive keyboard letter buttons
    private List<LetterButton> _keyboardButtons;

    // Currently active guess being typed
    private GuessBox[] _currentGuess;

    // Index of the next letter to fill in the current guess
    private int _guessIndex;

    // max number of rounds
    private int _maxRounds;
}
