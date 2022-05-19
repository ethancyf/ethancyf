using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class DateTimeUtils
    {
        public static string[] AllDateTimeFormat = new string[] { "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss", "yyyy-M-d HH:mm:ss", "yyyy-M-d H:m:s", "dd/MM/yyyy HH:mm:ss", "yyyy/M/d HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyyMMddHHmmss", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy" };
        public static string DateFormatStr = "yyyy-MM-dd";
        public static string DateTimeFormatStr = "yyyy-MM-dd HH:mm:ss";
        public static string DateTimeFullFormatStr = "yyyy-MM-dd HH:mm:ss.fff";
        public static DateTime DBMaxDateTime = new DateTime(0x270f, 12, 0x1f);
        public static DateTime DBMinDateTime = new DateTime(0x6d9, 1, 1);
        public static readonly LogUtils log = new LogUtils(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string TimeFormatStr = "HH:mm:ss";

        public static DateTime CorrectDateTimeForDB(DateTime dt, bool isSmallDateTime)
        {
            DateTime dBMinDateTime;
            DateTime dBMaxDateTime;
            if (isSmallDateTime)
            {
                dBMinDateTime = new DateTime(0x76c, 1, 1);
                dBMaxDateTime = new DateTime(0x81f, 6, 6);
            }
            else
            {
                dBMinDateTime = DBMinDateTime;
                dBMaxDateTime = DBMaxDateTime;
            }
            if (dt < dBMinDateTime)
            {
                return dBMinDateTime;
            }
            if (dt > dBMaxDateTime)
            {
                return dBMaxDateTime;
            }
            return dt;
        }

        public static string FormatDateTime(object objDateTime)
        {
            return FormatDateTime(objDateTime, "yyyy-MM-dd");
        }

        public static string FormatDateTime(string dt)
        {
            try
            {
                return DateTime.Parse(dt).ToString("yyyy-MM-dd");
            }
            catch
            {
                return "";
            }
        }

        public static string FormatDateTimeToString(string dt)
        {
            try
            {
                return DateTime.ParseExact(dt, "yyyyMMdd", CultureInfo.CurrentCulture).ToString("dd-MM-yyyy");
            }
            catch
            {
                return "";
            }
        }

        public static string FormatDateTime(object objDateTime, string formatStr)
        {
            try
            {
                DateTime time = (DateTime)objDateTime;
                if (time <= DateTime.MinValue)
                {
                    return "";
                }
                return time.ToString(formatStr);
            }
            catch
            {
                return "";
            }
        }

        public static bool IsDateTime(string strDate)
        {
            try
            {
                DateTime.ParseExact(strDate, AllDateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
                return true;
            }
            catch (Exception exception)
            {
                log.Warn("IsDateTime " + strDate + "发生错误,原因:" + exception.Message);
                return false;
            }
        }

        public static bool IsDateTime(string strDate, string dateTimeFormatStr)
        {
            try
            {
                DateTime.ParseExact(strDate, dateTimeFormatStr, CultureInfo.CurrentCulture);
                return true;
            }
            catch (Exception exception)
            {
                log.Warn("IsDateTime " + strDate + " dateTimeFormatStr=" + dateTimeFormatStr + " 发生错误,原因:" + exception.Message);
                return false;
            }
        }

        public static DateTime StringToDateTime(string strDate)
        {
            try
            {
                return DateTime.ParseExact(strDate, AllDateTimeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            catch (Exception exception)
            {
                log.Debug("StringToDateTime strDate=" + strDate + " 出错，原因：" + exception.Message);
                return DateTime.Parse(strDate);
            }
        }

        public static DateTime StringToDateTime(string strDate, DateTime dtDefault)
        {
            try
            {
                return StringToDateTime(strDate);
            }
            catch (Exception exception)
            {
                log.Debug("StringToDateTime " + strDate + " 发生错误 返回默认值 ,原因:" + exception.Message);
                return dtDefault;
            }
        }

        public static DateTime StringToDateTime(string strDate, string dateTimeFormatStr)
        {
            return DateTime.ParseExact(strDate, dateTimeFormatStr, CultureInfo.CurrentCulture);
        }

        public static DateTime StringToDateTime(string strDate, string dateTimeFormatStr, DateTime dtDefault)
        {
            try
            {
                return DateTime.ParseExact(strDate, dateTimeFormatStr, CultureInfo.CurrentCulture);
            }
            catch (Exception exception)
            {
                log.Debug("StringToDateTime " + strDate + " dateTimeFormatStr=" + dateTimeFormatStr + " 发生错误 返回默认值 ,原因:" + exception.Message);
                return dtDefault;
            }
        }

        public static string ToMSSqlDateTimeStr(DateTime dt)
        {
            DateTime time = CorrectDateTimeForDB(dt, true);
            return ("convert(datetime,'" + time.ToString("yyyy-MM-dd HH:mm:ss") + "',21)");
        }

        public static string ToMSSqlDateTimeStr(object dt)
        {
            if ((dt != null) && !(dt is DBNull))
            {
                try
                {
                    return ToMSSqlDateTimeStr(DateTime.Parse(dt.ToString()));
                }
                catch
                {
                    return "null";
                }
            }
            return "null";
        }

        public static string ToMSSqlDateTimeStr(object dt, DateTime defaultDateTime)
        {
            if ((dt == null) || (dt is DBNull))
            {
                return ToMSSqlDateTimeStr(defaultDateTime);
            }
            try
            {
                return ToMSSqlDateTimeStr(DateTime.Parse(dt.ToString()));
            }
            catch
            {
                return ToMSSqlDateTimeStr(defaultDateTime);
            }
        }

        public static int QuarterOfYear
        {
            get
            {
                DateTime date = DateTime.Now.Date;
                return (1 + (date.Month / 3));
            }
        }

        public static DateTime ThisMonthFirstDate
        {
            get
            {
                return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }
        }

        public static DateTime ThisMonthLastDate
        {
            get
            {
                DateTime time = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                return time.AddMonths(1).AddDays(-1.0);
            }
        }

        public static DateTime ThisQuarterFirstDate
        {
            get
            {
                DateTime date = DateTime.Now.Date;
                return date.AddMonths(-((date.Month - 1) % 3)).AddDays((double)(1 - date.Day)).Date;
            }
        }

        public static DateTime ThisQuarterLastDate
        {
            get
            {
                DateTime date = DateTime.Now.Date;
                return date.AddMonths(-((date.Month - 1) % 3)).AddDays((double)(1 - date.Day)).AddMonths(3).AddDays(-1.0).Date;
            }
        }

        public static DateTime ThisWeekFirstDate
        {
            get
            {
                DayOfWeek dayOfWeek = Today.DayOfWeek;
                DateTime now = DateTime.Now;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        now = DateTime.Now.AddDays(-6.0);
                        break;

                    case DayOfWeek.Monday:
                        now = DateTime.Now;
                        break;

                    case DayOfWeek.Tuesday:
                        now = DateTime.Now.AddDays(-1.0);
                        break;

                    case DayOfWeek.Wednesday:
                        now = DateTime.Now.AddDays(-2.0);
                        break;

                    case DayOfWeek.Thursday:
                        now = DateTime.Now.AddDays(-3.0);
                        break;

                    case DayOfWeek.Friday:
                        now = DateTime.Now.AddDays(-4.0);
                        break;

                    case DayOfWeek.Saturday:
                        now = DateTime.Now.AddDays(-5.0);
                        break;
                }
                return new DateTime(now.Year, now.Month, now.Day);
            }
        }

        public static DateTime ThisWeekLastDate
        {
            get
            {
                DayOfWeek dayOfWeek = Today.DayOfWeek;
                DateTime now = DateTime.Now;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        now = DateTime.Now.AddDays(0.0);
                        break;

                    case DayOfWeek.Monday:
                        now = DateTime.Now.AddDays(6.0);
                        break;

                    case DayOfWeek.Tuesday:
                        now = DateTime.Now.AddDays(5.0);
                        break;

                    case DayOfWeek.Wednesday:
                        now = DateTime.Now.AddDays(4.0);
                        break;

                    case DayOfWeek.Thursday:
                        now = DateTime.Now.AddDays(3.0);
                        break;

                    case DayOfWeek.Friday:
                        now = DateTime.Now.AddDays(2.0);
                        break;

                    case DayOfWeek.Saturday:
                        now = DateTime.Now.AddDays(1.0);
                        break;
                }
                return new DateTime(now.Year, now.Month, now.Day);
            }
        }

        public static DateTime ThisYearFirstDate
        {
            get
            {
                return new DateTime(DateTime.Today.Year, 1, 1);
            }
        }

        public static DateTime ThisYearLastDate
        {
            get
            {
                DateTime time = new DateTime(DateTime.Today.Year, 1, 1);
                return time.AddYears(1).AddDays(-1.0);
            }
        }

        public static DateTime Today
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static DateTime Yesterday
        {
            get
            {
                DateTime time = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                return time.AddDays(-1.0);
            }
        }

        public static int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
    }
}
