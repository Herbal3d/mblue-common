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

namespace org.herbal3d.mblue.Logging {

    public class MBLoggerConfig {

        // Configuration subsection name
        public const string subSectionName = "MBLogger";

        // LogLevel is either "None", "Trace", "Debug", "Information", "Warning"
        // To enable multiple levels, use a comma separated list in the "LogSelections"
        //   property, e.g., "Critical,Error,Warning,Information"
        public string LogLevel { get; set; } = "Information";
        public string LogSelections { get; set; } = "Critical,Error,Warning,Information,DBADERROR";
    }

}