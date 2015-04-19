using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MealPlanner.Library;

namespace SimpleMealPlanner
{
	public class EditMealOptionModel
	{
		public bool SaveAction { get; set; }

		public List<MealOption> FullOutput { get; set; }
	}
}
