using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationView : ContentView
    {
        public AuthorizationView()
        {
            InitializeComponent();
        }
    }
}