using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Auxiliar.Interface;
using NeuralNetwork.Auxiliar.Other;

namespace NeuralNetwork.Model
{
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
            W = nnInitializer.GetW(index);
            B = nnInitializer.GetB(index);

            NextNe.Initialize(nnInitializer, ++index);
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
            Matrix<double> oOnW = PrevNe.O.MultiplyRowMatrixToSquareMatrix();
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

            //Console.WriteLine(W);
            //Console.WriteLine(B);

            if (NextNe.NextSy != null)
            {
                NextNe.NextSy.ApplyDeltas();
            }
        }
    }
}
