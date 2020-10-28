# ZEM Enterprice WebApp
A web application written in ASP .NET Core 3.1 for managing scanning for a company.

It's my first ever ASP .NET Core and .NET Core in general application so I probably butchered it a bunch, I never really done any JS or CSS so there is that as well! :D

## About

Repository contains two projects, one is your everyday .NET Framework client-server application and the other one is the ASP web application. The reason for that is, the company I've been writing this program for had some really old PCs in their scanning department and I couldn't bother to try and support IE6 or something similar so instead I made a client windows forms application for scanning which they can easily run on older hardware.

Web application has same features (and much more!) as scanning app so it can be easily standalone one (even should be!)

## Requirements 

For web and framework server:
* Microsoft .NET Core 3.1 Runtime 
* Microsoft SQL Server 2012 or newer
* .NET Framework 4.8
* Newest version of popular browsers (tested on chrome and firefox)

For client:
* .NET Framework 3.5

For both:
* Polish language (for real, the whole thing is in polish)

## Screenshots

I'm honestly not going to bother putting any screenshots sice it's quite a pain to do on github, sorry!

## Configuration

For web application you need to configure:
* appsettings.json with sql connection string
* hostsettings.json with your ip's and ports
* wwwroot/assets/customjs/ScanCommunicate.js with IP to your API, probably the same as with hostsettings.json

For client and server:
* setting file is being generated on first run, it's fairly straight forward and I'm sure you can figure it out!
