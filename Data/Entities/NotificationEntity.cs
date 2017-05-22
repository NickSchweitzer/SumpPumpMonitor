using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class NotificationEntity : TableEntity
    {
        public NotificationEntity()
        {
            AlertTypes = new List<AlertType>();
            NotificationTypes = new List<NotificationType>();
        }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string NotificationMethods
        {
            get { return string.Join("|", NotificationTypes); }
            set
            {
                // I'm sure there is a cooler LINQy way to do this
                string[] methods = value.Split("|".ToCharArray());
                NotificationTypes.Clear();
                foreach (var method in methods)
                    NotificationTypes.Add((NotificationType)Enum.Parse(typeof(NotificationType), method));
            }
        }

        public string Alerts
        {
            get { return string.Join("|", AlertTypes); }
            set
            {
                // I'm sure there is a cooler LINQy way to do this
                string[] alerts = value.Split("|".ToCharArray());
                AlertTypes.Clear();
                foreach (var alert in alerts)
                    AlertTypes.Add((AlertType)Enum.Parse(typeof(AlertType), alert));
            }
        }

        [IgnoreProperty]
        public List<AlertType> AlertTypes { get; set; }

        [IgnoreProperty]
        public List<NotificationType> NotificationTypes{ get; set; }
    }

    public enum NotificationType
    {
        Email,
        Phone
    }
}