using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar.Enum;
using System.IO;

namespace NeuralNetwork.Model
{
    [Serializable]
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

        public double Lr { get; set; } = 0.01;

        public bool IsExcelTest { get; set; }

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
            FirstNeurons.Initialize(NnInitializer, index: 0);
            this.ErrorEvaluatorEnum = ErrorEvaluatorEnum.Crossentropy;
        }



        public Matrix<double> Guess(double[] input)
        {
            Forward(input, doBackpropagation : false);
            return LastNeurons.O;
        }

        public void Forward(double[] input, double[] answer = null, bool? doBackpropagation = true)
        {
            if (doBackpropagation == true)
            {
                Epochs++;
            }

            FirstNeurons.Forward(input);

            if (answer != null)
            {
                Matrix<double> error = GetError(LastNeurons.I, answer);
                this.Errors.Add(error);
            }

            if (doBackpropagation == true && Epochs >= MaxEpochs)
            {
                Epochs = 0;
                Backpropagation();
            }
        }

        private void Backpropagation()
        {
            Matrix<double> error = GetGeneralError(Errors);
            LastNeurons.Backpropagation(error);
        }

        private Matrix<double> GetGeneralError(List<Matrix<double>> errors)
        {
            Matrix<double> error = Matrix<double>.Build.Dense(LastNeurons.O.RowCount, 1);
            foreach (Matrix<double> e in errors)
            {
                for (int i = 0; i < error.RowCount; i++)
                {
                    error[i, 0] += e[i, 0];
                }
            }

            for (int i = 0; i < error.RowCount; i++)
            {
                error[i, 0] = error[i, 0] / errors.Count;
            }

            return error;
        }

        public void OnBackPropagationEnded()
        {
            if (IsExcelTest)
            {
                string path = Environment.CurrentDirectory + "\\ExternalFiles\\ExcelTwoSixThreeTwo.txt";
                //WriteToBinaryFile(path, this);

                NeuralNetworkCls nnFromEcel = ReadFromBinaryFile<NeuralNetworkCls>(path);
                CheckIfNeuralNetworksAreEqual(nnFromEcel, this);
            }
        }

        private void CheckIfNeuralNetworksAreEqual(NeuralNetworkCls nnFromEcel, NeuralNetworkCls neuralNetworkCls)
        {
            for (int i = 0; i < nnFromEcel.NeuronsAndSynappses.Count; i++)
            {
                if (i % 2 == 1)
                {
                    Synapses s1 = nnFromEcel.NeuronsAndSynappses[i] as Synapses;
                    Synapses s2 = neuralNetworkCls.NeuronsAndSynappses[i] as Synapses;

                    if (!s1.W.Equals(s2.W) || !s1.B.Equals(s2.B))
                    {
                        throw new Exception("W or B are not equal.");
                    }
                }
            }
            System.Windows.Forms.MessageBox.Show("Excel test completed. The current neural network delivered the same results as the serialised class.", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
        }

        private Matrix<double> GetError(Matrix<double> o, double[] answer)
        {
            Matrix<double> error = Matrix<double>.Build.Dense(o.RowCount, o.ColumnCount);
            for (int i = 0; i < o.RowCount; i++)
            {
                error[i, 0] = FuncErrorEval(answer[i], o[i, 0]);
            }

            return error;
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
