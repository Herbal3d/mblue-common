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

using Microsoft.Extensions.Logging;

namespace org.herbal3d.mblue.Logging {

    public enum MBLogLevel : ulong {
        None            = 0x00000000,
        Trace           = 0x00000001,
        Debug           = 0x00000002,
        Information     = 0x00000004,
        Warning         = 0x00000008,
        Error           = 0x00000010,
        Critical        = 0x00000020,
        Unknown1        = 0x00000040,
        DBADERROR       = 0x00000080,
        // detailed logging flags
        DINIT           = 0x00000100,
        DINITDETAIL     = 0x00000200,
        DREST           = 0x00000400,
        DRESTDETAIL     = 0x00000800,
        DWORKQUEUE      = 0x00001000,
        DWORKQUEUEDETAIL= 0x00002000,
        DUI             = 0x00004000,
        DUIDETAIL       = 0x00008000,
        DCOMM           = 0x00010000,
        DCOMMDETAIL     = 0x00020000,
        DWORLD          = 0x00040000,
        DWORLDDETAIL    = 0x00080000,
        DUPDATE         = 0x00100000,
        DUPDATEDETAIL   = 0x00200000,
        DTEXTURE        = 0x00400000,
        DTEXTUREDETAIL  = 0x00800000,
        DRENDER         = 0x01000000,
        DRENDERDETAIL   = 0x02000000,
        DVIEW           = 0x04000000,
        DVIEWDETAIL     = 0x08000000
    }
    public interface IMBLogger : ILogger {
        // Log with our KLogLevel
        void Log(MBLogLevel level, string message, params object[] args);

        public void LogInfo(string message, params object[] args);

        public void LogDebug(string message, params object[] args);

        public void LogError(string message, params object[] args);
    }
}
