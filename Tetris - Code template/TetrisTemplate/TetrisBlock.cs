using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TetrisTemplate
{

    public class TetrisBlock
    {
        // Block data info
        public bool[,] BlockInfo = new bool[4, 4];
        private int currentRotation;
        public Vector2 BlockPosition;

        // Moving info
        private float startTime;
        private float durationGridMove = 1; // Gravity
        private bool isMoving = true;

        public TetrisBlock()
        {

            for (int i = 0; i < BlockInfo.GetLength(0); i++)
            {
                BlockInfo[0, i] = true;
            }


        }

        public void Rotate(bool clockWise)
        {
            if (!isMoving) return;

            currentRotation += 90;
            if (currentRotation >= 360) currentRotation = 0;

            bool[,] tempBlock = new bool[4, 4];

            // Rotating block
            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    tempBlock[x, y] = BlockInfo[y, x];
                }

            }


            BlockInfo = tempBlock;
            tempBlock = new bool[4, 4];

            // Flipping block if necesarry
            if (currentRotation >= 180 || currentRotation == 0)
            {
                for (int x = 0; x < BlockInfo.GetLength(0); x++)
                {
                    for (int y = 0; y < BlockInfo.GetLength(1); y++)
                    {
                        tempBlock[x, y] = BlockInfo[BlockInfo.GetLength(0) - 1 - x, y];
                    }

                }

                BlockInfo = tempBlock;

            }



        }

        public void UpdateBlock()
        {
            Move();
            DrawBlock();
        }

        private void Move()
        {
            float currenTime = (float)TetrisGame.GameTime.TotalGameTime.TotalSeconds;

            if(currenTime > startTime + durationGridMove && isMoving)
            {
                // Before moving let's set the occupied blocks cells to zero!
                for (int x = 0; x < BlockInfo.GetLength(0); x++)
                {
                    for (int y = 0; y < BlockInfo.GetLength(1); y++)
                    {                
                       TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Empty;
                    }
                }

                BlockPosition += new Vector2(0, 1);
                startTime = (float)TetrisGame.GameTime.TotalGameTime.TotalSeconds;
            }

            if (BlockPosition.Y >= TetrisGrid.Height - BlockInfo.GetLength(1)) isMoving = false;
        }

        private void DrawBlock()
        {
            // Setting block data.
            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    if (BlockInfo[x, y] == true) TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Occupied;
                    else TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Empty;
                }
            }
        }
    }
}
