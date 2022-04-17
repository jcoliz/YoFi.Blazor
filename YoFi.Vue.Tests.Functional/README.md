# YoFi.Vue.Tests.Functional

Functional tests for the Vue.Js SPA version of YoFi.

## How to run the tests

### 1. Start the Vue app in the background

There is a handy "startbg.ps1" script in YoFi.Vue. 
However, it requires Powershell 7 to function correctly.

```
PS \YoFi.WebApi\YoFi.Vue> pwsh
PowerShell 7.2.2
Copyright (c) Microsoft Corporation.

https://aka.ms/powershell
Type 'help' to get help.

PS \YoFi.WebApi\YoFi.Vue> .\startbg.ps1
Microsoft (R) Build Engine version 17.1.0+ae57d105c for .NET
Copyright (C) Microsoft Corporation. All rights reserved.

  Determining projects to restore...
  All projects are up-to-date for restore.
  YoFi.Core -> \YoFi.WebApi\submodules\YoFi\YoFi.Core\bin\Debug\net6.0\YoFi.Core.dll
  YoFi.Data -> \YoFi.WebApi\submodules\YoFi\YoFi.Data\bin\Debug\net6.0\YoFi.Data.dll
  YoFi.WireApi -> \YoFi.WebApi\YoFi.WireApi\bin\Debug\net6.0\YoFi.WireApi.dll
  YoFi.Vue -> \YoFi.WebApi\YoFi.Vue\bin\Debug\net6.0\YoFi.Vue.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:00.91

Id     Name            PSJobTypeName   State         HasMoreData     Location             Command
--     ----            -------------   -----         -----------     --------             -------
1      vueclient       BackgroundJob   Running       True            localhost             npm run serve
3      vueserver       BackgroundJob   Running       True            localhost             dotnet run
```

### 2. Verify it's running properly

To ensure the client and server started correctly, you can use Receive-Job

```
PS \YoFi.WebApi\YoFi.Vue> receive-job 1

> clientapp@0.1.0 serve
> vue-cli-service serve

 INFO  Starting development server...

  App running at:
  - Local:   http://localhost:8080/
  - Network: http://192.168.1.110:8080/

  Note that the development build is not optimized.
  To create a production build, run npm run build.

Build finished at 21:28:05 by 0.000s

PS \YoFi.WebApi\YoFi.Vue> receive-job 3
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7271
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5003
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: \YoFi.WebApi\YoFi.Vue\
```

### 3. Run functional tests from this directory

```
PS \YoFi.WebApi\YoFi.Vue> pushd ..\YoFi.Vue.Tests.Functional\
PS \YoFi.WebApi\YoFi.Vue.Tests.Functional> dotnet test
  Determining projects to restore...
  Restored \YoFi.WebApi\YoFi.Vue.Tests.Functional\YoFi.Vue.Tests.Functional.csproj (in 261 ms).
  YoFi.Vue.Tests.Functional -> \YoFi.WebApi\YoFi.Vue.Tests.Functional\bin\Debug\net6.0\YoFi.Vue.Tests.Functional.dll
Test run for \YoFi.Vue.Tests.Functional\bin\Debug\net6.0\YoFi.Vue.Tests.Functional.dll (.NETCoreApp,Version=v6.0)

A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    26, Skipped:     0, Total:    26, Duration: 1 m 31 s - YoFi.Vue.Tests.Functional.dll (net6.0)
```

(Ok, the fact that it takes 1:31 to run 26 tests is something I'm going to have to look at.)