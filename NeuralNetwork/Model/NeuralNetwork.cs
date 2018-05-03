using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Model
{
    public class NeuralNetworkCls
    {
        public Neurons FirstNeurons { get; set; }
        public Neurons LastNeurons { get; set; }

        public INeuralNetworkInitializer NnInitializer { get; set; }

        public NeuralNetworkCls()
        {
        }

        public void Create(int[] layers)
        {
            CreateLayers(layers);
        }

        private void CreateLayers(int[] nrOfNeuronsList)
        {
            FirstNeurons = new Neurons();
            FirstNeurons.Create(this, nrOfNeuronsList, index: 0);
        }

        public void Initialize()
        {
            if (NnInitializer != null)
            {
                FirstNeurons.Initialize(NnInitializer, index: 0);
            }
        }
    }
}
