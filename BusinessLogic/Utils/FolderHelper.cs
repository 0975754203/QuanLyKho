using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Utils
{
    public static class FolderHelper
    {
        public static void CheckExitFolder(this string FileFolder)
        {
            if (!Directory.Exists(FileFolder))
            {
                Directory.CreateDirectory(FileFolder);
            }
        }

        public static string DateTimeStringFolder(DateTime datetime)
        {
            return datetime.Year + "/" + datetime.Month.ToString("d2") + "/" + datetime.Day.ToString("d2") + "/";
        }
    }
}
