﻿using Auxiliar.Common.Enum;
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
                Random random = new Random();
                for (int row = 0; row < matrix.RowCount; row++)
                {
                    for (int col = 0; col < matrix.ColumnCount; col++)
                    {
                        double ran = (double)random.NextDouble() * 2D - 1D;
                        //double ran = (double)(random.NextDouble() + 0.5D) * 10D;

                        matrix[row, col] = ran;
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