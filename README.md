# Dog Agility Competition Management
Operating software for controlling the wireless sensors in a [Dog Agility Competition](https://en.wikipedia.org/wiki/Dog_agility).

[![Build status](https://ci.appveyor.com/api/projects/status/ydxgqsn30m7n92wf/branch/master?svg=true)](https://ci.appveyor.com/project/bkoelman/dogagilitycompetitionmanagement/branch/master)

# CIRCE Protocol
See [CIRCE Interface Specification](doc/CIRCE%20Interface%20Specification.md)

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
