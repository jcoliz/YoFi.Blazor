# YoFi Web API

This is an experimental project to prepare for moving [YoFi](https://github.com/jcoliz/yofi) to Blazor.

## Current Status

At this point, I'm happy with the idea of a thin layer of controllers on top of the Core layer's UI facing interfaces.
I'm calling it the YoFi.WireApi layer, and starting to flesh it out here.

I'm also happy with generating an OpenAPI specification out of these controllers, then using NSwag to generate a matching client SDK. The tests demonstrate using the WireApi with the client SDK and also directly.

There is probably enough functionality exposed through the Wire API that I could start building a Blazor prototype now.

## Architecture

[![System Architecture](/docs/images/YoFi-Blazor-Layers-R1.svg)](https://raw.githubusercontent.com/jcoliz/YoFi.WebApi/master/docs/images/YoFi-Blazor-Layers-R1.svg)