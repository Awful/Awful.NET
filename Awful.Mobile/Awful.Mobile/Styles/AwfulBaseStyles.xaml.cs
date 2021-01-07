// <copyright file="AwfulBaseStyles.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Awful.Mobile.Styles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AwfulBaseStyles : ResourceDictionary
    {
        public AwfulBaseStyles()
        {
            InitializeComponent();
        }
    }
}