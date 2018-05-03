using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace NeuralNetwork.Model
{
    public class NnInitializerMediumDotComExample : INeuralNetworkInitializer
    {
        public List<Matrix<double>> WList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     ,   0.3     },
                {   0.3     ,   0.2     ,   0.7     },
                {   0.4     ,   0.3     ,   0.9     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.2     ,   0.3     ,   0.5     },
                {   0.3     ,   0.5     ,   0.7     },
                {   0.6     ,   0.4     ,   0.8     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.4     ,   0.8     },
                {   0.3     ,   0.7     ,   0.2     },
                {   0.5     ,   0.2     ,   0.9     }}),
        };

        public List<Matrix<double>> BList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   1   },
                {   1   },
                {   1   }}),
             DenseMatrix.OfArray(new double[,] {
                {   1   },
                {   1   },
                {   1   }}),
             DenseMatrix.OfArray(new double[,] {
                {   1   },
                {   1   },
                {   1   }}),
        };

        public NnInitializerMediumDotComExample()
        {
        }

        public Matrix<double> GetW(int index)
        {
            Matrix<double> w = WList[index];
            return w;
        }

        public Matrix<double> GetB(int index)
        {
            Matrix<double> b = BList[index];
            return b;
        }
    }
}
