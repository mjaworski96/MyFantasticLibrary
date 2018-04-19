# README #
Language: C#
IDE: Visual Studio Community 2017
Technology: .NET Standard 2.0

# PROJECTS #

* Components - creating and using of independent  Components, Version: 1.1.2
	- Provides:
		- ComponentContract - exports components.
		- Loader - loads components.
	- Dependencies:
		- Logger 1.7.2
		- ConfigurationManager 1.1.0

* Logger - logging application state, Version: 1.7.2
	- Provides: 
		- FileLogger - logs data to file.
		- ConsoleLogger - logs data to console.
		- NullLogger - logs all messages.
	- Dependencies:
		- ConfigurationManager 1.1.0
	
* ConfigurationManager - allows to load/save config using .NET Standard 2.0, Version: 1.1.0
	- Provides:
		- Configuration - loads/saves configuration to/from file.
		
* Legion - multithread, multiplatform, multidevice independent task management, Version: not released yet
	- Provides:
		- In memory tasks managemet - optimal usage of CPUs on one device.
		- Via network tasks management - optimal usage of CPUs on multiple devices.
		- LegionContract - allows to create own tasks.
	- Dependencies:
		- ConfugurationManager
		- Logger
		- Components

# CHANGELOG #
* Components:
	- 1.0.0
		- Added loading types from dll.
	- 1.1.0
		- Added null checks.
		- Changed dependency from Logger 1.6.0 to Logger 1.7.0.
	- 1.1.1
		- Changed dependency from Logger 1.7.0 to Logger 1.7.1.
		- Changed dependency from ConfigurationManager 1.0.0 to ConfigurationManager 1.1.0.
	- 1.1.2
		- Changed dependency from Logger 1.7.1 to Logger 1.7.2.


* Logger:
	- 1.0.0 
		- Added logging to file.
	- 1.1.0
		- Added loggging to console.
	- 1.2.0
		- Added filter.
	- 1.3.0
		- Added color to console logger.
	- 1.4.0
		- Added multiple colors to console logger.
	- 1.5.0
		- Added NullLogger.
		- Added Critical LogType.
	- 1.6.0
		- Ported to .NET Standard 2.0.
		- Added dependncy to ConfigurationManager 1.0.0.
	- 1.7.0
		- Added null checks.
	- 1.7.1
		- Changed dependency from ConfigurationManager 1.0.0 to ConfigurationManager 1.1.0.
		- Added null check on logging message.
	- 1.7.2
		- Added posibility to change config filename.

	
* ConfigurationManager:
	- 1.0.0
		- Added configuration loading from file.
	- 1.1.0
		- Added null checks.

# FUTURE RELEASES #
* Legion
	- First release that provides independent tasks management. Information about tasks and their parameters will be exchanged by memory (Partially done, but not relased) or via TCP sockets.
* Components
	- Components configuration as part of other project configuration (DONE, but not relased).
	
# OTHER INFORMATION #
Host project is for testing or giving example how use other projects. 