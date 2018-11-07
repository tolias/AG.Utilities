using System;

namespace AG.Utilities.Time
{
    public static class TimeHumanizer
    {
        public static string GetShortForm(DateTime dateTime, bool timeIsFirst = false)
        {
            DateTime now = DateTime.Now;

            string sDate = null;
            if (now.Year != dateTime.Year) sDate = dateTime.ToString(@"yyyy\.MM\.dd");
            else if (now.Month != dateTime.Month) sDate = dateTime.ToString(@"MM\.dd");
            else if (now.Day != dateTime.Day)
            {
                int daysDifference = dateTime.Day - now.Day;
                switch (daysDifference)
                {
                    case -1:
                        sDate = "yesterday";
                        break;
                    case 0:
                        sDate = "today";
                        break;
                    case 1:
                        sDate = "tomorrow";
                        break;
                    default:
                        sDate = dateTime.ToString(@"dd");
                        break;
                }
            }

            string time = dateTime.ToString("HH:mm:ss");
            if (sDate == null)
                return time;
            else
            {
                if (timeIsFirst)
                    return time + " " + sDate;
                else
                    return sDate + " " + time;
            }
        }

        public static string GetShortForm(this TimeSpan timeSpan, ShowTimeOptions showTimeOptions = ShowTimeOptions.MinutesSeconds)
        {
            return GetShortFormFromSeconds((int)timeSpan.TotalSeconds, showTimeOptions);
        }

        private static string MakeTwoDigits(int number)
        {
            return number < 10 ? ("0" + number) : number.ToString();
        }

        public static string GetShortFormFromSeconds(int seconds, ShowTimeOptions showTimeOptions = ShowTimeOptions.MinutesSeconds)
        {
            var secondsInMinute = 60;
            var secondsInHour = 60 * secondsInMinute;
            var secondsInDay = 24 * secondsInHour;
            var daysOnly = seconds / secondsInDay;
            var secondsWithoutDays = seconds - (daysOnly * secondsInDay);
            var hoursOnly = secondsWithoutDays / secondsInHour;
            var secondsWithoutHours = seconds - (hoursOnly * secondsInHour);
            var minutesOnly = secondsWithoutHours / secondsInMinute;
            var secondsOnly = secondsWithoutHours - (minutesOnly * secondsInMinute);

            string res;

            if (daysOnly > 0)
            {
                res = daysOnly + "d" + hoursOnly + ":" + MakeTwoDigits(minutesOnly) + ":" + MakeTwoDigits(secondsOnly);
            }
            else if (showTimeOptions == ShowTimeOptions.HoursMinutesSeconds || hoursOnly > 0)
            {
                res = hoursOnly + ":" + MakeTwoDigits(minutesOnly) + ":" + MakeTwoDigits(secondsOnly);
            }
            else if (showTimeOptions == ShowTimeOptions.MinutesSeconds || minutesOnly > 0)
            {
                res = minutesOnly + ":" + MakeTwoDigits(secondsOnly);
            }
            else
            {
                res = secondsOnly.ToString();
            }
            return res;
        }
    }
}
