using System;
using System.Net.Mime;
using System.Web;
using SafeMailLink.Core.Filters;
using SafeMailLink.Core.Utilities;
using umbraco.IO;

namespace Our.Umbraco.SafeMailLink.Modules
{
	public class RegisterFilters : IHttpModule
	{
		public const string InstallKey = "SafeMailLinkModuleInstalled";

		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.ReleaseRequestState += new EventHandler(this.RegisterEncodeMailLinkFilter);
			context.PreSendRequestHeaders += new EventHandler(this.RegisterEncodeMailLinkFilter);
		}

		private void RegisterEncodeMailLinkFilter(object sender, EventArgs e)
		{
			if (HttpContext.Current != null)
			{
				var context = HttpContext.Current;
				if (!context.Items.Contains(InstallKey))
				{
					var response = context.Response;
					var currentExecutionFilePath = context.Request.CurrentExecutionFilePath;

					if ((response.ContentType == MediaTypeNames.Text.Html) && (!this.IsReservedPath(currentExecutionFilePath)))
					{
						var parser = new Parser(response.ContentEncoding);
						var filter = new ResponseFilterStream(response.Filter);
						filter.TransformString += new Func<string, string>(parser.EncodeMailLink);
						response.Filter = filter;
					}

					context.Items.Add(InstallKey, new object());
				}
			}
		}

		private bool IsReservedPath(string path)
		{
			var reservedPaths = new[]
			{
				SystemDirectories.Base,
				SystemDirectories.Bin,
				SystemDirectories.Config,
				SystemDirectories.Css,
				SystemDirectories.Data,
				SystemDirectories.Install,
				SystemDirectories.MacroScripts,
				SystemDirectories.Masterpages,
				SystemDirectories.Media,
				SystemDirectories.Packages,
				SystemDirectories.Preview,
				SystemDirectories.Scripts,
				SystemDirectories.Umbraco,
				SystemDirectories.Umbraco_client,
				SystemDirectories.Usercontrols,
				SystemDirectories.Webservices,
				SystemDirectories.Xslt
			};

			foreach (var reservedPath in reservedPaths)
			{
				if (path.StartsWith(IOHelper.ResolveUrl(string.Concat(reservedPath, "/"))))
				{
					return true;
				}
			}

			return false;
		}
	}
}