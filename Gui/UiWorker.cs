using Auxiliar.Common.Enum;
using Auxiliar.Worker;
using Auxiliar.Wrapper;
using NeuralNetworkNew.Worker;
using NeuralNetworkNew.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public class UiWorker
    {
        public UiForm Form { get; set; }
        public Trainer Trainer { get; set; }
        public DataWorker DataWorker { get; set; }

        public UiWorker()
        {
        }

        public void Create(UiForm uiForm)
        {
            Form = uiForm;

            DataWorker = new DataWorker();
            DataWorker.Create(new WrapperFileWorker() { Problem = ProblemEnum.Digits });
            DataWorker.Initialize();
            List<List<byte[]>> data = DataWorker.GetTrainData();
            List<List<byte[]>> testData = DataWorker.GetTestData();

            WrapperTrainer wrapper = new WrapperTrainer() { Data = data, TestData = testData };

            Trainer = new Trainer();
            Trainer.Create(wrapper);
            Trainer.Initialize();
        }

        public void CreateBlackAndWhiteTxtFilesUsingNpy()
        {
            DataWorker.CreateBlackAndWhiteTxtFilesUsingNpy();
        }

        public void Train()
        {
            Task.Run(() => Trainer.Train());
        }

        public void StopTraining()
        {
            Trainer.StopTraining();
        }

        public int Guess(byte[] imgAsByte)
        {
            return Trainer.Guess(imgAsByte);
        }

        public TestReport Test()
        {
            List<List<byte[]>> testData = DataWorker.GetTestData();
            return Trainer.Test(testData);
        }

        public byte[] GetRandomImage(int index)
        {
            try
            {
                byte[] img = Trainer.TestData[index][new Random().Next(0, Trainer.TestData[0].Count)];
                return img;
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("OutOfRange");

                return Trainer.Data[0][0];
            }
            
        }
    }
}
