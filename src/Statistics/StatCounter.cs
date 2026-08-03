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

using System.Text.Json.Nodes;

namespace org.herbal3d.mblue.Statistics {

    public class StatCounter : Stat<long> {
        public StatCounter(string pName, string pDescription, string pUnit = "")
                            : base(pName, pDescription, pUnit) {
            InternalValue = 0;
        }

        public override void Event() {
            InternalValue = InternalValue + 1;
        }
        public override void Event(int pCount) {
            InternalValue = InternalValue + pCount;
        }

        public override JsonNode GetDump() {
            JsonObject map = new JsonObject();
            map["Name"] = Name;
            map["Description"] = Description;
            map["Unit"] = Unit;
            map["Value"] = InternalValue;
            return map;
        }
    }
}
