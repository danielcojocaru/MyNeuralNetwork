using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Worker
{
    public class FileWorker
    {
        public const int _len = 28;
        public const int _total = _len * _len; // 784
        public const int _prefix = 80;

        public const int _dataLen = 1000;

        //public Encoding Encoding { get; set; } = Encoding.GetEncoding(1252);

        List<List<byte[]>> Data = new List<List<byte[]>>();

        public List<string> Entities { get; set; } = new List<string>()
        {
            "airplane",
            "apple",
            "banana",
            "bed",
            "bicycle",
            "car",
            "cat",
        };

        public int ObjCount { get; set; }

        public string NpyDirectoryPath { get; set; } = @"C:\Useful\NN\npy";
        public string TxtDirectoryPath { get; set; } = @"C:\Useful\NN\txt";

        public FileWorker()
        {

        }

        public void Initialize()
        {
            ObjCount = Entities.Count;
            ReadAllFilesFromNpy();
        }
        
        public void CreateBlackAndWhiteTxtFilesUsingNpy()
        {
            Parallel.ForEach(Entities, /*new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount},*/ (entity) =>
            //foreach (string entity in Entities)
            {
                byte[] data = File.ReadAllBytes(Path.Combine(NpyDirectoryPath, entity + ".npy"));

                string targetFile = Path.Combine(TxtDirectoryPath, entity + ".txt");
                using (StreamWriter writer = new StreamWriter(targetFile))
                {
                    int j = 0;
                    for (int i = _prefix; i < data.Length; i++)
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
            );
        }

        public void ReadAllFilesFromNpy()
        {
            //Parallel.ForEach(Entities, /*new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount },*/ (entity) =>
            foreach (string entity in Entities)
            {
                byte[] data = File.ReadAllBytes(Path.Combine(NpyDirectoryPath, entity + ".npy"));
                List<byte[]> list = new List<byte[]>();
                Data.Add(list);

                int obj = 0;
                int j = 0;
                byte[] currentBytes = new byte[_total];
                for (int i = _prefix; i < data.Length; i++)
                {
                    currentBytes[j] = data[i];
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
            //);
        }
    }
}
