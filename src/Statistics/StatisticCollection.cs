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

using OMVSD = LibreMetaverse.StructuredData;

namespace org.herbal3d.mblue.Statistics {

    public class StatisticCollection : IDumpable {

        private readonly List<IStat> Stats = new List<IStat>();

        public string CollectionName { get; private set; }

        public StatisticCollection(string pCollectionName = "") {
            CollectionName = pCollectionName;
        }

        public void AddStat(IStat pStat) {
            Stats.Add(pStat);
        }

        /// <summary>
        /// Return an OSDArray of  the collection of statistics
        /// </summary>
        /// <returns></returns>
        public OMVSD.OSD GetDump() {
            OMVSD.OSDArray map = new OMVSD.OSDArray();
            foreach (IDumpable stat in Stats) {
                OMVSD.OSD statValues = stat.GetDump();
                if (statValues != null) {
                    map.Add(statValues);
                }
            }
            return map;
        }
    }
}
