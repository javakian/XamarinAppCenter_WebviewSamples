using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace XamarinAppCenter_WebviewSamples
{
	public class App : Application
	{
        public bool usingAppCenter = false;

		WebView Webview;
		Picker SamplePicker;

		Dictionary<string, string> WebviewExamples;

		// ##################################################
		// Please replace this with the URL of your sample
		string baseWebURL = "http://www.pachead.com/Xamarin/samples/xtc/XamarinTestCloud_WebviewSamples/HTML/";
        
		/// <summary>
		/// Initializes a new instance of the <see cref="T:XamarinAppCenter_WebviewSamples.App"/> class.
		/// </summary>
		public App()
		{
			
			WebviewExamples = new Dictionary<string, string>{
				{"Textboxes", "textboxes.html"},
				{"Buttons", "buttons.html"},
				{"Dropdown/Select Menu", "dropdownmenus.html"},
				{"Date Picker", "datepicker.html"},
				{"Ajax/Async Request", "ajaxrequest.html"},
				{"Radio Buttons", "radiogroup.html"},
				{"Checkboxes", "checkboxgroup.html"},
				{"jQuery Colorpicker", "colorpicker.html"},
				{"jQuery Spinner", "spinner.html"},
				{"Agreement (scrollable div w/ button)", "scrollingdiv.html"},
				{"Embedded Video / iFrame", "embeddedvideo.html"},
				{"Code & Document References", "references.html"}
			};


			// WEBVIEW =========================================================================
			Webview = new WebView() { 
				AutomationId = "myWebview",
				WidthRequest = 400,
				HeightRequest = 1000
			};


			// SAMPLE PICKER =========================================================================
			SamplePicker = new Picker()
			{
				Title = "Examples",
				AutomationId = "myPicker",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				WidthRequest = 240
				                                 
			};

			// SamplePicker - On SelectedIndexChanged...
			SamplePicker.SelectedIndexChanged += (sender, args) =>
			{
				if (SamplePicker.SelectedIndex == -1)
				{
					SamplePicker.SelectedIndex = 0;
				}
				else
				{
					// Navigate to the sample
					Webview.Source = baseWebURL + WebviewExamples[SamplePicker.Items[SamplePicker.SelectedIndex]];
					System.Diagnostics.Debug.WriteLine("Selected " + baseWebURL + WebviewExamples[SamplePicker.Items[SamplePicker.SelectedIndex]]);
				}
			};


			// Add each example to the picker
			foreach (var item in WebviewExamples){SamplePicker.Items.Add(item.Key);}


			var content = new ContentPage
			{
				Title = "XTC Webview Samples",
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.StartAndExpand,
					Children = {
						new StackLayout
						{
							HorizontalOptions =LayoutOptions.StartAndExpand,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Orientation = StackOrientation.Horizontal,
							Children = {
								new Label{Text="Examples:", VerticalOptions=LayoutOptions.Center},
								SamplePicker
							}
						},
						Webview
					}
				}
			};

			MainPage = new NavigationPage(content);
		}

		/// <summary>
		/// On Start, do the following
		/// </summary>
		protected override void OnStart()
		{
            // Only start appcenter if we have a real key
            if (Keys.AndroidAppCenterKey != "AndroidKey" || Keys.iOSAppCenterKey != "iOSKey" || Keys.UWPAppCenterKey != "UWPKey")
            {
                // Start AppCenter
                AppCenter.Start(
                    "android=" + Keys.AndroidAppCenterKey + ";" + 
                    "uwp={" + Keys.UWPAppCenterKey + "};" +
                    "ios={" + Keys.AndroidAppCenterKey + "}",
                    typeof(Analytics), 
                    typeof(Crashes)
                );
                System.Diagnostics.Debug.WriteLine($"AppCenter - Enabled");
            } else
                System.Diagnostics.Debug.WriteLine($"AppCenter - Disabled");

            // Select the first sample
            SamplePicker.SelectedIndex = 0;
		}

	}
}

