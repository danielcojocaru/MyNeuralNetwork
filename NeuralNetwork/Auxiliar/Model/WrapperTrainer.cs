using NeuralNetwork.Auxiliar.Interface;
using NeuralNetwork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Auxiliar.Model
{
    public class WrapperTrainer
    {
        public NeuralNetworkCls Nn { get; set; }
        public List<List<byte[]>> Data { get; set; }
    }
}
