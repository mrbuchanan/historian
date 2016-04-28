# historian

Welcome to historian, this is a tool intended to provide internal, application agnostic logging and analysis.

## History of historian
The idea for historian came about while working on a project that required a lot of logging to determine what was going on, but was writing to file using log4net or similar, making the log files hard to find and read.

## Basic Features
- Remote logging (*Historian.Remote*)
- Choice of logging backends on Service side:
  - MemoryLogger
  - MemoryWBackupLogger
- Simple Message format (handles HTML)
- Simple Dashboard allowing you to view messages

## Planned Features
- Logging:
  - Timestamps
  - SqlLogger (*Note, that this should only be used with the HangFire logging option turned on*)
- Dashboard:
  - Overview page, showing an overview of messages logged
  - Graphs showing proportion of messages by kind/severity
  - Throughput graphs for each channel
  - Pattern Filters:
    - Email Notifications
    - Match number of requests in timeframe and notify
    - Match severity over time and notify
