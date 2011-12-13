using System;
using Our.Umbraco.SafeMailLink.Filters;
using umbraco;
using umbraco.BusinessLogic;

namespace Our.Umbraco.SafeMailLink.Events
{
	public class ApplicationEventsHandler : ApplicationBase
	{
		public ApplicationEventsHandler()
		{
			UmbracoDefault.BeforeRequestInit += new UmbracoDefault.RequestInitEventHandler(this.UmbracoDefault_BeforeRequestInit);
		}

		protected void UmbracoDefault_BeforeRequestInit(object sender, RequestInitEventArgs e)
		{
			var transformation = new Transformation(e.Context.Response.ContentEncoding);
			
			var filter = new ResponseFilterStream(e.Context.Response.Filter);
			filter.TransformString += new Func<string, string>(transformation.EncodeMailLink);

			e.Context.Response.Filter = filter;
		}
	}
}