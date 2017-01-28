using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M3LMS.ViewModels
{
    public class UploadsViewModel
    {
        public long ID { get; set; }
        public List<File> Uploads { get; set; }

        public UploadsViewModel()
        {
            this.Uploads = new List<File>();
        }
    }


    public class File
    {
        public string FilePath { get; set; }
        public long FileID { get; set; }
        public string FileName { get; set; }
    }
}