using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Our.Umbraco.SafeMailLink.Events;
using Our.Umbraco.SafeMailLink.Modules;

[assembly: PreApplicationStartMethod(typeof(ApplicationEventsHandler), "RegisterModules")]

namespace Our.Umbraco.SafeMailLink.Events
{
	public class ApplicationEventsHandler
	{
		private static bool modulesRegistered;

		public ApplicationEventsHandler()
		{
		}

		public static void RegisterModules()
		{
			if (modulesRegistered)
			{
				return;
			}

			DynamicModuleUtility.RegisterModule(typeof(RegisterFilters));

			modulesRegistered = true;
		}
	}
}