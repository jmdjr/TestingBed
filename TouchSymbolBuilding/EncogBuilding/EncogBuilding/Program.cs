using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace EncogBuilding
{
    class Program
    {
        static string fileLocation = @"C:\Users\jmdco\AppData\LocalLow\Lost Seraph, LLC\TouchSymbolBuilding\Test.json";

        static void Main(string[] args)
        {
            List<SymbolShape> TrainingSet = null;

            using (StreamReader file = File.OpenText(fileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                TrainingSet = (List<SymbolShape>)serializer.Deserialize(file, typeof(List<SymbolShape>));
            }

            Execute();

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
        /// <summary>
        /// Input for the XOR function.
        /// </summary>
        public static double[][] XORInput = {
            new[] {0.0, 0.0},
            new[] {1.0, 0.0},
            new[] {0.0, 1.0},
            new[] {0.80, 0.40}
        };

        /// <summary>
        /// Ideal output for the XOR function.
        /// </summary>
        public static double[][] XORIdeal = {
            new[] {0.0},
            new[] {0.1},
            new[] {0.2},
            new[] {0.3}
        };

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="app">Holds arguments and other info.</param>
        public static void Execute()
        {
            // create a neural network, without using a factory
            var network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 3));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            // create training data
            IMLDataSet trainingSet = new BasicMLDataSet(XORInput, XORIdeal);

            // train the neural network
            IMLTrain train = new ResilientPropagation(network, trainingSet);

            int epoch = 1;

            do
            {
                train.Iteration();
                Console.WriteLine(@"Epoch #" + epoch + @" Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.01);

            // test the neural network
            Console.WriteLine(@"Neural Network Results:");
            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + @"," + pair.Input[1]
                                  + @", actual=" + output[0] + @",ideal=" + pair.Ideal[0]);
            }
        }
    }

    [Serializable]
    public class SymbolShape
    {
        [JsonProperty]
        List<Vector3> shapePoints = new List<Vector3>();
    }

    class Vector3
    {
        //
        // Summary:
        //     X component of the vector.
        public float x = 0;
        //
        // Summary:
        //     Y component of the vector.
        public float y = 0;
        //
        // Summary:
        //     Z component of the vector.
        public float z = 0;
    }
}
