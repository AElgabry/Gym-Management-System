using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;



namespace GymManagement.BLL.Services.AttachmentService
{
	public class AttachmentService : IAttachmentService
	{



		private readonly long maxSize = 5*1024*1024;
		private readonly string[] allowedExtentions = [".jpg", ".jpeg", ".png"];
		private readonly IWebHostEnvironment env;

		public AttachmentService( IWebHostEnvironment env)
		{
			this.env = env;
		}

		public async Task<Result> DeleteAsync(string folderName, string fileName)
		{

			if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName)) return Result.NotFound("Cannot find the file");

			try
			{
				var path = env.ContentRootPath;
				var fullPath = Path.Combine(path, folderName, fileName);
				if (!File.Exists(fullPath)) return Result.NotFound("Cannot find the file");
				File.Delete(fullPath);
				return Result.Ok();
			}
			catch (Exception ex)
			{
				return Result.Faild($"Failed to delete the file : {ex.Message}");
			}
		}

		public (Stream Stream, string ContentType)? GetFile(string fileName, string folderName)
		{
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return null;	

			var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);
			if (!File.Exists(fullPath)) return null;

			var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
			var extention = Path.GetExtension(fullPath).ToLower();
			var contentType = extention switch
			{
				".png" => "image/png",
				".jpg" or ".jpeg" => "image/jpeg",
				_ => "application/octet-stream"
			};
			return (stream, contentType);
		}

		public async Task<Result<string>?> UploadAsync(Stream fileStream, string folderName, string fileName, CancellationToken ct = default)	
		{

		  	if (fileStream is null || !fileStream.CanRead) return Result<string>.Faild("Failed to read the file");
			if (fileStream.Length == 0 || fileStream.Length > maxSize) return Result<string>.Faild("The file has exceeded tha maximum file size");

			var extention = Path.GetExtension(fileName);
			if (string.IsNullOrEmpty(extention) || !allowedExtentions.Contains(extention)) return Result<string>.Faild("Wrong extention");

			var uploadsFolder = Path.Combine(env.ContentRootPath, folderName);
			Directory.CreateDirectory(uploadsFolder);

			var storedFileName = $"{Guid.NewGuid()}{fileName}";
			var filePath = Path.Combine(uploadsFolder, storedFileName);

			try
			{
				await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
				await fileStream.CopyToAsync(fs, ct);
				return Result<string>.Ok(storedFileName);
			}
			catch (Exception ex)
			{
				return Result<string>.Faild($"Failed to upload the file : {ex}");
			}

		}
	}
}
