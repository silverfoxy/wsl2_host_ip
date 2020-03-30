# wsl2_host_ip
This is a simple script that will add an entry "wsl2.host" within Windows hosts file that will point to the dynamic host IP of WSL2.

# Intro

This project is part of my blog post about setting up PHP development environment on Windows while debugging PHP from Linux over WSL2 [(https://www.silverf0x00.com/setting-up-xdebug-for-phpstorm-on-windows-wsl2/)](https://www.silverf0x00.com/setting-up-xdebug-for-phpstorm-on-windows-wsl2/). 
One of the requirements of this setup is to give the IP address of Windows WSL2 interface to XDebug on Linux. 
This IP address is subject to change everytime the machine is rebooted. 
This will then require an update to IDE configuration update to use the new IP address.

To make this dynamic, I created this C# program that will read the IP address everytime the user logs in from the corresponding network interface. 
It will then update the hosts file and add an entry named `wsl2.host` that will point to this IP address. 
This way, you can use wsl2.host in config files without needing to update them everytime. 

# Setup

Download the release version of this program and put it under "Program Files". 
Now create an entry under Windows Task Scheduler, set it up to execute this program after login with elevated privileges (To be able to update hosts file).

# Configuration

If you want to read the IP address of another network interface or set the IP address under a different host name, update lines 
[14](https://github.com/silverfoxy/wsl2_host_ip/blob/a29d47f33696702c0f421beb4d451f38b26629e6/Program.cs#L14) and [28](https://github.com/silverfoxy/wsl2_host_ip/blob/a29d47f33696702c0f421beb4d451f38b26629e6/Program.cs#L28) of [Program.cs](https://github.com/silverfoxy/wsl2_host_ip/blob/a29d47f33696702c0f421beb4d451f38b26629e6/Program.cs). 
To debug this program if it does not update hosts file, look for Application logs under Windows Event Viewer.
