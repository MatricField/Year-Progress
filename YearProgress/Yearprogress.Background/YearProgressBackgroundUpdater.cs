using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using YearProgress.Core;

namespace YearProgress.Background
{
    public sealed class YearProgressBackgroundUpdater:
        IBackgroundTask
    {
        private ProgressTracker Tracker;
        private uint UpdateSequalNumber = 1;

        private const string toastTag = "YearProgress_0000";
        private const string toastGroup = "YearProgress";

        public YearProgressBackgroundUpdater()
        {
            Tracker = new ProgressTracker();
            SetupUpdatableProgressBar();
        }
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //note: no async operation exists
            //var deferral = taskInstance.GetDeferral();
            Tracker.UpdateYearProgress();
            UpdateProgressBar();
            //deferral.Complete();
        }

        private void UpdateProgressBar()
        {
            var data = CreateNotificationData();
            var updateResult =
                ToastNotificationManager.CreateToastNotifier().Update(data, toastTag, toastGroup);
        }

        private void SetupUpdatableProgressBar()
        {
            // Define visual tree
            var content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                    {
                        new AdaptiveText
                        {
                            Text="Time goes by..."
                        },
                        new AdaptiveProgressBar()
                        {
                            Title = "Here Is How Much Life You Could Still Waste",
                            Value = new BindableProgressBarValue("progressValue"),
                            ValueStringOverride = new BindableString("progressString"),
                            Status = new BindableString("progressStatus")
                        }
                    }
                    }
                }
            };

            var toast = new ToastNotification(content.GetXml());

            toast.Tag = toastTag;
            toast.Group = toastGroup;

            toast.Data = CreateNotificationData();

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private NotificationData CreateNotificationData()
        {
            var data = new NotificationData()
            {
                SequenceNumber = UpdateSequalNumber++
            };
            var values = data.Values;
            values["progressValue"] = (Tracker.CurrentProgressValue / Tracker.CompleteProgressValue).ToString();
            values["progressString"] = $"{DateTime.Now.DayOfYear} / {new DateTime(DateTime.Now.Year, 12, 31).DayOfYear} days";
            values["progressStatus"] = GetStatusString();
            return data;
        }

        private string GetStatusString()
        {
            switch(DateTime.Now.Month)
            {
                case 1:
                case 2:
                case 3:
                    return "No Rush...";
                case 4:
                case 5:
                case 6:
                    return "You Should Get Started...";
                case 7:
                case 8:
                case 9:
                    return "Time Goes Short...";
                case 10:
                case 11:
                case 12:
                    return "Running Out of Time...";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
