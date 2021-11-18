#!/bin/bash

apt-get update
apt-get --assume-yes upgrade


if ! test -f /opt/dotnet/dotnet
then
    echo "Download dotnet runtime 5"
    wget https://download.visualstudio.microsoft.com/download/pr/254a9fbb-e834-470c-af08-294c274a349f/ee755caf0b8a801cf30dcdc0c9e4273d/aspnetcore-runtime-5.0.5-linux-arm.tar.gz
    echo "extract runtime to /opt/dotnet"
    mkdir /opt/dotnet
    tar zxf aspnetcore-runtime-5.0.5-linux-arm.tar.gz -C /opt/dotnet
    echo "delete archive"
    rm aspnetcore-runtime-5.0.5-linux-arm.tar.gz
fi


sudo cat <<EOF > /etc/systemd/system/datalogger.service
[Unit]
Description=DataLogger-Service

[Service]
ExecStart=/opt/dotnet/dotnet DataLogger.dll
WorkingDirectory=/home/pi/
Restart=on-failure
SyslogIdentifier=DataLogger-service
PrivateTmp=true
Type=idle

[Install]
WantedBy=multi-user.target
EOF

STRING="dtoverlay=pi3-disable-bt"
FILE="/boot/config.txt"

if [ ! -z $(grep "$STRING" "$FILE") ];
        then
                echo "Serialconfiguration correctly provided"
        else
                echo "Disable bluetooth"
		sudo sh -c "printf "%s" $STRING >> $FILE"
                sudo systemctl disable hciuart
 fi



systemctl daemon-reload
systemctl enable datalogger.service
raspi-config nonint do_serial 1
sed -i '/enable_uart=0/d' /boot/config.txt
reboot

#systemctl start DataLogger.service


