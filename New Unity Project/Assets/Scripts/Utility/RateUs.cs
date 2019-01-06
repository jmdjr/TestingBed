using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RateUs : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                Application.OpenURL("itms-apps://itunes.apple.com/la/app/i-d-l-e/id1398120027");
                //NativeReviewRequest.RequestReview();
            });
        }
    }
}
