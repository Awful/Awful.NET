using System;
using System.Collections.Generic;
using System.Text;
using Template10.Mvvm;

namespace Mazui.ViewModels
{
    public class MazuiViewModel : ViewModelBase
    {
        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }
    }
}
