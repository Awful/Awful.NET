![alt text](http://i.imgur.com/iIuWbuj.png "Awful Forums Reader")

# What is this?!?

Awful Forums Reader is a Windows 10 UAP/Windows 8.1/Windows Phone 8.1 App. It allows you to access the [Something Awful](https://forums.somethingawful.com) on a variety of different devices; Table, Phone, Desktop, Xbox, IoT and Hololens. It's really intended for tablets and phones, but it does work well on Desktops and could be extended further.

![alt text](http://i.imgur.com/o96E4UE.png "Awful Forums Reader")

###NOTE!

This version is currently a work in progress. The goal is to modularize Awful into several shared projects, making it easier to use each part in other apps. So if we want to create, say, a iOS or Android app in Xamarin, it will be much easier to do so while sharing code between all the projects. 

# How do I build this?!?

1. Open `mazui.sln` and build the Mazui project.

# What about the 8.1 project?
You can check it out in the [legacy branch](https://github.com/drasticactions/Awful-Forums-Reader/tree/legacy). Same as above; clone Awful Forums Library one directory above that directory, and it should just build. 

# What's the difference between this an Awful-Forums-Reader?!?

This is an updated version that uses Template 10 for a back end framework. I could have branched it, but I didn't.
