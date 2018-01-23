using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace YearProgress.Background
{
    public static class YearProgressBackgrroundUpdaterRegistrationManager
    {
        private const string GROUP_ID = "890D5E95-5A51-4994-B377-610F79EAA9FF";
        private const string FRIENDLY_NAME = "YearProgressBackgroundUpdater";
        private const string TASK_NAME = "YearProgressBackgroundUpdater";

        public static BackgroundTaskRegistrationGroup RegistrationGroup { get; }

        static YearProgressBackgrroundUpdaterRegistrationManager()
        {
            if (RegistrationGroup == null)
            {
                RegistrationGroup =
                    BackgroundTaskRegistration.GetTaskGroup(GROUP_ID) ??
                       new BackgroundTaskRegistrationGroup(GROUP_ID, FRIENDLY_NAME);
            }
        }

        public static IAsyncOperation<BackgroundTaskRegistration> RegisterTasks(string taskSuffix, IBackgroundTrigger trigger)
        {
            return DoRegisterTasks(taskSuffix, trigger).AsAsyncOperation();
        }

        private static async Task<BackgroundTaskRegistration> DoRegisterTasks(string taskSuffix, IBackgroundTrigger trigger)
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            var taskName = TASK_NAME + taskSuffix;
            switch (backgroundAccessStatus)
            {
                case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                case BackgroundAccessStatus.AlwaysAllowed:
                    foreach (var task in RegistrationGroup.AllTasks)
                    {
                        if (task.Value.Name == taskName)
                        {
                            task.Value.Unregister(true);
                            break;
                        }
                    }
                    var taskBuilder = new BackgroundTaskBuilder();
                    taskBuilder.Name = taskName;
                    taskBuilder.TaskGroup = RegistrationGroup;
                    taskBuilder.TaskEntryPoint = typeof(YearProgressBackgroundUpdater).FullName;
                    taskBuilder.SetTrigger(trigger);
                    return taskBuilder.Register();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
