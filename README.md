# YoFi.Spa

This is an experimental project to port the [YoFi](https://github.com/jcoliz/yofi) Personal Finance Manager to a Single Page App.

## Current Status

* STARTED: Now porting in earnest to Vue. The functional tests in YoFi.Vue.Tests.Functional give a progress report on how far along that is.
* DONE: Experimented with Blazor. Decided I didn't like it. Initial loading (even on pro-quality Blazor sites) is just too much.
* DONE: Open API specification. NSwag to create and consume this spec. 
* DONE: WireApi layer. Provides access to Core layer's UI facing interfaces through Api controllers.
* DONE: Project infrastructure

## Architecture

This was my original thinking for when I was working on Blazor. 
Obviously, I need to update this now for Vue.

Conceptually, it's still accurate. The difference is that there is no
generated client. Right now, just calling endpoints directly. I'll 
consider generating a client in the future. Also there is no separate client project. Client is contained in the YoFi.Vue/clientapp directory.


[![System Architecture](/docs/images/YoFi-Blazor-Layers-R1.svg)](https://raw.githubusercontent.com/jcoliz/YoFi.WebApi/master/docs/images/YoFi-Blazor-Layers-R1.svg)