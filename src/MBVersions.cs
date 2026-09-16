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

namespace org.herbal3d.mblue.Common {

    // A class that is created and maintained to keep track of version information for the project.
    public class MBVersions {

        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public Dictionary<string, string> OtherVersions { get; set; }

        public MBVersions() {
            AppName = "Blue";
            AppVersion = "Unknown";
            OtherVersions = new Dictionary<string, string>();
        }
        public MBVersions(string appName, string appVersion) {
            AppName = appName;
            AppVersion = appVersion;
            OtherVersions = new Dictionary<string, string>();
        }

        public void AddOtherVersion(string key, string value) {
            OtherVersions[key] = value;
        }


    }
}
