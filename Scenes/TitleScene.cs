using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using System.Collections.Generic;

namespace Zoordle.Scenes;

public class TitleScene : Scene
{
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Set the position and origin for the Dungeon text.
        Vector2 size = _font5x.MeasureString(ZOORDLE_TEXT);
        _zoordleTextPos = new Vector2(640, 100);
        _zoordleTextOrigin = size * 0.5f;

        // Set the position and origin for the press enter text.
        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _pressEnterPos = new Vector2(640, 620);
        _pressEnterOrigin = size * 0.5f;
    }

    public override void LoadContent()
    {
        // Load the font for the standard text.
        _font = Core.Content.Load<SpriteFont>("fonts/fonts");

        // Load the font for the title text.
        _font5x = Content.Load<SpriteFont>("fonts/fonts");

        // Texture2D lettersTexture = Core.Content.Load<Texture2D>("images/letter_background");

        // _letterAtlas = TextureAtlas.FromGrid(lettersTexture, 64, 64, "letter");

        // TextureAtlas bgAtlas = TextureAtlas.FromFile(Content, )

        Texture2D bgTexture = Content.Load<Texture2D>("images/background");

        _background = new Sprite(bgTexture)
        {
            Position = Vector2.Zero,
            Scale = Vector2.One
        };

    }

        public override void Update(GameTime gameTime)
    {
        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }
    }

    public override void Draw(GameTime gameTime)
    {
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

    
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
 
    /*
        varibles
    */
    private const string ZOORDLE_TEXT = "Zoordle!";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";

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

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

}
