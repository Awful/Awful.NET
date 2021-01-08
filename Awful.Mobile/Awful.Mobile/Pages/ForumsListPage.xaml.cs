using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Awful.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForumsListPage : AwfulPage
    {
        public ForumsListPage()
        {
            InitializeComponent();
            this.BindingContext = App.Container.Resolve<ForumsListViewModel>();
        }
    }
}