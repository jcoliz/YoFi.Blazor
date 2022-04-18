# YoFi.Spa

This is an experimental project to port the [YoFi](https://github.com/jcoliz/yofi) Personal Finance Manager to a Single Page App.

## Current Status

* STARTED: Now porting in earnest to Vue. The functional tests in YoFi.Vue.Tests.Functional give a progress report on how far along that is.
* DONE: Experimented with Blazor. Decided I didn't like it. Initial loading (even on pro-quality Blazor sites) is just too much.
* DONE: Open API specification. NSwag to create and consume this spec. 
* DONE: WireApi layer. Provides access to Core layer's UI facing interfaces through Api controllers.
* DONE: Project infrastructure

## Architecture

Here's the high-level whole-project architecture, showing the AspNet and Vue applications side by side, highlighting their differences as well as their shared underpinnings.

[![System Architecture](/docs/images/YoFi-Layers-R2.svg)](https://raw.githubusercontent.com/jcoliz/YoFi.WebApi/master/docs/images/YoFi-Layers-R2.svg)