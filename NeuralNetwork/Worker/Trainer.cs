using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar.Interface;
using NeuralNetwork.Auxiliar.Model;
using NeuralNetwork.Model;
using NeuralNetwork.Model.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Worker
{
    public class Trainer
    {
        public NeuralNetworkCls Nn { get; set; }
        public List<List<byte[]>> Data { get; set; }
        public Dictionary<int, double[]> Answer { get; set; } = new Dictionary<int, double[]>();

        public int StepsToCalculatePrecision { get; set; } = 1000;
        public int Step { get; set; } = -1;
        public int StepForPrecision { get; set; } = -1;
        public double Precision { get; set; } = 0;

        public int MyProperty { get; set; }

        public void Create(WrapperTrainer wrapper)
        {
            this.Nn = wrapper.Nn;
            this.Data = wrapper.Data;
        }

        public void Initialize()
        {
            if (Nn == null)
            {
                InitializeNnForGoogleQuickdraw();
            }
            SetAnswer();
        }

        private void SetAnswer()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                double[] answer = new double[Data.Count];
                answer[i] = 1;

                Answer.Add(i, answer);
            }
        }

        private void InitializeNnForGoogleQuickdraw()
        {
            NeuralNetworkCls nn = new NeuralNetworkCls();
            nn.FunctionInitializer = new FunctionInitializerRandom();
            //nnNew.NnInitializer = new NnInitializerXor();

            int nrOfInputs = FileWorker._total;

            nn.Create(new int[] { nrOfInputs, 64, 64, Data.Count });
            nn.Initialize();

            Nn = nn;
        }

        public object Process()
        {
            Random r = new Random();
            int objCount = Data.Count;
            int imgCount = Data[0].Count;

            double max = 0;


            while (true)
            {
                Step++;
                StepForPrecision++;

                //int objIndex = 2;
                int objIndex = r.Next(0, objCount); // 1 to 6
                int imgIndex = r.Next(0, imgCount); // 1 to 1000

                double[] input = Data[objIndex][imgIndex].Select(Convert.ToDouble).ToArray();
                double[] answer = Answer[objIndex];

                Nn.Forward(input, answer, doBackpropagation: true);

                double result = Nn.LastNeurons.O[objIndex, 0];
                max = GetMax(Nn.LastNeurons.O);

                if (max == result && max != 0)
                {
                    Correct++;
                }

                if (StepForPrecision == StepsToCalculatePrecision)
                //if (true)
                {
                    StepForPrecision = -1;
                    //Precision = Nn.LastNeurons.O[objIndex, 0];
                    Console.WriteLine(Correct);

                    Nn.Lr = ((double)StepsToCalculatePrecision - (double)Correct) / (double)StepsToCalculatePrecision / 100D * 2D;

                    Correct = 0;
                }
            }

            return null;
        }

        private int Correct = 0;

        private double GetMax(Matrix<double> o)
        {
            double max = -1;
            for (int i = 0; i < o.RowCount; i++)
            {
                if (max < o[i, 0])
                {
                    max = o[i, 0];
                }
            }
            return max;

        }

    }
}
