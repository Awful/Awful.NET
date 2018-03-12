using System;
using Awful.Database.Context;
using Awful.Services;
using Awful.ViewModels;
using Microsoft.EntityFrameworkCore;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Awful
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public static ShellViewModel ShellViewModel { get; set; }

        public App()
        {
            InitializeComponent();

            #region Database
            using (var db = new UserAuthContext())
            {
                db.Database.Migrate();
            }
            using (var db = new ForumsContext())
            {
                db.Database.Migrate();
            }
            #endregion

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
