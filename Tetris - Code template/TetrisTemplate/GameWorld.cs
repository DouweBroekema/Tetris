using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TetrisTemplate;

class GameWorld
{

    public static Random Random { get { return random; } }
    static Random random;
    SpriteFont font;


    // Classes
    TetrisGrid grid;
    public static InputHelper InputHelper = new InputHelper();
    TetrisBlock tetrisBlock;


    // GameState
    public static GameState gameState;
    public enum GameState
    {
        Playing,
        GameOver
    }



    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Playing;
        //comment
        //extra comment

        // Test 123
         Random newrandom = new Random();
        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");

        grid = new TetrisGrid();

        tetrisBlock = new TetrisBlock();

    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        InputHelper.Update(gameTime);
    }

    public void Update(GameTime gameTime)
    {
        if (InputHelper.KeyPressed(Keys.Space))
        {
            tetrisBlock.Rotate(true);
        }

        //tetrisBlock.Rotate(true);

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        grid.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }

    public void Reset()
    {
    }

}
