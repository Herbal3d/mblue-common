// Copyright 2025 Robert Adams
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace org.herbal3d.mblue.Logging {

    /// <summary>
    /// Wrapper around ILogger to provide mblue specific logging features.
    /// Implements IMBLogger for dependency injection.
    /// </summary>
    /// <typeparam name="T">The type associated with the logger (for categorization).</typeparam>
    public class MBLogger<T> : IMBLogger {

        private readonly ILogger<T> _log;
        private readonly string _categoryName = typeof(T).FullName ?? nameof(T);
        private readonly IOptions<MBLoggerConfig> _options;
        private MBLogLevel _logLevelFlags;

        public MBLogger(ILogger<T> pLogger,
                       IOptions<MBLoggerConfig> pOptions) {
            _log = pLogger ?? throw new ArgumentNullException(nameof(pLogger));
            _options = pOptions ?? throw new ArgumentNullException(nameof(pOptions));

            // If "LogLevel" is not set, expect a list of setting in "LogSelections" to
            //    determine the log level flags.
            // That is, if "LogLevel" is set, it overrides "LogSelections".
            if (String.IsNullOrWhiteSpace(_options.Value.LogLevel)) {
                // Set the log level flags based on the configuration
                // "LogSelections" is a comma-separated list of log levels, e.g., "Critical,Error,Warning,Information"
                string[] userSelections = _options.Value.LogSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                MBLogLevel flags = MBLogLevel.None;
                foreach (string selection in userSelections) {
                    if (Enum.TryParse<MBLogLevel>(selection.Trim(), true, out MBLogLevel parsedLevel)) {
                        flags |= parsedLevel;
                    } else {
                        // Log a warning if the user provided an invalid log level
                        _log.LogWarning($"Invalid log level '{selection}' in configuration. Ignoring.");
                    }
                }
                _logLevelFlags = flags;
            } else {
                // If "LogLevel" is set, use it to determine the log level flags
                MBLogLevel allBadThings = MBLogLevel.Information
                                            | MBLogLevel.Warning
                                            | MBLogLevel.Error
                                            | MBLogLevel.Critical
                                            | MBLogLevel.DBADERROR;
                switch (_options.Value.LogLevel.Trim().ToLowerInvariant()) {
                    case "none":
                        _logLevelFlags = MBLogLevel.None;
                        break;
                    case "trace":
                        _logLevelFlags = allBadThings | MBLogLevel.Trace;
                        break;
                    case "debug":
                        _logLevelFlags = allBadThings | MBLogLevel.Debug;
                        break;
                    case "information":
                        _logLevelFlags = allBadThings | MBLogLevel.Information;
                        break;
                    case "warning":
                        _logLevelFlags = allBadThings | MBLogLevel.Warning;
                        break;
                    case "error":
                        _logLevelFlags = allBadThings | MBLogLevel.Error;
                        break;
                    case "critical":
                        _logLevelFlags = allBadThings | MBLogLevel.Critical;
                        break;
                    default:
                        // Log a warning if the user provided an invalid log level
                        _log.LogWarning($"Invalid log level '{_options.Value.LogLevel}' in configuration. Defaulting to 'Debug'.");
                        _logLevelFlags = allBadThings | MBLogLevel.Debug;
                        break;
                }
            }
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull {
            return _log.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return _log.IsEnabled(logLevel);
        }

        // Long form for reaching into the underlying MS logger
        void ILogger.Log<TState>(LogLevel logLevel,
                                 EventId eventId,
                                 TState state,
                                 Exception? exception,
                                 Func<TState, Exception?, string> formatter) {
            _log.Log(logLevel, eventId, state, exception, formatter);
        }

        // Log function that is used everywhere
        public void Log(MBLogLevel pLevel, string pMessage, params object[] pArgs) {
            switch (pLevel) {
                case MBLogLevel.Trace:
                    _log.Log(LogLevel.Trace, pMessage, pArgs);
                    break;
                case MBLogLevel.Debug:
                    _log.Log(LogLevel.Debug, pMessage, pArgs);
                    break;
                case MBLogLevel.Information:
                    _log.Log(LogLevel.Information, pMessage, pArgs);
                    break;
                case MBLogLevel.Warning:
                    _log.Log(LogLevel.Warning, pMessage, pArgs);
                    break;
                case MBLogLevel.Error:
                    _log.Log(LogLevel.Error, pMessage, pArgs);
                    break;
                case MBLogLevel.Critical:
                    _log.Log(LogLevel.Critical, pMessage, pArgs);
                    break;
                case MBLogLevel.None:
                    // Do nothing
                    break;
                case MBLogLevel.DBADERROR:
                    _log.Log(LogLevel.Error, pMessage, pArgs);
                    break;
                default:
                    // Might be one of the detailed debugging flags
                    if (((ulong)_logLevelFlags & (ulong)pLevel) != 0) {
                        _log.Log(LogLevel.Information, pMessage, pArgs);
                    } else {
                        // Unknown level, log as Information
                        _log.Log(LogLevel.Information, pMessage, pArgs);
                    }
                    break;
            }
        }

        public void LogInfo(string pMessage, params object[] args) {
            Log(MBLogLevel.Information, pMessage, args);
        }

        public void LogDebug(string pMessage, params object[] args) {
            Log(MBLogLevel.Debug, pMessage, args);
        }

        public void LogError(string pMessage, params object[] args) {
            Log(MBLogLevel.Error, pMessage, args);
        }

        /// <summary>
        /// Custom MB logging with predefined formatting.
        /// </summary>
        public void LogMB(LogLevel level, string pMessage, Exception? exception = null) {
            string formattedMessage = $"[MBlue] {pMessage}";
            _log.Log(level, exception, formattedMessage);
        }

        /// <summary>
        /// Log with a custom category instead of the default type category.
        /// </summary>
        public void LogWithCategory(LogLevel level, string category, string pMessage, Exception? exception = null) {
            string formattedMessage = $"[{category}] {pMessage}";
            _log.Log(level, exception, formattedMessage);
        }
    }
}
