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
        private bool canMove = true;

        public TetrisBlock()
        {

            for (int i = 0; i < BlockInfo.GetLength(0); i++)
            {
                BlockInfo[0, i] = true;
            }


        }

        public void Rotate(bool clockWise)
        {
            if (clockWise)
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
            else
            {
                if (!isMoving) return;

                currentRotation += 90;
                if (currentRotation >= 360) currentRotation = 0;

                bool[,] tempBlock = new bool[4, 4];

                //Rotate block counterclockwise
                for(int x = 0;x < BlockInfo.GetLength(0); x++)
                {
                    for (int y = 0;y < BlockInfo.GetLength(1); y++)
                    {
                        tempBlock[y, x] = BlockInfo[x, y];
                    }
                }

                BlockInfo = tempBlock;
                tempBlock = new bool[4, 4];

                // Flipping block if necesarry
                if (currentRotation >= 90 || currentRotation <=270)
                {
                    for (int x = 0; x < BlockInfo.GetLength(0); x++)
                    {
                        for (int y = 0; y < BlockInfo.GetLength(1); y++)
                        {
                            tempBlock[x, y] = BlockInfo[x, BlockInfo.GetLength(1) -1 - y];
                        }

                    }
                    BlockInfo = tempBlock;

                    Debug.WriteLine(currentRotation);
                }
            }



        }

        public void UpdateBlock()
        {
            Move();
            DrawBlock();
        }

        public void MoveBlock(Vector2 direction)
        {
            //we first check if this doesn't move our block out of the array
            /*
            if (BlockPosition.X - direction.X >= TetrisGrid.Width - BlockInfo.GetLength(0) || (BlockPosition.X + direction.X <= TetrisGrid.Width + BlockInfo.GetLength(0)) || (BlockPosition.Y + direction.Y >= TetrisGrid.Height - BlockInfo.GetLength(1)))
            {
                //we cant move 
                canMove = false;
            }
            */
            if (BlockPosition.Y + direction.Y >= TetrisGrid.Height - BlockInfo.GetLength(1))
            {
                canMove = false;
            }
            else
            {
                canMove = true;
            }


            if (canMove)
            {
                //before we move the tetrisblock, we first set the currently occupied block cells to false/empty
                for (int x = 0; x < BlockInfo.GetLength(0); x++)
                {
                    for (int y = 0; y < BlockInfo.GetLength(1); y++)
                    {
                        TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Empty;
                    }
                }

                BlockPosition += direction;
            }
            else
            {
                //we don't move
            }
            
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
