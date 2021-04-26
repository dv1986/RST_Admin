using ModelNotification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceNotification
{
    public interface INotificationService
    {
        bool AddNotification(Notification request);

        IList<Notification> UpdateNotification(List<Notification> tasks);

        IList<Notification> DeleteNotification(List<Notification> tasks);

        List<Notification> GetNotification(string QueryConditionPartParam);
        List<Notification> GetApprovedNotification();
    }
}
