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

namespace NeuralNetwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeNeuralNetwork();
        }

        private NeuralNetwork nn;

        private void InitializeNeuralNetwork()
        {
            nn = new NeuralNetwork();
            nn.Create(new int[] { 2, 2, 1 });
            nn.Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        double[] _input1 = { 0, 0 };
        double[] _input2 = { 0, 1 };
        double[] _input3 = { 1, 0 };
        double[] _input4 = { 1, 1 };
        
        private void button1_Click(object sender, EventArgs e)
        {
            Matrix<double> output = nn.Guess(_input1);
            label1.Text = output[0, 0].ToString(); ;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Matrix<double> output = nn.Guess(_input2);
            label2.Text = output[0, 0].ToString(); ;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Matrix<double> output = nn.Guess(_input3);
            label3.Text = output[0, 0].ToString(); ;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Matrix<double> output = nn.Guess(_input4);
            label4.Text = output[0, 0].ToString(); ;
        }

        private void RecreateNn_Click(object sender, EventArgs e)
        {
            InitializeNeuralNetwork();

            //Matrix<double> m1 = Matrix<double>.Build.Dense(2, 1);
            //Matrix<double> m2 = Matrix<double>.Build.SameAs(m1);
            //Matrix<double> m3 = Matrix<double>.Build.Dense(2, 1);

            //m1[0, 0] = 1;
            //m1[1, 0] = 2;
            //m2[0, 0] = 3;
            //m2[1, 0] = 4;

            //m2 = m1.Multiply(4);

            //m3 = m1.Multiply(m2);

            //Console.WriteLine(m1);
            //Console.WriteLine(m2);
            //Console.WriteLine(m3);
        }

        private void Train_Click(object sender, EventArgs e)
        {
            Random ran = new Random();

            double[][] allInputs = { _input1, _input2, _input3, _input4 };
            double[][] y = { new double[] { 0 }, new double[] { 1 }, new double[] { 1 }, new double[] { 0 } };

            for (int i = 0; i < 10000; i++)
            {
                int index = ran.Next(0, 4);
                double[] input = allInputs[index];
                double[] answer = y[index];

                nn.Train(input, answer);
            }

        }

        private void SetWAndB_Click(object sender, EventArgs e)
        {
            Layer l1 = nn.Layers[1];
            l1.W[0, 0] = 20;
            l1.W[0, 1] = 20;
            l1.W[1, 0] = -20;
            l1.W[1, 1] = -20;

            l1.B[0, 0] = -10;
            l1.B[1, 0] = 30;

            Layer l2 = nn.Layers[2];
            l2.W[0, 0] = 20;
            l2.W[0, 1] = 20;
            l2.B[0, 0] = -30;
        }

        private void SetAt75_Click(object sender, EventArgs e)
        {
            Layer l1 = nn.Layers[1];
            l1.W[0, 0] = 15;
            l1.W[0, 1] = 10;
            l1.W[1, 0] = -15;
            l1.W[1, 1] = -10;

            l1.B[0, 0] = -10;
            l1.B[1, 0] = 30;

            Layer l2 = nn.Layers[2];
            l2.W[0, 0] = 20;
            l2.W[0, 1] = 20;
            l2.B[0, 0] = -30;
        }
    }
}
