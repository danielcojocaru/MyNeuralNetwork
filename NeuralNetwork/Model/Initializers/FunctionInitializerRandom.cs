using NeuralNetwork.Auxiliar.Enum;
using NeuralNetwork.Auxiliar.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Model.Initializers
{
    public class FunctionInitializerRandom : IFunctionInitializer
    {
        public ErrorEvaluatorEnum GetErrorEvaluatorEnum()
        {
            return ErrorEvaluatorEnum.Simple;
        }

        public ResultEnum GetResultEnum(int index)
        {
            if (index == 0)
            {
                return ResultEnum.Simple;
            }
            else if(index == 1)
            {
                return ResultEnum.Sigmoid;
            }
            else
            {
                return ResultEnum.Sigmoid;
            }
        }
    }
}
