using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TextGeneration
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }

    public interface ILoadingBar
    {
        void SetNumTasks(int n);
        Task<string> LoadText(string fileuri);
    }

    public partial class App : Application
	{
		public App (ILoadingBar loader)
		{
			InitializeComponent();

            MainPage = new MainPage(loader);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
