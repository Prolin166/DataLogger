!! PAY ATTENTION!!
    	FILE ENCODING MAY BE MS-DOS.. 
    	LINEENDINGS HAVE TO BE LINEFEED

Step 1:
	Download Raspbian Lite .img-Datei
	Use "Etcher" or other software to unzip it to the micro sd-card
	Create a file named "ssh" in main directory without filtype ending
Step 2:
	Put sd-card into the raspberry pi
Step 3:
	Start winscp  and log onto the raspberry pi
		User: pi
		PW: raspberry
Step 4:
	Copy Release build of DatenloggerBM to the main directory
	Copy setup.sh to the main directory
Step 5:
	Use "Putty" to log on raspberry pi
    	Run setup.sh as root (chmod +x setup.sh + sudo ./setup.sh)


