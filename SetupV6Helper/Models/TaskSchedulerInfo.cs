using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;

namespace SetupV6Helper.Models
{
    public sealed class TaskSchedulerInfo
    {
        private static readonly string RXM_HELPER_SCH_TASKS = "RXM Helper";

        public string Name { get; set; }

        public TaskSchedulerInfo(string name)
        {
            Name = name;
        }

        public static bool ScheduledTaskCreated()
        {
            ITaskService taskService = new TaskScheduler.TaskScheduler();
            taskService.Connect();

            try
            {
                if (EnumAllRXMTasks(taskService.GetFolder("\\")).Count.Equals(2))
                {
                    return true;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            return false;
        }


        public static List<TaskSchedulerInfo> EnumAllRXMTasks(ITaskFolder itf)
        {
            var tasks = new List<TaskSchedulerInfo>();
            IRegisteredTaskCollection taskCol = itf.GetTasks(0);
            foreach(IRegisteredTask ts in taskCol)
            {
                tasks.Add(new TaskSchedulerInfo(ts.Name));
            }

            return tasks.Where(p => p.Name.StartsWith(RXM_HELPER_SCH_TASKS)).ToList();
        }
    }
}
