using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Infrastructure
{
    public class ImageHelper
    {
        public static bool Resize_Picture(byte[] Filebytes, string FileName, string PathToSave, int FinalWidth, int FinalHeight, int ImageQuality)
        {
            System.Drawing.Bitmap NewBMP;
            System.Drawing.Graphics graphicTemp;

            using (Stream stream = new MemoryStream(Filebytes, false))
            {
                using (Image img = Image.FromStream(stream))
                {
                    string _path = PathToSave + "Original_" + FileName;
                    img.Save(_path);
                    foreach (var prop in img.PropertyItems)
                    {
                        if (prop.Id == 0x0112) //value of EXIF
                        {
                            int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                            RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                            img.RotateFlip(rotateFlipType);
                            break;
                        }
                    }
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
                    int iWidth;
                    int iHeight;
                    if (img.Height > 1920)
                    {
                        FinalHeight = 1920;
                        if ((FinalHeight == 0) && (FinalWidth != 0))
                        {
                            iWidth = FinalWidth;
                            iHeight = (bmp.Size.Height * iWidth / bmp.Size.Width);
                        }
                        else if ((FinalHeight != 0) && (FinalWidth == 0))
                        {
                            iHeight = FinalHeight;
                            iWidth = (bmp.Size.Width * iHeight / bmp.Size.Height);
                        }
                        else
                        {
                            iWidth = FinalWidth;
                            iHeight = FinalHeight;
                        }
                    }
                    else
                    {
                        FinalHeight = img.Height;
                        if ((FinalHeight == 0) && (FinalWidth != 0))
                        {
                            iWidth = FinalWidth;
                            iHeight = (bmp.Size.Height * iWidth / bmp.Size.Width);
                        }
                        else if ((FinalHeight != 0) && (FinalWidth == 0))
                        {
                            iHeight = FinalHeight;
                            iWidth = (bmp.Size.Width * iHeight / bmp.Size.Height);
                        }
                        else
                        {
                            iWidth = FinalWidth;
                            iHeight = FinalHeight;
                        }
                    }
                    NewBMP = new System.Drawing.Bitmap(iWidth, iHeight);
                    graphicTemp = System.Drawing.Graphics.FromImage(NewBMP);
                    graphicTemp.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    graphicTemp.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphicTemp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphicTemp.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphicTemp.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    graphicTemp.DrawImage(bmp, 0, 0, iWidth, iHeight);
                    graphicTemp.Dispose();
                    System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
                    System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);
                    encoderParams.Param[0] = encoderParam;
                    System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
                    for (int fwd = 0; fwd <= arrayICI.Length - 1; fwd++)
                    {
                        if (arrayICI[fwd].FormatDescription.Equals("JPEG"))
                        {
                            NewBMP.Save(PathToSave + FileName, arrayICI[fwd], encoderParams);
                        }
                    }

                    NewBMP.Dispose();
                    bmp.Dispose();
                }
            }

            return true;
        }
        public static void ImageResize(string inputPath, string fileName, string outFileName, int size, int quality)
        {
            try
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(Path.Combine(inputPath, fileName)))
                {
                    foreach (var prop in img.PropertyItems)
                    {
                        if (prop.Id == 0x0112) //value of EXIF
                        {
                            int orientationValue = img.GetPropertyItem(prop.Id).Value[0];
                            RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                            img.RotateFlip(rotateFlipType);
                            break;
                        }
                    }
                    using (var image = new Bitmap(img))
                    {

                        int width, height;
                        if (image.Width > image.Height)
                        {
                            width = size;
                            height = Convert.ToInt32(image.Height * size / (double)image.Width);
                        }
                        else
                        {
                            width = Convert.ToInt32(image.Width * size / (double)image.Height);
                            height = size;
                        }
                        var resized = new Bitmap(width, height);
                        using (var graphics = Graphics.FromImage(resized))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(image, 0, 0, width, height);
                            using (var output = File.Open(Path.Combine(inputPath, outFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                var qualityParamId = Encoder.Quality;
                                var encoderParameters = new EncoderParameters(1);
                                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                                var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(im => im.FormatID == ImageFormat.Png.Guid);
                                resized.Save(output, codec, encoderParameters);
                                output.Close();
                                output.Dispose();
                                graphics.Dispose();
                                resized.Dispose();
                            }
                        }
                        image.Dispose();
                    }
                    img.Dispose();
                }
            }
            catch
            {
                throw;
            }
            //return inputPath;
        }
        private static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }
        private static void DeleteExitingImage(string filePath)
        {
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image\\Programcilar", "controlller.jpg");
            //var path = Path.Combine(filePath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}