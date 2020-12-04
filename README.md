# Awful.NET

Awful.NET is a cross-platform browser for the Something Awful forums, based on [Xamarin](https://dotnet.microsoft.com/apps/xamarin) and [.NET](https://dotnet.microsoft.com). It's not a replacement for the existing apps available. Instead, it's a way to experiment with getting Something Awful apps on the broadest range of platforms, all under the same codebase.


## An Unofficial App

This app is not endorsed by Something Awful.

## Build

Make sure you clone this repository with the submodules (`--recursive`). If you don't, thread tags will not appear in the app. 

Windows

- Download [Visual Studio](https://visualstudio.microsoft.com)
- For Workloads, select:
  - Xamarin
  - .NET Core
- After installing, open `Awful.sln`
- Visual Studio may prompt to install Android SDKs, install them.
- To run the Android build, select `Awful.Mobile.Droid` in the project selection dropdown, and deploy. It should build and deploy to your device or simulator.
- To run the iOS app, you either need to set up a [Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/) install on a Mac and sync them together, or set up [Hot Restart](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/deploy-test/hot-restart). This does require installing iTunes, however. Select `Awful.Mobile.iOS` and deploy.

Mac

- Download [Visual Studio](https://visualstudio.microsoft.com)
- For Workloads, make sure iOS, Android, and Mac are checked.
- After installing, open `Awful.sln`
- Visual Studio for Mac may prompt to install Android SDKs, install them.
- To run the Android build, select `Awful.Mobile.Droid` in the project selection dropdown, and deploy. It should build and deploy to your device or simulator.
- To run the iOS app, you need XCode installed. If you don't, you will be prompted to install it. Select `Awful.Mobile.iOS` and deploy.

## Awful.Console

Awful.Console is a playground for testing the Awful Managers and web templates. It is useful if you want to test rendering a template or making a backend change in a manager without needing to write a unit test or run the app.

## Awful.Test

Awful.Test are unit tests for the underlying backend libraries. Currently, it targets Awful.Core, Awful.Database, and Awful.UI.
