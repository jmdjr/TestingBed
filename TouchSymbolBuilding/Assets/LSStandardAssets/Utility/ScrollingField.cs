using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingField : MonoBehaviour {
    private float FieldSize = 0;

    void Start () {
        Image image = GetComponentInChildren<Image>();

        FieldSize = ((RectTransform)image.transform).rect.height;
        Debug.Log(FieldSize);
    }

    float offset = 0;
	void Update () {
        offset += 1;

        offset = offset % FieldSize;

        transform.localPosition = new Vector3(0, -1 * (offset + 0.5f * FieldSize));
	}
}
