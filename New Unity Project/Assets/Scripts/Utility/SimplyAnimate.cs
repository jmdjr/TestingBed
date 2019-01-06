using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplyAnimate : MonoBehaviour
{
    private static float DEFAULT_FRAMERATE = 30;

    public bool Repeat = true;
    public Sprite[] Frames;

    [Header("Delay")]
    public float InitialDelay = 0;
    public bool randomizeInitialDelay = false;

    [Header("Frame Rates")]
    [Range(1, 60)]
    public float DefaultFrameRate = DEFAULT_FRAMERATE;

    [Range(1, 60)]
    [Tooltip("Each value will be pared with its accompanying Frame. "
        + "\nIf there are more frames than tweaks, then DefaultFrameRate is applied."
        + "\nAll extra tweaks are ignored. ")]

    public float[] TweakRate;

    [Header("Random FrameRate")]
    public bool randomFrameRate = false;
    public float minRandRate = 0;
    public float maxRandRate = 60;

    private float setFrameRate = 0;

    private float InvertedFrameRate { get { return 1 / setFrameRate; } }

    private Image target;
    private SpriteRenderer target2;
    private int frameIndex = 0;
    private float currentTime;
    private bool ableFramed = false;

    private void SetSprite(Sprite sprite)
    {
        if (target) target.sprite = sprite;
        if (target2) target2.sprite = sprite;
    }

    public void SetFirstFrameGraphic()
    {
        if (Frames != null && Frames.Length > 0)
        {
            SetSprite(Frames[0]);
        }
    }

    // Use this for initialization
    private void Start()
    {
        if (randomizeInitialDelay) InitialDelay = UnityEngine.Random.value;

        target = GetComponent<Image>();
        target2 = GetComponent<SpriteRenderer>();
        setFrameRate = DefaultFrameRate;
        SetDetails();

        StartCoroutine(AnimateMe());
    }

    public void StopPlay()
    {
        ableFramed = false;
    }

    private void SetDetails()
    {
        if (Frames != null && Frames.Length > 0)
        {
            ableFramed = true;
            SetSprite(Frames[frameIndex]);
        }
    }

    public void StartPlay()
    {
        ableFramed = true;
    }

    private void NextFrame()
    {
        frameIndex = (++frameIndex) % Frames.Length;
        SetSprite(Frames[frameIndex]);
    }

    private IEnumerator AnimateMe()
    {
        if (ableFramed)
        {
            do
            {
                currentTime += Time.deltaTime;
                if (InitialDelay > 0 && currentTime < InitialDelay)
                {
                    continue;
                }
                else if (InitialDelay != 0)
                {
                    currentTime -= InitialDelay;
                    InitialDelay = 0;
                }

                if (currentTime >= InvertedFrameRate)
                {
                    currentTime -= InvertedFrameRate;
                    NextFrame();

                    if (randomFrameRate)
                    {
                        setFrameRate = UnityEngine.Random.Range(minRandRate, maxRandRate);
                    }
                    else if (TweakRate.Length > 0 && frameIndex < TweakRate.Length)
                    {
                        setFrameRate = TweakRate[frameIndex];
                    }

                    if (setFrameRate == 0)
                    {
                        setFrameRate = DefaultFrameRate;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            while (Repeat && ableFramed);
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void LoadFrames(Sprite[] frames, float initialDelay = 0, bool randomizeFramerate = false)
    {
        Frames = frames;
        InitialDelay = initialDelay;
        randomFrameRate = randomizeFramerate;
        SetDetails();
    }
}
