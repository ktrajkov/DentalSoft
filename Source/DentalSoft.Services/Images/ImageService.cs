namespace DentalSoft.Services.Images
{
    using DentalSoft.Data.Contracts.Images;
    using DentalSoft.Data.Models.Images;
    using DentalSoft.Data.Services;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Drawing.Imaging;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public class ImageService : IImageService
    {
        public void Save(IEnumerable<HttpPostedFileBase> files, int patientId, ImageType imageType, int? toothNumber)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    string patientFolter = Resource.PatientsImagesPath + patientId.ToString(); ;
                    string imagesFolder = patientFolter + "/" + imageType.ToString();
                    string imagesPath = "~/" + imagesFolder;

                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(imagesPath));

                    string fileNameOnly = toothNumber != null ? toothNumber.ToString() : Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    string tempFileName = fileNameOnly + extension;
                    var path = HttpContext.Current.Server.MapPath(imagesPath);
                    var fullPath = Path.Combine(path, tempFileName);

                    var newFullPath = GetUniqueFilePath(fullPath, fileNameOnly, extension, path);
                    var pngPaths = new List<string> ();

                    if (ImageFormat.Tiff == GetImageFormat(extension))
                    {
                        pngPaths.AddRange(SaveTiffToPngs(file.InputStream, newFullPath, ImageFormat.Png));
                    }
                    else
                    {
                        file.SaveAs(newFullPath);
                        pngPaths.Add(newFullPath);
                    }

                    var repository = RepositoryManager.GetRepository<ImageModel, DentalSoft.Data.Models.Images.Image>();
                    foreach (var pngPath in pngPaths)
                    {
                        repository.Save(new ImageModel
                        {
                            ToothNumber = toothNumber,
                            PatientId = patientId,
                            ImageUrl = imagesFolder + "/" + Path.GetFileName(pngPath),
                            Type = imageType
                        });
                    }
                    repository.SaveChanges();
                }
            }

        }

        public void Remove(int patientId, Data.Models.Images.ImageType imageType, string fileName)
        {
            var physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/" + Resource.PatientsImagesPath + patientId.ToString() + "/" + imageType + "/"), fileName);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            var repository = RepositoryManager.GetRepository<ImageModel, DentalSoft.Data.Models.Images.Image>();
            var filter = new ImageFilter
            {
                ImageUrl = Resource.PatientsImagesPath + patientId.ToString() + "/" + imageType + "/" + fileName,
                PatientId = patientId,
                Type = imageType
            };
            var currentImage = repository.AllToModel<ImageFilter>(filter).FirstOrDefault();
            repository.Delete(currentImage);
            repository.SaveChanges();
        }

        public object GetTeethWithImages(int patientId)
        {
           return  RepositoryManager.GetRepositoryForEntity<DentalSoft.Data.Models.Images.Image>()
               .All<ImageFilter>(new ImageFilter
            {
                PatientId = patientId,
                Type = ImageType.Tooth
            }).GroupBy(x => x.ToothNumber).Select(x => new { ToothNumber = x.Key.Value });
        }


        #region Private Members

        private string[] SaveTiffToPngs(Stream inputStream, string fileName, ImageFormat format)
        {
            using (System.Drawing.Image imageFile = System.Drawing.Image.FromStream(inputStream))
            {
                FrameDimension frameDimensions = new FrameDimension(
                    imageFile.FrameDimensionsList[0]);

                // Gets the number of pages from the tiff image (if multipage) 
                int frameNum = imageFile.GetFrameCount(frameDimensions);
                string[] pngPaths = new string[frameNum];

                for (int frame = 0; frame < frameNum; frame++)
                {
                    // Selects one frame at a time and save as png. 
                    imageFile.SelectActiveFrame(frameDimensions, frame);
                    using (Bitmap bmp = new Bitmap(imageFile))
                    {
                        var directoryName = Path.GetDirectoryName(fileName);
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                        pngPaths[frame] = frame == 0 ? String.Format("{0}\\{1}.png", directoryName, fileNameWithoutExtension)
                            : String.Format("{0}\\{1}({2}).png", directoryName, fileNameWithoutExtension, frame);

                        bmp.Save(pngPaths[frame], format);
                    }
                }
                return pngPaths;
            }
        }

        private ImageFormat GetImageFormat(string format)
        {
            switch (format.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new NotImplementedException();
            }
        }

        public string GetUniqueFilePath(string fullPath, string fileNameOnly, string extension, string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles(fileNameOnly + ".*");
            int count = 1;
            string newFullPath = fullPath;
            while (files.Length > 0)
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
                files = dir.GetFiles(tempFileName + ".*");
            }
            return newFullPath;
        }

        #endregion
    }
}
