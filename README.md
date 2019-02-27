# README #
Language: C#
IDE: Visual Studio 2017
Technology: .NET Standard 2.0

# PROJECTS #

* Components - creating and using of independent  Components, Version: 1.2.0
	- Provides:
		- ComponentContract - exports components.
		- Loader - loads components.
	- Dependencies:
		- Logger 1.7.3
		- ConfigurationManager 1.2.0

* Logger - logging application state, Version: 1.7.3
	- Provides: 
		- FileLogger - logs data to file.
		- ConsoleLogger - logs data to console.
		- NullLogger - ignores all messages.
	- Dependencies:
		- ConfigurationManager 1.2.0
	
* ConfigurationManager - allows to load/save configuration, Version: 1.2.0
	- Provides:
		- Configuration - loads/saves configuration to/from xml file.
		
* Legion - multithread, multiplatform, multidevice independent task management, Version: 1.0.0
	- Provides:
		- In memory tasks managemet - optimal usage of CPUs on one device.
		- Via network tasks management - optimal usage of CPUs on multiple devices.
		- LegionContract - allows to create own tasks.
	- Dependencies:
		- ConfugurationManager 1.2.0
		- Logger 1.7.3
		- Components 1.2.0
		- ApplicationInformationExchange 1.0.0
		
* ApplicationInformationExchange - socket based communication for small messages, Version 1.0.0
	- Provides
		- Socker client-server communication
	- Dependencies:
		- ConfigurationManager 1.2.0

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
	- 1.2.0
		- Added components configuration as part of other project configuration.
		- Added AssemblyName to Component.
		- Added multi thrad safe singleton.
		- Changed dependency from Logger 1.7.2 to Logger 1.7.3.
		- Changed dependecy from ConfigurationManager 1.1.0 to ConfigurationManager 1.2.0.
		
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
	- 1.7.3
		- Removed unhandled exception when file with logs does not exist.
		- Changed dependecy from ConfigurationManager 1.1.0 to ConfigurationManager 1.2.0.
	
* ConfigurationManager:
	- 1.0.0
		- Added configuration loading from file.
	- 1.1.0
		- Added null checks.
	- 1.2.0
		- Changed file syntax to XML.
		
* ApplicationInformationExchange:
	- 1.0.0
		- Initial release, socket client-server communication
		
* Legion
	- 1.0.0
		- First release, in memory and network optimal task execution.
		
# OTHER INFORMATIONS #
Host project is for testing or giving example how use other projects. 
Wiki contains more information about each project.
