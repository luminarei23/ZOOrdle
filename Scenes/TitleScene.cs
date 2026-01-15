using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;

namespace Zoordle.Scenes;

public class TitleScene : Scene
{
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        Core.ExitOnEscape = true;

        // Set the position and origin for the Dungeon text.
        Vector2 size = _font5x.MeasureString(ZOORDLE_TEXT);
        _zoordleTextPos = new Vector2(960, 300);
        _zoordleTextOrigin = size * 0.5f;

        // Set the position and origin for the press enter text.
        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new Vector2(960, 620);
        _pressEnterOrigin = size * 0.5f;

        _roundsPos = new Vector2(960, 720);
        _roundsOrigin = _font.MeasureString(NUM_OF_ROUNDS_TEXT) * new Vector2(0.5f, 0f);
    }

    public override void LoadContent()
    {
        // Load the font for the standard text.
        _font = Core.Content.Load<SpriteFont>("fonts/fonts");

        // Load the font for the title text.
        _font5x = Content.Load<SpriteFont>("fonts/fonts");

        Texture2D bgTexture = Content.Load<Texture2D>("images/background");

        _background = new Sprite(bgTexture)
        {
            Position = Vector2.Zero,
            Scale = Vector2.One
        };
    }

    public override void Update(GameTime gameTime)
    {
        // Handle the num of rounds input
        HandleNumericInput();
        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {   
            if(string.IsNullOrWhiteSpace(_numOfRounds))
            {
                _maxRounds = 10;
            }
            else if (!int.TryParse(_numOfRounds, out _maxRounds))
            {
                    _maxRounds = 10;
            }
            else if (_maxRounds < 3 || _maxRounds > 10)
            {
                _maxRounds = 10;
            }

            Core.ChangeScene(new GameScene(_maxRounds));
        }
    }

    public override void Draw(GameTime gameTime)
    {   
        string roundsText = $"{NUM_OF_ROUNDS_TEXT} {_numOfRounds}";

        Core.GraphicsDevice.Clear(Color.LightSkyBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        _background.Draw(Core.SpriteBatch);
        
        // The color to use for the drop shadow text.
        Color dropShadowColor = Color.Black * 0.5f;

        // Draw the zoordle text slightly offset from it is original position and
        // with a transparent color to give it a drop shadow.
        Core.SpriteBatch.DrawString(_font5x, ZOORDLE_TEXT, _zoordleTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, _zoordleTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the Zoordle text on top of that at its original position.
        Core.SpriteBatch.DrawString(_font5x, ZOORDLE_TEXT, _zoordleTextPos, Color.DarkSeaGreen, 0.0f, _zoordleTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        Core.SpriteBatch.DrawString(_font5x, PRESS_ENTER_TEXT, _pressEnterPos + new Vector2(10, 10), dropShadowColor, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the press enter text.
        Core.SpriteBatch.DrawString(_font, PRESS_ENTER_TEXT, _pressEnterPos, Color.DarkSeaGreen, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the number of rounds input shadow
        Core.SpriteBatch.DrawString(_font, roundsText, _roundsPos + new Vector2(10, 10), dropShadowColor, 0.0f, _roundsOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the number of rounds input
        Core.SpriteBatch.DrawString(_font, roundsText, _roundsPos, Color.DarkSeaGreen, 0.0f, _roundsOrigin, 1.0f, SpriteEffects.None, 0.0f);

        Core.SpriteBatch.End();
    }

    public void HandleNumericInput()
    {
        if(_numOfRounds.Length >= 2)
            return;

        if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D0))
            _numOfRounds += "0";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D1))
            _numOfRounds += "1";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D2))
            _numOfRounds += "2";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D3))
            _numOfRounds += "3";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D4))
            _numOfRounds += "4";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D5))
            _numOfRounds += "5";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D6))
            _numOfRounds += "6";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D7))
            _numOfRounds += "7";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D8))
            _numOfRounds += "8";
        else if(Core.Input.Keyboard.WasKeyJustPressed(Keys.D9))
            _numOfRounds += "9";

        if(Core.Input.Keyboard.WasKeyJustPressed(Keys.Back) && _numOfRounds.Length > 0)
        {
            _numOfRounds = _numOfRounds.Substring(0, _numOfRounds.Length - 1);
        }
    }
 
    /*
        varibles
    */

    // Text constants
    private const string ZOORDLE_TEXT = "Zoordle!";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";
    private const string NUM_OF_ROUNDS_TEXT = "Number of Rounds";

    // Size of each letter in pixels
    private const int LETTER_SIZE = 64;

    // background of the scene
    private Sprite _background;
    
    // The font to use to render normal text.
    private SpriteFont _font;

    // The font used to render the title text.
    private SpriteFont _font5x;

    // The position to draw the dungeon text at.
    private Vector2 _zoordleTextPos;

    // The origin to set for the dungeon text.
    private Vector2 _zoordleTextOrigin;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    private Vector2 _roundsPos;

    private Vector2 _roundsOrigin;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    // Amount of rounds captured from input
    private string _numOfRounds = "";

    // Maximum number of rounds allowed
    private int _maxRounds;
}

