using System;
using System.ComponentModel;
using System.Threading;

namespace YearProgress.Core
{
    public class ProgressTracker
    {
        public ProgressTracker()
        {
            var now = DateTime.Now;
            var fullProgressSpan = new DateTime(now.Year, 12, 31, 23, 59, 59) - new DateTime(now.Year, 1, 1);
            CompleteProgressValue = Convert.ToDouble(fullProgressSpan.Ticks);
            UpdateYearProgress();
        }

        protected ProgressTracker(NoOpConstructorFlag dummy)
        {

        }

        /// <summary>
        /// The numerator of
        /// the ratio representing the current progress.
        /// </summary>
        /// <seealso cref="CompleteProgressValue"/>
        public double CurrentProgressValue { get; protected set; }

        /// <summary>
        /// The denominator of
        /// the ratio representing the current progress.
        /// </summary>
        /// <seealso cref="CurrentProgressValue"/>
        public double CompleteProgressValue { get; protected set; }

        public virtual void UpdateYearProgress()
        {
            var now = DateTime.Now;
            var elapsedTime = now - new DateTime(now.Year, 1, 1);
            var newProgressValue = Convert.ToDouble(elapsedTime.Ticks);
            CurrentProgressValue = newProgressValue;
        }

        protected struct NoOpConstructorFlag { }
    }
}
