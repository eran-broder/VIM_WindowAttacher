﻿using System.Runtime.CompilerServices;

namespace Vigor.Functional
{
    public static class TaskExtensions
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Task<TResult> AsTask<T, TResult>(this Task<T> task)
            where T : TResult
            where TResult : class
        {
            return task.Success(t => t.Result as TResult);
        }

        public static Task Success(this Task task, Action<Task> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task Error(this Task task, Action<Task> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnFaulted);

        public static Task NoSuccess(this Task task, Action<Task> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.NotOnRanToCompletion);

        public static Task Canceled(this Task task, Action<Task> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnCanceled);

        public static Task Always(this Task task, Action<Task> action) => task.ContinueWith(action);

        public static Task Always(this Task task) => task.ContinueWith(_ => { });

        public static Task<TNewResult> Success<TNewResult>(this Task task, Func<Task, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<TNewResult> Error<TNewResult>(this Task task, Func<Task, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnFaulted);

        public static Task<TNewResult> NoSuccess<TNewResult>(this Task task, Func<Task, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.NotOnRanToCompletion);

        public static Task<TNewResult> Canceled<TNewResult>(this Task task, Func<Task, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnCanceled);

        public static Task<TNewResult> Always<TNewResult>(this Task task, Func<Task, TNewResult> action) =>
            task.ContinueWith(action);

        public static Task<T> Success<T>(this Task<T> task, Action<Task<T>> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<T> Error<T>(this Task<T> task, Action<Task<T>> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnFaulted);

        public static Task<T> NoSuccess<T>(this Task<T> task, Action<Task<T>> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.NotOnRanToCompletion);

        public static Task<T> Canceled<T>(this Task<T> task, Action<Task<T>> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnCanceled);

        public static Task<T> Always<T>(this Task<T> task, Action<Task<T>> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.None);

        public static Task<TNewResult> Success<T, TNewResult>(this Task<T> task, Func<Task<T>, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<TNewResult> Error<T, TNewResult>(this Task<T> task, Func<Task<T>, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnFaulted);

        public static Task<TNewResult> NoSuccess<T, TNewResult>(this Task<T> task, Func<Task<T>, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.NotOnRanToCompletion);

        public static Task<TNewResult> Canceled<T, TNewResult>(this Task<T> task, Func<Task<T>, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.OnlyOnCanceled);

        public static Task<TNewResult> Always<T, TNewResult>(this Task<T> task, Func<Task<T>, TNewResult> action) =>
            Helper.SpecificContinue(task, action, TaskContinuationOptions.None);


        public static Task When<T>(this Task<T> task, TaskContinuationOptions continuationOptions,
            Action<Task<T>> continuationAction)
        {
            return task.ContinueWith(continuationAction, continuationOptions);
        }

        public static Task<TNewResult> When<T, TNewResult>(this Task<T> task,
            TaskContinuationOptions continuationOptions, Func<Task<T>, TNewResult> continuationAction)
        {
            return task.ContinueWith(continuationAction, continuationOptions);
        }

        public static bool IsFinished(this Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    return true;
                default:
                    return false;
            }
        }

        public static Task Map(
            this Task task,
            Action<Task> success,
            Action<Task> noSuccess)
        {
            return task.ContinueWith(task1 =>
            {
                if (task1.Status == TaskStatus.RanToCompletion)
                    success(task1);
                else
                    noSuccess(task1);
            });
        }

        public static Task<TNewResult> Map<TNewResult>(
            this Task task,
            Func<Task, TNewResult> success,
            Func<Task, TNewResult> noSuccess)
        {
            return task.ContinueWith(task1 =>
            {
                if (task1.Status == TaskStatus.RanToCompletion)
                    return success(task1);
                else
                    return noSuccess(task1);
            });
        }

        public static Task Map<T>(
            this Task<T> task,
            Action<Task<T>> success,
            Action<Task<T>> noSuccess)
        {
            return task.ContinueWith(task1 =>
            {
                if (task1.Status == TaskStatus.RanToCompletion)
                    success(task1);
                else
                    noSuccess(task1);
            });
        }

        public static Task<TNewResult> Match<T, TNewResult>(
            this Task<T> task,
            Func<Task<T>, TNewResult> success,
            Func<Task<T>, TNewResult> noSucces)
        {
            return task.ContinueWith(task1 =>
            {
                if (task1.Status == TaskStatus.RanToCompletion)
                    return success(task1);
                else
                    return noSucces(task1);
            });
        }

        public static Task Match<T>(
            this Task<T> task,
            Action<Task<T>> success,
            Action<Task<T>> noSucces)
        {
            return task.ContinueWith(task1 =>
            {
                if (task1.Status == TaskStatus.RanToCompletion)
                    success(task1);
                else
                    noSucces(task1);
            });
        }

        class Helper
        {
            public static Task<T> SpecificContinue<T>(Task<T> task, Action<Task<T>> action, TaskContinuationOptions option)
            {
                task.ContinueWith(action, option);
                return task;
            }

            public static Task SpecificContinue(Task task, Action<Task> action, TaskContinuationOptions option)
            {
                task.ContinueWith(action, option);
                return task;
            }

            public static Task<TNewResult> SpecificContinue<T, TNewResult>(Task<T> task, Func<Task<T>, TNewResult> action, TaskContinuationOptions option) =>
                task.ContinueWith(action, option);

            public static Task<TNewResult> SpecificContinue<TNewResult>(Task task, Func<Task, TNewResult> action, TaskContinuationOptions option) =>
                task.ContinueWith(action, option);
        }
    }
}
