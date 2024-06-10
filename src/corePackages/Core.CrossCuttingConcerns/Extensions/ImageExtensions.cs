using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Extensions;
public static class ImageExtensions
{
    public static Image Resize(this Image image, int width, int height) // Uzantı metodunuz
    {
        // create a new bitmap with the desired size
        var resizedImage = new Bitmap(width, height);
        // create a graphics object to draw the image
        using (var graphics = Graphics.FromImage(resizedImage))
        {
            // set the quality options
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            // draw the image with the new size
            graphics.DrawImage(image, 0, 0, width, height);
        }
        // return the resized image
        return resizedImage;
    }
}
