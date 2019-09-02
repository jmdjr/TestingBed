using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenLink : MonoBehaviour {

    public string url;

	// Use this for initialization
	void Start () {
        Button button = gameObject.GetComponent<Button>();
        if(button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                Application.OpenURL(url);
            });
        }
	}
}
