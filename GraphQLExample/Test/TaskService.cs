﻿using GraphQLExample.Subscriptions;

namespace GraphQLExample.Test
{
    public class TaskService
    {
        private readonly List<TaskItem> tasksItems = new List<TaskItem>();
        private readonly ISubscriptionService subscriptions;

        public TaskService(ISubscriptionService subscriptions)
        {
            this.subscriptions = subscriptions;
        }

        public void Add(TaskItem taskItem)
        {
            tasksItems.Add(taskItem);

            subscriptions.PublishAsync(new TaskAdded
            {
                TaskItem = taskItem
            });
        }

        public List<TaskItem> QueryAll(string projectId)
        {
            return tasksItems.Where(x => x.ProjectId == projectId).ToList();
        }
    }
}