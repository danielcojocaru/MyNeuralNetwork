using Auxiliar.Common.Enum;
using Auxiliar.Common.Interface;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Initializer
{
    public class NnInitializerXor : INeuralNetworkInitializer
    {
        public List<Matrix<double>> WList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     },
                {   0.3     ,   0.3     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     }}),
        };

        public List<Matrix<double>> BList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   0.1     },
                {   0.2     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     }}),
        };

        public NnInitializerXor()
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
