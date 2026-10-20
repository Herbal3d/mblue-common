// Copyright 2026 Robert Adams
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Extensions.DependencyInjection;

namespace org.herbal3d.mblue.Common {

    /// <summary>
    /// Snapshot of the service type to implementation type mappings registered
    /// in the DI <see cref="IServiceCollection"/>. The built service provider does not
    /// expose its registrations so this captures them for use with
    /// ActivatorUtilities, which needs a concrete type.
    /// Create this after all the AddServices calls have been made.
    /// </summary>
    public class ServiceTypeMap {
        private readonly Dictionary<Type, Type> _map;

        public ServiceTypeMap(IServiceCollection services) {
            _map = new Dictionary<Type, Type>();
            // Later registrations win, as they do when resolving.
            foreach (var d in services) {
                if (d.IsKeyedService) continue;
                if (d.ImplementationType != null) {
                    _map[d.ServiceType] = d.ImplementationType;
                }
            }
        }

        /// <summary>
        /// Return the implementation type registered for <paramref name="serviceType"/>.
        /// If there is no registered implementation type, the passed type is returned.
        /// </summary>
        public Type Resolve(Type serviceType) {
            return _map.TryGetValue(serviceType, out var implType) ? implType : serviceType;
        }
    }
}
