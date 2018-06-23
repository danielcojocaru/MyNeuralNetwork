using Auxiliar.Common.Enum;
using Auxiliar.Worker;
using Auxiliar.Wrapper;
using NeuralNetworkNew.Worker;
using NeuralNetworkNew.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    public class UiWorker
    {
        public UiForm Form { get; set; }

        public UiWorker()
        {
        }

        public void Create(UiForm uiForm)
        {
            Form = uiForm;
        }

        public void Start()
        {
            //FileWorker w = new FileWorker();
            //w.Initialize();
            //w.CreateBlackAndWhiteTxtFilesUsingNpy();

            FileWorker w = new FileWorker();
            w.Create(new WrapperFileWorker() { Problem = ProblemEnum.QuickDraw});
            w.Initialize();
            List<List<byte[]>> data = w.ReadAllFilesFromNpy();

            WrapperTrainer wrapper = new WrapperTrainer() { Data = data };

            Trainer trainer = new Trainer();
            trainer.Create(wrapper);
            trainer.Initialize();
            trainer.Process();
        }

    }
}
