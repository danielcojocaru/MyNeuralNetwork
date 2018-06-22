using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Auxiliar.Interface
{
    public interface INeuralNetwork
    {
        void Forward(double[] input, double[] targets, bool doBackpropagation = true);
        Matrix<double> Guess(double[] input);
        void Create(int[] layers);
        void Initialize();
    }
}
