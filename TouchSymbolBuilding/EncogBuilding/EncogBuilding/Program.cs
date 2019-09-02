using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace EncogBuilding
{
    class Program
    {
        static string SerializedData = "";

        static void Main(string[] args)
        {
            List<List<Vector3>> Points = JsonConvert.DeserializeObject<List<List<Vector3>>>(SerializedData);







            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
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
