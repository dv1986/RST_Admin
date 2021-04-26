
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RST.Admin.Web.Api.Helper
{
    public class ImageCompressHelper
    {
        public static void CompressImage(string OriginalImagePath, string ThumbnailImagePath, IFormFile file, string fileName)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), OriginalImagePath);
            var fullPath = Path.Combine(pathToSave, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Get a bitmap. The using statement ensures objects  
            // are automatically disposed from memory after use.  
            using (Bitmap bmp1 = new Bitmap(fullPath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                // Create an Encoder object based on the GUID  
                // for the Quality parameter category.  
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.  
                // An EncoderParameters object has an array of EncoderParameter  
                // objects. In this case, there is only one  
                // EncoderParameter object in the array.  
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                // High
                //EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp1.Save(@"F:\Images\TestPhotoQualityHundred.jpg", jpgEncoder, myEncoderParameters);

                // Medium
                //myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp1.Save(@"F:\Images\TestPhotoQualityFifty.jpg", jpgEncoder, myEncoderParameters);


                pathToSave = Path.Combine(Directory.GetCurrentDirectory(), ThumbnailImagePath);
                fullPath = Path.Combine(pathToSave, fileName);
                // Save the bitmap as a JPG file with zero quality level compression.  
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 5L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(fullPath, jpgEncoder, myEncoderParameters);
            }
        }

        private void FtpUploadFile(string filename, string to_uri, string user_name, string password)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request =
                (FtpWebRequest)WebRequest.Create(to_uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Get network credentials.
            request.Credentials =
                new NetworkCredential(user_name, password);

            // Read the file's contents into a byte array.
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            // Write the bytes into the request stream.
            request.ContentLength = bytes.Length;
            using (Stream request_stream = request.GetRequestStream())
            {
                request_stream.Write(bytes, 0, bytes.Length);
                request_stream.Close();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
