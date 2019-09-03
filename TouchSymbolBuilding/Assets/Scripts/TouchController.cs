using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public static TouchController Instance { get; private set; } = null;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SymbolShapes = SLUtility.IO.Load<List<SymbolShape>>("Test", FileParsingFlags.JSON, true);

        if (SymbolShapes == null)
        {
            SymbolShapes = new List<SymbolShape>();
        }

        TotalSymbols = SymbolShapes.Count;
    }

    List<SymbolShape> SymbolShapes = new List<SymbolShape>();

    public int TotalSymbols { get; private set; } = 0;

    List<Vector3> shape = new List<Vector3>();
    bool isRecording = false;
    public LineRenderer LineRender;

    // Start is called before the first frame update
    void Start()
    {

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
                if(shape.Count() == 0 || shape.Last() != position)
                {
                    shape.Add(position);
                    LineRender.positionCount = shape.Count();
                    LineRender.SetPositions(shape.ToArray());
                }
            }
            else if(shape.Count > 1)
            {
                StopRecording();
                var symbol = new SymbolShape();
                symbol.SetPoints(shape);
                SymbolShapes.Add(symbol);

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

    public void DrawLoadedShape(int index)
    {
        if(SymbolShapes.Count <= 0 || index >= SymbolShapes.Count)
        {
            return;
        }
    }
}

[Serializable]
public class SymbolShape
{
    [JsonProperty]
    List<Vector3> shapePoints = new List<Vector3>();

    public void SetPoints(List<Vector3> points)
    {
        shapePoints = points;
        NormalizeShape();
    }

    private void NormalizeShape()
    {
        Vector3 Min = Vector3.zero;
        Vector3 Max = Vector3.zero;

    }
}
