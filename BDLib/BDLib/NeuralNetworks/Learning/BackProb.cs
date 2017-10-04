using AForge.Neuro.Learning;
using AForge.Neuro;
using BDLib.NeuralNetworks;
using BDLib.BDLibInfo;

namespace BDLib.NeuralNetworks.Learning
{
    public class BackProb
    {
        public NeuralNetwork Train(BackProbArgs X)
        {
            if (!Info.Moduls.Contains("NeuralNetworks/Learning/BackProb.cs"))
                Info.Moduls.Add("NeuralNetworks/Learning/BackProb.cs");

            BackPropagationLearning Trainer
                = new BackPropagationLearning(X.NetworkToTrain.ANetwork);

            Trainer.Momentum     = (X.Momentem <= 0)     ? (3)    : (X.Momentem);
            Trainer.LearningRate = (X.LearningRate <= 0) ? (0.01) : (X.LearningRate);
            uint tick = Settings.LearningUpdateTick;
            while (true)
            {
                double Error = Trainer.RunEpoch(X.Inputs, X.Outputs);
                if (Error <= X.WantedERROR) break;
                if (tick == 0)
                {
                    Settings.OnUpdateEvent(this, new LearningUpdateArgs()
                    {
                        ERROR = Error,
                        Inputs = X.Inputs,
                        Outputs = X.Outputs,
                        NetworkState = X.NetworkToTrain,
                    });
                    tick = Settings.LearningUpdateTick;
                }
                else tick--;
            }
            
            return X.NetworkToTrain;
        }
    }
}
