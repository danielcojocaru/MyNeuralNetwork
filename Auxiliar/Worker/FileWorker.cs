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
    public class FileWorker
    {
        public const int _len = 28;
        public const int _total = _len * _len; // 784
        public const int _dataLen = 1000;

        public int Prefix;

        List<List<byte[]>> Data = new List<List<byte[]>>();

        public List<string> Entities { get; set; }

        public string NpyDirectoryPath { get; set; } = @"C:\Useful\NN\npy";
        public string TxtDirectoryPath { get; set; } = @"C:\Useful\NN\txt";
        public string FilesPath { get; set; }

        public ProblemEnum? Problem { get; set; }

        public FileWorker()
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

        private string GetFilesPath(string entity)
        {
            return Path.Combine(NpyDirectoryPath, FilesPath, entity + ".npy");
        }

        public List<List<byte[]>> ReadAllFilesFromNpy()
        {
            //Parallel.ForEach(Entities, /*new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },*/ (entity) =>
            foreach (string entity in Entities)
            {
                byte[] data = File.ReadAllBytes(GetFilesPath(entity));
                List<byte[]> list = new List<byte[]>();
                Data.Add(list);

                int obj = 0;
                int j = 0;
                byte[] currentBytes = new byte[_total];
                for (int i = Prefix; i < data.Length; i++)
                {
                    currentBytes[j] = data[i] == (byte)0 ? data[i] : (byte)1;
                    //currentBytes[j] = data[i];

                    if (++j == _total)
                    {
                        list.Add(currentBytes);
                        j = 0;
                        currentBytes = new byte[_total];

                        if (++obj == _dataLen)
                            break;
                    }


                }
            }
            return Data;
            //);
        }
    }
}
