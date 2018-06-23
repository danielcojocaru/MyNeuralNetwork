using Auxiliar.Common.Enum;
using Auxiliar.Common.Interface;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Body
{
    [Serializable]
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
        public Func<Matrix<double>> OonIFunc { get; set; }

        public Matrix<double> I { get; set; }
        public Matrix<double> O { get; set; }
        public Matrix<double> E { get; set; }

        public Matrix<double> OonI { get; set; }

        public Synapses PrevSy { get; set; }
        public Synapses NextSy { get; set; }

        private void UpdateOutputFunc(ResultEnum value)
        {
            switch (value)
            {
                case ResultEnum.Simple:
                    OutputFunc = SimpleOutputFunc;
                    OonIFunc = SimpleOonIFunc;
                    break;
                case ResultEnum.Relu:
                    OutputFunc = ReluOutputFunc;
                    OonIFunc = ReluOonIFunc;
                    break;
                case ResultEnum.Sigmoid:
                    OutputFunc = SigmoidOutputFunc;
                    OonIFunc = SigmoidOonIFunc;
                    break;
                case ResultEnum.Softmax:
                    OutputFunc = SoftmaxOutputFunc;
                    OonIFunc = SoftmaxOonIFunc;
                    break;
                default:
                    break;
            }
        }

        private Matrix<double> ReluOonIFunc()
        {
            Matrix<double> output = ApplyFunction(O, ReluPrime);
            return output;
        }

        private Matrix<double> SigmoidOonIFunc()
        {
            Matrix<double> output = ApplyFunction(O, SigmoidPrime);
            return output;
        }

        private Matrix<double> SoftmaxOonIFunc()
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(O.RowCount, O.ColumnCount);
            double divisor = 0;
            for (int i = 0; i < O.RowCount; i++)
            {
                divisor += Math.Exp(O[i, 0]);
            }
            divisor = Math.Pow(divisor, 2);

            for (int i = 0; i < O.RowCount; i++)
            {
                double first = Math.Exp(O[i, 0]);
                double secord = 0;

                for (int j = 0; j < O.RowCount; j++)
                {
                    if (i == j)
                        continue;

                    secord += Math.Exp(O[j, 0]);
                }

                toReturn[i, 0] = first * secord / divisor;
            }
            return toReturn;
        }

        private Matrix<double> SimpleOonIFunc()
        {
            throw new NotImplementedException();
        }

        private Matrix<double> SimpleOutputFunc(Matrix<double> input)
        {
            Matrix<double> output = input * 1;
            return output;
        }

        private Matrix<double> SigmoidOutputFunc(Matrix<double> input)
        {
            Matrix<double> output = ApplyFunction(input, Sigmoid);
            return output;
        }

        private Matrix<double> ReluOutputFunc(Matrix<double> input)
        {
            Matrix<double> output = ApplyFunction(input, Relu);
            return output;
        }

        private Matrix<double> SoftmaxOutputFunc(Matrix<double> input)
        {
            double sum = GetSoftmaxSum(input);
            Matrix<double> output = ApplySoftmax(input, sum);
            return output;
        }

        private Matrix<double> ApplySoftmax(Matrix<double> input, double sum)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(input.RowCount, input.ColumnCount);
            if (input != null)
            {
                for (int row = 0; row < input.RowCount; row++)
                {
                    for (int col = 0; col < input.ColumnCount; col++)
                    {
                        output[row, col] = Math.Exp(input[row, col]) / sum;
                    }
                }
            }
            return output;
        }

        private double Sigmoid(double input)
        {
            double output = 1D / (1D + Math.Pow(Math.E, -input));
            return output;
        }

        private double SigmoidPrime(double input)
        {
            //double output = 1 / (1 + Math.Exp(input)) * (1 - 1 / (1 + Math.Exp(input)));
            double output = input * (1 - input);
            return output;
        }

        private double ReluPrime(double input)
        {
            return input <= 0 ? 0 : 1;
        }

        private double Relu(double input)
        {
            double output = input > 0 ? input : 0;
            return output;
        }

        private Matrix<double> ApplyFunction(Matrix<double> input, Func<double, double> func)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(input.RowCount, input.ColumnCount);
            if (input != null)
            {
                for (int row = 0; row < input.RowCount; row++)
                {
                    for (int col = 0; col < input.ColumnCount; col++)
                    {
                        output[row, col] = func(input[row, col]);
                    }
                }
            }
            return output;
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
            return Math.Max(0, input);
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
            I = Matrix<double>.Build.Dense(nrOfNeurons, 1);
            O = Matrix<double>.Build.Dense(nrOfNeurons, 1);

            if (parent.FunctionInitializer != null)
            {
                ResultType = parent.FunctionInitializer.GetResultEnum(index);
            }
            else
            {
                ResultType = (ResultEnum)index;
            }

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
            I = w.Multiply(o).Add(b);
            O = OutputFunc(I);

            PrintForwardStep(Parent.PrintStep);

            if (NextSy != null)
            {
                NextSy.Forward(O);
            }
            else
            {
                // forward finished
                //Console.WriteLine("NEW:");
                //Console.WriteLine(O);
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
                if (Double.IsNaN(I[0, 0]) || Double.IsNaN(O[0, 0]))
                {

                }

                //Console.WriteLine("I : ");
                //Console.WriteLine(I);
                //Console.WriteLine("O : ");
                //Console.WriteLine(O);
            }
        }

        private void SetI(double[] input)
        {
            for (int i = 0; i < this.I.RowCount; i++)
            {
                I[i, 0] = input[i];
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
                //+ " => "
                //+ this.O.ToMatrixString(2, 4, 3, 4, "=", "||", @"\\", " ", "|", x => x.ToString("G3"))
                ;
        }

        public void Backpropagation(Matrix<double> error)
        {
            //Console.WriteLine("NEW: Received E:");
            //Console.WriteLine(error);

            // It's the Output-layer with neurons
            if (NextSy == null)
            {
                //OLD:
                //Matrix<double> error = errorOrEOnI; // just for the understanding of the code
                //Matrix<double> diagonalError = error.GetFirstColumnAsDiagonalMatrix();
                //Matrix<double> oOnI = OonIFunc();
                //Matrix<double> eOnI = diagonalError.Multiply(oOnI);

                //PrevSy.Backpropagation(eOnI);


                Matrix<double> oOnI = OonIFunc();

                //Console.WriteLine("NEW: error (1):");
                //Console.WriteLine(error);
                //Console.WriteLine("NEW: oOnI (2):");
                //Console.WriteLine(oOnI);

                E = error;
                Matrix<double> eOnI = SimpleMultiply(error, oOnI);
                //Matrix<double> eOnI = oOnI;

                //Console.WriteLine("NEW: eOnI (3):");
                //Console.WriteLine(eOnI);

                PrevSy.Backpropagation(eOnI);

            }
            // It's a hidden Neural-layer but (not the first or the last)
            else if (PrevSy != null)
            {
                //OLD:
                //Matrix<double> eOnIPrev = errorOrEOnI; // just for the understanding of the code
                //Matrix<double> eOnO = eOnIPrev.Transpose().Multiply(NextSy.W);
                //Matrix<double> oOnI = OonIFunc();
                //Matrix<double> eOnI = eOnO.GetFirstRowAsDiagonalMatrix().Multiply(oOnI);

                //PrevSy.Backpropagation(eOnI);

                //Console.WriteLine("NEW: Prev E (1):");
                //Console.WriteLine(error);
                //Console.WriteLine("NEW: W (2):");
                //Console.WriteLine(NextSy.W);

                Matrix<double> eOnO = NextSy.W.Transpose().Multiply(error);
                E = eOnO;
                Matrix<double> oOnI = OonIFunc();

                //Console.WriteLine("NEW: E (3):");
                //Console.WriteLine(eOnO);

                //Console.WriteLine("NEW: error (1):");
                //Console.WriteLine(oOnI);
                //Console.WriteLine("NEW: oOnI (2):");
                //Console.WriteLine(oOnI);

                Matrix<double> eOnI = SimpleMultiply(eOnO, oOnI);

                PrevSy.Backpropagation(eOnI);
            }
            // It's the input Neural-layer
            else
            {
                NextSy.ApplyDeltas();
            }
        }

        private Matrix<double> SimpleMultiply(Matrix<double> first, Matrix<double> second)
        {
            Matrix<double> toReturn = Matrix<double>.Build.Dense(first.RowCount, first.ColumnCount);

            for (int i = 0; i < first.RowCount; i++)
            {
                for (int j = 0; j < first.ColumnCount; j++)
                {
                    toReturn[i, j] = first[i, j] * second[i, j];
                }
            }
            return toReturn;
        }

    }
}
