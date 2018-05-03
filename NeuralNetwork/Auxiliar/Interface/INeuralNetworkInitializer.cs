using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Auxiliar.Interface
{
    public interface INeuralNetworkInitializer
    {
        Matrix<double> GetW(int index);
        Matrix<double> GetB(int index);
    }
}
