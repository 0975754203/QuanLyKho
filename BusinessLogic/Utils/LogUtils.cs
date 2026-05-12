
using BusinessLogic.Extensions;
using BusinessLogic.Utils;
using NLog;
using SQLDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BusinessLogic.Utils
{
    public static class LogUtils
    {
        private static ILogger logger = LogExtension.GetLogger();
        public static void Log(this Exception ex, string message = "")
        {
            var msg = string.IsNullOrEmpty(message) ? ex.Message : message;
            msg = msg + "||||" + ex.StackTrace + "||||" + ex.GetInnerException();
            logger.Error(ex, msg);
        }
        /// <summary>
        /// Lấy inner message sâu nhất!
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerException(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }

        /// <summary>
        /// Lấy toàn bộ thông tin lỗi!
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetInnerExceptions(this Exception exception)
        {
            var limit = 0;
            var result = new List<string>();
            var ex = exception;
            while (ex != null && limit++ < 10)
            {
                result.Add(ex.Message);
                ex = ex.InnerException;
            }

            var inner = string.Join("->", result.ToArray());
            return inner;
        }

    }


}