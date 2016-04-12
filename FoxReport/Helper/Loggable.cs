using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxReport.Helper
{
    public class Loggable<T> where T : class
    {
        private static Lazy<ILog> log = new Lazy<ILog>(() =>
        {
            return LogManager.GetLogger(typeof(T));
        });
        private static ILog logger;
        public static ILog Logger
        {
            get
            {
                return logger ?? log.Value;
            }
            set
            {
                logger = value;
            }
        }
    }
}