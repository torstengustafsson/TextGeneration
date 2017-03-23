using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech.Tts;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using TextGeneration.Droid;
using Android.Graphics;

[assembly: Dependency(typeof(TextToSpeechImplementation))]
[assembly: Dependency(typeof(LoadingBar))]

namespace TextGeneration.Droid
{
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public TextToSpeechImplementation() { }

        public void Speak(string text)
        {
            var ctx = Forms.Context; // useful for many Android SDK features
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(ctx, this);
            }
            else
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
                Console.WriteLine("fhgdsgahfgdfdh");
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }
        #endregion
    }

    public class LoadingBar : ILoadingBar
    {
        static int counter = 0;
        static ProgressDialog dialog = null;

        public void SetNumTasks(int n)
        {
            counter = n;
        }

        public async Task<string> LoadText(string fileuri)
        {
            if (dialog == null)
            {
                dialog = new ProgressDialog(Xamarin.Forms.Forms.Context);
                dialog.SetTitle("Download");
                dialog.SetMessage("Downloading File");
            }

            if (!dialog.IsShowing) { dialog.Show(); }
            string text = await new WebClient().DownloadStringTaskAsync(fileuri);

            if (--counter <= 0) { dialog.Dismiss(); }

            return text;
        }
    }

    [Activity (Label = "TextGeneration", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);
            
            Forms.Init (this, bundle);
			LoadApplication (new TextGeneration.App ());
		}
    }
}

