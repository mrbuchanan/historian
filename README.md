# Historian

Welcome to historian, this is a tool intended to provide internal, application agnostic logging and analysis.

## Documentation
Go to the [wiki](../../wiki/Home) for documentation.

### Basic Features
- Logging
  - Timestamps
  - Choice of logging backends on service side:
    - MemoryLogger
    - MemoryWBackupLogger
    - SqlLogger (*Note, that this should only be used with the HangFire logging option turned on*)
  - log4net Appender (Historian.Log4Net), using Historian.Remote to provide logging to Historian from log4net
- Remote logging (*Historian.Remote*)
- Simple Message format (handles HTML)
- Simple Dashboard allowing you to view messages
  - Overview page, showing an overview of messages logged
  - Graphs showing proportion of messages by kind/severity
  - Throughput graphs for each channel. *Note that this is for the last twelve hours, and shows one line for each category of message received.*
  - View messages for the last day by kind (Debug, Information, Warning, Error and WTF)
  - Reload of selected Channel if page is refreshed.

### Planned Features
- Dashboard:
  - Search all messages received on a channel (inputs present, but functionality not)
  - Pattern Filters:
    - Email Notifications
    - Match number of requests in timeframe and notify
    - Match severity over time and notify
