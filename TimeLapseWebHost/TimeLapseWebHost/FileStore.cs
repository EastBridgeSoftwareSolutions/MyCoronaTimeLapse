﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TimeLapseWebHost
{
    public class FileStore : IFileStore
    {
        private readonly IWebHostEnvironment _env;
        private readonly string imagesRoot = "Images";

        public FileStore(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task Create(IFormFile uploadedFile, string id)
        {
            string userpath = GetUserFolder(id);
            var filename = DateTime.Now.Ticks + ".png";
            var filePath = Path.Combine(userpath, filename);
            Directory.CreateDirectory(userpath);
            using (var stream = File.Create(filePath))
            {
                await uploadedFile.CopyToAsync(stream);
            }
        }

        public List<string> GetAll(string id)
        {
            string userpath = GetUserFolder(id);
            return Directory.GetFiles(userpath).ToList();
        }

        public string GetUserFolder(string id)
        {
            return Path.Combine(_env.WebRootPath, imagesRoot, id);
        }

        public string GetFFMPEGFolder()
        {
            return Path.Combine(_env.ContentRootPath, "FFMPEG");
        }
    }
}
