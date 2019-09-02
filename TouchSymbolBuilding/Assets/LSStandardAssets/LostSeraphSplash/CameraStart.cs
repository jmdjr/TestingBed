using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraStart : MonoBehaviour
{
    public int SceneIndex = 1;
    public float TimeUntilClickAway = 2;
    public float TotalDelay = 5;
    public float TransitionTime = 2;

    public Button ClickAwayButton;
    public Transform target;
    private float delay = 0;

    public float step; 

    private void Start()
    {
        ClickAwayButton.onClick.AddListener(() => {
            if(delay >= TimeUntilClickAway)
            {
                TransitionIntoGame();
            }
        });

        step = (target.position - transform.position).magnitude / TransitionTime;


        //yield return new WaitUntil(() => AudioDefinitions.IsReady);

        //AudioMaster.PlaySoundEffect("Thunder");
    }

    void Update()
    {
        delay += Time.deltaTime;
        float takeStep = step * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, takeStep);

        if(delay >= TotalDelay)
        {
            TransitionIntoGame();
        }
    }

    void TransitionIntoGame()
    {
        //AudioMaster.StopAll();
        SceneManager.LoadScene(SceneIndex);
    }
}