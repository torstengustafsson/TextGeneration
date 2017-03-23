using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TextGeneration
{
	public partial class MainPage : ContentPage
	{
        public static ILoadingBar loader;

        public static bool lotrChecked = true;
        public static bool hpChecked = false;
        public static bool stormChecked = false;

        public MainPage(ILoadingBar l)
        {
            InitializeComponent();

            lengthSlider.Minimum = 0;
            lengthSlider.Maximum = 100;
            lengthSlider.Value = 20;

            nSlider.Minimum = 0;
            nSlider.Maximum = 20;
            nSlider.Value = 4;

            var lotrRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() => {
                    checkLOTR.Source = lotrChecked ? ImageSource.FromFile("CheckboxUnchecked.png") : ImageSource.FromFile("CheckboxChecked.png");
                    lotrChecked = !lotrChecked;
                })
            };
            var hpRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() => {
                    checkHP.Source = hpChecked ? ImageSource.FromFile("CheckboxUnchecked.png") : ImageSource.FromFile("CheckboxChecked.png");
                    hpChecked = !hpChecked;
                })
            };
            var stormRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() => {
                    checkStorm.Source = stormChecked ? ImageSource.FromFile("CheckboxUnchecked.png") : ImageSource.FromFile("CheckboxChecked.png");
                    stormChecked = !stormChecked;
                })
            };

            checkLOTR.GestureRecognizers.Add(lotrRecognizer);
            checkHP.GestureRecognizers.Add(hpRecognizer);
            checkStorm.GestureRecognizers.Add(stormRecognizer);

            loader = l;
        }
    }

    class MainPageViewModel : INotifyPropertyChanged
    {
        // ICommand implementations
        public ICommand SetText { protected set; get; }
        public ICommand Speak { protected set; get; }

        string text = "";
        string lotr1, lotr2, lotr3, hp1, hp2, hp3, storm1, storm2;


        double sliderLength;
        double sliderN;

        // For displaying slider values
        string sliderLength_S;
        string sliderN_S;

        // sliderLength and sliderN need to be converted and stored as int
        int actualLengthValue;
        int actualNValue;

        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public MainPageViewModel()
        {
            SetText = new Command(async () =>
            {
                string url = "https://raw.githubusercontent.com/torstengustafsson/TextGeneration/master/text/";

                if (MainPage.lotrChecked || MainPage.hpChecked || MainPage.stormChecked)
                {
                    if (MainPage.lotrChecked && (lotr1 == null || lotr2 == null || lotr3 == null))
                    {
                        // Load lord of the rings texts
                        try
                        {
                            MainPage.loader.SetNumTasks(3); // For progress dialog
                            lotr1 = await DependencyService.Get<ILoadingBar>().LoadText(url + "lotr/lordofrings.txt");
                            lotr2 = await DependencyService.Get<ILoadingBar>().LoadText(url + "lotr/twotowers.txt");
                            lotr3 = await DependencyService.Get<ILoadingBar>().LoadText(url + "lotr/returnofking.txt");
                        }
                        catch (Exception e) { Debug.WriteLine(e.ToString()); }
                    }
                    if (MainPage.hpChecked && (hp1 == null || hp2 == null || hp3 == null))
                    {
                        // Load harry potter texts
                        try
                        {
                            MainPage.loader.SetNumTasks(3); // For progress dialog
                            hp1 = await DependencyService.Get<ILoadingBar>().LoadText(url + "harrypotter/hp1.txt");
                            hp2 = await DependencyService.Get<ILoadingBar>().LoadText(url + "harrypotter/hp2.txt");
                            hp3 = await DependencyService.Get<ILoadingBar>().LoadText(url + "harrypotter/hp3.txt");
                        }
                        catch (Exception e) { Debug.WriteLine(e.ToString()); }
                    }
                    if (MainPage.stormChecked && (storm1 == null || storm2 == null))
                    {
                        // Load stormbringer texts
                        try
                        {
                            MainPage.loader.SetNumTasks(2); // For progress dialog
                            storm1 = await DependencyService.Get<ILoadingBar>().LoadText(url + "stormbringer/stormbringer.txt");
                            storm2 = await DependencyService.Get<ILoadingBar>().LoadText(url + "stormbringer/elricofmelniborne.txt");
                        }
                        catch (Exception e) { Debug.WriteLine(e.ToString()); }
                    }
                }
                else { return; } // Return if nothing is selected

                List<string> texts = new List<string>();

                if (MainPage.lotrChecked)
                {
                    texts.Add(lotr1);
                    texts.Add(lotr2);
                    texts.Add(lotr3);
                }
                if (MainPage.hpChecked)
                {
                    texts.Add(hp1);
                    texts.Add(hp2);
                    texts.Add(hp3);
                }
                if (MainPage.stormChecked)
                {
                    texts.Add(storm1);
                    texts.Add(storm2);
                }

                Text = TextGenerator.GenerateText(actualLengthValue, actualNValue, texts);
            });

            Speak = new Command(() =>
            {
                if (text != null && text != "")
                {
                    DependencyService.Get<ITextToSpeech>().Speak(text);
                }
                else
                {
                    DependencyService.Get<ITextToSpeech>().Speak("You must generate a text first");
                }
            });
        }

        public string Text
        {
            protected set
            {
                if (text != value)
                {
                    text = value ?? text;
                    OnPropertyChanged("Text");
                }
            }

            get { return text; }
        }

        public string SliderLength_S
        {
            protected set
            {
                if (sliderLength_S != value)
                {
                    sliderLength_S = value ?? sliderLength_S;
                    OnPropertyChanged("SliderLength_S");
                }
            }

            get { return sliderLength_S; }
        }

        public string SliderN_S
        {
            protected set
            {
                if (sliderN_S != value)
                {
                    sliderN_S = value ?? sliderN_S;
                    OnPropertyChanged("SliderN_S");
                }
            }

            get { return sliderN_S; }
        }

        public double SliderLength
        {
            get { return sliderLength; }
            set
            {
                sliderLength = value;
                actualLengthValue = sliderLength > 5 ? (int)Math.Round(sliderLength) : 5;
                SliderLength_S = "Num Ngrams = " + actualLengthValue;
                OnPropertyChanged("SliderLength");
            }
        }

        public double SliderN
        {
            get { return sliderN; }
            set
            {
                sliderN = value;
                actualNValue = sliderN > 1 ? (int)Math.Round(sliderN) : 1;
                SliderN_S = "N-Value = " + actualNValue;
                OnPropertyChanged("SliderN");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }
    }
}
