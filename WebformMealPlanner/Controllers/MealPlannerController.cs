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

		//
		// GET: /MealPlanner/

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public JsonResult GetMealPlan()
		{
			return Json( _repository.GetMealPlanViewModel() );
		}

		[HttpPost]
		public JsonResult GetMealOptions()
		{
			return Json( _repository.GetMealOptionsViewModel() );
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
		public JsonResult RemoveMealPlanDay( string day )
		{
			return Json( _repository.RemoveMealPlanDay( day ) );
		}

		[HttpPost]
		public JsonResult RemoveMealOption( string name )
		{
			return Json( _repository.RemoveMealOption( name ) );
		}

		private MealPlannerRepository _repository;
	}
}
