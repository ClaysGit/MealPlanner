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
				appPool = serverManager.ApplicationPools.Single( p => String.Equals( p.Name, appPoolName ) );
				foreach ( var attr in appPool.Attributes )
				{
					try
					{
						Console.WriteLine( String.Format( "Attr '{0}' has value {1}", attr.Name, attr.Value ) );
					}
					catch ( Exception e )
					{
						Console.WriteLine( String.Format( "Attr '{0}' does not have a valid Value.\n\t\tException: {1}", attr.Name, e.Message ) );
					}
				}

				appPool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
				appPool.ProcessModel.

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
