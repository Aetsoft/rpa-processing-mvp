using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstraction
{
    public interface IMessageBus
    {
        /// <summary>
        /// Fire-and-forget jobs are executed only once and almost immediately after creation.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string Run(Expression<Action> expression);
        /// <summary>
        /// Fire-and-forget jobs are executed only once and almost immediately after creation.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string Run(Expression<Func<Task>> expression);
        /// <summary>
        /// Fire-and-forget jobs are executed only once and almost immediately after creation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        string Run<T>(Expression<Action<T>> expression) where T : class;
        /// <summary>
        /// Fire-and-forget jobs are executed only once and almost immediately after creation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        string Run<T>(Expression<Func<T, Task>> expression) where T : class;
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        void Recurring(Expression<Action> expression, string cron);
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        void Recurring(Expression<Func<Task>> expression, string cron);
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        /// <param name="timeZoneInfo"></param>
        void Recurring(Expression<Action> expression, string cron, TimeZoneInfo timeZoneInfo);
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        /// <param name="timeZoneInfo"></param>
        void Recurring(Expression<Func<Task>> expression, string cron, TimeZoneInfo timeZoneInfo);
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        void Recurring<T>(Expression<Action<T>> expression, string cron) where T : class;
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        void Recurring<T>(Expression<Func<T, Task>> expression, string cron) where T : class;
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        /// <param name="timeZoneInfo"></param>
        void Recurring<T>(Expression<Action<T>> expression, string cron, TimeZoneInfo timeZoneInfo) where T : class;
        /// <summary>
        /// Recurring jobs fire many times on the specified CRON schedule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="cron"></param>
        /// <param name="timeZoneInfo"></param>
        void Recurring<T>(Expression<Func<T, Task>> expression, string cron, TimeZoneInfo timeZoneInfo) where T : class;
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        string ScheduleFromNow<T>(Expression<Action<T>> expression, TimeSpan timeSpan) where T : class;
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        string ScheduleInSpecificTime<T>(Expression<Action<T>> expression, DateTimeOffset dateTimeOffset) where T : class;
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        string ScheduleFromNow<T>(Expression<Func<T, Task>> expression, TimeSpan timeSpan) where T : class;
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        string ScheduleInSpecificTime<T>(Expression<Func<T, Task>> expression, DateTimeOffset dateTimeOffset) where T : class;
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        string ScheduleFromNow(Expression<Action> expression, TimeSpan timeSpan);
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        string ScheduleInSpecificTime(Expression<Action> expression, DateTimeOffset dateTimeOffset);
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        string ScheduleFromNow(Expression<Func<Task>> expression, TimeSpan timeSpan);
        /// <summary>
        /// Delayed jobs are executed only once too, but not immediately, after a certain time interval.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        string ScheduleInSpecificTime(Expression<Func<Task>> expression, DateTimeOffset dateTimeOffset);
        /// <summary>
        /// Continuations are executed when its parent job has been finished (OnlyOnSucceededState).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parentJobId"></param>
        /// <returns></returns>
        string ContinueJobWith<T>(Expression<Action<T>> expression, string parentJobId) where T : class;
        /// <summary>
        /// Continuations are executed when its parent job has been finished (OnlyOnSucceededState).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parentJobId"></param>
        /// <returns></returns>
        string ContinueJobWith<T>(Expression<Func<T, Task>> expression, string parentJobId) where T : class;
        /// <summary>
        /// Continuations are executed when its parent job has been finished (OnlyOnSucceededState).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parentJobId"></param>
        /// <returns></returns>
        string ContinueJobWith<T>(Expression<Action> expression, string parentJobId) where T : class;
        /// <summary>
        /// Continuations are executed when its parent job has been finished (OnlyOnSucceededState).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="parentJobId"></param>
        /// <returns></returns>
        string ContinueJobWith<T>(Expression<Func<Task>> expression, string parentJobId) where T : class;

    }
}
