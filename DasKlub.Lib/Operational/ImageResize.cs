using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DasKlub.Lib.Operational
{
    /// <summary>
    ///     http://www.codeproject.com/news/191424/Resizing-an-Image-On-The-Fly-using-NET
    /// </summary>
    public static class ImageResize
    {
        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }


        public static Image FixedSize(Image imgPhoto, int width, int height, Color bgColor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent;

            float nPercentW = (width/(float) sourceWidth);
            float nPercentH = (height/(float) sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int) ((width - (sourceWidth*nPercent))/2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int) ((height - (sourceHeight*nPercent))/2);
            }

            var destWidth = (int) (sourceWidth*nPercent);
            var destHeight = (int) (sourceHeight*nPercent);

            var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(bgColor);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image Crop(Image imgPhoto, int width, int height, AnchorPosition Anchor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            const int sourceX = 0;
            const int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = (width/(float) sourceWidth);
            nPercentH = (height/(float) sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int) (height - (sourceHeight*nPercent));
                        break;
                    default:
                        destY = (int) ((height - (sourceHeight*nPercent))/2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int) (width - (sourceWidth*nPercent));
                        break;
                    default:
                        destX = (int) ((width - (sourceWidth*nPercent))/2);
                        break;
                }
            }

            var destWidth = (int) (sourceWidth*nPercent);
            var destHeight = (int) (sourceHeight*nPercent);
            var bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}