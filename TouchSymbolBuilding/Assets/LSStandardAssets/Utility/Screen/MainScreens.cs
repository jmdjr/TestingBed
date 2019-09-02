using DigitalRuby.Tween;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Screen = LS.Screen;
/******************************************************************************/
/* This is the Main Screens object, it controls the UI aspects of the game    */
/* Any popup screen you see (wining, loading, title, level select are all     */
/* Controlled through this object. if you want to launch a screen,            */
/*     this is where you go.                                                  */
/******************************************************************************/

// Write to an additional Partial MainScreens class, filled with the screen launches.
//  they should follow the following code snippet. individual overrides of Screen can be made and used 
// as the return type for convenience.
//
//public DecisionScreen LaunchDecisionScreen()
//{
//    StartCoroutine(RunCavasFadeIn());
//    return (DecisionScreen)Launch(Decision);
//}

namespace LS
{
    public partial class MainScreens : MonoBehaviour
    {
        public static MainScreens Instance;

        [Header("References")]
        public Transform targetScreenArea;
        public Image backgroundCover;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator RunCavasFadeIn()
        {
            if (backgroundCover == null || backgroundCover.color.a >= 1.0f)
            {
                yield break;
            }

            FloatTween iTweenFade = TweenFactory.Tween(null, 0.0f, 1.0f, 0.5f, TweenScaleFunctions.Linear,
                (progress) =>
                {
                    if (backgroundCover)
                    {
                        Color c = backgroundCover.color;
                        c.a = progress.CurrentValue;
                        backgroundCover.color = c;
                    }
                });

            while (iTweenFade.State == TweenState.Running)
            {
                iTweenFade.Update(Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator RunCavasFadeOut()
        {
            if (backgroundCover == null || backgroundCover.color.a <= 0.0f)
            {
                yield break;
            }

            Color c = backgroundCover.color;

            FloatTween iTweenFadeOut = TweenFactory.Tween(null, 1.0f, 0.0f, 0.5f, TweenScaleFunctions.Linear,
                (progress) =>
                {
                    if (backgroundCover)
                    {
                        c.a = progress.CurrentValue;
                    }
                });

            while (!iTweenFadeOut.Update(Time.deltaTime))
            {
                backgroundCover.color = c;
                yield return new WaitForEndOfFrame();
            }
        }

        protected Screen Launch(Screen control)
        {
            LS.Screen instance = null;

            if (control && targetScreenArea)
            {
                instance = Instantiate(control, targetScreenArea);
                instance.gameObject.SetActive(true);
                instance.Open();
            }

            return instance;
        }
    }
}