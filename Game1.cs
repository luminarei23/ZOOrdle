using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using Zoordle.Scenes;

namespace Zoordle;

public class Game1 : Core
{
    public Game1() : base("Zoordle", 1920, 1080, false)
    {
        
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Start the game with the title scene.
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
        //TODO: load theme music later
    }    
}
