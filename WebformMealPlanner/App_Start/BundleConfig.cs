﻿using System.Web;
using System.Web.Optimization;

namespace WebformMealPlanner
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles( BundleCollection bundles )
		{
			bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
						"~/Scripts/JQuery/jquery-{version}.js" ) );

			bundles.Add( new ScriptBundle( "~/bundles/jquerydefault" ).Include(
						"~/Scripts/Default/jquery-{version}.js" ) );

			bundles.Add( new ScriptBundle( "~/bundles/jqueryui" ).Include(
						"~/Scripts/Default/jquery-ui-{version}.js" ) );

			bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
						"~/Scripts/Default/jquery.unobtrusive*",
						"~/Scripts/Default/jquery.validate*" ) );

			bundles.Add( new ScriptBundle( "~/bundles/angular" ).Include(
				"~/Scripts/Angular/angular.js",
				"~/Scripts/Angular/*.js"
				) );

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
						"~/Scripts/Default/modernizr-*" ) );

			bundles.Add( new StyleBundle( "~/Content/css" ).Include( "~/Content/site.css" ) );

			bundles.Add( new StyleBundle( "~/Content/themes/base/css" ).Include(
						"~/Content/themes/base/jquery.ui.core.css",
						"~/Content/themes/base/jquery.ui.resizable.css",
						"~/Content/themes/base/jquery.ui.selectable.css",
						"~/Content/themes/base/jquery.ui.accordion.css",
						"~/Content/themes/base/jquery.ui.autocomplete.css",
						"~/Content/themes/base/jquery.ui.button.css",
						"~/Content/themes/base/jquery.ui.dialog.css",
						"~/Content/themes/base/jquery.ui.slider.css",
						"~/Content/themes/base/jquery.ui.tabs.css",
						"~/Content/themes/base/jquery.ui.datepicker.css",
						"~/Content/themes/base/jquery.ui.progressbar.css",
						"~/Content/themes/base/jquery.ui.theme.css" ) );

			bundles.Add( new StyleBundle( "~/Content/bootstrap-css" ).Include(
				"~/Content/bootstrap-3.3.4-dist/css/*.css"
				) );

			bundles.Add( new StyleBundle( "~/Content/bootstrap-css-cerulean" ).Include(
				"~/Content/bootstrap-3.3.4-dist/css/bootstrap-theme.css",
				"~/Content/themes/cerulean.css"
				) );

			bundles.Add( new StyleBundle( "~/Content/bootstrap-css-flatly" ).Include(
				"~/Content/bootstrap-3.3.4-dist/css/bootstrap-theme.css",
				"~/Content/themes/flatly.css"
				) );

			bundles.Add( new ScriptBundle( "~/Content/bootstrap-js" ).Include(
				"~/Content/bootstrap-3.3.4-dist/js/bootstrap.js"
				) );
		}
	}
}