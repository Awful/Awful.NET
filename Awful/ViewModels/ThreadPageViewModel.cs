using Awful.Controls;
using Awful.Parser.Models.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.ViewModels
{
    public class ThreadPageViewModel : ThreadBaseViewModel
    {
        public ThreadView ThreadView { get; set; }

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public async Task Init(Thread thread)
        {
            Selected = thread;
            App.ShellViewModel.Header = Selected.Name;
            LoginUser();
            await ThreadView.LoadThread(Selected);
        }
    }
}
