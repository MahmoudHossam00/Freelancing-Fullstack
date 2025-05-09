using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Freelancing.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
        }

        public async Task<string> UploadBase64ImageAsync(string base64Image, string folder = "uploads")
        {
            if (string.IsNullOrWhiteSpace(base64Image))
                return null;

            var match = Regex.Match(base64Image, @"^data:image\/[a-zA-Z]+;base64,(.+)$");
            if (!match.Success)
                throw new Exception("Invalid base64 image format");

            var bytes = Convert.FromBase64String(match.Groups[1].Value);

            using var stream = new MemoryStream(bytes);
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription("image.jpg", stream),
                Folder = folder,
                PublicId = Guid.NewGuid().ToString(),
                Transformation = new Transformation()
                    .Width(800).Height(800).Crop("limit")
                    .Quality("auto").FetchFormat("auto")
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.StatusCode == System.Net.HttpStatusCode.OK
                ? result.SecureUrl.ToString()
                : throw new Exception($"Upload failed: {result.Error?.Message}");
        }

        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId)) return;
            await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        }

        public string ExtractPublicId(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;

            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            if (segments.Length < 2) return null;

            var folder = segments[^2];
            var fileName = Path.GetFileNameWithoutExtension(segments[^1]);
            return $"{folder}/{fileName}";
        }
    }
}
