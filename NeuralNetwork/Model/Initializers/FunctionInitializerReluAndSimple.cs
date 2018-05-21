using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork.Auxiliar.Enum;

namespace NeuralNetwork.Model.Initializers
{
    public class FunctionInitializerReluAndSimple : IFunctionInitializer
    {
        public ErrorEvaluatorEnum GetErrorEvaluatorEnum()
        {
            return ErrorEvaluatorEnum.Simple;
        }

        public ResultEnum GetResultEnum(int index)
        {
            return ResultEnum.Relu;
        }
    }
}
