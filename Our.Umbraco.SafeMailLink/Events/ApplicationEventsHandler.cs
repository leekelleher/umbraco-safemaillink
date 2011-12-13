using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Our.Umbraco.SafeMailLink.Modules;
using umbraco.BusinessLogic;

namespace Our.Umbraco.SafeMailLink.Events
{
	public class ApplicationEventsHandler : ApplicationBase
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