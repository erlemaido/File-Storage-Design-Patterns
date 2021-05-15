using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace WebApp.Services
{
    public class S3StorageService
    {
        private readonly AmazonS3Client _s3Client;
        private const string BucketName = "cloud-photo-storage";
        private const string FolderName = "assets";

        public S3StorageService()
        {
            _s3Client = new AmazonS3Client(new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1
            });
        }

        public string AddItem(IFormFile file, string subFolderName)
        {
            var fileName = Guid.NewGuid() + "_" + file.FileName;
            var objectKey = $"{FolderName}/{subFolderName}/{fileName}";
            
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = objectKey,
                ContentType = file.ContentType
            };

            _s3Client.PutObjectAsync(putObjectRequest);
            
            return fileName;
        }
    }
}