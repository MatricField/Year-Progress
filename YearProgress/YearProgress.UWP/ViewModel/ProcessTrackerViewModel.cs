using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YearProgress.Core;

namespace YearProgress.UWP.ViewModel
{
    public class ProgressTrackerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ProgressTracker Model { get; }

        public int Minimum { get; } = 0;

        private int _Maximum;

        public int Maximum
        {
            get => _Maximum;
            set
            {
                Interlocked.Exchange(ref _Maximum, value);
                OnPropertyChanged(nameof(Maximum));
            }
        }

        private int _Value;

        public int Value
        {
            get => _Value;
            set
            {
                Interlocked.Exchange(ref _Value, value);
                OnPropertyChanged(nameof(Value));
            }
        }

        public ProgressTrackerViewModel()
            : this(new ProgressTracker())
        {
        }

        public ProgressTrackerViewModel(ProgressTracker model)
        {
            Model = model;
            Recalculate();
        }

        public void UpdateProgress()
        {
            Model.UpdateYearProgress();
            Recalculate();
        }

        protected virtual void Recalculate()
        {
            var now = DateTime.Now;
            var yearSpan = new DateTime(now.Year, 12, 31) - new DateTime(now.Year, 1, 1);
            Maximum = yearSpan.Days;
            Value =
                Convert.ToInt32(
                    (Model.CurrentProgressValue / Model.CompleteProgressValue) * Maximum
                    );
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
