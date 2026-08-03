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

        private readonly ILogger<T> _innerLogger;
        private readonly string _categoryName = typeof(T).FullName ?? nameof(T);
        private readonly IOptions<MBLoggerConfig> _options;

        public MBLogger(ILogger<T> pLogger,
                       IOptions<MBLoggerConfig> pOptions) {
            _innerLogger = pLogger ?? throw new ArgumentNullException(nameof(pLogger));
            _options = pOptions ?? throw new ArgumentNullException(nameof(pOptions));
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull {
            return _innerLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel) {
            return _innerLogger.IsEnabled(logLevel);
        }

        // Long form for reaching into the underlying MS logger
        void ILogger.Log<TState>(LogLevel logLevel,
                                 EventId eventId,
                                 TState state,
                                 Exception? exception,
                                 Func<TState, Exception?, string> formatter) {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }

        // Log function that is used everywhere
        public void Log(MBLogLevel pLevel, string pMessage, params object[] pArgs) {
            switch (pLevel) {
                case MBLogLevel.Trace:
                    _innerLogger.Log(LogLevel.Trace, pMessage, pArgs);
                    break;
                case MBLogLevel.Debug:
                    _innerLogger.Log(LogLevel.Debug, pMessage, pArgs);
                    break;
                case MBLogLevel.Information:
                    _innerLogger.Log(LogLevel.Information, pMessage, pArgs);
                    break;
                case MBLogLevel.Warning:
                    _innerLogger.Log(LogLevel.Warning, pMessage, pArgs);
                    break;
                case MBLogLevel.Error:
                    _innerLogger.Log(LogLevel.Error, pMessage, pArgs);
                    break;
                case MBLogLevel.Critical:
                    _innerLogger.Log(LogLevel.Critical, pMessage, pArgs);
                    break;
                case MBLogLevel.None:
                    // Do nothing
                    break;
                case MBLogLevel.DBADERROR:
                    _innerLogger.Log(LogLevel.Error, pMessage, pArgs);
                    break;
                default:
                    // Might be one of the detailed debugging flags
                    if (((ulong)_options.Value.LogLevelFlags & (ulong)pLevel) != 0) {
                        _innerLogger.Log(LogLevel.Information, pMessage, pArgs);
                    } else {
                        // Unknown level, log as Information
                        _innerLogger.Log(LogLevel.Information, pMessage, pArgs);
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
            _innerLogger.Log(level, exception, formattedMessage);
        }

        /// <summary>
        /// Log with a custom category instead of the default type category.
        /// </summary>
        public void LogWithCategory(LogLevel level, string category, string pMessage, Exception? exception = null) {
            string formattedMessage = $"[{category}] {pMessage}";
            _innerLogger.Log(level, exception, formattedMessage);
        }
    }
}
