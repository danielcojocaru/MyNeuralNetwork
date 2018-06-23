using Auxiliar.Common.Enum;
using Auxiliar.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkNew.Initializer
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
