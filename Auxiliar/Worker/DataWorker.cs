using Auxiliar.Common.Enum;
using Auxiliar.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliar.Worker
{
    public class DataWorker
    {
        public const int _len = 28;
        public const int _total = _len * _len; // 784
        public const int _dataLenTrain = 100000;
        public const int _dataLenTest = 2000;

        public int Prefix;

        public List<string> Entities { get; set; }

        public string NpyDirectoryPath { get; set; } = @"C:\Useful\NN\npy";
        public string TxtDirectoryPath { get; set; } = @"C:\Useful\NN\txt";
        public string CreatedDirectoryPath { get; set; } = @"C:\Useful\NN\Created";
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
                case ProblemEnum.OwnDigits:
                    Prefix = 0;
                    Entities = Enum.GetNames(typeof(DigitsEnum)).ToList();
                    FilesPath = "OwnDigits";
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
            Parallel.ForEach(Entities, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount}, (entity) =>
            //foreach (string entity in Entities)
            {
                byte[] data = File.ReadAllBytes(GetFilesPath(entity));
                WriteToTxtFile(data, entity);
            }
            );
        }

        public void DoCustomStuff()
        {
            // Nothing here
        }

        public string WriteToTxtFile(byte[] data, string entity)
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
            return targetFile;
        }

        public void WriteToNpyFile(byte[] data, int entityIndex)
        {
            WriteToNpyFile(data, Entities[entityIndex]);
        }

        public void WriteToNpyFile(List<byte[]> data, int entityIndex)
        {
            WriteToNpyFile(data, Entities[entityIndex]);
        }

        public void WriteToNpyFile(byte[] data, string entity)
        {
            string targetFile = Path.Combine(CreatedDirectoryPath, entity + ".npy");

            using (var fs = new FileStream(targetFile, FileMode, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        private FileMode FileMode = FileMode.Append;

        public void WriteToNpyFile(List<byte[]> dataList, string entity)
        {
            string targetFile = Path.Combine(CreatedDirectoryPath, entity + ".npy");

            using (var fs = new FileStream(targetFile, FileMode, FileAccess.Write))
            {
                foreach (byte[] data in dataList)
                {
                    fs.Write(data, 0, data.Length);
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
            int skipBytes = _dataLenTrain * _total;
            //int dataLen = _dataLen / 5;
           

            List<List<byte[]>> testData = GetTrainData(skipBytes, _dataLenTest); 

            return testData;
        }

        public List<List<byte[]>> GetTrainData(int skipBytes = 0, int dataLen = _dataLenTrain)
        {
            GetFilesIfMissing();

            List<List<byte[]>> data = new List<List<byte[]>>();
            foreach (string fileName in Entities)
            {
                string filePath = GetFilesPath(fileName);

                byte[] dataFromFile = File.ReadAllBytes(filePath);
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

        private void GetFilesIfMissing()
        {
            // Google is unfriendly
            //string imgUrl = "https://storage.cloud.google.com/quickdraw_dataset/full/numpy_bitmap/apple.npy";


            //Parallel.ForEach(Entities, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount}, (entity) =>
            //{
            //    //using (WebClient wc = new WebClient())
            //    //{
            //    //    wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 5.1; rv:10.0.2) Gecko/20100101 Firefox/10.0.2";
            //    //    wc.DownloadFile(imgUrl, "apple.npy");
            //    //    //byte[] data = wc.DownloadData(imgUrl);
            //    //}


            //    using (CookiesAwareWebClient client = new CookiesAwareWebClient())
            //    {

            //    }


            //});


        }
    }
}
