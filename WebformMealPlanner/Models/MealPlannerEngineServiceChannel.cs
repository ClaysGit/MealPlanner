using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace WebformMealPlanner.Models
{
	class MealPlannerEngineServiceChannel
	{
		public MealPlannerEngineServiceChannel( string uri )
		{
			_baseAddress = new Uri( uri );
		}

		public MealPlan GetMealPlan()
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				return channel.GetMealPlan();
			}
		}

		public List<MealOption> GetMealOptions()
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				return channel.GetMealOptions();
			}
		}

		public MealPlannerConfiguration GetConfiguration()
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				return channel.GetConfiguration();
			}
		}

		public void SetMealPlan( MealPlan mealPlan )
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				channel.SetMealPlan( mealPlan );
			}
		}

		public void SetMealOptions( List<MealOption> mealOptions )
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				channel.SetMealOptions( mealOptions );
			}
		}

		public void SetConfiguration( MealPlannerConfiguration configuration )
		{
			using ( WebChannelFactory<IMealPlannerEngineService> cf = new WebChannelFactory<IMealPlannerEngineService>( _baseAddress ) )
			{
				IMealPlannerEngineService channel = cf.CreateChannel();

				channel.SetConfiguration( configuration );
			}
		}

		private Uri _baseAddress;
	}
}
