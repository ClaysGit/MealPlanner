using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MealPlanner.Library;

namespace SimpleMealPlanner
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			LoadData();
		}

		private void LoadData()
		{
			_mealOptions = new Serializer().GetMealOptions();
			_mealPlan = new Serializer().GetMealPlan();

			MealOptionsDropdown.ItemsSource = new ObservableCollection<MealOption>( _mealOptions );
			MealOptionsDropdown.SelectedIndex = 0;
			MealPlanDisplayer.ItemsSource = new ObservableCollection<MealPlanDay>( _mealPlan.MealPlanDays );
		}

		private List<MealOption> _mealOptions;
		private MealPlan _mealPlan;

		private void EditMealOption_Click( object sender, RoutedEventArgs e )
		{
			var form = new EditMealOption();
			form.InitlializeForUse( _mealOptions );
			form.ShowDialog();
			var mealOptionsFormOutput = form.GetModel();

			if ( mealOptionsFormOutput.SaveAction )
			{
				new Serializer().SetMealOptions( _mealOptions );
				LoadData();
			}
		}

		private void AddDay_Click( object sender, RoutedEventArgs e )
		{
			var mealPlanDaysList = _mealPlan.MealPlanDays.ToList();
			var lastDate = mealPlanDaysList.Any() ? mealPlanDaysList.Max( m => m.Day ) : DateTime.Now;
			var nextDate = lastDate + new TimeSpan( 1, 0, 0, 0 );
			mealPlanDaysList.Add( new MealPlanDay
				{
					Breakfast = (MealOption)MealOptionsDropdown.SelectedItem,
					Day = nextDate
				} );
			_mealPlan.MealPlanDays = mealPlanDaysList;
			new Serializer().SetMealPlan( _mealPlan );

			LoadData();
		}
	}
}
