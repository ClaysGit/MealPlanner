using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebformMealPlanner.Models;

namespace WebformMealPlanner.Controllers
{
	public class MealPlannerController : Controller
	{
		public MealPlannerController()
		{
			_repository = new MealPlannerRepository();
		}

		public ActionResult Index()
		{
            return View(_repository.GetDefaultEngineViewModel());
		}

        [HttpPost]
        public JsonResult GetIndexViewModel()
        {
            return Json(_repository.GetIndexViewModel());
        }

		[HttpPost]
		public JsonResult AddMealPlanDay( MealPlanDayPersistModel day )
		{
			return Json( _repository.AddMealPlanDay( day ) );
		}

		[HttpPost]
		public JsonResult AddMealOption( MealOptionViewPersistModel mealOption )
		{
			return Json( _repository.AddMealOption( mealOption ) );
		}

        [HttpPost]
        public JsonResult SetMealPlannerConfiguration(MealPlannerConfigurationViewPersistModel configuration)
        {
            return Json(_repository.SetMealPlannerConfiguration(configuration));
        }

		[HttpPost]
		public JsonResult RemoveMealPlanDay( JavascriptDateTime day )
		{
			return Json( _repository.RemoveMealPlanDay( day ) );
		}

		[HttpPost]
		public JsonResult RemoveMealOption( string name )
		{
			return Json( _repository.RemoveMealOption( name ) );
		}

        [HttpPost]
        public EmptyResult SetDefaultEngineConfiguration(DefaultEngineViewPersistModel configuration)
        {
            _repository.SetDefaultEngineConfiguration(configuration);

            return new EmptyResult();
        }

		private MealPlannerRepository _repository;
	}
}
