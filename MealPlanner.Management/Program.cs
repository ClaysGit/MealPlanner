using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealPlanner.Management
{
	static class Program
	{
		static void Main()
		{
			var webServiceManager = new WebServiceManager();
			webServiceManager.EnsureWebService();
		}
	}
}
