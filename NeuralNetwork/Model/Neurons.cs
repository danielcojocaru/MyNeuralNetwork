using MathNet.Numerics.LinearAlgebra;
using NeuralNetwork.Auxiliar;
using NeuralNetwork.Auxiliar.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Auxiliar.Interface;

namespace NeuralNetwork.Model
{
    public class Neurons
    {
        public NeuralNetworkCls Parent { get; set; }

        public ResultEnum ResultType { get; set; }

        public Matrix<double> I { get; set; }
        public Matrix<double> O { get; set; }

        public Synapses PrevSy { get; set; }
        public Synapses NextSy { get; set; }

        public void Create(NeuralNetworkCls parent, int[] nrOfNeuronsList, int index)
        {
            Create(parent, nrOfNeuronsList, index, null);
        }

        public void Create(NeuralNetworkCls parent, int[] nrOfNeuronsList, int index, Synapses prevSy)
        {
            Parent = parent; 
            PrevSy = prevSy; // could be null

            int nrOfNeurons = nrOfNeuronsList[index];
            I = Matrix<double>.Build.Dense(nrOfNeurons, 1);
            O = Matrix<double>.Build.Dense(nrOfNeurons, 1);

            ResultType = (ResultEnum)index;

            if (index < nrOfNeuronsList.Length - 1)
            {
                NextSy = new Synapses();
                NextSy.Create(Parent, nrOfNeuronsList, index, this);
            }
        }

        public void Initialize(INeuralNetworkInitializer nnInitializer, int index)
        {
            if (NextSy != null)
            {
                NextSy.Initialize(nnInitializer, index);
            }
        }
    }
}
