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
    public class Synapses
    {
        public NeuralNetworkCls Parent { get; set; }
        public Neurons PrevNe { get; set; }
        public Neurons NextNe { get; set; }

        public Matrix<double> W { get; set; }
        public Matrix<double> B { get; set; }

        public Matrix<double> dW { get; set; }
        public Matrix<double> dB { get; set; }

        public void Create(NeuralNetworkCls parent, int[] nrOfNeuronsList, int index, Neurons prevNe)
        {
            if (parent.CompleteObjList)
            {
                parent.NeuronsAndSynappses.Add(this);
            }

            Parent = parent;
            PrevNe = prevNe;

            int prevNrOfInputs = nrOfNeuronsList[index];
            int nextNrOfNeuros = nrOfNeuronsList[++index];
            W = Matrix<double>.Build.Dense(nextNrOfNeuros, prevNrOfInputs);
            B = Matrix<double>.Build.Dense(nextNrOfNeuros, 1);

            NextNe = new Neurons();
            NextNe.Create(parent, nrOfNeuronsList, index, this);
        }

        public void Initialize(INeuralNetworkInitializer nnInitializer, int index)
        {
            if (nnInitializer != null)
            {
                W = nnInitializer.GetW(index);
                B = nnInitializer.GetB(index);
            }
            else
            {
                Randomize(this.W);
                Randomize(this.B);

                //Console.WriteLine(B);
            }

            NextNe.Initialize(nnInitializer, ++index);
        }

        private void Randomize(Matrix<double> matrix)
        {
            if (matrix != null)
            {
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        int mean = 0;
                        int stdDev = 1;

                        double u1 = 1.0 - _random.NextDouble(); //uniform(0,1] random doubles
                        double u2 = 1.0 - _random.NextDouble();
                        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                        double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

                        randNormal *= Math.Sqrt(2D / matrix.ColumnCount);

                        //double randNormal = (double)_random.NextDouble() * 2D - 1D;

                        matrix[row, col] = randNormal;
                    }
                }
            }
        }

        private double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }

        private static Random _random = new Random();

        public void Forward(Matrix<double> o)
        {
            PrintForwardStep(Parent.PrintStep);
            NextNe.Forward(o, W, B);
        }

        private void PrintForwardStep(bool printStep)
        {
            if (printStep)
            {
                if (Double.IsNaN(W[0, 0]) || Double.IsNaN(B[0, 0]))
                {

                }

                //Console.WriteLine("W : ");
                //Console.WriteLine(W);
                //Console.WriteLine("B : ");
                //Console.WriteLine(B);
            }
        }

        public void Backpropagation(Matrix<double> eOnI)
        {
            Matrix<double> oOnW = PrevNe.O.Transpose();
            //Matrix<double> eOnW = eOnI.Multiply(oOnW);

            dB = eOnI * Parent.Lr;
            dW = dB.Multiply(oOnW);

            //Console.WriteLine("Synapsen dB:");
            //Console.WriteLine(dB);
            //Console.WriteLine(dB);
            //Console.WriteLine(dW);

            PrevNe.Backpropagation(NextNe.E);
            //PrevNe.Backpropagation(eOnI);
        }

        public void ApplyDeltas()
        {
            W = W - dW;
            B = B - dB;

            //Console.WriteLine(W);
            //Console.WriteLine(B);

            if (NextNe.NextSy != null)
            {
                NextNe.NextSy.ApplyDeltas();
            }
            else
            {
                Parent.OnBackPropagationEnded();
            }
        }
    }
}
