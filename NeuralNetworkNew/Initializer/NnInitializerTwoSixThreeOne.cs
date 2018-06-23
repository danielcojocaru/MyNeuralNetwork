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
    [Serializable]
    public class NnInitializerTwoSixThreeOne : INeuralNetworkInitializer
    {
        public List<Matrix<double>> WList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     },
                {   0.3     ,   0.3     },
                {   0.2     ,   0.5     },
                {   0.7     ,   0.8     },
                {   0.2     ,   0.3     },
                {   0.5     ,   0.7     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     ,   0.2     ,   0.5     ,   0.4     ,   0.4     },
                {   0.3     ,   0.5     ,   0.9     ,   0.4     ,   0.3     ,   0.2     },
                {   0.8     ,   0.4     ,   0.2     ,   0.1     ,   0.9     ,   0.6     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     ,   0.2     ,   0.2     }}),
        };

        public List<Matrix<double>> BList { get; set; } = new List<Matrix<double>>()
        {
             DenseMatrix.OfArray(new double[,] {
                {   0.1     },
                {   0.2     },
                {   0.3     },
                {   0.4     },
                {   0.5     },
                {   0.6     }}),
            DenseMatrix.OfArray(new double[,] {
                {   0.1     },
                {   0.2     },
                {   0.3     }}),
             DenseMatrix.OfArray(new double[,] {
                {   0.1     }}),
        };

        public NnInitializerTwoSixThreeOne()
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
