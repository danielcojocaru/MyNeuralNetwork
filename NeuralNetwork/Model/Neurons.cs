using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar;
using NeuralNetwork.Auxiliar.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Auxiliar.Interface;
using MathNet.Numerics;

namespace NeuralNetwork.Model
{
    public class Neurons
    {
        public NeuralNetworkCls Parent { get; set; }

        private ResultEnum _resultType;
        public ResultEnum ResultType
        {
            get
            {
                return _resultType;
            }
            set
            {
                _resultType = value;
                UpdateOutputFunc(value);
            }
        }

        public Func<Matrix<double>, Matrix<double>> OutputFunc { get; set; }

        public Matrix<double> I { get; set; }
        public Matrix<double> O { get; set; }

        public Synapses PrevSy { get; set; }
        public Synapses NextSy { get; set; }

        private void UpdateOutputFunc(ResultEnum value)
        {
            switch (value)
            {
                case ResultEnum.Simple:
                    OutputFunc = SimpleFunc;
                    break;
                case ResultEnum.Relu:
                    OutputFunc = ReluFunc;
                    break;
                case ResultEnum.Sigmoid:
                    OutputFunc = SigmoidFunc;
                    break;
                case ResultEnum.Softmax:
                    OutputFunc = SoftmaxFunc;
                    break;
                default:
                    break;
            }
        }

        private Matrix<double> SimpleFunc(Matrix<double> input)
        {
            Matrix<double> output = input * 1;
            return output;
        }

       
        private Matrix<double> SigmoidFunc(Matrix<double> input)
        {
            Matrix<double> output = ApplyFunction(input, Sigmoid);
            return output;
        }

        private Matrix<double> ReluFunc(Matrix<double> input)
        {
            Matrix<double> output = ApplyFunction(input, Relu);
            return output;
        }

        

        private Matrix<double> SoftmaxFunc(Matrix<double> input)
        {
            double sum = GetSoftmaxSum(input);
            Matrix<double> output = ApplySoftmax(input, sum);
            return output;
        }

        private Matrix<double> ApplySoftmax(Matrix<double> matrix, double sum)
        {
            if (matrix != null)
            {
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        matrix[row, col] = Math.Exp(matrix[row, col]) / sum;
                    }
                }
            }
            return matrix;
        }

        private double Sigmoid(double input)
        {
            double output = 1D / (1D + Math.Pow(Math.E, -input));
            return output;
        }

        private double Relu(double input)
        {
            double output = input > 0 ? input : 0;
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

        private double GetSoftmaxSum(Matrix<double> matrix)
        {
            double sum = 0;
            if (matrix != null)
            {
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        sum += Math.Exp(matrix[row, col]);
                    }
                }
            }
            return sum;
        }

        private double ReluFunc(double input)
        {
            return input;
        }
        public void Create(NeuralNetworkCls parent, int[] nrOfNeuronsList, int index)
        {
            Create(parent, nrOfNeuronsList, index, null);
        }

        public void Create(NeuralNetworkCls parent, int[] nrOfNeuronsList, int index, Synapses prevSy)
        {
            if (parent.CompleteObjList)
            {
                parent.NeuronsAndSynappses.Add(this);
            }

            Parent = parent; 
            PrevSy = prevSy; // could be null

            int nrOfNeurons = nrOfNeuronsList[index];
            I = Matrix<double>.Build.Dense(1, nrOfNeurons);
            O = Matrix<double>.Build.Dense(1, nrOfNeurons);

            ResultType = (ResultEnum)index;

            if (index < nrOfNeuronsList.Length - 1)
            {
                NextSy = new Synapses();
                NextSy.Create(Parent, nrOfNeuronsList, index, this);
            }
            else
            {
                parent.LastNeurons = this;
            }
        }

        public void Forward(Matrix<double> o, Matrix<double> w, Matrix<double> b)
        {
            I = o.Multiply(w).Add(b);
            O = OutputFunc(I);

            PrintForwardStep(Parent.PrintStep);

            if (NextSy != null)
            {
                NextSy.Forward(O);
            }
        }

        public void Forward(double[] input)
        {
            SetI(input);
            O = OutputFunc(I);

            PrintForwardStep(Parent.PrintStep);

            NextSy.Forward(O);
        }

        private void PrintForwardStep(bool printStep)
        {
            if (printStep)
            {
                Console.WriteLine("I : ");
                Console.WriteLine(I);
                Console.WriteLine("O : ");
                Console.WriteLine(O);
            }
        }

        private void SetI(double[] input)
        {
            //if (this.ResultType == ResultEnum.Sigmoid)
            //{

            //}

            for (int i = 0; i < this.I.ColumnCount; i++)
            {
                I[0, i] = input[i];
            }
        }

        public void Initialize(INeuralNetworkInitializer nnInitializer, int index)
        {
            if (NextSy != null)
            {
                NextSy.Initialize(nnInitializer, index);
            }
        }

        public override string ToString()
        {
            return this.ResultType + " : "
                //+ this.I.ToString("G2")
                + this.I.ToMatrixString(2, 4, 3, 4, "=", "||", @"\\", " ", "|", x => x.ToString("G3"))
                + " => "
                + this.O.ToMatrixString(2, 4, 3, 4, "=", "||", @"\\", " ", "|", x => x.ToString("G3"))
                ;
        }

        public void Backpropagation()
        {
            throw new NotImplementedException();
        }

    }
}
