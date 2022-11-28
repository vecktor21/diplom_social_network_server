using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Services;
using server.ViewModels;
using System.IO;
using System.Net.Mime;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private ApplicationContext db;
        private IWebHostEnvironment _env;
        private FileService fileService;
        private GroupService groupService;
        public FileController(ApplicationContext ct, IWebHostEnvironment env, FileService fileService, GroupService groupService)
        {
            this.db = ct;
            _env = env;
            this.fileService = fileService;
            this.groupService = groupService;
        }

        //получение всех файлов
        [HttpGet]
        public IActionResult Files()
        {
            return Json(db.Files);
        }


        //получить все UserFiles
        [HttpGet("[action]")]
        public IActionResult UserFiles()
        {
            var userFiles = db.UserFiles.Include(x => x.User).Include(x => x.File).ToList().Select(x => new
            {
                user = x.User.Login,
                userId = x.UserId,
                fileName = x.File.LogicalName,
                fileId = x.FileId
            });
            return Json(userFiles);
        }



        //получить все GroupFiles
        [HttpGet("[action]")]
        public IActionResult GroupFiles()
        {
            var groupFiles = db.GroupFiles.Include(x => x.Group).Include(x => x.File).ToList().Select(x => new
            {
                group = x.Group.GroupName,
                groupId = x.GroupId,
                fileName = x.File.LogicalName,
                fileId = x.FileId
            });
            return Json(groupFiles);
        }




        //получить все файлы пользователя
        [HttpGet("[action]/{userId}/{fileType?}")]
        public IActionResult UserFiles(int userId, string? fileType)
        {
            List<Models.File> userFiles = new List<Models.File>();
            if (string.IsNullOrEmpty(fileType)){
                userFiles.AddRange( db.UserFiles
                .Include(x => x.File)
                .Where(x => x.UserId == userId)
                .Select(x => x.File));
            }
            else
            {
                userFiles.AddRange(db.UserFiles
               .Include(x => x.File)
               .Where(x => x.UserId == userId && x.File.FileType == fileType)
               .Select(x => x.File));
            }
            return Json(userFiles);
        }




        //получить все файлы группы
        [HttpGet("[action]/{groupId}/{fileType?}")]
        public IActionResult GroupFiles(int groupId, string? fileType)
        {
            List<Models.File> groupFiles = new List<Models.File>();
            if (string.IsNullOrEmpty(fileType))
            {
                groupFiles.AddRange(db.GroupFiles
                .Include(x => x.File)
                .Where(x => x.GroupId == groupId)
                .Select(x => x.File));
            }
            else
            {
                groupFiles.AddRange(db.GroupFiles
               .Include(x => x.File)
               .Where(x => x.GroupId == groupId && x.File.FileType == fileType)
               .Select(x => x.File));
            }
            return Json(groupFiles);
        }




        //загрузка одного файла
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            string userName = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }
            User user = db.Users.FirstOrDefault(x => x.Login == userName);
            if (user == null)
            {
                return Unauthorized();
            }
            if (file == null)
            {
                return BadRequest();
            }
            try
            {
                //проверка на то, куда файл загружается - в группу или на страницу пользователя
                Models.File newFile = await fileService.UploadFile(file, _env);
                int groupId;

                if (HttpContext.Request.Headers.ContainsKey("Group"))
                {
                    groupId = int.Parse(HttpContext.Request.Headers["Group"]);
                    Group group = db.Groups.FirstOrDefault(x => x.GroupId == groupId);
                    if(group == null)
                    {
                        fileService.DeleteFile(newFile.FileLink, _env);
                        return NotFound("группа не найдена");
                    }
                    //проверка на то, что пользователь является лидером группы
                    if (groupService.CheckUserRole(user.UserId, groupId, new List<int> { 1, 2 }))
                    {
                        fileService.LoadSingleFileToDataBase(db, newFile, group);
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                else
                {
                    fileService.LoadSingleFileToDataBase(db, newFile, user);
                }


                await db.SaveChangesAsync();
                return Json(new
                {
                    newFile.FileType,
                    newFile.FileLink,
                    newFile.FileId,
                    newFile.LogicalName,
                    newFile.PhysicalName
                });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }




        //загрузка нескольких файлов
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> AddFiles(IFormFileCollection files)
        {

            string userName = HttpContext.User.Identity.Name;
            //проверка авторизации пользователя
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }
            User user = db.Users.FirstOrDefault(x => x.Login == userName);
            //проверка существования пользователя
            if (user == null)
            {
                return Unauthorized();
            }
            if (files == null)
            {
                return BadRequest();
            }
            try
            {
                //загрузка файлов на сервер, но без привязки к конкретной сущности
                //то есть файлы есть на сервере, но они не принадлежат ни группе, ни пользователю
                List<Models.File> newFiles = new List<Models.File>();

                foreach (var file in files)
                {
                    newFiles.Add(await fileService.UploadFile(file, _env));
                }
                int groupId;
                //проверка - куда загружаются файлы пользователю или в группу

                if (HttpContext.Request.Headers.ContainsKey("Group"))
                {
                    //если группе - то они загружаются на сервер
                    groupId = int.Parse(HttpContext.Request.Headers["Group"]);

                    if (groupService.CheckUserRole(user.UserId, groupId, new List<int> { 1, 2 }))
                    {
                        Group group = db.Groups.FirstOrDefault(x => x.GroupId == groupId);
                        if (group == null)
                        {
                            //но если вдруг группы не оказалось - то они удаляются
                            foreach (var file in newFiles)
                            {
                                fileService.DeleteFile(file.FileLink, _env);
                            }
                            return NotFound("группа не найдена");
                        }
                        //если группа существует - только что загруженные файлы привязываются к указанной группе
                        fileService.LoadMultipleFileToDataBase(db, newFiles, group);
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                else
                {
                    //если загрузка пользователю - то загружаются сразу
                    fileService.LoadMultipleFileToDataBase(db, newFiles, user);
                }

                await db.SaveChangesAsync();

                return Json(newFiles
                    .Select(x =>
                        new
                        {
                            x.FileType,
                            x.FileLink,
                            x.FileId,
                            x.LogicalName,
                            x.PhysicalName,
                            x.PublicationDate
                        })
                    .ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //загрузка файлов без привязки к пользователю\группе. используется для вложений в комментах
        [HttpPut("[action]")]
        public async Task<IActionResult> AddAnonimousFiles(IFormFileCollection files)
        {
            if (files == null)
            {
                return BadRequest();
            }
            try
            {
                //загрузка файлов на сервер
                List<Models.File> newFiles = new List<Models.File>();

                foreach (var file in files)
                {
                    newFiles.Add(await fileService.UploadFile(file, _env));
                }

                //сохранение файлов а базе данных
                db.Files.AddRange(newFiles);

                await db.SaveChangesAsync();

                return Json(newFiles
                    .Select(x =>
                        new
                        {
                            x.FileType,
                            x.FileLink,
                            x.FileId,
                            x.LogicalName,
                            x.PhysicalName,
                            x.PublicationDate
                        })
                    .ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //удаление файла пользователя
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            var file = db.UserFiles.FirstOrDefault(x => x.FileId == fileId);
            var fileObj = db.Files.FirstOrDefault(x => x.FileId == fileId);
            if (file == null || fileObj == null)
            {
                return NotFound("файл не найден");
            }
            try
            {
                db.UserFiles.Remove(file);
                await db.SaveChangesAsync();
                fileService.DeleteFile(fileObj.FileLink, _env);
                return Ok();
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }



        //удаление файла группы
        [HttpDelete("{fileId}/{groupId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int fileId, int groupId)
        {
            string userName = HttpContext.User.Identity.Name;
            //проверка авторизации пользователя
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }
            User user = db.Users.FirstOrDefault(x => x.Login == userName);
            //проверка существования пользователя
            if (user == null)
            {
                return Unauthorized();
            }
            var file = db.GroupFiles.FirstOrDefault(x => x.FileId == fileId && x.GroupId == groupId);
            var fileObj = db.Files.FirstOrDefault(x => x.FileId == fileId);
            if (file == null || fileObj == null)
            {
                return NotFound("файл не найден");
            }
            try
            {
                if (groupService.CheckUserRole(user.UserId, groupId, new List<int> { 1, 2 }))
                {
                    db.GroupFiles.Remove(file);
                    await db.SaveChangesAsync();
                    fileService.DeleteFile(fileObj.FileLink, _env);
                    return Ok();
                }
                return Forbid();

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //получение одного файла
        [HttpGet("{fileId}")]
        public IActionResult Files(int fileId)
        {
            var files = db.Files
                .Where(x => x.FileId == fileId);
            return Json(files.ToList());

        }
    }
}
