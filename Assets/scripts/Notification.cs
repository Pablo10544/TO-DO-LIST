using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class Notification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateNotificationChannel();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateNotificationChannel() {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notification Channel",
            Importance = Importance.High,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    public void sendNotifications(string Title, string description,DateTime time) {
        var notification = new AndroidNotification();
        notification.Title = Title;
        notification.Text = description;
        notification.FireTime = time;

       var id= AndroidNotificationCenter.SendNotification(notification, "channel_id");
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id)==NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelNotification(id);
             AndroidNotificationCenter.SendNotification(notification, "channel_id");

        }
    }
}
