using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MealPlanner.Library;

namespace SimpleMealPlanner
{
	/// <summary>
	/// Interaction logic for MealOption.xaml
	/// </summary>
	public partial class EditMealOption : Window
	{
		public EditMealOption()
		{
			InitializeComponent();
		}

		public void InitlializeForUse( List<MealOption> currentOptions )
		{
			Model = new EditMealOptionModel();
			Model.FullOutput = currentOptions;
			Model.SaveAction = false;

			IngredientsListBox.ItemsSource = Model.FullOutput;
		}

		public EditMealOptionModel GetModel()
		{
			return Model;
		}

		private EditMealOptionModel Model { get; set; }
		private MealOption EditingModel { get; set; }

		private void IngredientTextBox_KeyDown( object sender, KeyEventArgs e )
		{
			//Add ingredient
			if ( e.Key == Key.Enter )
			{

			}
		}

		private void IngredientsListBox_MouseDoubleClick( object sender, MouseButtonEventArgs e )
		{
			//Remove selected ingredient
		}

		private void MealOptionListBox_MouseDoubleClick( object sender, MouseButtonEventArgs e )
		{
			//Edit selected meal
		}
	}
}
