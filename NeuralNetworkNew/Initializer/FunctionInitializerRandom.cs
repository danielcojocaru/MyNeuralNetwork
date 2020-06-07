using Auxiliar.Common.Enum;
using Auxiliar.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Initializer
{
    [Serializable]
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
            else if (index == 1 || index == 2)
            {
                return ResultEnum.Relu;
                //return ResultEnum.Sigmoid;
            }
            else
            {
                return ResultEnum.Softmax;
            }
        }
    }
}
