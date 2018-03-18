using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Layer
    {
        #region Basic

        public static double _lr = 0.2;

        public Matrix<double> X { get; set; }
        public Matrix<double> W { get; set; }
        public Matrix<double> B { get; set; }
        public Matrix<double> E { get; set; }

        public Layer Previous { get; set; }
        public Layer Next { get; set; }

        private Random rnd = new Random();

        public Layer()
        { }

        public void Create(int nrOfOutputs)
        {
            CreateXAndB(nrOfOutputs);
        }

        public void Create(int nrOfOutputs, int nrOfInputs)
        {
            CreateXAndB(nrOfOutputs);
            CreateWAndE(nrOfOutputs, nrOfInputs);
        }

        private void CreateXAndB(int nrOfOutputs)
        {
            this.X = Matrix<double>.Build.Dense(nrOfOutputs, 1);
            this.B = Matrix<double>.Build.Dense(nrOfOutputs, 1);
        }

        private void CreateWAndE(int nrOfOutputs, int nrOfInputs)
        {
            this.E = Matrix<double>.Build.Dense(nrOfOutputs, 1);
            this.W = Matrix<double>.Build.Dense(nrOfOutputs, nrOfInputs);
        }

        public void Randomize()
        {
            Randomize(this.W);
            Randomize(this.B);
        }

        private void Randomize(Matrix<double> matrix)
        {
            ApplyFunction(matrix, Random);
        }

        private Matrix<double> Sigmoid(Matrix<double> matrix)
        {
            Matrix<double> sigmoided = ApplyFunction(matrix, Sigmoid);
            return sigmoided;
        }

        private double Sigmoid(double input)
        {
            double output = 1D / (1D + Math.Pow(Math.E, -input));
            return output;
        }

        private double Random(double input)
        {
            double output = (double)rnd.NextDouble() * 2D - 1D;
            return output;
        }

        private Matrix<double> ApplyFunction(Matrix<double> matrix, Func<double, double> func)
        {
            if (matrix != null)
            {
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        matrix[row, col] = func(matrix[row, col]);
                    }
                }
            }
            return matrix;
        }

        private Matrix<double> CopyAndApplyFunction(Matrix<double> matrix, Func<double, double> func)
        {
            if (matrix != null)
            {
                Matrix<double> newMatrix = Matrix<double>.Build.SameAs(matrix);
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        newMatrix[row, col] = func(matrix[row, col]);
                    }
                }
                return newMatrix;
            }
            return matrix;
        }


        #endregion

        #region Forward

        public Matrix<double> FeedInput()
        {
            Matrix<double> I = Previous.X;
            this.X = Sigmoid(W.Multiply(I).Add(B));

            if (Next != null)
            {
                return Next.FeedInput();
            }
            else
            {
                return X;
            }
        }

        public Matrix<double> Guess(double[] input)
        {
            SetX(input);
            Matrix<double> output = Next.FeedInput();
            return output;
        }

        private void SetX(double[] input)
        {
            for (int i = 0; i < this.X.RowCount; i++)
            {
                X[i, 0] = input[i];
            }
        }

        #endregion

        public void Backwards(double[] targets)
        {
            CalculateErrors(targets);
            Previous.PropagateErrors();
            ModifyWAndB();
        }

        public void ModifyWAndB()
        {
            if (W != null)
            {
                ModifyWAndBLocal();
                Previous.ModifyWAndB();
            }
        }

        private void ModifyWAndBLocal()
        {
            Matrix<double> newE = E.Multiply(_lr);
            Matrix<double> gradients = CopyAndApplyFunction(this.X, DSigmoid);
            Matrix<double> deltaB = SimpleMultiply(newE, gradients);
            Matrix<double> deltaW = deltaB.Multiply(Previous.X.Transpose());

            this.B = this.B.Add(deltaB);
            this.W = this.W.Add(deltaW);
        }

        private Matrix<double> SimpleMultiply(Matrix<double> first, Matrix<double> second)
        {
            for (int i = 0; i < first.RowCount; i++)
            {
                for (int j = 0; j < first.ColumnCount; j++)
                {
                    first[i, j] *= second[i, j];
                }
            }
            return first;
        }

        private double DSigmoid(double input)
        {
            return input * (1 - input);
        }

        private void PropagateErrors()
        {
            if (Previous != null)
            {
                this.E = Next.W.Transpose().Multiply(Next.E);
                Previous.PropagateErrors();
            }
        }

        private void CalculateErrors(double[] targets)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                //int minusPlus = 1;
                //if (targets[i] - X[i, 0] < 0)
                //{
                //    minusPlus = -1;
                //}

                //E[i, 0] = Math.Pow(targets[i] - X[i, 0], 2) * minusPlus;

                E[i, 0] = targets[i] - X[i, 0];
            }
        }
    }
}

