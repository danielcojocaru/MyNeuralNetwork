using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Auxiliar.Interface;
using NeuralNetwork.Auxiliar.Other;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NeuralNetwork.Model
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
            B = Matrix<double>.Build.Dense(1, nextNrOfNeuros);

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
                Random random = new Random(); 
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        matrix[row, col] = (double)random.NextDouble() * 2D - 1D;
                    }
                }
            }
        }

        public void Forward(Matrix<double> o)
        {
            PrintForwardStep(Parent.PrintStep);
            NextNe.Forward(o, W, B);
        }

        private void PrintForwardStep(bool printStep)
        {
            if (printStep)
            {
                Console.WriteLine("W : ");
                Console.WriteLine(W);
                Console.WriteLine("B : ");
                Console.WriteLine(B);
            }
        }

        public void Backpropagation(Matrix<double> eOnI)
        {
            Matrix<double> oOnW = PrevNe.O.MultiplyRowMatrix(eOnI.ColumnCount);
            Matrix<double> eOnW = eOnI.Multiply(oOnW);

            dB = eOnI.SumSquareMatrixAsOneRowMatrix() * Parent.Lr;
            dW = eOnW.Transpose() * Parent.Lr;

            //Console.WriteLine(dB);
            //Console.WriteLine(dW);

            PrevNe.Backpropagation(eOnI);
        }

        public void ApplyDeltas()
        {
            W = W - dW;
            B = B - dB;

            Console.WriteLine(W);
            Console.WriteLine(B);

            if (NextNe.NextSy != null)
            {
                NextNe.NextSy.ApplyDeltas();
            }
            else
            {
                Parent.OnBackPropagationEnded();
            }
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
