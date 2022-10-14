# Dog Agility Competition Management

[![Build status](https://ci.appveyor.com/api/projects/status/ydxgqsn30m7n92wf/branch/master?svg=true)](https://ci.appveyor.com/project/bkoelman/dogagilitycompetitionmanagement/branch/master)

This repository contains the operating software for controlling various wireless devices (gates, remote controls and LED-displays) at [Dog Agility Competition](https://en.wikipedia.org/wiki/Dog_agility) events. The system was demonstrated at the Agility European Open Event in 2013 (video below).

[![Agility European Open 2013](http://img.youtube.com/vi/Zy6irCVxGwA/0.jpg)](http://www.youtube.com/watch?v=Zy6irCVxGwA "Agility European Open 2013")
![Demo-Agility-hardware](https://user-images.githubusercontent.com/10324372/195837880-ed1d5b26-ea71-480c-9874-1cc55c7b5cac.jpg)

To facilitate development and testing, an emulator is included to replace the remote controls, time sensors and displays. It enables running without setting up a USB/COM connection to the associated hardware.

![Demo-Inline-keypad-emulator](https://user-images.githubusercontent.com/10324372/195838290-32e337aa-3ee5-45fd-9df3-e04d30940a10.jpg)
![Demo-Logging-input](https://user-images.githubusercontent.com/10324372/195838308-de4f9d93-c95d-4df5-ad9d-ec912702332b.jpg)

![Demo-Logging-output](https://user-images.githubusercontent.com/10324372/195838331-ac6d24d6-947c-4df7-b454-8dbe21da682a.jpg)

# CIRCE Protocol
Wireless devices communicate over a [CAN bus](https://en.wikipedia.org/wiki/CAN_bus), whose messages are collected by a mediator device that is connected to a computer. The mediator and computer exchange information through the [CIRCE](doc/CIRCE%20Interface%20Specification.md) (Computer Interface to Race Course Equipment) protocol.

# Running without loopback cable

1. Download and install
	https://sourceforge.net/projects/com0com/files/com0com/3.0.0.0/com0com-3.0.0.0-i386-and-x64-signed.zip/download

2. Start > Programs > com0com > Setup Command Prompt

	```
	command> change CNCA0 EmuBR=yes,EmuOverrun=yes,PortName=COM8
	command> change CNCB0 EmuBR=yes,EmuOverrun=yes,PortName=COM9
	command> quit
	```

3. Make a Debug build of [DogAgilityCompetitionManagement.sln](src/DogAgilityCompetitionManagement.sln) and run it. Alternatively, download the latest binaries from https://ci.appveyor.com/project/bkoelman/dogagilitycompetitionmanagement/branch/master/artifacts.
