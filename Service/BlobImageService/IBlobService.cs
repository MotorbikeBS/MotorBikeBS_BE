using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BlobImageService
{
	public interface IBlobService
	{
		Task<string> GetBlob(string blobName, string containerName);
		Task<bool> DeleteBlob(string blobName, string containerName);
		Task<string> UploadBlob(string blobName, string containerName, IFormFile file);
	}
}
