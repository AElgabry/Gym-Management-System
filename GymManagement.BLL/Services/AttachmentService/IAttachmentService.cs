using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.AttachmentService
{
	public interface IAttachmentService
	{
		Task<Result<string>?> UploadAsync( Stream fileStream , string folderName , string fileName , CancellationToken ct = default);
		Task<Result> DeleteAsync(string folderName, string fileName);
		(Stream Stream, string ContentType)? GetFile(string fileName, string folderName);


	}
}
