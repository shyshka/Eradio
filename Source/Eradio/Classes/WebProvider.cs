using Android.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Eradio
{
    public static class WebProvider
    {
        public static Bitmap GetImageBitmapFromUrl(string url)
        {            
            Bitmap imageBitmap = null;
            try
            {
                string fileName = System.IO.Path.GetFileName(url);
                string fileOutputPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);

                if (File.Exists(fileOutputPath)) imageBitmap = BitmapFactory.DecodeFile(fileOutputPath);
                else
                {
                    WebClient webClient = new WebClient();
                    byte[] imageBytes = webClient.DownloadData(url);
                    File.WriteAllBytes(fileOutputPath, imageBytes);
                    if (imageBytes != null && imageBytes.Length > 0)
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    webClient.Dispose();
                }
            }
            catch { return null; }
            return imageBitmap;
        }
    }
}