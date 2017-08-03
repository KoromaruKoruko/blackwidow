using AForge.Neuro;

namespace BDLib.NeuralNetworks
{
    public delegate void LearningUpdateEvent(object Sender, LearningUpdateArgs Args);
    public struct LearningUpdateArgs
    {
        public NeuralNetwork NetworkState;
        public double ERROR;
        public double[][] Inputs;
        public double[][] Outputs;
    }
    public struct NeuralNetwork
    {
        public ActivationNetwork ANetwork;
        public DistanceNetwork DNetwork;
        public bool IsANetworkSet { get { return ANetwork != null; } }
        public bool IsDNetwokrSet { get { return DNetwork != null; } }
    }
    public struct BackProbArgs
    {
        public NeuralNetwork NetworkToTrain;
        public double[][] Inputs;
        public double[][] Outputs;
        public double WantedERROR;
        public double LearningRate;
        public double Momentem;
    }
    public struct SOMTrainerArgs
    {
        public NeuralNetwork NetworkToTrain;
        public double[][] Inputs;
        public double WantedERROR;
        public double LearningRate;
        public double LearningRadius;
    }
}

namespace BDLib.NeuralNetworks.Learning
{
    public static class Settings
    {
        public static uint LearningUpdateTick = 1000;
        public static event LearningUpdateEvent UpdateEvent;
        internal static void OnUpdateEvent(object s, LearningUpdateArgs e)
        {
            if (UpdateEvent != null)
                UpdateEvent.Invoke(s, e);
        }
    }
}