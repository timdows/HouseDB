# HouseDB

## Used commands
cd HouseDB.Data
dotnet ef --startup-project ..\HouseDB.Api migrations add Initial 
dotnet ef --startup-project ..\HouseDB.Api database update 

## Server hosting ACC
systemctl --user enable kestrel-housedbprod.service
systemctl --user start kestrel-housedbprod.service
systemctl --user stop kestrel-housedbprod.service
journalctl --user-unit kestrel-housedbprod.service --no-pager