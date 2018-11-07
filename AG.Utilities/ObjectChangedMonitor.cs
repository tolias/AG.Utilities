using System;
using System.Collections.Generic;
using System.Text;
using AG.Utilities.Binding;

namespace AG.Utilities
{
    public class ObjectChangedMonitor<TObject, TSelectedValue> : IDisposable
    {
        private System.Timers.Timer _tmr = new System.Timers.Timer();
        public TSelectedValue PreviousValue { get; private set; }

        public TObject Object;
        public Func<TObject, TSelectedValue> Selector;

        public event ChangedEventHandler<TSelectedValue> Changed;

        public ObjectChangedMonitor(Func<TObject, TSelectedValue> selector, TObject obj = default(TObject), int checkInterval = 1000)
        {
            _tmr.Elapsed += _tmr_Elapsed;
            Object = obj;
            Selector = selector;
            PreviousValue = Selector(Object);
            CheckInterval = checkInterval;
        }

        public double CheckInterval
        {
            get { return _tmr.Interval; }
            set { _tmr.Interval = value; }
        }

        public void StartMonitoring()
        {
            _tmr.Enabled = true;
        }

        public void StopMonitoring()
        {
            _tmr.Enabled = false;
        }

        private void _tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_tmr)
            {
                var newValue = Selector(Object);
                if (PreviousValue.EqualsWithNullHandling(newValue))
                {
                    return;
                }
                if (Changed != null)
                {
                    var changedEventArgs = new ChangedEventArgs<TSelectedValue>(PreviousValue, newValue);
                    Changed(this, changedEventArgs);
                }
                PreviousValue = newValue;
            }
        }

        public void Dispose()
        {
            StopMonitoring();
        }
    }
}
