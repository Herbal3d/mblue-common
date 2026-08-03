# mblue-common

`mblue-common` is a shared .NET common library for MBlue projects.  
It provides reusable routines that can be used across services and applications, including structured logging helpers, runtime statistics types, and hashing utilities.

Target framework: `net10.0`

## Logging

Logging is centered around [`MBLogger<T>`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Logging/MBLogger.cs), a wrapper over `Microsoft.Extensions.Logging.ILogger<T>` with MBlue-specific log levels in [`MBLogLevel`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Logging/IMBLogger.cs).

### What it provides

- Standard log levels (`Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`)
- MBlue feature/debug flags like `DCOMM`, `DWORLDDETAIL`, `DRENDERDETAIL`
- Convenience methods: `LogInfo`, `LogDebug`, `LogError`
- Optional category/prefix helpers (`LogMB`, `LogWithCategory`)

### Example usage

```csharp
using org.herbal3d.mblue.Logging;

public class Worker
{
    private readonly MBLogger<Worker> _log;

    public Worker(MBLogger<Worker> log)
    {
        _log = log;
    }

    public void Run(string endpoint)
    {
        _log.LogInfo("Starting worker for endpoint {Endpoint}", endpoint);
        _log.Log(MBLogLevel.DCOMM, "Sending request to {Endpoint}", endpoint);
        _log.LogError("Example failure for endpoint {Endpoint}", endpoint);
    }
}
```

### Example configuration

`MBLogger<T>` reads flags from [`MBLoggerConfig`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Logging/MBLoggerConfig.cs) (`MBLogger` section, `LogLevelFlags` value).

```json
{
  "MBLogger": {
    "LogLevelFlags": "Information,DCOMM,DWORLDDETAIL"
  }
}
```

## Statistics

Statistics types are in [src/Statistics/](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Statistics).  
They implement [`IStat`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Statistics/IStat.cs) and are JSON-dumpable through [`IDumpable`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/IDumpable.cs).

### What it provides

- [`StatCounter`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Statistics/StatCounter.cs): incrementing counter
- [`StatNumber`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Statistics/StatNumber.cs): increment/decrement numeric stat
- [`StatisticCollection`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/Statistics/StatisticCollection.cs): groups stats and dumps as `JsonArray`

### Example usage

```csharp
using org.herbal3d.mblue.Statistics;
using System.Text.Json.Nodes;

var requests = new StatCounter("requests_total", "Total processed requests", "requests");
requests.Event();
requests.Event(3); // total = 4

var queueDepth = new StatNumber("queue_depth", "Current queue depth", "items");
queueDepth.Increment(10);
queueDepth.Decrement(2); // value = 8

var stats = new StatisticCollection("worker");
stats.AddStat(requests);
stats.AddStat(queueDepth);

JsonNode dump = stats.GetDump(); // JSON array containing each stat dump
```

## Hashing

Hashing utilities are in [`BHasher.cs`](/mnt/f/Robert/dev/mblue/mblue-common.worktrees/readme-overview-logging-stats-hashing/src/BHasher.cs).

### What it provides

- A common interface: `IBHasher`
- Hash value types:
  - `BHashULong` (64-bit hash value)
  - `BHashBytes` (byte-array hash value)
- Implementations:
  - `BHasherMdjb2`
  - `BHasherMD5`
  - `BHasherSHA256`
  - `BHasherSHA512`
- Two usage patterns:
  - Incremental (`Add(...)` then `Finish()`)
  - One-shot (`Finish(byte[])`)

### Example usage

```csharp
using org.herbal3d.mblue;
using System.Text;

byte[] payload = Encoding.UTF8.GetBytes("hello world");

// One-shot hash
IBHasher oneShotHasher = new BHasherSHA256();
BHash oneShot = oneShotHasher.Finish(payload);
string oneShotHex = oneShot.ToString();

// Incremental hash
IBHasher incrementalHasher = new BHasherSHA256();
incrementalHasher.Add("hello ");
incrementalHasher.Add("world");
BHash incremental = incrementalHasher.Finish();
bool same = oneShot.Equals(incremental);
```
