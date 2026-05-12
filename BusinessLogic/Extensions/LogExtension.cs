using BusinessLogic.Utils;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Extensions
{
    /// <summary>
    /// Logger extension configuration
    /// </summary>
    public static class LogExtension
    {
        /// <summary>
        /// Default logger name
        /// </summary>
        public const string DefaultLoggerName = "Main";

        /// <summary>
        /// Static constructor
        /// </summary>
        static LogExtension()
        {
            Config();
        }

        /// <summary>
        /// Cấu hình log cho toàn bộ ứng dụng
        /// </summary>
        /// <returns></returns>
        private static void Config()
        {
            try
            {
                if (File.Exists("NLog.config"))
                {
                    GetLogger().Debug("Logging use NLog.config file");
                    return;
                }
                else
                {
                    Console.WriteLine("Config NLog ...");
                }
                // log Info
                var logFileName = ConfigurationUtility.GetConfigurationSettingValue("LogFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/${logger}.log");
                var logLayout = ConfigurationUtility.GetConfigurationSettingValue("LogLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logArchiveAboveSize = ConfigurationUtility.GetConfigurationSettingValue("LogArchiveAboveSize", "5242880");
                var logSize = int.Parse(logArchiveAboveSize);

                // log Error
                var logErrorFileName = ConfigurationUtility.GetConfigurationSettingValue("LogErrorFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/error.log");
                var logErrorLayout = ConfigurationUtility.GetConfigurationSettingValue("LogErrorLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logErrorArchiveAboveSize = ConfigurationUtility.GetConfigurationSettingValue("LogErrorArchiveAboveSize", "5242880");
                var logErrorSize = int.Parse(logErrorArchiveAboveSize);


                var config = new LoggingConfiguration();
                var logDetailFileTarget = new FileTarget
                {
                    FileName = logFileName,
                    Layout = logLayout,
                    ArchiveAboveSize = logSize
                };
                var logErrorFileTarget = new FileTarget
                {
                    FileName = logErrorFileName,
                    Layout = logErrorLayout,
                    ArchiveAboveSize = logErrorSize
                };

                config.AddTarget("file", logDetailFileTarget);
                config.AddTarget("error", logErrorFileTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, logDetailFileTarget));
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, logErrorFileTarget));
                LogManager.Configuration = config;
            }
            catch (Exception ex)
            {
                ex.Log();
                throw ex;
            }
        }

        /// <summary>
        /// Get logger theo tên và loại
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Logger GetLogger(string name, Type type)
        {
            return LogManager.GetLogger(name, type);
        }

        /// <summary>
        /// Get logger theo tên
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Logger GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        /// <summary>
        /// Tạo logger mặc định 
        /// </summary>
        /// <returns></returns>
        public static Logger GetLogger()
        {
            return GetLogger(DefaultLoggerName);
        }

        /// <summary>
        /// Support quick reference from object with name of type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Logger GetLogger(this object obj)
        {
            if (obj == null)
            {
                return GetLogger();
            }
            else
            {
                return GetLogger(obj.GetType().Name);
            }
        }

        /// <summary>
        /// Extension logging with default logger
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(this object obj, Exception ex, string msg = "Có lỗi xảy ra!")
        {
            GetLogger(obj).Error(ex, msg);
        }

        /// <summary>
        /// Extension logging with default logger
        /// </summary>
        /// <param name="obj">Đối tượng log</param>
        /// <param name="msg">Thông điệp log</param>
        public static void Log(this object obj, string msg)
        {
            GetLogger(obj).Info(msg);
        }

        /// <summary>
        /// Log User Action
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="newVal"></param>
        /// <param name="oldVal"></param>
        public static void LogActionByUser(this object obj, string action, string message, object user, object newVal, object oldVal = null)
        {
            GetLogger(obj).Info("{eventType} {action}: {message} by {@user} with {@newVal} from {@oldVal}", "AuditLog", action, message, user, newVal, oldVal);
        }

        /// <summary>
        /// Log User Action by Created Flag
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <param name="user"></param>
        /// <param name="newVal"></param>
        /// <param name="oldVal"></param>
        public static void LogActionByUser(this object obj, bool isCreated, string message, object user, object newVal, object oldVal = null)
        {
            if (isCreated)
            {
                LogCreatedByUser(obj, message, user, newVal, oldVal);
            }
            else
            {
                LogUpdatedByUser(obj, message, user, newVal, oldVal);
            }
        }

        /// <summary>
        /// Thao tác xóa dữ liệu
        /// </summary>
        /// <param name="obj">Nơi thay đổi, nên log trong Manager/Controller</param>
        /// <param name="message">Thông điệp hoặc đối tượng tác động</param>
        /// <param name="user">Người thay đổi (tên tài khoản)</param>
        /// <param name="newVal">Giá trị mới</param>
        /// <param name="oldVal">Giá trị ban đầu</param>
        public static void LogDeletedByUser(this object obj, string message, object user, object newVal, object oldVal = null)
        {
            LogActionByUser(obj, "DeletedByUser", message, user, newVal, oldVal);
        }

        /// <summary>
        /// Thao tác cập nhật, thay đổi dữ liệu
        /// </summary>
        /// <param name="obj">Nơi thay đổi, nên log trong Manager/Controller</param>
        /// <param name="message">Thông điệp hoặc đối tượng tác động</param>
        /// <param name="user">Người thay đổi (tên tài khoản)</param>
        /// <param name="newVal"></param>
        /// <param name="oldVal"></param>
        public static void LogUpdatedByUser(this object obj, string message, object user, object newVal, object oldVal = null)
        {
            LogActionByUser(obj, "UpdatedByUser", message, user, newVal, oldVal);
        }

        /// <summary>
        /// Thao tác tạo dữ liệu tạo mới
        /// </summary>
        /// <param name="obj">Nơi thay đổi, nên log trong Manager/Controller</param>
        /// <param name="message">Thông điệp hoặc đối tượng tác động</param>
        /// <param name="user">Người thay đổi (tên tài khoản)</param>
        /// <param name="newVal"></param>
        /// <param name="oldVal"></param>
        public static void LogCreatedByUser(this object obj, string message, object user, object newVal, object oldVal = null)
        {
            LogActionByUser(obj, "CreatedByUser", message, user, newVal, oldVal);
        }
    }
}
