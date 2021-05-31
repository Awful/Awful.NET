# Awful.Core

Awful.Core is a cross-platform library for accessing the Something Awful forums, based on [.NET](https://dotnet.microsoft.com).

## An Unofficial Library

This library is not endorsed by Something Awful.

## Awful.Mobile

If you're looking for the previous Mobile app that was hosted here, it's now located [here](https://github.com/drasticactions/Awful.Mobile).

## Build

Windows

- Download [Visual Studio](https://visualstudio.microsoft.com)
- For Workloads, select:
  - .NET Core
- After installing, open `Awful.sln`

Mac

- Download [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/)
- After installing, open `Awful.sln`

---

## Awful.Core

Awful.Core is the main entry point for interacting with Something Awful. It handles the requests to Something Awful (`AwfulClient` and `Manager`) and parsing the HTML and JSON endpoints to return to the caller (`Handler').

## Awful.Console

Awful.Console is a playground for testing the Awful Managers. It is useful if you want to test making a backend change in a manager without needing to write a unit test or run the app.

## Awful.Test

Awful.Test are unit tests for the underlying backend libraries. Currently, it targets Awful.Core.
