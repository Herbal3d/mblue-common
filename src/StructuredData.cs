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
    public static Quaternion FromObject(object obj) {
        if (obj is Quaternion v) {
            return v;
        }
        if (obj is float[] arr && arr.Length == 4) {
            return new Quaternion(arr[0], arr[1], arr[2], arr[3]);
        }
        if (obj is double[] darr && darr.Length == 4) {
            return new Quaternion((float)darr[0], (float)darr[1], (float)darr[2], (float)darr[3]);
        }
        if (obj is int[] iarr && iarr.Length == 4) {
            return new Quaternion(iarr[0], iarr[1], iarr[2], iarr[3]);
        }
        if (obj is string s) {
            // TODO: Check for JSON representation of Quaternion
            var parts = s.Split(',');
            if (parts.Length == 4 &&
                float.TryParse(parts[0], out var x) &&
                float.TryParse(parts[1], out var y) &&
                float.TryParse(parts[2], out var z) &&
                float.TryParse(parts[3], out var w)) {
                return new Quaternion(x, y, z, w);
            }
        }
        Type type = obj.GetType();
        if (type.GetField("x") != null && type.GetField("y") != null && type.GetField("z") != null && type.GetField("w") != null) {
#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return new Quaternion(
                (float)Convert.ChangeType(type.GetField("x").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("y").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("z").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("w").GetValue(obj), typeof(float))
            );
        }
        if (type.GetField("X") != null && type.GetField("Y") != null && type.GetField("Z") != null && type.GetField("W") != null) {
            return new Quaternion(
                (float)Convert.ChangeType(type.GetField("X").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("Y").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("Z").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("W").GetValue(obj), typeof(float))
            );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }
        throw new ArgumentException("Object is not of type convertible to Quaternion", nameof(obj));
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

    public static Vector3 Zero => new Vector3(0, 0, 0);
    public static Vector3 One => new Vector3(1, 1, 1);
    public static Vector3 FromObject(object obj) {
        if (obj is Vector3 v) {
            return v;
        }
        if (obj is float[] arr && arr.Length == 3) {
            return new Vector3(arr[0], arr[1], arr[2]);
        }
        if (obj is double[] darr && darr.Length == 3) {
            return new Vector3((float)darr[0], (float)darr[1], (float)darr[2]);
        }
        if (obj is int[] iarr && iarr.Length == 3) {
            return new Vector3(iarr[0], iarr[1], iarr[2]);
        }
        if (obj is string s) {
            // TODO: Check for JSON representation of Vector3
            var parts = s.Split(',');
            if (parts.Length == 3 &&
                float.TryParse(parts[0], out var x) &&
                float.TryParse(parts[1], out var y) &&
                float.TryParse(parts[2], out var z)) {
                return new Vector3(x, y, z);
            }
        }
        Type type = obj.GetType();
        if (type.GetField("x") != null && type.GetField("y") != null && type.GetField("z") != null) {
#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return new Vector3(
                (float)Convert.ChangeType(type.GetField("x").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("y").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("z").GetValue(obj), typeof(float))
            );
        }
        if (type.GetField("X") != null && type.GetField("Y") != null && type.GetField("Z") != null) {
            return new Vector3(
                (float)Convert.ChangeType(type.GetField("X").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("Y").GetValue(obj), typeof(float)),
                (float)Convert.ChangeType(type.GetField("Z").GetValue(obj), typeof(float))
            );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }
        throw new ArgumentException("Object is not of type convertible to Vector3", nameof(obj));
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
