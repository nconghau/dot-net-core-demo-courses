using System;

namespace CourseDemo.Services
{
    public class Utils
    {
        public static long GetTimestamp()
        {
            DateTime datetime = DateTime.Now;
            return ((DateTimeOffset)datetime).ToUnixTimeSeconds(); 
        }
    }

}
