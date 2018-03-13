# README #
Language: C#
IDE: Visual Studio Community 2017

# PROJECTS #

* Components - creating and using of indepentent Components, Version: 1.0.0
	Provides:
	ComponentContract - exports components
	Loader - loads components.
	Dependencies:
		Logger 1.6.0
		ConfigurationManager 1.0.0

* Logger - logging application state, Version: 1.6.0
	Provides: 
		FileLogger - logs data to file
		ConsoleLogger - logs data to console
		NullLogger - logs all messages
	Dependencies:
		ConfigurationManager 1.0.0
	
* ConfigurationManager - allows to load/save config using .NET Standard 2.0, Version: 1.0.0
	Provides:
		Configuration - loads/saves configuration to/from file

# VERSIONS #
* Components:
1.0.0
	- Added loading types from dll.
* Logger:
1.0.0 
	- Added logging to file.
1.1.0
	- Added loggging to console.
1.2.0
	- Added filter.
1.3.0
	- Added color to console logger.
1.4.0
	- Added multiple colors to console logger.
1.5.0
	- Added NullLogger.
	- Added Critical LogType.
1.6.0
	- Ported to .NET Standard 2.0
	- Added dependncy to ConfigurationManager 1.0.0
* ConfigurationManager:
1.0.0
	- Added configuration loading from file.