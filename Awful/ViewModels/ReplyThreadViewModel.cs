﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.ViewModels
{
    public class ReplyThreadViewModel : NewPostBaseViewModel
    {
        public SmiliesViewModel SmiliesViewModel { get; set; }
        public PreviewViewModel PreviewViewModel { get; set; }
        public PreviousPostsViewModel PreviousPostsViewModel { get; set; }

    }
}