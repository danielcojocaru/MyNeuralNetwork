using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar.Enum;

namespace NeuralNetwork.Model
{
    public class NeuralNetworkCls
    {
        public Neurons FirstNeurons { get; set; }
        public Neurons LastNeurons { get; set; }

        public INeuralNetworkInitializer NnInitializer { get; set; }

        public bool PrintStep = false;
        public bool CompleteObjList = true;
        public List<object> NeuronsAndSynappses { get; set; } = new List<object>();
        public int Epochs { get; set; }
        public int MaxEpochs { get; set; } = 1;
        public List<Matrix<double>> Errors { get; set; } = new List<Matrix<double>>();

        private ErrorEvaluatorEnum _errorEvaluatorEnum;
        public ErrorEvaluatorEnum ErrorEvaluatorEnum
        {
            get
            {
                return _errorEvaluatorEnum;
            }
            set
            {
                SetFuncErrorEval(value);
                _errorEvaluatorEnum = value;
            }
        }

        private void SetFuncErrorEval(ErrorEvaluatorEnum errorEvaluatorEnum)
        {
            switch (errorEvaluatorEnum)
            {
                case ErrorEvaluatorEnum.Logaritmic:
                    FuncErrorEval = LogaritmicError;                   
                    break;
                case ErrorEvaluatorEnum.Crossentropy:
                    FuncErrorEval = CrossentropyError;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private double LogaritmicError(double expected, double received)
        {
            double toReturn = (-1) * (expected * Math.Log10(received) + (1 - expected) * Math.Log10(1 - received));
            return toReturn;
        }

        private double CrossentropyError(double expected, double received)
        {
            double toReturn = (-1) * (expected * (1 / received) + (1 - expected) * (1 / (1 - received)));
            return toReturn;
        }

        public Func<double, double, double> FuncErrorEval { get; set; }

        public NeuralNetworkCls()
        {
        }

        public void Create(int[] layers)
        {
            CreateLayers(layers);
        }

        private void CreateLayers(int[] nrOfNeuronsList)
        {
            FirstNeurons = new Neurons();
            FirstNeurons.Create(this, nrOfNeuronsList, index: 0);
        }

        public void Initialize()
        {
            if (NnInitializer != null)
            {
                FirstNeurons.Initialize(NnInitializer, index: 0);
            }

            this.ErrorEvaluatorEnum = ErrorEvaluatorEnum.Crossentropy;
        }

        public void Forward(double[] input, double[] answer)
        {
            Epochs++;
            FirstNeurons.Forward(input);
            Matrix<double> error = GetError(LastNeurons.O, answer);
            this.Errors.Add(error);

            Console.WriteLine(error);

            if (Epochs >= MaxEpochs)
            {
                Epochs = 0;
                Backpropagation();
            }
        }

        private void Backpropagation()
        {
            Matrix<double> error = GetGeneralError(Errors);
            LastNeurons.Backpropagation();
        }

        private Matrix<double> GetGeneralError(List<Matrix<double>> errors)
        {
            Matrix<double> error = Matrix<double>.Build.Dense(1, LastNeurons.O.ColumnCount);
            foreach (Matrix<double> e in errors)
            {
                for (int i = 0; i < error.ColumnCount; i++)
                {
                    error[0, i] += e[0, i];
                }
            }

            for (int i = 0; i < error.ColumnCount; i++)
            {
                error[0, i] = error[0, i] / errors.Count;
            }

            return error;
        }

        private Matrix<double> GetError(Matrix<double> o, double[] answer)
        {
            for (int i = 0; i < o.ColumnCount; i++)
            {
                o[0, i] = FuncErrorEval(answer[i], o[0, i]);
            }

            return o;
        }

    }
}
