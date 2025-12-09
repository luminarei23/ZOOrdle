using System;
using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Buttons;

namespace Zoordle;

public class Game1 : Core
{
    private Sprite _background;

    private SpriteFont _font;

    private int _letters_position;

    private Vector2 _letterTextPosition;

    private Vector2 _letterTextOrigin;

    // private Tilemap _tilemap;
    
    public Game1() : base("Zoordle", 1920, 1080, false)
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

       // _letterTextPosition = new Vector2()

    }

    protected override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        _background = atlas.CreateSprite("background");

        //     // Create the tilemap from the XML configuration file.
        // _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        // _tilemap.Scale = new Vector2(4.0f, 4.0f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        CheckKeyboardInput();

        base.Update(gameTime);
    }
    // private void CheckLetterState()
    // {
    //     if (Bounds.Contains(mouse.Position) &&
    //         mouse.LeftButton == ButtonState.Pressed)
    //     {
    //         IsClicked = true;
    //     }
    //     else
    //     {
    //         IsClicked = false;
    //     }
    // }
    private void CheckKeyboardInput()
    {

        // If the space key is held down, the movement speed increases by 1.5

        // float speed = 5.0f;
        // if (Input.Keyboard.IsKeyDown(Keys.Space))
        // {
        //     speed *= 1.5f;
        // }

        // If the W or Up keys are down, move the slime up on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        // {
        //     _pawPosition.Y -= speed;
        // }

        // // if the S or Down keys are down, move the slime down on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        // {
        //     _pawPosition.Y += speed;
        // }

        // // If the A or Left keys are down, move the slime left on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        // {
        //     _pawPosition.X -= speed;
        // }

        // // If the D or Right keys are down, move the slime right on the screen.
        // if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        // {
        //     _pawPosition.X += speed;
        // }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSkyBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the tilemap.
        // _tilemap.Draw(SpriteBatch);
                // Loading a SpriteFont Description using the content pipeline
        SpriteFont font = Content.Load<SpriteFont>("fonts/fonts");
        string message = "ZOORDLE";

        //Vector2 textSize = SpriteFont.MeasureString(message);

        // Position will be the center of the screen.
        Vector2 position = new Vector2(
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        ) * 0.5f;

        SpriteBatch.DrawString(
            font,                   // font
            message,     // text
            Vector2.Zero,           // position
            Color.White,            // color
            0.0f,                   // rotation
            Vector2.Zero,           // origin
            Vector2.One,            // scale
            SpriteEffects.None,     // effects
            0.0f                    // layerDepth            // color
    );


        _background.Draw(SpriteBatch, Vector2.Zero);

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }
    
}
