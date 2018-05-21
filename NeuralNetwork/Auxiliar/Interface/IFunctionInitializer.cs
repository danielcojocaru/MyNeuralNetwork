using NeuralNetwork.Auxiliar.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Auxiliar.Interface
{
    public interface IFunctionInitializer
    {
        ResultEnum GetResultEnum(int index);
        ErrorEvaluatorEnum GetErrorEvaluatorEnum();
    }
}
