using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollerSelectorItem : MonoBehaviour {

    public Text NameLabel;
    public Image Icon;

    [HideInInspector]
    public string Name;

    [HideInInspector]
    public string Data;

    public void Start()
    {   
    }

    public ScrollerSelectorItem Set(string name, string data, Sprite sprite)
    {
        Name = name;
        Data = data;

        if (NameLabel) NameLabel.text = name;
        if (Icon) Icon.sprite = sprite;

        return this;
    }
}
