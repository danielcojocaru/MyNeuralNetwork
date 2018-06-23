using Auxiliar.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auxiliar.Common.Interface
{
    public interface IFunctionInitializer
    {
        ResultEnum GetResultEnum(int index);
        ErrorEvaluatorEnum GetErrorEvaluatorEnum();
    }
}
