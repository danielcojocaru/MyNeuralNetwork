using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public Layer FirstLayer { get; set; }
        public Layer LastLayer { get; set; }

        public NeuralNetwork()
        {}

        public void Create(int[] layers)
        {
            CreateLayers(layers);

            FirstLayer = Layers[0];
            LastLayer = Layers[Layers.Count - 1];
        }

        private void CreateLayers(int[] layers)
        {
            Layer lastLayer = null; ;
            for (int i = 0; i < layers.Length; i++)
            {
                int nrOfOutputs = layers[i];
                Layer layer = new Layer();

                if (i == 0)
                {
                    layer.Create(nrOfOutputs);
                }
                else
                {
                    int nrOfInputs = layers[i - 1];
                    layer.Create(nrOfOutputs, nrOfInputs);

                    // ading the neighbors layers references
                    layer.Previous = lastLayer;
                    lastLayer.Next = layer;
                }

                lastLayer = layer;
                Layers.Add(layer);
            }
        }

        public void Initialize()
        {
            RandomizeLayers();
        }

        private void RandomizeLayers()
        {
            foreach (Layer layer in Layers)
            {
                layer.Randomize();
            }
        }

        public Matrix<double> Guess(double[] input)
        {
            Matrix<double> output = FirstLayer.Guess(input);
            return output;
        }

        public void Train(double[] input, double[] targets)
        {
            FirstLayer.Guess(input);
            LastLayer.Backwards(targets);
        }

        //public Matrix<double> Guess(double[] input)
        //{
        //    Layers[0].Guess(input);

        //    for (int i = 1; i < Layers.Count; i++)
        //    {
        //        Layer inputLayer = Layers[i - 1];
        //        Layer outputLayer = Layers[i];

        //        outputLayer.FeedInput(inputLayer);
        //    }

        //    Matrix<double> output = Layers[Layers.Count - 1].X;
        //    Console.WriteLine(output);
        //    return output;
        //}
    }
}
