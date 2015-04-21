using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.Administration;

using MealPlanner.Library;

namespace MealPlanner.Management
{
    class WebServiceManager
    {
        public void EnsureWebService()
        {
            var serverManager = new ServerManager();
            ApplicationPool appPool;
            Site site;
            var appPoolChanged = EnsureAppPool(serverManager, out appPool);
            var siteChanged = EnsureSite(serverManager, out site);
            var needsCycle = appPoolChanged || siteChanged;

            if (needsCycle)
            {
                if (!(site.State == ObjectState.Stopped))
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
            appPool = serverManager.ApplicationPools.SingleOrDefault(p => String.Equals(p.Name, appPoolName));

            if (appPool == null)
            {
                serverManager.ApplicationPools.Add(appPoolName);
                appPool = serverManager.ApplicationPools.Single(p => String.Equals(p.Name, appPoolName));

                return true;
            }

            return false;
        }

        private bool EnsureSite(ServerManager serverManager, out Site site)
        {
            var siteName = WebServiceName + SiteSuffix;
            site = serverManager.Sites.SingleOrDefault(s => String.Equals(s.Name, siteName));

            if (site == null)
            {
                var config = new Serializer().GetConfiguration();
                serverManager.Sites.Add(siteName, "http", String.Format("*:{0}", config.WebService.Port), config.WebDeploymentPath);

                site = serverManager.Sites.Single(s => String.Equals(s.Name, siteName));

                return true;
            }

            return false;
        }

        private const string WebServiceName = "MealPlanner";
        private const string AppPoolSuffix = "AppPool";
        private const string SiteSuffix = "Site";
    }
}
