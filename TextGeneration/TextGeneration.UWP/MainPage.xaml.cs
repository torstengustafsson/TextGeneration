using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
[assembly: Xamarin.Forms.Dependency(typeof(LoadingBar))]

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

        public void SetNumTasks(int n)
        {
            counter = n;
        }

        public async Task<string> LoadText(string fileuri)
        {
            Debug.WriteLine("hejhejhej");
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(fileuri);
            string text = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("tex = " + text);
            return text;
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
