using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Abstraction;
using Hangfire;

namespace TestSelfHostedApp.Services
{
    public class MessageBus: IMessageBus
    {
        public string Run(Expression<Action> expression)
        {
            var jobId = BackgroundJob.Enqueue(expression);
            return jobId;
        }
        public string Run(Expression<Func<Task>> expression)
        {
            var jobId = BackgroundJob.Enqueue(expression);
            return jobId;
        }
        public string Run<T>(Expression<Action<T>> expression) where T:class
        {
            var jobId = BackgroundJob.Enqueue<T>(expression);
            return jobId;
        }
        public string Run<T>(Expression<Func<T, Task>> expression) where T : class
        {
            var jobId = BackgroundJob.Enqueue<T>(expression);
            return jobId;
        }


        public void Recurring(Expression<Action> expression, string cron)
        {
            RecurringJob.AddOrUpdate(expression, cron);
        }
        public void Recurring(Expression<Func<Task>> expression, string cron)
        {
            RecurringJob.AddOrUpdate(expression, cron);
        }
        public void Recurring(Expression<Action> expression, string cron,TimeZoneInfo timeZoneInfo)
        {
            RecurringJob.AddOrUpdate(expression, cron, timeZoneInfo);
        }
        public void Recurring(Expression<Func<Task>> expression, string cron, TimeZoneInfo timeZoneInfo)
        {
           RecurringJob.AddOrUpdate(expression, cron, timeZoneInfo);
        }

        public void Recurring<T>(Expression<Action<T>> expression, string cron) where T : class
        {
            RecurringJob.AddOrUpdate<T>(expression, cron);
        }
        public void Recurring<T>(Expression<Func<T,Task>> expression, string cron) where T : class
        {
            RecurringJob.AddOrUpdate<T>(expression, cron);
        }
        public void Recurring<T>(Expression<Action<T>> expression, string cron, TimeZoneInfo timeZoneInfo) where T : class
        {
            RecurringJob.AddOrUpdate<T>(expression, cron, timeZoneInfo);
        }
        public void Recurring<T>(Expression<Func<T,Task>> expression, string cron, TimeZoneInfo timeZoneInfo) where T : class
        {
            RecurringJob.AddOrUpdate<T>(expression, cron, timeZoneInfo);
        }

        public string ScheduleFromNow<T>(Expression<Action<T>> expression, TimeSpan timeSpan) where T : class
        {
            var jobId = BackgroundJob.Schedule<T>(expression, timeSpan);
            return jobId;
        }
        public string ScheduleInSpecificTime<T>(Expression<Action<T>> expression, DateTimeOffset dateTimeOffset) where T : class
        {
            var jobId = BackgroundJob.Schedule<T>(expression, dateTimeOffset);
            return jobId;
        }

        public string ScheduleFromNow<T>(Expression<Func<T,Task>> expression, TimeSpan timeSpan) where T : class
        {
            var jobId = BackgroundJob.Schedule<T>(expression, timeSpan);
            return jobId;
        }
        public string ScheduleInSpecificTime<T>(Expression<Func<T, Task>> expression, DateTimeOffset dateTimeOffset) where T : class
        {
            var jobId = BackgroundJob.Schedule<T>(expression, dateTimeOffset);
            return jobId;
        }
        public string ScheduleFromNow(Expression<Action> expression, TimeSpan timeSpan) 
        {
            var jobId = BackgroundJob.Schedule(expression, timeSpan);
            return jobId;
        }
        public string ScheduleInSpecificTime(Expression<Action> expression, DateTimeOffset dateTimeOffset) 
        {
            var jobId = BackgroundJob.Schedule(expression, dateTimeOffset);
            return jobId;
        }
        public string ScheduleFromNow(Expression<Func<Task>> expression, TimeSpan timeSpan)
        {
            var jobId = BackgroundJob.Schedule(expression, timeSpan);
            return jobId;
        }
        public string ScheduleInSpecificTime(Expression<Func<Task>> expression, DateTimeOffset dateTimeOffset)
        {
            var jobId = BackgroundJob.Schedule(expression, dateTimeOffset);
            return jobId;
        }
        /// <summary>
        /// Continuations are executed when its parent job has been finished (OnlyOnSucceededState).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parentJobId"></param>
        /// <returns></returns>
        public string ContinueJobWith<T>(Expression<Action<T>> expression, string parentJobId) where T : class
        {
            return BackgroundJob.ContinueJobWith(parentJobId, expression,JobContinuationOptions.OnlyOnSucceededState);
        }
        public string ContinueJobWith<T>(Expression<Func<T, Task>> expression, string parentJobId) where T : class
        {
            return BackgroundJob.ContinueJobWith(parentJobId, expression, JobContinuationOptions.OnlyOnSucceededState);
        }
        public string ContinueJobWith<T>(Expression<Action> expression, string parentJobId) where T : class
        {
            return BackgroundJob.ContinueJobWith(parentJobId, expression, JobContinuationOptions.OnlyOnSucceededState);
        }
        public string ContinueJobWith<T>(Expression<Func<Task>> expression, string parentJobId) where T : class
        {
            return BackgroundJob.ContinueJobWith(parentJobId, expression, JobContinuationOptions.OnlyOnSucceededState);
        }

    }
}
