# HouseDB

## Used commands
cd HouseDB.Data
dotnet ef --startup-project ..\HouseDB.Api migrations add Initial 
dotnet ef --startup-project ..\HouseDB.Api database update 