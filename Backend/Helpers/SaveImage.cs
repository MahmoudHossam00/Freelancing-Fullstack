namespace Freelancing.Helpers
{
	public static class SaveImage
	{
		public static string Save(this IFormFile file)
		{
			var covername = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), ImageSettings.ImagesPath); 
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, covername);
			using (var stream = System.IO.File.Create(filePath))
			{
				file.CopyTo(stream);
			}
			return covername;
		}
		public static string Save2(this IFormFile file)
		{
			var covername = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, covername);
			using (var stream = System.IO.File.Create(filePath))
			{
				file.CopyTo(stream);
			}
			return covername;
		}
		public static void Delete(string fileName)
		{
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), ImageSettings.ImagesPath);
			string filePath = Path.Combine(folderPath, fileName);

			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}
		}
	}
}
