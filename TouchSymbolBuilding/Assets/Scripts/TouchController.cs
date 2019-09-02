using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    List<List<Vector3>> SymbolShapes = new List<List<Vector3>>();

    List<Vector3> SymbolShape = new List<Vector3>();
    bool isRecording = false;
    public LineRenderer LineRender;

    // Start is called before the first frame update
    void Start()
    {
        SymbolShapes = SLUtility.IO.Load<List<List<Vector3>>>("Test", FileParsingFlags.JSON, true);

        if(SymbolShapes == null)
        {
            SymbolShapes = new List<List<Vector3>>();
        }

        StartRecording();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecording)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = transform.position.z;
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                if(SymbolShape.Count() == 0 || SymbolShape.Last() != position)
                {
                    SymbolShape.Add(position);
                    LineRender.positionCount = SymbolShape.Count();
                    LineRender.SetPositions(SymbolShape.ToArray());
                }
            }
            else if(SymbolShape.Count > 1)
            {
                StopRecording();
                SymbolShapes.Add(SymbolShape);
                SLUtility.IO.Save(SymbolShapes, "Test", FileParsingFlags.JSON);
            }
        }
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void AddNewDraw()
    {

    }
}
