using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Request
{
    public static class WebContent
    {
        public static Bitmap LoadPicture(string url)
        {
            string fileName = Path.GetFileName(url);
            string fileDiskPath = Path.Combine(Application.StartupPath, "Images", fileName);
            if (File.Exists(fileDiskPath))
                return new Bitmap(fileDiskPath);

            WebClient wc = new WebClient();
            wc.DownloadFile(url, fileDiskPath);
            return new Bitmap(fileDiskPath);            
        }
    }
}
