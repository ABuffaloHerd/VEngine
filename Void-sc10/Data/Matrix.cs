using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEngine.Data
{
    public static class Matrix
    {
        /// <summary>
        /// Takes in a clockwise or counter clockwise rotation and returns 
        /// a rotation matrix for that direction
        /// </summary>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static int[,] RotationToMatrix(Rotation rot)
        {
            int[,] matrix = new int[2, 2]
            {
                { 0, 0 },
                { 0, 0 }
            };
            switch (rot)
            {
                case Rotation.CW:
                    matrix[1, 0] = 1;
                    matrix[0, 1] = -1;
                    break;

                case Rotation.CCW:
                    matrix[1, 0] = -1;
                    matrix[0, 1] = 1;
                    break;
            }

            return matrix;
        }
    }
}
