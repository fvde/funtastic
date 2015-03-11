using funtastic.DataModel;
using funtastic.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace funtastic.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private MobileServiceCollection<Joke, Joke> best;
        public MobileServiceCollection<Joke, Joke> Best
        {
            get { return best; }
            set
            {
                best = value;
                NotifyPropertyChanged();
            }
        }

        private String test;
        public String Test
        {
            get { return test; }
            set
            {
                test = value;
                NotifyPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Initialize();
        }

        public async void Initialize()
        {
            await DataService.Instance.Initialize();

            // Needs to be called after Initialization
            DataService.Instance.RegisterDataSources(ref best);
            Best = best;

            Test = "#yolo";
        }
    }
}
