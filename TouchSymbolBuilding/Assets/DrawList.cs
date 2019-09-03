using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawList : MonoBehaviour
{
    public Button template = null;
    public Transform TargetList = null;

    private List<Button> spawnedButtons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        int buttonsToMake = TouchController.Instance.TotalSymbols;
    }

    public void LoadExsistingSymbols(int totalShapes)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
