using AForge.Neuro.Learning;
using BDLib.NeuralNetworks;
using BDLib.BDLibInfo;

namespace BDLib.NeuralNetworks.Learning
{
    public class SOMTrainer
    {
        public NeuralNetwork Train(SOMTrainerArgs X)
        {
            if (!Info.Moduls.Contains("NeuralNetworks/Learning/SOMLearnig.cs"))
                Info.Moduls.Add("NeuralNetworks/Learning/SOMLearnig.cs");

            SOMLearning Trainer =
                new SOMLearning(X.NetworkToTrain.DNetwork);

            Trainer.LearningRadius = (X.LearningRadius <= 0) ? (5) : (X.LearningRadius);
            Trainer.LearningRate = (X.LearningRate <= 0) ? (0.01) : (X.LearningRate);
            uint tick = Settings.LearningUpdateTick;
            while(true)
            {
                double Error = Trainer.RunEpoch(X.Inputs);
                if (Error <= X.WantedERROR) break;
                if (tick == 0)
                {
                    Settings.OnUpdateEvent(this, new LearningUpdateArgs()
                    {
                        ERROR = Error,
                        Inputs = X.Inputs,
                        Outputs = null,
                        NetworkState = X.NetworkToTrain,
                    });
                }
                else tick--;
            }


            return X.NetworkToTrain;
        }
    }
}
