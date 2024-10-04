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
        public bool[,] BlockInfo = new bool[4, 4];
        /// <summary>
        /// 0100
        /// 0100
        /// 0100
        /// 0100
        /// 
        /// 0000
        /// 1111
        /// 0000
        /// 0000
        /// 
        /// 
        /// 
        /// </summary>

        private int currentRotation;


        public TetrisBlock()
        {

            for (int i = 0; i < BlockInfo.GetLength(0); i++)
            {
                BlockInfo[0, i] = true;
            }


        }    

        public void Rotate(bool clockWise)
        {

            currentRotation += 90;
            if (currentRotation >= 360) currentRotation = 0;

            bool[,] tempBlock = BlockInfo;

            /*
            // Rotating block info.         
            for (int x = 0; x < BlockInfo.GetLength(0); x++) 
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {                  
                    tempBlock[x, y] = BlockInfo[y, x];
                    //else tempGrid[x, y] = BlockInfo[BlockInfo.GetLength(0) - y, BlockInfo.GetLength(1) - x];
                    row += BlockInfo[x, y];
                }

                Debug.WriteLine(row);
            }

            */

            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for (int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    tempBlock[x, y] = BlockInfo[y, x];
                }

            }


            BlockInfo = tempBlock;


            for (int x = 0; x < BlockInfo.GetLength(0); x++)
            {
                for(int y = 0; y < BlockInfo.GetLength(1); y++)
                {
                    if (BlockInfo[x, y] == true) TetrisGrid.Grid[x + 5, y + 5] = TetrisGrid.GridCellInfo.Occupied;

                }
            }

            Debug.WriteLine("Rotating...");

        }
    }
}
