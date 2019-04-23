using SA.CrossPlatform.App;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using SA.iOS.UIKit;

public class UM_WelcomeController : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonsPanel = null;
    [SerializeField] private GameObject m_FeatureViewport = null;
    
    [SerializeField] private Text m_LocaleLabel = null;

    private Scene m_currentlyFeaturedScene;

    private void Start()
    {
       SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
       var buttons =  m_ButtonsPanel.GetComponentsInChildren<Button>();
       foreach (var button in buttons)
       {
           var currentButton = button;
           button.onClick.AddListener(() =>
           {
               var sceneLink = currentButton.GetComponent<UM_SceneLink>();
               if (sceneLink != null)
               {
                   LoadScene(sceneLink.SceneName);
               }
           });
       }

       InitMainScreenServices();
    }

    private void InitMainScreenServices()
    {
        LocaleInfo();
        UM_LocalNotificationsExample.SubscribeToTheNotificationEvents();
    }

    private void LocaleInfo()
    {
        var currentLocale = UM_Locale.GetCurrentLocale();
        m_LocaleLabel.text =  string.Format("<b>Locale Info:</b> " +
                             "\n CountryCode: {0} " +
                             "\n LanguageCode: {1}" +
                             "\n CurrencyCode: {2}" +
                             "\n CurrencySymbol: {3}",
            currentLocale.CountryCode,
            currentLocale.LanguageCode,
            currentLocale.CurrencyCode,
            currentLocale.CurrencySymbol);
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_currentlyFeaturedScene = scene;
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            if (rootGameObject.GetComponent<Canvas>() == null)
            {
                Destroy(rootGameObject);
            }
            else
            {
                var canvasRect = rootGameObject.GetComponent<RectTransform>();
                rootGameObject.transform.SetParent(m_FeatureViewport.transform);
                canvasRect.anchorMin = new Vector2(0, 0);
                canvasRect.anchorMax = new Vector2(1, 1);
                
                canvasRect.transform.localScale = Vector3.one;
                canvasRect.anchoredPosition  = Vector2.zero;

                canvasRect.offsetMin = Vector2.zero;
                canvasRect.offsetMax = Vector2.zero;

            }
        }
    }

    private void LoadScene(string sceneName)
    {
        if (sceneName.Equals(m_currentlyFeaturedScene.name))
        {
            return;
        }

        m_FeatureViewport.Clear();
        if (m_currentlyFeaturedScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(m_currentlyFeaturedScene);
        }
       
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    
    
}



