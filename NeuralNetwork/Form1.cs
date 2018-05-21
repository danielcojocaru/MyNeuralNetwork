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

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeNeuralNetworkNew();
            InitializeNeuralNetworkOld();

            UsedNn = nnNew;

            TrainNn3();
            //ExcelTestTwoSixThreeTwo();

        }

        private INeuralNetwork UsedNn;

        private void InitializeNeuralNetworkNew()
        {
            nnNew = new NeuralNetworkCls();
            nnNew.FunctionInitializer = new FunctionInitializerSigmoidAndSimple();
            nnNew.NnInitializer = new NnInitializerXor();
            nnNew.Create(new int[] { 2, 2, 1 });
            nnNew.Initialize();
        }

        private void InitializeNeuralNetworkOld()
        {
            nnOld = new NeuralNetworkOld();
            nnOld.NnInitializer = new NnInitializerXor();
            nnOld.Create(new int[] { 2, 2, 1 });
            nnOld.Initialize();
        }


        //private void ExcelTestTwoSixThreeTwo()
        //{
        //    nn = new NeuralNetworkCls();
        //    nn.Create(new int[] { 2, 6, 3, 2 });
        //    nn.NnInitializer = new NnInitializerTwoSixThreeTwo();
        //    nn.IsExcelTest = true;
        //    FunctionInitializerReluAndSimple
        //    nn.Initialize();
        //    nn.Forward(new double[] { 1, 2 }, new double[] { 1, 0 });
        //}

        ///// <summary>
        ///// This tests if the neural network still does the Forward and Backpropagation properly. The expected results are in the file Nn.xlsx
        ///// </summary>
        //private void ExcelTest()
        //{
        //    nn = new NeuralNetworkCls();
        //    nn.Create(new int[] { 3, 3, 3, 3 });
        //    nn.NnInitializer = new NnInitializerMediumDotComExample();
        //    nn.IsExcelTest = true;
        //    nn.Initialize();
        //    nn.Forward(new double[] { 0.1, 0.2, 0.7 }, new double[] { 1, 0, 0 });
        //}

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
        }

        private void Train_Click(object sender, EventArgs e)
        {
            TrainNn3();
        }

        private void TrainNn3()
        {
            List<double[]> inputs = new List<double[]>();

            for (int i = 0; i < 10000; i++)
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

                //Console.WriteLine("OLD:");
                //nnOld.Forward(input, answer, doBackpropagation: true);

                //Console.WriteLine("NEW:");
                nnNew.Forward(input, answer, doBackpropagation: true);

                //Console.WriteLine("OLD:");
                //Console.WriteLine("1:");
                //Console.WriteLine("W:");
                //Console.WriteLine(nnOld.Layers[1].W);
                //Console.WriteLine("B:");
                //Console.WriteLine(nnOld.Layers[1].B);
                //Console.WriteLine("I:");
                //Console.WriteLine(nnOld.Layers[1].I);
                //Console.WriteLine("O:");
                //Console.WriteLine(nnOld.Layers[1].O);
                //Console.WriteLine("2:");
                //Console.WriteLine("W:");
                //Console.WriteLine(nnOld.Layers[2].W);
                //Console.WriteLine("B:");
                //Console.WriteLine(nnOld.Layers[2].B);
                //Console.WriteLine("I:");
                //Console.WriteLine(nnOld.Layers[1].I);
                //Console.WriteLine("O:");
                //Console.WriteLine(nnOld.Layers[2].O);


                //Console.WriteLine("NEW:");
                //Console.WriteLine("1:");
                //Console.WriteLine("W:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[1] as Synapses).W);
                //Console.WriteLine("B:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[1] as Synapses).B);
                //Console.WriteLine("I:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[2] as Neurons).I);
                //Console.WriteLine("O:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[2] as Neurons).O);
                //Console.WriteLine("2:");
                //Console.WriteLine("W:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[3] as Synapses).W);
                //Console.WriteLine("B:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[3] as Synapses).B);
                //Console.WriteLine("I:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[4] as Neurons).I);
                //Console.WriteLine("O:");
                //Console.WriteLine((nnNew.NeuronsAndSynappses[4] as Neurons).O);







            }
        }

        //private void TrainNn2()
        //{
        //    double[][] allInputs = { _input1, _input2, _input3, _input4 };
        //    List<double[]> inputs = allInputs.ToList();

        //    for (int i = 0; i < 10000; i++)
        //    {
        //        Shuffle(inputs);
        //        foreach (double[] input in inputs)
        //        {
        //            double[] answer = GetAnswer(input);
        //            nn.Train(input, answer);
        //        }
        //    }
        //}

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
            //LayerOld l1 = nnOld.Layers[1];
            //l1.W[0, 0] = 20;
            //l1.W[0, 1] = 20;
            //l1.W[1, 0] = -20;
            //l1.W[1, 1] = -20;

            //l1.B[0, 0] = -10;
            //l1.B[1, 0] = 30;

            //LayerOld l2 = nnOld.Layers[2];
            //l2.W[0, 0] = 20;
            //l2.W[0, 1] = 20;
            //l2.B[0, 0] = -30;
        }

        private void SetAt75_Click(object sender, EventArgs e)
        {
            //LayerOld l1 = nnOld.Layers[1];
            //l1.W[0, 0] = 15;
            //l1.W[0, 1] = 10;
            //l1.W[1, 0] = -15;
            //l1.W[1, 1] = -10;

            //l1.B[0, 0] = -10;
            //l1.B[1, 0] = 30;

            //LayerOld l2 = nnOld.Layers[2];
            //l2.W[0, 0] = 20;
            //l2.W[0, 1] = 20;
            //l2.B[0, 0] = -30;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //nnOld.PrintToExcel();
        }

        
    }
}
