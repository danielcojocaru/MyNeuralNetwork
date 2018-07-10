using Auxiliar.Common.Enum;
using Auxiliar.Other;
using Auxiliar.Worker;
using Auxiliar.Wrapper;
using NeuralNetworkNew.Body;
using NeuralNetworkNew.Worker;
using NeuralNetworkNew.Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public class UiWorker
    {
        public UiForm Form { get; set; }
        public Trainer Trainer { get; set; }
        public DataWorker DataWorker { get; set; }


        public Dictionary<int, List<byte[]>> CreatedData { get; set; } = new Dictionary<int, List<byte[]>>();

        public UiWorker()
        {
        }

        public void Create(UiForm uiForm)
        {
            Form = uiForm;

            DataWorker = new DataWorker();
            DataWorker.Create(new WrapperFileWorker() { Problem = ProblemEnum.QuickDraw });
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

        public string Guess(byte[] imgAsByte)
        {
            int index = Trainer.Guess(imgAsByte);
            string guessed = DataWorker.Entities[index];

            return guessed;
        }

        public string GetEntityName(int index)
        {
            string entity = DataWorker.Entities[index];
            return entity;
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
                byte[] img = GetRandom(Trainer.TestData, index);
                return img;
            }
            catch (Exception)
            {
                try
                {
                    byte[] img = GetRandom(Trainer.Data, index);
                    MessageBox.Show("No test data found.");
                    return img;
                }
                catch (Exception)
                {
                    MessageBox.Show("OutOfRange");
                    return Trainer.Data[0][0];
                }
            }
        }

        private byte[] GetRandom(List<List<byte[]>> data, int index)
        {
            return data[index][new Random().Next(0, data[0].Count)];
        }

        public void ImgToNpyFile(string fileName = "testImg")
        {
            byte[] imgAsByte = Form.GetImgFromPictureBox();

            DataWorker w = new DataWorker();
            w.WriteToNpyFile(imgAsByte, "testImg");
            string filePath = string.Format(@"C:\Useful\NN\Created\{0}.npy", fileName);

            try
            {
                Process myProcess = new Process();
                Process.Start("notepad++.exe", filePath);
            }
            catch (Exception)
            {
                MessageBox.Show(Form, "Could not open file");
            }
        }

        public void ImgToTxtFile(string fileName = "testImg")
        {
            byte[] imgAsByte = Form.GetImgFromPictureBox();

            DataWorker w = new DataWorker();
            string filePath = w.WriteToTxtFile(imgAsByte, "testImg");

            try
            {
                Process myProcess = new Process();
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception)
            {
                MessageBox.Show(Form, "Could not open file");
            }
        }

        public int SaveCurrentImgToData(int index)
        {
            List<byte[]> dataList;
            if (!CreatedData.ContainsKey(index))
            {
                dataList = new List<byte[]>();
                CreatedData.Add(index, dataList);
            }
            else
            {
                dataList = CreatedData[index];
            }

            byte[] imgAsByte = Form.GetImgFromPictureBox();
            dataList.Add(imgAsByte);
            return dataList.Count;
        }

        public void SaveCreatedDataToNpy()
        {
            foreach (KeyValuePair<int, List<byte[]>> entry in CreatedData)
            {
                DataWorker.WriteToNpyFile(entry.Value, entry.Key);
            }

            CreatedData = new Dictionary<int, List<byte[]>>();
        }

        public void RemoveLastInserted(int index)
        {
            if (CreatedData.ContainsKey(index))
            {
                List<byte[]> list = CreatedData[index];
                if (list.Count > 0)
                {
                    list.RemoveAt(list.Count - 1);
                }
                else
                {
                    MessageBox.Show(Form, "There is no data for that key. Count = 0.");
                }
            }
            else
            {
                MessageBox.Show(Form, "There is no data for that key");
            }
        }

        public void DoCustomStuff()
        {
            DataWorker.DoCustomStuff();
        }

        public void Serialize()
        {
            StaticMethodsClass.WriteToBinaryFile(@"C:\Useful\NN\BestOne.txt", Trainer.Nn);
        }

        public void Deserialize()
        {
            NeuralNetworkCls serializedNn = GetSerializedCls();
            Trainer.Nn = serializedNn;
        }

        private NeuralNetworkCls GetSerializedCls()
        {
            string path = Environment.CurrentDirectory + "\\Serialized\\BestOne.txt";
            NeuralNetworkCls serializedNn = StaticMethodsClass.ReadFromBinaryFile<NeuralNetworkCls>(path);
            return serializedNn;
        }
    }
}
