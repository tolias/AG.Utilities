using System;

namespace AG.Utilities.Time
{
    public static class DateTimeExtensions
    {
        public static bool FallsInRange(this DateTime dateTime, DateTime rangeStart, DateTime rangeEnd)
        {
            return dateTime > rangeStart && dateTime < rangeEnd;
        }
        public static int GetAge(this DateTime birthdate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthdate.Year;

            if (birthdate > today.AddYears(-age))
                age--;
            return age;
        }

        public static Period GetAgeWithMonthsAndDays(this DateTime birthdate)
        {
            return Period.GetPeriodBetween(birthdate, DateTime.Today);
        }

        public static DateTime ChangeYear(this DateTime dateTime, int year)
        {
            return dateTime.AddYears(year - dateTime.Year);
        }

        public static DateTime ChangeMonth(this DateTime dateTime, int month)
        {
            return dateTime.AddMonths(month - dateTime.Month);
        }

        public static DateTime ChangeDay(this DateTime dateTime, int day)
        {
            return dateTime.AddDays(day - dateTime.Day);
        }

        public static DateTime ChangeHour(this DateTime dateTime, int hour)
        {
            return dateTime.AddHours(hour - dateTime.Hour);
        }

        public static DateTime ChangeMinute(this DateTime dateTime, int minute)
        {
            return dateTime.AddMinutes(minute - dateTime.Minute);
        }

        public static DateTime ChangeSecond(this DateTime dateTime, int second)
        {
            return dateTime.AddSeconds(second - dateTime.Second);
        }
    }
}
