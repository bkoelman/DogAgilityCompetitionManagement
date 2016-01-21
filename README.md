# Dog Agility Competition Management
Operating software for controlling the wireless sensors in a [Dog Agility Competition](https://en.wikipedia.org/wiki/Dog_agility).

[![Build status](https://ci.appveyor.com/api/projects/status/ydxgqsn30m7n92wf?svg=true)](https://ci.appveyor.com/project/bkoelman/dogagilitycompetitionmanagement)

# CIRCE Protocol
See [CIRCE Interface Specification](doc/CIRCE%20Interface%20Specification.md)

# Running without loopback cable

1. Download and install
	http://sourceforge.net/projects/com0com/files/com0com/2.2.2.0/com0com-2.2.2.0-x64-fre-signed.zip/download

2. Start > Programs > com0com > Setup Command Prompt

	```
	command> change CNCA0 EmuBR=yes,EmuOverrun=yes,PortName=COM8
	command> change CNCB0 EmuBR=yes,EmuOverrun=yes,PortName=COM9
	command> quit
	```

3. Make a Debug build of [DogAgilityCompetitionManagement.sln](src/DogAgilityCompetitionManagement.sln) and run it
