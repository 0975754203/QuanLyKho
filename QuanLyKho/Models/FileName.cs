
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using QuanLyKho;
using System.Web;

namespace QuanLyKho.Models
{
    public class FileNames
    {
        public int FileId
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }
        public string FilePath
        {
            get;
            set;
        }
    }
    public class FileDownloadsModel
    {
        public List<FileNames> GetFile(string mauin)
        {
            string fileSavePath = null;
            if (mauin != null)
            {
                fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\Content\" + mauin);

            }
           
            List<FileNames> listFiles = new List<FileNames>();
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                listFiles.Add(new FileNames()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return listFiles;
        }
    }

   


}