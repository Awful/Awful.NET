// <copyright file="AwfulContainer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Awful.Database.Context;
using Awful.UI.ViewModels;

namespace Awful.UI
{
    public static class AwfulContainer
    {
        public static IContainer BuildContainer(ContainerBuilder builder)
        {
            builder.RegisterType<AwfulContext>().SingleInstance();
            builder.RegisterType<AwfulViewModel>();
            builder.RegisterType<SigninViewModel>();
            //builder.RegisterType<MainViewModel>();

            return builder.Build();
        }
    }
}
