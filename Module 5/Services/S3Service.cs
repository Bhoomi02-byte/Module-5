using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Module_5.Services
{
    public class S3Service : IS3Service
    {

        private readonly BlogDbContext _context;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(BlogDbContext context, IAmazonS3 s3Client, IConfiguration config)
        {
            _context = context;
            _s3Client = s3Client;
            _bucketName = config["AWS:BucketName"];

        }

        public async Task<string> UploadImageAsync(int postId, int userId, IFormFile image, HttpRequest request)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null) return JsonHelper.GetMessage(138);

            if (post.AuthorId != userId) return JsonHelper.GetMessage(150);


            var extension = Path.GetExtension(image.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!allowedExtensions.Contains(extension.ToLower()))
                return JsonHelper.GetMessage(151);


            var fileName = $"{Guid.NewGuid()}{extension}";
            using (var stream = image.OpenReadStream())
            {
                var uploadRequest = new Amazon.S3.Model.PutObjectRequest
                {
                    InputStream = stream,
                    BucketName = _bucketName,
                    Key = $"uploads/{fileName}",
                    ContentType = image.ContentType,
                    CannedACL = Amazon.S3.S3CannedACL.PublicRead
                };

                await _s3Client.PutObjectAsync(uploadRequest);
            }

            //var imageUrl = $"https://{_bucketName}.s3.amazonaws.com/uploads/{fileName}";
            var imageUrl = $"https://d1atjs2iemmyqu.cloudfront.net/uploads/{fileName}";
            post.ImageUrl = imageUrl;

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(152);

        }

        public async Task<string> GeneratePresignedUrl(string fileName)
        {
            var extension = Path.GetExtension(fileName).Replace(".", "");

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = "uploads/"+fileName,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Verb = HttpVerb.PUT,
                ContentType = $"image/{extension}"
            };

            string url = await _s3Client.GetPreSignedURLAsync(request);
            return url;
        }

    }
}
