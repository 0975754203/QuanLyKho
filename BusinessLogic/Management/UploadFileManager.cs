using BusinessLogic.Utils;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace BusinessLogic.Management
{
    public class UploadFileManager
    {
        #region Singleton
        private static UploadFileManager _instance;
        private UploadFileManager() { }
        public static UploadFileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UploadFileManager();
                }
                return _instance;
            }
        }
        #endregion
        private const int OrientationKey = 0x0112;
        private const int NotSpecified = 0;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;
        #region Public Interface
        public string SaveImgHocSinh(HttpPostedFileBase file, string patch, string folder, Guid IdHS)
        {
            var InputFileName = IdHS.ToString() + Path.GetFileName(file.FileName);
            var ServerSavePath = Path.Combine(patch + folder);
            FolderHelper.CheckExitFolder(ServerSavePath);
            ServerSavePath = ServerSavePath + "/" + InputFileName;

            ResizeImage(file.InputStream, ServerSavePath);
            //Save file to server folder  
            //file.SaveAs(ServerSavePath);
            return folder + "/" + InputFileName;
        }
        public void ResizeImage(Stream input, string file)
        {
            using (var image = Image.FromStream(input))
            {
                var newWidth = 89;
                var newHeight = 115;
                using (var newBitmap = new Bitmap(newWidth, newHeight))
                {
                    using (var imageScaler = Graphics.FromImage(newBitmap))
                    {
                        imageScaler.CompositingQuality = CompositingQuality.HighQuality;
                        imageScaler.SmoothingMode = SmoothingMode.HighQuality;
                        imageScaler.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                        imageScaler.DrawImage(image, imageRectangle);

                        // Fix orientation if needed.
                        if (image.PropertyIdList.Contains(OrientationKey))
                        {
                            var orientation = (int)image.GetPropertyItem(OrientationKey).Value[0];
                            switch (orientation)
                            {
                                case NotSpecified: // Assume it is good.
                                case NormalOrientation:
                                    // No rotation required.
                                    break;
                                case MirrorHorizontal:
                                    newBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    break;
                                case UpsideDown:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                    break;
                                case MirrorVertical:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                                    break;
                                case MirrorHorizontalAndRotateRight:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                                    break;
                                case RotateLeft:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                    break;
                                case MirorHorizontalAndRotateLeft:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                                    break;
                                case RotateRight:
                                    newBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    break;
                                default:
                                    throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                            }
                        }
                        newBitmap.Save(file, image.RawFormat);
                    }
                }
            }
        }
        #endregion
        #region 


        #endregion
    }
}
