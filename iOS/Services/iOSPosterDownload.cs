using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MovieDownload;

namespace MovieApp.iOS
{
	public class iOSPosterDownload : IPosterDownload
	{
		private ImageDownloader _downloader;

		public iOSPosterDownload()
		{
			_downloader = new ImageDownloader(new StorageClient());
		}

		public async Task<string> DownloadImage(string path)
		{
			var localPath = "";
			if (path != null)
			{
				localPath = _downloader.LocalPathForFilename(path);
				var token = new CancellationToken();
				if (!File.Exists(localPath))
				{
					await _downloader.DownloadImage(path, localPath, token);
				}
			}
			return localPath;
		}
	}
}
