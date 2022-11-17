using Microsoft.EntityFrameworkCore;
using server.Models;
using System.Security.Cryptography;

namespace server.Services
{
    public class FileService
    {
        public void DeleteFile(string path, IWebHostEnvironment env)
        {
            string fullPath = env.WebRootPath + path;
            if (!System.IO.File.Exists(fullPath))
            {
                throw new FileNotFoundException();
            }
            System.IO.File.Delete(fullPath);
        }

        public async Task< Models.File > UploadFile(IFormFile file, IWebHostEnvironment env) 
        {
            string contentType = file.ContentType.Split("/")[0];
            string htmlElem = "";
            Guid g = Guid.NewGuid();
            string path = "/files";
            string fileName = Path.GetFileName(file.FileName.Replace(" ", "_"));
            string fileType;
            if (contentType == "image")
            {
                path += "/images";
                fileType = "image";
            }
            else if (contentType == "video")
            {
                path += "/videos";
                fileType = "video";
            }
            else
            {
                fileType = "document";
                path += "/documents";
            }
            using (FileStream steam = new FileStream(env.WebRootPath + path + "/" + g + fileName, FileMode.Create))
            {
                await file.CopyToAsync(steam);
            }
            return new Models.File
            {
                FileLink = path + "/" + g + fileName,
                LogicalName = fileName,
                PhysicalName = g + fileName,
                PublicationDate = DateTime.Now,
                FileType = fileType
            };
        }   
        public void LoadSingleFileToDataBase(ApplicationContext db, Models.File newFile, User user)
        {
            db.UserFiles.Add(new UserFile
            {
                File = newFile,
                User = user
            });
        }
        public void LoadSingleFileToDataBase(ApplicationContext db, Models.File newFile, Group group)
        {
            db.GroupFiles.Add(new GroupFile
            {
                File = newFile,
                Group = group
            });
        }
        public void LoadMultipleFileToDataBase(ApplicationContext db, List<Models.File> newFiles, User user)
        {
            List<UserFile> userFiles = newFiles
                    .Select(x =>
                        new UserFile
                        {
                            File = x,
                            User = user
                        })
                    .ToList();
            db.UserFiles.AddRange(userFiles);
        }
        public void LoadMultipleFileToDataBase(ApplicationContext db, List<Models.File> newFiles, Group group)
        {
            List<GroupFile> groupFiles = newFiles
                    .Select(x =>
                        new GroupFile
                        {
                            File = x,
                            Group = group
                        })
                    .ToList();
            db.GroupFiles.AddRange(groupFiles);
        }
    }
}
