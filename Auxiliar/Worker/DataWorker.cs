using Auxiliar.Common.Enum;
using Auxiliar.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliar.Worker
{
    public class DataWorker
    {
        public const int _len = 28;
        public const int _total = _len * _len; // 784
        public const int _dataLen = 1000;

        public int Prefix;

        public List<string> Entities { get; set; }

        public string NpyDirectoryPath { get; set; } = @"C:\Useful\NN\npy";
        public string TxtDirectoryPath { get; set; } = @"C:\Useful\NN\txt";
        public string FilesPath { get; set; }

        public ProblemEnum? Problem { get; set; }

        public DataWorker()
        {
        }

        public void Create(WrapperFileWorker wrapper)
        {
            Problem = wrapper.Problem;
        }

        public void Initialize()
        {
            if (Problem == null)
                Problem = ProblemEnum.Digits;

            switch (Problem)
            {
                case ProblemEnum.Xor:
                    throw new NotImplementedException();
                case ProblemEnum.Digits:
                    Prefix = 84;
                    Entities = Enum.GetNames(typeof(DigitsEnum)).ToList();
                    FilesPath = "Digits";
                    break;
                case ProblemEnum.QuickDraw:
                    Prefix = 80;
                    Entities = Enum.GetNames(typeof(QuickDrawEnum)).ToList();
                    FilesPath = "QuickDraw";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void CreateBlackAndWhiteTxtFilesUsingNpy()
        {
            Parallel.ForEach(Entities, /*new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount},*/ (entity) =>
            //foreach (string entity in Entities)
            {
                byte[] data = File.ReadAllBytes(GetFilesPath(entity));
                WriteToFile(data, entity);
            }
            );
        }

        public void WriteToFile(byte[] data, string entity)
        {
            string targetFile = Path.Combine(TxtDirectoryPath, entity + ".txt");
            using (StreamWriter writer = new StreamWriter(targetFile))
            {
                int j = 0;
                for (int i = Prefix; i < data.Length; i++)
                {
                    writer.Write(data[i] > 0 ? '1' : '0');
                    if (++j == _len)
                    {
                        writer.WriteLine();
                        j = 0;
                    }
                }
            }
        }

        private string GetFilesPath(string fileName)
        {
            return Path.Combine(NpyDirectoryPath, FilesPath, fileName + ".npy");
        }

        public List<List<byte[]>> GetTestData()
        {
            // 1000 x 
            int skipBytes = _dataLen * _total;
            //int dataLen = _dataLen / 5;
            int dataLen = 200;

            List<List<byte[]>> testData = GetTrainData(skipBytes, dataLen); 

            return testData;
        }

        public List<List<byte[]>> GetTrainData(int skipBytes = 0, int dataLen = _dataLen)
        {
            List<List<byte[]>> data = new List<List<byte[]>>();

            foreach (string fileName in Entities)
            {
                byte[] dataFromFile = File.ReadAllBytes(GetFilesPath(fileName));
                List<byte[]> list = new List<byte[]>();
                data.Add(list);

                int obj = 0;
                int j = 0;
                byte[] currentBytes = new byte[_total];
                for (int i = Prefix + skipBytes; i < dataFromFile.Length; i++)
                {
                    currentBytes[j] = dataFromFile[i] == 0 ? (byte)0 : (byte)1;
                    //currentBytes[j] = data[i];

                    if (++j == _total)
                    {
                        list.Add(currentBytes);
                        j = 0;
                        currentBytes = new byte[_total];

                        if (++obj == dataLen)
                            break;
                    }
                }
            }
            return data;
        }

        //private List<List<byte[]>> GetTrainDataForDigits_2()
        //{
        //    List<List<byte[]>> data = new List<List<byte[]>>();
        //    byte[] dataFromFile = File.ReadAllBytes(GetFilesPath("digits_2_train"));

        //    //WriteToFile(dataFromFile, "All");

        //    for (int k = 0; k < 10; k++)
        //    {
        //        int skipBytes = 6000 * k;

        //        List<byte[]> list = new List<byte[]>();
        //        data.Add(list);

        //        int j = 0;
        //        byte[] currentBytes = new byte[_total];
        //        for (int i = Prefix + skipBytes; i < Prefix + skipBytes + 6000 * _total; i++)
        //        {
        //            currentBytes[j] = dataFromFile[i] == 0 ? (byte)0 : (byte)1;
        //            //currentBytes[j] = data[i];

        //            if (++j == _total)
        //            {
        //                list.Add(currentBytes);
        //                j = 0;
        //                currentBytes = new byte[_total];
        //            }
        //        }
        //    }

        //    //WriteToFile(data[0][0], "0");
        //    //WriteToFile(data[0][data[0].Count - 1], "0Final");

        //    return data;
        //}
    }
}
