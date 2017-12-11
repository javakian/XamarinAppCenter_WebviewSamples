using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace XamarinAppCenter_WebviewSamples.UITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{

		private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(20);

		IApp app;
		Platform platform;


		string WebviewName = "WebView"; // defaulting to Android


		public Tests(Platform platform)
		{
			this.platform = platform;
		}


		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);

		}


		/// <summary>
		/// Set the name of our webview.
		/// It appears there is a difference between Webview names based on the OS, and if we're using Xamarin Forms.
		/// </summary>
		public void SetWebViewName()
		{

			if (platform == Platform.iOS)
			{
				// This will have a different name if we aren't using Forms.  Try playing with app.repl(), then type "tree" (no quotes) into the command prompt
				WebviewName = "Xamarin_Forms_Platform_iOS_WebViewRenderer";
				return;
			}

			if (platform == Platform.Android)
			{
				WebviewName = "WebView";
				return;
			}

		}

		/// <summary>
		/// Some phones have a 'Done' button in their pickers, try clicking on it
		/// </summary>
		public void CheckForDoneButton()
		{
			AppResult[] results = app.Query(c => c.Marked("Done"));
			if (results.Any())
				app.Tap(e => e.Marked("Done"));

		}


		/// <summary>
		/// Load the sample page by scrolling (OS specific) picker down to correct test
		/// </summary>
		/// <param name="testName">Test name.</param>
		public void ScrollPickerToTest(string pickerName, string testName)
		{
			// Click on the picker
			app.Tap(x => x.Marked(pickerName));

			// Try clicking on the test right away, if we see it
			AppResult[] results = app.Query(c => c.Marked(testName));
			if (results.Any())
			{
				app.Tap(e => e.Marked(testName));
			}
			else {
			
				// Lets start by scrolling down the list, if we can't find it we'll try going back up the list
				// (maybe the test we want is higher then our current pose)
				try
				{
					// Look for the test by scrolling down
					if (platform == Platform.iOS)
						app.ScrollDownTo(z => z.Marked(testName), x => x.Class("UIPickerTableView").Index(0), timeout: DefaultTimeout);
					else
						app.ScrollDownTo(testName, withinMarked: "select_dialog_listview");
				}
				catch 
				{
					// Didn't find it going down, lets try going up now...
					try
					{
						// Look for the test by scrolling up
						if (platform == Platform.iOS)
							app.ScrollUpTo(z => z.Marked(testName), x => x.Class("UIPickerTableView").Index(0), timeout: DefaultTimeout);
						else
							app.ScrollUpTo(testName, withinMarked: "select_dialog_listview");
					}
					catch (Exception exTriedUp)
					{
						// Tried going both directions... lets give up :(
						throw exTriedUp;
					}
				}

				// Click on the test
				app.Tap(x => x.Text(testName));
			}

			CheckForDoneButton();

		}


		[Test]
		public void _AllTests()
		{
			
			Textboxes();

			Buttons();

			Dropdown();

			DatePicker();

			AjaxRequest();

			RadioButtons();

			Checkboxes();

			jQueryColorpicker();

			jQuerySpinner();

			Agreement();

			EmbeddedVideo();

			References();
		}


		/// <summary>
		/// Test for interacting with Textboxes
		/// </summary>
		[Test]
		public void Textboxes()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Textboxes");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#text2"), timeout: DefaultTimeout);

			// Ensure we can see the object
			app.ScrollDownTo(c => c.Class(WebviewName).Css("#text2"));

			app.Screenshot("Before Textbox Test");

			// Focus it
			app.Tap(c => c.Class(WebviewName).Css("#text2"));

			// Clear any text thats already there
			if (platform == Platform.iOS)
				app.EnterText(x => x.Class(WebviewName).Css("#text2"), "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b");
			else 
				app.ClearText(); // Android
			
			// Enter some new text
			app.EnterText(c => c.Class(WebviewName).Css("#text2"), "Hello :)");
			app.DismissKeyboard();

			Thread.Sleep(500);
			app.Screenshot("After Textbox Test");
		}


		/// <summary>
		/// Test for interacting with Buttons
		/// </summary>
		[Test]
		public void Buttons()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Buttons");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#divControls"), timeout: DefaultTimeout);

			// Ensure we can see the object
			app.ScrollDownTo(c => c.Css("#divControls"));
			app.Screenshot("Before Button Click");

			// 	Click on the button to change the colors
			app.Tap(c => c.Css("#btnGetRandomColors").Index(0));

			Thread.Sleep(500);
			app.Screenshot("After Button Click");
		}


		/// <summary>
		/// Test for interacting with Dropdowns / Select Menus
		/// </summary>
		[Test]
		public void Dropdown()
		{

			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Dropdown/Select Menu");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#selectColor"), timeout: DefaultTimeout);

			app.Screenshot("Before Dropdown/Select Tests");

			// =========================================================
			// Dropdown / Select - Color ===============================
			// Scroll down until you find the Color Dropdown
			app.ScrollDownTo(c => c.Css("#selectColor"));

			// Click on the dropdown
			app.Tap(c => c.Css("#selectColor").Index(0));

			// Select Purple
			app.Tap(x => x.Text("Purple"));

			CheckForDoneButton();

			Thread.Sleep(500);
			app.Screenshot("After Dropdown/Select Test - Color Purple");

			// =========================================================
			// Dropdown / Select - Number ==============================
			// Scroll down until you find the Number Dropdown
			app.ScrollDownTo(c => c.Css("#number"));

			// Click on the dropdown
			app.Tap(c => c.Css("#number").Index(0));

			// Select 3
			app.Tap(x => x.Text("3"));

			CheckForDoneButton();

			Thread.Sleep(500);
			app.Screenshot("After Dropdown/Select Test - Number 3");

		}


		/// <summary>
		/// Test for interacting with Date Pickers
		/// </summary>
		[Test]
		public void DatePicker()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Date Picker");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#datepickerPop"), timeout: DefaultTimeout);

			app.Screenshot("Before Date Selected Tests");

			// =========================================================
			// Calendar - Popup ========================================
			// Scroll down until you find the date popup textbox
			app.ScrollDownTo(c => c.Css("#datepickerPop"));

			// Focus the textbox
			app.Tap(x => x.Class(WebviewName).Css("#datepickerPop"));

			// Hide the keyboard
			app.DismissKeyboard(); // In case it popped up

			Thread.Sleep(500);

			// Select the second week & second day on the popup calendar
			app.Tap(x => x.Class(WebviewName).XPath("//DIV[@id=\"ui-datepicker-div\"]/TABLE/TBODY/TR[2]/TD[2]/A"));

			Thread.Sleep(500);
			app.Screenshot("After Date Selected (Popup) Test");


			// =========================================================
			// Calendar - Inline =======================================
			// Scroll down until you find the inline calendar
			app.ScrollDownTo(c => c.Css("#datepickerInline"));
			app.Tap(x => x.Class(WebviewName).Css("#datepickerInline"));

			Thread.Sleep(500);

			// Select the third week & third day in the inline calendar
			app.Tap(x => x.Class(WebviewName).XPath("//DIV[@class=\"ui-datepicker-inline ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all\"]/TABLE/TBODY/TR[3]/TD[3]/A"));

			Thread.Sleep(500);
			app.Screenshot("After Date Selected (Inline) Test");
		}

		/// <summary>
		/// Test for interacting with Ajax/Async Requests
		/// </summary>
		[Test]
		public void AjaxRequest()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Ajax/Async Request");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#githubRepos"), timeout: DefaultTimeout);

			// Ensure we can see the object
			app.ScrollDownTo(c => c.Css("#githubRepos"));
			app.Screenshot("Before Ajax/Async Request Test");


			// Click on the dropdown/select
			app.Tap(x => x.Class(WebviewName).Css("#githubRepos"));

			// Select a Repo
			// todo - add a way to skip over this test if we've hit the github rate limit
			// todo - add check to select something else if less than 3 items
			if (platform == Platform.iOS)
				app.Tap(x => x.Class("UIPickerTableView").Descendant("UILabel").Index(2));

			else
				app.Tap(x => x.Index(2));
				   
			CheckForDoneButton();

			Thread.Sleep(2000); // Give the browser some time to grab the results

			// Select a Stargazer from the Stargazers dropdown
			app.Tap(x => x.Class(WebviewName).Css("#githubReposStargazers"));

			// todo - add check to select something else if less than 3 items
			if (platform == Platform.iOS)
				app.Tap(x => x.Class("UIPickerTableView").Descendant("UILabel").Index(2));

			else
				app.Tap(x => x.Index(2));

			CheckForDoneButton();

			Thread.Sleep(500);
			app.Screenshot("Finished Ajax/Async Request Test");
		}


		/// <summary>
		/// Test for interacting with Radio Buttons
		/// </summary>
		[Test]
		public void RadioButtons()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Radio Buttons");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#radioGroupA3"), timeout: DefaultTimeout);

			// Ensure we can see the object
			app.ScrollDownTo(c => c.Css("#radioGroupA3"));
			app.Screenshot("Before clicking on a radiobutton Test");

			// Change the Radio selection from #1 to #3
			app.Tap(c => c.Css("#radioGroupA3").Index(0));

			Thread.Sleep(500);
			app.Screenshot("After clicking on a radiobutton Test");
		}


		/// <summary>
		/// Test for interacting with Checkboxes
		/// </summary>
		[Test]
		public void Checkboxes()
		{
			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Checkboxes");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#checkboxesA3"), timeout: DefaultTimeout);

			app.Screenshot("Before Checkboxes Test");

			// Ensure we can see the object
			app.ScrollDownTo(c => c.Css("#checkboxesA3"));

			//	Uncheck #1
			app.Tap(c => c.Css("#checkboxesA1").Index(0));

			//	Check #2 & #3
			app.Tap(c => c.Css("#checkboxesA2").Index(0));
			app.Tap(c => c.Css("#checkboxesA3").Index(0));

			Thread.Sleep(500);
			app.Screenshot("Unselected Checkbox1, select the other two");

		}


		/// <summary>
		/// Test for interacting with jQuery's Color picker sample
		/// (https://jqueryui.com/slider/#colorpicker)
		/// </summary>
		[Test]
		public void jQueryColorpicker()
		{

			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "jQuery Colorpicker");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#blue"), timeout: DefaultTimeout);

			// Scroll down until you find the Sliders
			//	(in this case, scroll to the blue one, since its last on the screen)
			app.ScrollDownTo(c => c.Css("#blue"));
			app.Screenshot("Before Color Slider Test");

			// Click on the center of the Red &amp; Blue slider, this will move the sliders to the middle
			app.Tap(x => x.Class(WebviewName).Css("#red"));
			app.Tap(x => x.Class(WebviewName).Css("#blue"));

			Thread.Sleep(500);
			app.Screenshot("After Color Slider Test");

		}


		/// <summary>
		/// Test for interacting with jQuery's Number Spinner sample
		/// (https://jqueryui.com/spinner/)
		/// </summary>
		[Test]
		public void jQuerySpinner()
		{

			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "jQuery Spinner");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#spinnerDemo"), timeout: DefaultTimeout);

			// Scroll down until you find the Spinner
			app.ScrollDownTo(c => c.Css("#spinnerDemo"));
			app.Screenshot("Before Spinner Test");

			// Click the Up arrow twice
			//	Note: Could also be "#spinnerDemo > p > span > a.ui-button.ui-widget.ui-spinner-button.ui-spinner-down.ui-corner-br.ui-button-icon-only"
			//	Various web browsers have developer tools, which are awesome for finding the names/ids/classes of objects created by 3rd party frameworks, like jQuery
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-icon.ui-icon-triangle-1-n"));  // Up - 300 to 301
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-icon.ui-icon-triangle-1-n"));  // Up - 301 to 302

			// Click the Down arrow four times
			//	Note: Could also be "#spinnerDemo > p > span > a.ui-button.ui-widget.ui-spinner-button.ui-spinner-down.ui-corner-br.ui-button-icon-only"
			//	Various web browsers have developer tools, which are awesome for finding the names/ids/classes of objects created by 3rd party frameworks, like jQuery
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-button-icon.ui-icon.ui-icon-triangle-1-s"));   // Down - 302 to 301
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-button-icon.ui-icon.ui-icon-triangle-1-s"));   // Down - 301 to 300
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-button-icon.ui-icon.ui-icon-triangle-1-s"));   // Down - 300 to 299
			app.Tap(x => x.Class(WebviewName).Css("SPAN.ui-button-icon.ui-icon.ui-icon-triangle-1-s"));   // Down - 299 to 298

			Thread.Sleep(500);
			app.DismissKeyboard(); // In case it popped up

			Thread.Sleep(500);
			app.Screenshot("After Spinner Test");

		}


		/// <summary>
		/// Test for interacting with a Scrollable Agreement / Div
		/// </summary>
		[Test]
		public void Agreement()
		{

			//Coming Soon !

			return;

		}


		/// <summary>
		/// Test for interacting with an Embedded Video
		/// </summary>
		[Test]
		public void EmbeddedVideo()
		{

			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Embedded Video / iFrame");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#embeddedvideo"), timeout: DefaultTimeout);

			app.Screenshot("Before Embedded Video Test");

			// Scroll down until you find the embedded video
			app.ScrollDownTo(c => c.Css("#embeddedvideo"));

			// Click on the embeeded window frame
			app.Tap(c => c.Css("#embeddedvideo").Index(0));

			if (platform == Platform.iOS)
			{
				app.Screenshot("Embedded Video Playing");
				Thread.Sleep(3000); // Watch the video for 3 seconds
				app.Tap(x => x.Marked("ExitFullScreenButton")); // Exit full screen mode
			}

			Thread.Sleep(2000);
			app.Screenshot("After Embedded Video Test");

		}


		/// <summary>
		/// Test for displaying any references used in the making of this sample
		/// </summary>
		[Test]
		public void References()
		{

			SetWebViewName();

			// Scroll the test picker to the correct test
			ScrollPickerToTest("myPicker", "Code & Document References");

			// Look for our test page to finish loading
			app.WaitForElement(c => c.Class(WebviewName).Css("#refDialog"), timeout: DefaultTimeout);

			app.Screenshot("References");

		}

	}
}

