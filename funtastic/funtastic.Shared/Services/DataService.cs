using funtastic.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace funtastic.Services
{
    public class DataService
    {
        // Current in memory items
        private MobileServiceCollection<Joke, Joke> best;

        // Table items (both local and remote)
        private IMobileServiceSyncTable<Joke> jokeTable;

        private static DataService instance;

        public bool IsInitialized { get; set; }

        private DataService()
        {
            jokeTable = App.MobileService.GetSyncTable<Joke>();
        }

        public static DataService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataService();
                }
                return instance;
            }
        }

        public async Task Initialize()
        {
            if (!IsInitialized)
            {
                await InitLocalStoreAsync();
                await RefreshJokes();
                IsInitialized = true;
            }
        }

        public void RegisterDataSources(ref MobileServiceCollection<Joke, Joke> bestRef)
        {
            if (!IsInitialized)
            {
                throw new Exception("This may not be called before initialization!)");
            }

            bestRef = best;
        }

        public async Task AddJoke(Joke item)
        {
            await jokeTable.InsertAsync(item);
            best.Add(item);

            await SyncAsync(); // offline sync
        }

        public async Task RateJoke(Joke item)
        {

        }
        public async Task ReportJoke(Joke item)
        {

        }


        private async Task RefreshJokes()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                best = await jokeTable
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
        }

        #region Offline Sync

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<Joke>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }

            await SyncAsync();
        }

        private async Task SyncAsync()
        {
            String errorString = null;

            try
            {
                await App.MobileService.SyncContext.PushAsync();
                await jokeTable.PullAsync("joke", jokeTable.CreateQuery()); // first param is query ID, used for incremental sync
            }

            catch (MobileServicePushFailedException ex)
            {
                errorString = "Push failed because of sync errors: " +
                  ex.PushResult.Errors.Count + " errors, message: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorString = "Pull failed: " + ex.Message +
                  "\n\nIf you are still in an offline scenario, " +
                  "you can try your Pull again when connected with your Mobile Serice.";
            }

            if (errorString != null)
            {
                MessageDialog d = new MessageDialog(errorString);
                await d.ShowAsync();
            }
        }

        #endregion
    }
}
