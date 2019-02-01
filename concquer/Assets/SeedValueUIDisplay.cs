using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedValueUIDisplay : MonoBehaviour
{
    public Text text;

    public void Update()
    {
        if(text)
        {
            text.text = $"SeedValue: { Map_Randomizer.GenerationSeed }";
        }
    }
}
