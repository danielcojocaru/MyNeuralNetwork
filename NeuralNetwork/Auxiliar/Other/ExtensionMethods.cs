using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Auxiliar.Other
{
    public static class ExtensionMethods
    {
        public static Matrix<double> GetFirstRowAsDiagonalMatrix(this Matrix<double> @this)
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(@this.ColumnCount, @this.ColumnCount);

            for (int i = 0; i < @this.ColumnCount; i++)
            {
                toReturn[i, i] = @this[0, i];
            }

            return toReturn;
        }

        public static Matrix<double> GetDiagonalMatrixAsOneRowMatrix(this Matrix<double> @this)
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(1, @this.ColumnCount);

            for (int i = 0; i < @this.ColumnCount; i++)
            {
                toReturn[0, i] = @this[i, i];
            }

            return toReturn;
        }

        public static Matrix<double> SumSquareMatrixAsOneRowMatrix(this Matrix<double> @this)
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(1, @this.ColumnCount);

            for (int i = 0; i < @this.ColumnCount; i++)
            {
                for (int j = 0; j < @this.RowCount; j++)
                {
                    toReturn[0, i] += @this[j, i];
                }
            }

            return toReturn;
        }

        public static Matrix<double> MultiplyRowMatrixToSquareMatrix(this Matrix<double> @this)
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(@this.ColumnCount, @this.ColumnCount);

            for (int i = 0; i < @this.ColumnCount; i++)
            {
                for (int j = 0; j < @this.ColumnCount; j++)
                {
                    toReturn[j, i] = @this[0, i];
                }
            }

            return toReturn;
        }
    }
}
