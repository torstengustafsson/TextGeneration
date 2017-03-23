using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TextGeneration.UWP;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplementation))]

namespace TextGeneration.UWP
{
    public class TextToSpeechImplementation : ITextToSpeech
    {
        public TextToSpeechImplementation() { }

        public async void Speak(string text)
        {
            var mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }

    public class LoadingBar : ILoadingBar
    {
        static int counter = 0;
        //static ProgressBar progBar = null;

        public void SetNumTasks(int n)
        {
            counter = n;
        }

        public async Task<string> LoadText(string fileuri)
        {
            //if (progBar == null)
            //{
            //    progBar = new ProgressBar();
            //}

            //string text = await new WebClient().DownloadStringTaskAsync(fileuri);
            WebRequest request = WebRequest.Create(fileuri);
            request.Credentials = CredentialCache.DefaultCredentials;
            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            WebResponse response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string text = reader.ReadToEnd();

            return text;
        }

        Task<string> ILoadingBar.LoadText(string fileuri)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadingBar loader = new LoadingBar();
            LoadApplication(new TextGeneration.App(loader));
        }
    LoadingBar loader = new LoadingBar();}
}
