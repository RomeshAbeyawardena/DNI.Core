namespace DNI.Core.Services
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;

    public class JobState
    {
        public Job Job { get; }

        public object State { get; }

        public JobState(Job job, object state)
        {
            Job = job;
            State = state;
        }
    }

    public class Job : IDisposable
    {
        public bool IsRunning { get; set; }

        public ISubject<JobState> Subject { get; }

        private Action<Job, object> JobTask { get; }

        private int Interval { get; } = 1000;

        private readonly Timer timer;
        private object state;

        public Job(object initialState, Action<Job, object> jobTask, int interval)
        {
            Subject = new Subject<JobState>();
            JobTask = jobTask;
            Interval = interval;
            IsRunning = true;
            state = initialState;
            timer = new Timer(callback, state, TimeSpan.FromMilliseconds(Interval), TimeSpan.FromMilliseconds(Interval));
        }

        public void UpdateState(object newState)
        {
            state = newState;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public async Task Running(CancellationToken cancellationToken)
        {
            while (IsRunning)
            {
                await Task.Delay(Interval, cancellationToken);
            }

            Subject.OnCompleted();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool gc)
        {
            IsRunning = false;
            timer.Dispose();
        }

        private void callback(object state1)
        {
            JobTask?.Invoke(this, state);
            Subject.OnNext(new JobState(this, state));
            timer.Change(TimeSpan.FromMilliseconds(Interval), TimeSpan.FromMilliseconds(Interval));
        }
    }
}
