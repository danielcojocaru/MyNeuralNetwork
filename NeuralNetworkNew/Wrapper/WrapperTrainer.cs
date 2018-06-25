using NeuralNetworkNew.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Wrapper
{
    public class WrapperTrainer
    {
        public NeuralNetworkCls Nn { get; set; }
        public List<List<byte[]>> Data { get; set; }
        public List<List<byte[]>> TestData { get; set; }
    }
}
