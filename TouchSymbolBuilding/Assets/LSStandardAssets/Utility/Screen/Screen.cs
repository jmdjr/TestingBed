using DigitalRuby.Tween;
using System.Collections;
using UnityEngine;

namespace LS
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Screen : MonoBehaviour
    {
        private CanvasGroup alpha;
        public float transitionTime = 1;
        public AudioClip BackgroundMusic = null;

        protected virtual void OnOpened() { }

        protected virtual void OnClosed() { }

        public void Close()
        {
            StartCoroutine(EndScreen());
        }

        protected void PlayBackgroundMusic()
        {
            if (BackgroundMusic && !MainAudio.IsPlayingClip(BackgroundMusic))
            {
                MainAudio.PlayWithFade(BackgroundMusic);
            }
        }

        public void Open()
        {
            PlayBackgroundMusic();
            StartCoroutine(StartScreen());
        }

        private void Awake()
        {
            alpha = GetComponent<CanvasGroup>();
        }

        private float newAlpha = 0;

        IEnumerator EndScreen()
        {
            if (alpha)
            {
                alpha.blocksRaycasts = false;

                var iTween = TweenFactory.Tween(null, 1, 0, transitionTime, TweenScaleFunctions.QuadraticEaseIn,
                    (progress) =>
                    {
                        newAlpha = progress.CurrentValue;
                    });

                while (!iTween.Update(Time.deltaTime))
                {
                    alpha.alpha = newAlpha;
                    yield return new WaitForEndOfFrame();
                }

                alpha.alpha = 0;

                OnClosed();
            }

            Destroy(gameObject);
        }

        IEnumerator StartScreen()
        {
            if (alpha)
            {
                alpha.blocksRaycasts = false;

                var iTween = TweenFactory.Tween(null, 0, 1, transitionTime, TweenScaleFunctions.QuadraticEaseIn,
                    (progress) =>
                    {
                        newAlpha = progress.CurrentValue;
                    });

                while (!iTween.Update(Time.deltaTime))
                {
                    alpha.alpha = newAlpha;
                    yield return new WaitForEndOfFrame();
                }

                alpha.alpha = 1;
                alpha.blocksRaycasts = true;

                OnOpened();
            }
        }
    }
}
