using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarsRover
{
    public class CodecUtility
    {
        ImageCodecInfo myImageCodecInfo;
        System.Drawing.Imaging.Encoder myEncoder;
        EncoderParameter myEncoderParameter;
        EncoderParameters myEncoderParameters;
        TypeConverter ImageConverter;

        public CodecUtility()
        {

        }

        public Bitmap DecompressJPEGArrayToBpm(byte[] JPEGArray)
        {
            Bitmap aBitmap = (System.Drawing.Bitmap)((new System.Drawing.ImageConverter()).ConvertFrom(JPEGArray));

            return aBitmap; 
        }

        public byte[] CompressBmpToJPEGArray(long CompressionRate, Bitmap aBitmap)
        {
            byte[] byteArray = new byte[0];
            MemoryStream stream = new MemoryStream();
            
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, CompressionRate);
            myEncoderParameters.Param[0] = myEncoderParameter;
                        
            aBitmap.Save(stream, myImageCodecInfo, myEncoderParameters);
            stream.Close();
            byteArray = stream.ToArray();

            return byteArray;
        }

        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for(j = 0; j < encoders.Length; ++j)
            {
                if(encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    
    }
}
