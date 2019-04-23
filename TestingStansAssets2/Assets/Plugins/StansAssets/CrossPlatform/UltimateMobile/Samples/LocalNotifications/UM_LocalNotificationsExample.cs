using UnityEngine;
using SA.CrossPlatform.UI;
using UnityEngine.UI;
using SA.CrossPlatform.Notifications;

public class UM_LocalNotificationsExample : MonoBehaviour
{
#pragma warning disable 649
    [Header("Unified API Buttons")] 
    [SerializeField] private Button m_Create5SecNotification = null;
#pragma warning restore 649
    
    private void Start()
    {
        var client = UM_NotificationCenter.Client;

        m_Create5SecNotification.onClick.AddListener(() =>
        {
            
            var content = new UM_Notification();
            
            content.SetTitle("Title");
            content.SetBody("Body message");
            content.SetSmallIconName("myIcon.png");
            content.SetSoundName("mySound.wav");

            //Get unique request id, if notification will be posted with the same request id old one will be replaced with the new one
            var requestId = 1; 
            //2 seconds
            var trigger = new UM_TimeIntervalNotificationTrigger(2);
            var request = new UM_NotificationRequest(requestId, content, trigger);
            client.AddNotificationRequest(request, (result) => {
                if(result.IsSucceeded) {
                    UM_DialogsUtility.ShowMessage("Succeeded", "Notification was successfully scheduled.");
                } else {
                    UM_DialogsUtility.ShowMessage("Failed", result.Error.FullMessage);
                }
            });
        });
    }

    public static void SubscribeToTheNotificationEvents()
    {
        var client = UM_NotificationCenter.Client;
        var startRequest = client.LastOpenedNotification;
        if(startRequest != null) {
            UM_DialogsUtility.ShowMessage("Launched via Notification", GetNotificationInfo(startRequest));
            //if this isn't null on your app launch, means user launched your app by clicking on notification icon
        }
        
        client.OnNotificationReceived.AddListener(request => {
            //Notification was received while app is running
            UM_DialogsUtility.ShowMessage("Notification Received", GetNotificationInfo(request));
        });
        
        client.OnNotificationClick.AddListener(request => {
            //User clicked on notification while app is running
            UM_DialogsUtility.ShowMessage("Restored from background via Notification", GetNotificationInfo(request));
        });
    }

    private static string GetNotificationInfo(UM_NotificationRequest request)
    {
        return string.Format("request.Identifier: {0} \n request.Content.Title: {1}",
            request.Identifier,
            request.Content.Title);
    }
}
