dotnet build
Push-Location clientapp
Start-Job -Name vueclient -ScriptBlock { npm run serve }
Pop-Location
$env:Storage__BlobContainerName = "yofi-uitest"
$env:Demo__IsEnabled = "true"
$env:Clock__Now = "2022-12-31"
$env:ConnectionStrings__DefaultConnection = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=yofi-test-functional;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
Start-Job -Name vueserver -ScriptBlock { dotnet run }
Remove-Item Env:\ConnectionStrings__DefaultConnection
Remove-Item Env:\Clock__Now
Remove-Item Env:\Demo__IsEnabled
Remove-Item Env:\Storage__BlobContainerName