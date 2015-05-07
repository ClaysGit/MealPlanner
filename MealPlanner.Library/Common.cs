using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MealPlanner.Library
{
	public class Serializer : IMealPlanDataProvider
	{
		public List<MealOption> GetMealOptions()
		{
			return LoadFile<List<MealOption>>( MealOptionsFilename );
		}

		public MealPlan GetMealPlan()
		{
			return LoadFile<MealPlan>( MealPlanFilename );
		}

		public MealPlannerConfiguration GetConfiguration()
		{
			return LoadFile<MealPlannerConfiguration>( MealPlannerConfigurationFileName );
		}

		public void SetMealOptions( List<MealOption> mealOptions )
		{
			SaveFile( MealOptionsFilename, mealOptions );
		}

		public void SetMealPlan( MealPlan mealPlan )
		{
			SaveFile( MealPlanFilename, mealPlan );
		}

		public void SetConfiguration( MealPlannerConfiguration configuration )
		{
			SaveFile( MealPlannerConfigurationFileName, configuration );
		}

		private T LoadFile<T>( string filename )
			where T : new()
		{
			var xmlSerializer = new XmlSerializer( typeof( T ) );
			var filePath = GetFullPath( filename );

			try
			{
				using ( var streamReader = new StreamReader( filePath ) )
				{
					var obj = (T)xmlSerializer.Deserialize( streamReader );
					return obj;
				}
			}
			catch ( FileNotFoundException )
			{
				using ( var streamWriter = new StreamWriter( filePath ) )
				{
					var newObj = new T();

					xmlSerializer.Serialize( streamWriter, newObj );
					return newObj;
				}
			}
		}

		private void SaveFile<T>( string filename, T obj )
		{
			var xmlSerializer = new XmlSerializer( typeof( T ) );
			var filePath = GetFullPath( filename );

			using ( var streamWriter = new StreamWriter( filePath ) )
			{
				xmlSerializer.Serialize( streamWriter, obj );
			}
		}

		private string GetFullPath( string filename )
		{
			var folderPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), AppFolder );
			if ( !Directory.Exists( folderPath ) )
			{
				Directory.CreateDirectory( folderPath );
			}

			return Path.Combine( folderPath, filename );
		}

		private const string AppFolder = "MealPlanner";
		private const string MealOptionsFilename = "mealoptions.xml";
		private const string MealPlanFilename = "mealplan.xml";
		private const string MealPlannerConfigurationFileName = "mealplanner.config.xml";
	}
}
