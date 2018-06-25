using Auxiliar.Worker;
using MathNet.Numerics.LinearAlgebra;
using NeuralNetworkNew.Body;
using NeuralNetworkNew.Initializer;
using NeuralNetworkNew.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auxiliar.Wrapper;

namespace NeuralNetworkNew.Worker
{
    public class Trainer
    {
        public NeuralNetworkCls Nn { get; set; }
        public List<List<byte[]>> Data { get; set; }
        public List<List<byte[]>> TestData { get; set; }
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
            this.TestData = wrapper.TestData;
        }

        public void Initialize()
        {
            if (Nn == null)
            {
                InitializeNnFor24x24Drawings();
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


        private void InitializeNnFor24x24Drawings()
        {
            NeuralNetworkCls nn = new NeuralNetworkCls();
            nn.FunctionInitializer = new FunctionInitializerRandom();
            //nnNew.NnInitializer = new NnInitializerXor();

            int nrOfInputs = DataWorker._total;

            nn.Create(new int[] { nrOfInputs, 64, 64, Data.Count });
            nn.Initialize();

            Nn = nn;
        }


        public bool IsStopTraining { get; set; } = false;

        public void StopTraining()
        {
            IsStopTraining = true;
        }


        public TestReport Test(List<List<byte[]>> testData)
        {
            TestReport report = new TestReport();
            report.SetNrOfEntities(testData.Count);

            //foreach (List<byte[]> groupData in testData)
            for (int i = 0; i < testData.Count; i++)
            {
                List<byte[]> groupData = testData[i];
                double[] answer = Answer[i];

                foreach (byte[] inputAsByte in groupData)
                {
                    double[] input = inputAsByte.Select(Convert.ToDouble).ToArray();
                    Nn.Forward(input, answer, doBackpropagation: false);

                    if (AnswerIsCorrect(answer))
                    {
                        report.AddCorrect(i);
                    }
                    else
                    {
                        report.AddIncorrect(i);
                    }
                }
            }
            return report;
        }

        public void Train()
        {
            IsStopTraining = false;
            Correct = 0;

            Random random = new Random();
            int objCount = Data.Count;
            int imgCount = Data[0].Count;

            double max = 0;

            while (!IsStopTraining)
            {
                Step++;
                StepForPrecision++;

                //int objIndex = 2;
                int objIndex = random.Next(0, objCount); // 1 to 6
                int imgIndex = random.Next(0, imgCount); // 1 to 1000

                double[] input = Data[objIndex][imgIndex].Select(Convert.ToDouble).ToArray();
                double[] answer = Answer[objIndex];

                Nn.Forward(input, answer, doBackpropagation: true);

                if (AnswerIsCorrect(answer))
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
        }

        private bool AnswerIsCorrect(double[] answer)
        {
            double maxAnswer = 0;
            double maxOutput = 0;
            int indexAnswer = 0;
            int indexOutput = 0;

            for (int i = 0; i < answer.Length; i++)
            {
                double currentAnswer = answer[i];
                if (currentAnswer > maxAnswer)
                {
                    maxAnswer = currentAnswer;
                    indexAnswer = i;
                }

                double currentOutput = Nn.LastNeurons.O[i, 0];
                if (currentOutput > maxOutput)
                {
                    maxOutput = currentOutput;
                    indexOutput = i;
                }
            }

            bool isCorrect = indexAnswer == indexOutput && maxAnswer != 0 && maxOutput != 0;
            return isCorrect;
        }

        public int Guess(byte[] imgAsByte)
        {
            double[] input = imgAsByte.Select(Convert.ToDouble).ToArray();
            Nn.Forward(input, null, doBackpropagation: false);

            int maxIndex = 0;
            double max = -1;
            for (int i = 0; i < Nn.LastNeurons.O.RowCount; i++)
            {
                double current = Nn.LastNeurons.O[i, 0];
                if (max < current)
                {
                    max = current;
                    maxIndex = i;
                }
            }

            return maxIndex;
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
