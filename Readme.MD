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

## Update script on Raspberry PI for the DomoticzExporter project
pm2 stop x
sshpass -p "supersecretpassword" scp -P port user@server:/path-to-zip.zip ~/temp-deploy
cd ~/temp-deploy
unzip -o deploy-publish-exporter.zip
cp -r publish-exporter/* ~/DomoticzExporter
chmod +x ~/DomoticzExporter/HouseDB.DomoticzExporter
pm2 start x