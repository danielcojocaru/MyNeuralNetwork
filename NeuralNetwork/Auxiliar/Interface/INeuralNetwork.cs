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
        void Forward(double[] input, double[] answer);
        Matrix<double> Guess(double[] input);
    }
}
