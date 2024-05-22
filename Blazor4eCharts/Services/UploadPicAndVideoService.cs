using FFMpegCore;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;


namespace Blazor4Learning.Services
{
    public class UploadPicAndVideoService
    {
        IWebHostEnvironment _Environment;
        long maxFileSize;
        List<string> FilePathList;
        public UploadPicAndVideoService(IWebHostEnvironment Environment)
        {
            _Environment = Environment;
            maxFileSize = 1024 * 1024 * 10;
            FilePathList = new List<string>();
        }

        public async Task<List<string>> UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.ContentType.Contains("video/")) // Check for video content type
                    {
                        // Handle video upload logic (e.g., validate size)
                        maxFileSize = 1024 * 1024 * 100;
                    }
                    else
                    {
                        maxFileSize = 1024 * 1024 * 10;
                    }
                    //生成随机名日期
                    string dateName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //生成两位随机数
                    Random objRandm = new Random();
                    dateName += objRandm.Next(0, 100).ToString("00");
                    var absolutefolderPath = Path.Combine(_Environment.ContentRootPath, "wwwroot", "CaseUploads");
                    var relateivefloderPath = Path.Combine("..", "CaseUploads");

                    var fileExtension = Path.GetExtension(file.Name).ToString();
                    if (!Directory.Exists(absolutefolderPath))
                    {
                        Directory.CreateDirectory(absolutefolderPath);
                    }
                    var absolutefileName = Path.Combine(absolutefolderPath, Path.ChangeExtension(dateName, fileExtension));
                    var relativefileName = Path.Combine(relateivefloderPath, Path.ChangeExtension(dateName, fileExtension));

                    //save file to local drive
                    await using FileStream fs = new(absolutefileName, FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                    //判断是否为MOV格式文件
                    if (fileExtension.ToUpper() == ".MOV")
                    {
                        var convertedVideoPath = absolutefileName.Replace(".MOV", ".mp4");
                        // or individual, per-run options
                        await FFMpegArguments
                            .FromFileInput(absolutefileName)
                            .OutputToFile(convertedVideoPath)
                            .ProcessAsynchronously();
                    }

                    if (fileExtension.ToUpper() == ".MOV")
                    {
                        FilePathList.Add(relativefileName.Replace(".MOV", ".mp4"));
                    }
                    else
                    {
                        FilePathList.Add(relativefileName);
                    }


                    

                }
                return FilePathList;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.StackTrace + ex.Source);
                return FilePathList;
            }

        }
    }
}
