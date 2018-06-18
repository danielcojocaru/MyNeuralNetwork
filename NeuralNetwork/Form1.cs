using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using NeuralNetwork.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using NeuralNetwork.Model.Initializers;
using NeuralNetwork.Auxiliar.Enum;
using NeuralNetwork.Auxiliar.Interface;
using System.IO;
using NeuralNetwork.Worker;

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        private string filePath = @"C:\Projects\ZZZ\MyNn\GoogleQuickDraw\apple.npy";

        




        private const int len = 28;
        private const int total = len * len; // 784
        private const int prefix = 80;

        public Form1()
        {
            InitializeComponent();

            //InitializeNeuralNetworkNew();
            //InitializeNeuralNetworkOld();
            //SetCurrentNn();

            //Compare_Click(null, null);
            //TrainNn3();
            //ExcelTestTwoSixThreeTwo();

            //Data = File.ReadAllBytes(filePath);


            //pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            //this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            //pictureBox.Size = new Size(112, 112);

            FileWorker w = new FileWorker();
            w.Initialize();
            w.ReadAllFilesFromNpy();
        }

        byte[] Data;
        int CurrentImgIndex = 0;

        private void NextImage()
        {
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    Color color = Data[prefix + CurrentImgIndex * total + len * i + j] > 0 ? Color.Black : Color.White;

                    ((Bitmap)pictureBox.Image).SetPixel(j, i, color);
                }
            }
            CurrentImgIndex++;
            pictureBox.Refresh();
        }

        private void SetCurrentNn()
        {
            UsedNn = nnNew;
        }

        private INeuralNetwork UsedNn;

        private void InitializeNeuralNetworkNew()
        {
            nnNew = new NeuralNetworkCls();
            nnNew.FunctionInitializer = new FunctionInitializerRandom();
            //nnNew.NnInitializer = new NnInitializerXor();
            nnNew.Create(new int[] { 2, 2, 1 });
            nnNew.Initialize();
        }

        private void InitializeNeuralNetworkOld()
        {
            nnOld = new NeuralNetworkOld();
            //nnOld.NnInitializer = new NnInitializerXor();
            nnOld.Create(new int[] { 2, 2, 1 });
            nnOld.Initialize();
        }

        private NeuralNetworkOld nnOld;
        private NeuralNetworkCls nnNew;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        double[] _input1 = { 0, 0 };
        double[] _input2 = { 0, 1 };
        double[] _input3 = { 1, 0 };
        double[] _input4 = { 1, 1 };
        
        private void CheckAnswer()
        {
            Matrix<double> output;

            output = UsedNn.Guess(_input1);
            label1.Text = output[0, 0].ToString();

            output = UsedNn.Guess(_input2);
            label2.Text = output[0, 0].ToString();

            output = UsedNn.Guess(_input3);
            label3.Text = output[0, 0].ToString();

            output = UsedNn.Guess(_input4);
            label4.Text = output[0, 0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //nn.Train(_input1, new double[] { 0 });
            //nn.PrintToExcel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //nn.Train(_input2, new double[] { 1 });
            //nn.PrintToExcel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //nn.Train(_input3, new double[] { 1 });
            //nn.PrintToExcel();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //nn.Train(_input4, new double[] { 0 });
            //nn.PrintToExcel();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CheckAnswer();
        }

        private void RecreateNn_Click(object sender, EventArgs e)
        {
            InitializeNeuralNetworkNew();
            InitializeNeuralNetworkOld();
            SetCurrentNn();
        }

        private void Train_Click(object sender, EventArgs e)
        {
            TrainNn3();
        }

        private void TrainNn3()
        {
            List<double[]> inputs = new List<double[]>();

            for (int i = 0; i < 40000; i++)
            {
                inputs.Add(_input1);
                inputs.Add(_input2);
                inputs.Add(_input3);
                inputs.Add(_input4);
            }

            Shuffle2(inputs);
                
            foreach (double[] input in inputs)
            {
                double[] answer = GetAnswer(input);

                UsedNn.Forward(input, answer, doBackpropagation: true);
            }
        }

        private double[] GetAnswer(double[] input)
        {
            bool one = input[0] == 1;
            bool two = input[1] == 1;
            bool result = one ^ two;

            if (result)
            {
                return new double[] { 1 };
            }
            else
            {
                return new double[] { 0 };
            }
        }

        private double[] GetAnswer2(double[] input)
        {
            bool one = input[0] == 1;
            bool two = input[1] == 1;
            bool result = one == two;

            if (result)
            {
                return new double[] { 1 };
            }
            else
            {
                return new double[] { 0 };
            }
        }

        private double[] GetAnswer3(double[] input)
        {
            bool one = input[0] == 1;
            bool two = input[1] == 1;
            bool result = one == two;

            if (two)
            {
                return new double[] { 1 };
            }
            else
            {
                return new double[] { 0 };
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle2<T>(IList<T> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void SetWAndB_Click(object sender, EventArgs e)
        {
        }

        private void SetAt75_Click(object sender, EventArgs e)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void Compare_Click(object sender, EventArgs e)
        {
            double[] input = _input2;
            double[] answer = GetAnswer(input);


            nnOld.Forward(input, answer, doBackpropagation: true);
            nnNew.Forward(input, answer, doBackpropagation: true);

            //Console.WriteLine("Old E:");
            //Console.WriteLine(nnOld.LastLayer.E);

            //Console.WriteLine(nnOld.Layers[1].B);
            //Console.WriteLine(((Synapses)nnNew.NeuronsAndSynappses[1]).B);
        }

        private void RecreateTrainSeeResults_Click(object sender, EventArgs e)
        {
            RecreateNn_Click(null, null);
            Train_Click(null, null);
            CheckAnswer();
        }

        private void NextImage_Click(object sender, EventArgs e)
        {
            NextImage();
        }

    }
}
