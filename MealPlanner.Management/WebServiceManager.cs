using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.Administration;

namespace MealPlanner.Management
{
	class WebServiceManager
	{
		public void EnsureWebService()
		{
			var serverManager = new ServerManager();
			ApplicationPool appPool;
			Site site;
			var appPoolChanged = EnsureAppPool( serverManager, out appPool );
			var siteChanged = EnsureSite( serverManager, out site );
			var needsCycle = appPoolChanged || siteChanged;

			if ( needsCycle )
			{
				if ( ! (site.State == ObjectState.Stopped ) )
				{
					site.Stop();
				}

				serverManager.CommitChanges();

				site.Start();
				appPool.Recycle();
			}
		}

		private bool EnsureAppPool(ServerManager serverManager, out ApplicationPool appPool)
		{
			var appPoolName = WebServiceName + AppPoolSuffix;
			appPool = serverManager.ApplicationPools.SingleOrDefault(p => String.Equals( p.Name, appPoolName ));

			if ( appPool == null )
			{
				serverManager.ApplicationPools.Add( appPoolName );


				return true;
			}

			return false;
		}

		private bool EnsureSite( ServerManager serverManager, out Site site )
		{
			site = null;
			return true;
		}

		private const string WebServiceName = "MealPlanner";
		private const string AppPoolSuffix = "AppPool";
		private const string SiteSuffix = "Site";
	}
}
