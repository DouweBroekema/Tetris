﻿using Microsoft.Xna.Framework; 
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        // References
        public InputHelper inputHelper;

        public TetrisBlock()
        {

            for (int i = 0; i < BlockInfo.GetLength(0); i++)
            {
                BlockInfo[0, i] = true;
            }

            inputHelper = GameWorld.InputHelper;
        }

        private void InputDetection()
        {
            if (inputHelper.KeyPressed(Keys.D))
            {
               Rotate(true);
            }
            if (inputHelper.KeyPressed(Keys.A))
            {
                Rotate(false);
            }
            if (inputHelper.KeyPressed(Keys.Left))
            {
                MoveBlock(new Vector2(-1, 0));
            }
            if (inputHelper.KeyPressed(Keys.Right))
            {
                MoveBlock(new Vector2(1, 0));
            }
            if (inputHelper.KeyPressed(Keys.Down))
            {
                MoveBlock(new Vector2(0, 1));
            }
        }

        public void UpdateBlock()
        {
            InputDetection();
            Move();
            DrawBlock();
        }
        

        // We call this function before changing the content of our actual block.
        // We pass in the data of the block after the operation would have happened.
        private bool PositionValid(bool[,] tempBlock, Vector2 newPosition)
        {
   
            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    // Check if this block is an occupied block inside of our own block.
                    // If not we don't care about collision because it's an empty cell.
                    if (!tempBlock[x, y]) continue;

                    // Check if we're trying to exceed the tetris grid width.
                    if ((x + newPosition.X) >= TetrisGrid.Width || x + newPosition.X < 0)
                    {
                        // We're trying to exceed the width of our tetris grid.
                        return false;
                    }

                    // Check if we're trying to exceed the tetris grid height
                    if ((y + newPosition.Y) >= TetrisGrid.Height || y + newPosition.Y < 0)
                    {
                        // We're trying to exceed the height of our tetris grid.
                        return false;
                    }

                    // Check if we collide with a block
                    if (TetrisGrid.Grid[(int)(x + newPosition.X), (int)(y + newPosition.Y)] == TetrisGrid.GridCellInfo.Occupied)
                    {
                        if (BlockInfo[x, y]) continue;
                        if (tempBlock[x, y]) continue;
                        // We're trying to move our block to an already occupied position!
                        return false;
                    }
                    
                }
            }

            // It seems we've passed the checks. Now we know we are trying to do a valid operation.
            return true;
       
        }

        // Move block down vertically
        private void Move()
        {
            float currenTime = (float)TetrisGame.GameTime.TotalGameTime.TotalSeconds;

            if(currenTime > startTime + durationGridMove && isMoving)
            {
                // Before moving let's set the occupied blocks cells to zero!
                ResetPreviousBlock();

                if(PositionValid(BlockInfo, BlockPosition + new Vector2(0, 1)))
                    BlockPosition += new Vector2(0, 1);

                startTime = (float)TetrisGame.GameTime.TotalGameTime.TotalSeconds;

            }

            // Stop moving the block down.
            if (!PositionValid(BlockInfo, BlockPosition + new Vector2(0, 1))) isMoving = false;

        }



        // Move block horizontally
        public void MoveBlock(Vector2 direction)
        {
            if (!isMoving) return;

            //before we move the tetrisblock, we first set the currently occupied block cells to false/empty if they are within the tetris grid.
            ResetPreviousBlock();

            if (PositionValid(BlockInfo, BlockPosition + direction)) BlockPosition += direction;

        }
        public void Rotate(bool clockWise)
        {
            if (!isMoving) return;

            if (clockWise)
            {

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

                // Checking if position is valid before we apply our rotation.
                if (PositionValid(tempBlock, BlockPosition)) BlockInfo = tempBlock;
                //BlockInfo = tempBlock;

                tempBlock = new bool[4, 4];

                Debug.WriteLine(currentRotation);

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

                    if (PositionValid(tempBlock, BlockPosition)) BlockInfo = tempBlock;
                    //BlockInfo = tempBlock;
                }

            }
            else
            {


                currentRotation += 90;
                if (currentRotation >= 360) currentRotation = 0;

                bool[,] tempBlock = new bool[4, 4];

                //Rotate block counterclockwise
                for (int x = 0; x < BlockInfo.GetLength(0); x++)
                {
                    for (int y = 0; y < BlockInfo.GetLength(1); y++)
                    {
                        tempBlock[y, x] = BlockInfo[x, y];
                    }
                }

                if (PositionValid(tempBlock, BlockPosition)) BlockInfo = tempBlock;
                tempBlock = new bool[4, 4];

                // Flipping block if necesarry
                if (currentRotation >= 90 || currentRotation <= 270)
                {
                    for (int x = 0; x < BlockInfo.GetLength(0); x++)
                    {
                        for (int y = 0; y < BlockInfo.GetLength(1); y++)
                        {
                            tempBlock[x, y] = BlockInfo[x, BlockInfo.GetLength(1) - 1 - y];
                        }

                    }

                    // Checking if position is valid before we apply our rotation.
                    if (PositionValid(tempBlock, BlockPosition)) BlockInfo = tempBlock;

                    Debug.WriteLine(currentRotation);
                }
            }



        }


        private void DrawBlock()
        {
            // Setting block data.
            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    if (BlockInfo[x, y] == true) TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Occupied;
                    else
                    {
                        // Check if empty spot is on the grid. If not we don't care about setting empty.
                        if (x + BlockPosition.X >= TetrisGrid.Width || y + BlockPosition.Y >= TetrisGrid.Height) continue;
                        if (x + BlockPosition.X < 0 || y + BlockPosition.Y < 0) continue;

                        TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Empty;
                    }
                }
            }
        }
        private void ResetPreviousBlock()
        {
            // Setting block data.
            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    // Check if empty spot is on the grid. If not we don't care about setting empty.
                    if (x + BlockPosition.X >= TetrisGrid.Width || y + BlockPosition.Y >= TetrisGrid.Height) continue;
                    if (x + BlockPosition.X < 0 || y + BlockPosition.Y < 0) continue;

                    TetrisGrid.Grid[x + (int)BlockPosition.X, y + (int)BlockPosition.Y] = TetrisGrid.GridCellInfo.Empty;
                }
            }
        }
    }
}
