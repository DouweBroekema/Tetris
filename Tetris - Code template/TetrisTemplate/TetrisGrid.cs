using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;

namespace TetrisTemplate
{

    class TetrisGrid
    {
        Texture2D emptyCell;

        Vector2 position;

        // Defining grid size
        public const int Width = 10;
        public const int Height = 20;

        // 2D array to hold info about each cell.
        public static GridCellInfo[,] Grid;


        public TetrisGrid()
        {
            emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
            position = Vector2.Zero;
            Clear();

            Grid = new GridCellInfo[Width, Height];

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Drawing the grid of empty cells.
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    spriteBatch.Draw(emptyCell, new Vector2(x * emptyCell.Width, y * emptyCell.Height), Color.White);

                }
            }

            // Let's fill in the cells that are occupied.
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    DrawOccupiedCell(spriteBatch, emptyCell, x, y);
                }
            }

        }

        private void DrawOccupiedCell(SpriteBatch spriteBatch, Texture2D cellTexture, int Width, int Height)
        {
            if (Grid[Width, Height] == GridCellInfo.Empty) spriteBatch.Draw(cellTexture, new Vector2(Width * emptyCell.Width, Height * emptyCell.Height), Color.White);
            else spriteBatch.Draw(cellTexture, new Vector2(Width * emptyCell.Width, Height * emptyCell.Height), Color.Black);

        }

        public void Clear()
        {

        }

        public enum GridCellInfo
        {
            Empty,
            Occupied
        }


    }
}

