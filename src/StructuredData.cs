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

namespace org.herbal3d.mblue.Common.StructuredData;

/// <summary>
/// This is a set of structures that are used to represent 3D data in a structured way.
/// These are simple structures with no methods, just data. They are used to represent 3D data
/// inside the mblue.ecm system to remove dependency on external libraries.
/// </summary>

public struct Quaternion {
    public float X;
    public float Y;
    public float Z;
    public float W;

    public Quaternion(float x, float y, float z, float w) {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
}

public struct Vector3 {
    public float X;
    public float Y;
    public float Z;

    public Vector3(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }
}

public struct Vector3d {
    public double X;
    public double Y;
    public double Z;

    public Vector3d(double x, double y, double z) {
        X = x;
        Y = y;
        Z = z;
    }
}

public struct Color4 {
    public float R;
    public float G;
    public float B;
    public float A;

    public Color4(float r, float g, float b, float a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}
