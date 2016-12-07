using System;
using System.Threading.Tasks;

namespace MovieApp
{
	public interface IPosterDownload
	{
		Task<string> DownloadImage(string path);
	}
}
