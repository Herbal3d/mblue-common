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

            // "LogLevel" sets the general log level for the application.
            // "LogSelections" are added to the log level if "LogLevel" is "debug".
            // If "LogLevel" is set, use it to determine the log level flags
            MBLogLevel allBadThings = MBLogLevel.Error
                                        | MBLogLevel.Critical
                                        | MBLogLevel.DBADERROR;
            switch (_options.Value.LogLevel.Trim().ToLowerInvariant()) {
                case "none":
                    _logLevelFlags = MBLogLevel.None;
                    _log.LogInformation("Log level set to 'None' from configuration.");
                    break;
                case "trace":
                    _logLevelFlags = allBadThings | MBLogLevel.Trace;
                    _log.LogInformation("Log level set to 'Trace' from configuration.");
                    break;
                case "debug":
                    _logLevelFlags = allBadThings | MBLogLevel.Debug;
                    _log.LogInformation("Log level set to 'Debug' from configuration.");
                    break;
                case "information":
                    _logLevelFlags = allBadThings | MBLogLevel.Information;
                    _log.LogInformation("Log level set to 'Information' from configuration.");
                    break;
                case "warning":
                    _logLevelFlags = allBadThings | MBLogLevel.Warning;
                    _log.LogInformation("Log level set to 'Warning' from configuration.");
                    break;
                case "error":
                    _logLevelFlags = allBadThings | MBLogLevel.Error;
                    _log.LogInformation("Log level set to 'Error' from configuration.");
                    break;
                case "critical":
                    _logLevelFlags = allBadThings | MBLogLevel.Critical;
                    _log.LogInformation("Log level set to 'Critical' from configuration.");
                    break;
                default:
                    // Log a warning if the user provided an invalid log level
                    _log.LogWarning($"Invalid log level '{_options.Value.LogLevel}' in configuration. Defaulting to 'Debug'.");
                    _logLevelFlags = allBadThings | MBLogLevel.Debug;
                    break;
            }
            if ((_logLevelFlags & MBLogLevel.Debug) != 0) {
                // "LogSelections" is a comma-separated list of log levels, e.g., "Critical,Error,Warning,Information"
                string[] userSelections = _options.Value.LogSelections.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                MBLogLevel flags = MBLogLevel.None;
                foreach (string selection in userSelections) {
                    if (Enum.TryParse<MBLogLevel>(selection.Trim(), true, out MBLogLevel parsedLevel)) {
                        _log.LogInformation($"Adding log level '{selection}' from configuration.");
                        flags |= parsedLevel;
                    } else {
                        // Log a warning if the user provided an invalid log level
                        _log.LogWarning($"Invalid log level '{selection}' in configuration. Ignoring.");
                    }
                }
                _logLevelFlags |= flags;
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
            if (((ulong)_logLevelFlags & (ulong)pLevel) != 0) {
                _log.Log(LogLevel.Information, pMessage, pArgs);
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
