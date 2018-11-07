using System;
using System.Collections.Generic;
using System.Text;

namespace AG.Utilities.Time
{
    public class Period : IComparable<Period>
    {
        public int Years;
        private int _months;
        private int _days;
        
        public bool MonthsAndDaysAreSpecified { get; private set; }

        public Period() { }

        public Period(int years)
        {
            Years = years;
        }

        public Period(int years, int months, int days)
        {
            Years = years;
            _months = months;
            _days = days;
            MonthsAndDaysAreSpecified = true;
        }

        public int Months
        {
            get { return _months; }
            set
            {
                _months = value;
                MonthsAndDaysAreSpecified = true;
            }
        }

        public int Days
        {
            get { return _days; }
            set
            {
                _days = value;
                MonthsAndDaysAreSpecified = true;
            }
        }

        public static Period GetPeriodBetween(DateTime startDate, DateTime endDate)
        {
            Period period;
            if (startDate > endDate)
            {
                period = GetPeriodBetweenWithoutCheck(endDate, startDate);
                period = -period;
            }
            else
            {
                period = GetPeriodBetweenWithoutCheck(startDate, endDate);
            }
            return period;
        }

        private static Period GetPeriodBetweenWithoutCheck(DateTime startDate, DateTime endDate)
        {
            var timeSpan = endDate - startDate;
            
            var days = endDate.Day - startDate.Day;
            int endDateMonth = endDate.Month;
            int endDateYears = endDate.Year;
            if (days < 0)
            {
                endDateMonth--;
                if(endDateMonth < 1)
                {
                    endDateMonth = 12;
                    endDateYears--;
                }
                int daysInLastMonth = DateTime.DaysInMonth(endDate.Year, endDateMonth);
                days = daysInLastMonth + endDate.Day - startDate.Day;
            }

            var months = endDateMonth - startDate.Month;
            if (months < 0)
            {
                endDateYears--;
                months = 12 + endDateMonth - startDate.Month;
            }

            var years = endDateYears - startDate.Year;

            return new Period(years, months, days);
        }

        public override int GetHashCode()
        {
            return _days;
        }

        public bool Equals(Period anotherPeriod)
        {
            if (anotherPeriod == null)
                return false;
            if (Years != anotherPeriod.Years)
                return false;
            if (MonthsAndDaysAreSpecified != anotherPeriod.MonthsAndDaysAreSpecified)
                return false;
            if (_months != anotherPeriod._months)
                return false;
            if (_days != anotherPeriod._days)
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Period);
        }

        public override string ToString()
        {
            if(MonthsAndDaysAreSpecified)
            {
                return string.Format("{0}.{1:D2}.{2:D2}", Years, _months, _days);
            }
            else
            {
                return Years.ToString();
            }
        }

        public int CompareTo(Period other)
        {
            if (other == null)
                return 1;

            if (Years > other.Years)
                return 1;
            else if (Years < other.Years)
                return -1;
            else
            {
                if (_months > other._months)
                    return 1;
                else if (_months < other._months)
                    return -1;
                else
                {
                    if (_days > other._days)
                        return 1;
                    else if (_days < other._days)
                        return -1;
                    else
                        return 0;
                }
            }
        }

        public static Period operator -(Period period)
        {
            return new Period(-period.Years, -period._months, -period._days)
            {
                MonthsAndDaysAreSpecified = period.MonthsAndDaysAreSpecified
            };
        }
    }
}
