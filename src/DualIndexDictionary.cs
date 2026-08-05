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

namespace org.herbal3d.mblue;

/// <summary>
/// Dictionary-like collection that stores a value once and indexes it by two unique keys.
/// </summary>
public class DualIndexDictionary<TKey1, TKey2, TValue>
    where TKey1 : notnull
    where TKey2 : notnull {

    private sealed class Entry {
        public required TKey1 Key1 { get; init; }
        public required TKey2 Key2 { get; init; }
        public required TValue Value { get; init; }
    }

    private readonly Dictionary<TKey1, Entry> m_byFirst = new();
    private readonly Dictionary<TKey2, Entry> m_bySecond = new();

    public int Count => m_byFirst.Count;

    public IEnumerable<TKey1> FirstKeys => m_byFirst.Keys;

    public IEnumerable<TKey2> SecondKeys => m_bySecond.Keys;

    public IEnumerable<TValue> Values => m_byFirst.Values.Select(static entry => entry.Value);

    public bool ContainsFirstKey(TKey1 key) => m_byFirst.ContainsKey(key);

    public bool ContainsSecondKey(TKey2 key) => m_bySecond.ContainsKey(key);

    public bool ContainsKey(TKey1 key) => ContainsFirstKey(key);

    public bool ContainsKey(TKey2 key) => ContainsSecondKey(key);

    public void Add(TKey1 key1, TKey2 key2, TValue value) {
        if (m_byFirst.ContainsKey(key1)) {
            throw new ArgumentException("An element with the same first key already exists.", nameof(key1));
        }

        if (m_bySecond.ContainsKey(key2)) {
            throw new ArgumentException("An element with the same second key already exists.", nameof(key2));
        }

        Entry entry = new() {
            Key1 = key1,
            Key2 = key2,
            Value = value,
        };

        m_byFirst.Add(key1, entry);
        m_bySecond.Add(key2, entry);
    }

    public bool Remove(TKey1 key1) {
        if (!m_byFirst.Remove(key1, out Entry? entry)) {
            return false;
        }

        m_bySecond.Remove(entry.Key2);
        return true;
    }

    public bool Remove(TKey2 key2) {
        if (!m_bySecond.Remove(key2, out Entry? entry)) {
            return false;
        }

        m_byFirst.Remove(entry.Key1);
        return true;
    }

    public bool Remove(TKey1 key1, TKey2 key2) {
        if (!m_byFirst.TryGetValue(key1, out Entry? entry)) {
            return false;
        }

        if (!EqualityComparer<TKey2>.Default.Equals(entry.Key2, key2)) {
            return false;
        }

        m_byFirst.Remove(key1);
        m_bySecond.Remove(key2);
        return true;
    }

    public bool TryGetValue(TKey1 key1, out TValue value) {
        if (m_byFirst.TryGetValue(key1, out Entry? entry)) {
            value = entry.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public bool TryGetValue(TKey2 key2, out TValue value) {
        if (m_bySecond.TryGetValue(key2, out Entry? entry)) {
            value = entry.Value;
            return true;
        }

        value = default!;
        return false;
    }

    public TValue GetByFirstKey(TKey1 key1) => m_byFirst[key1].Value;

    public TValue GetBySecondKey(TKey2 key2) => m_bySecond[key2].Value;

    public TValue? FindValue(Predicate<TValue> pred) {
        foreach (Entry entry in m_byFirst.Values) {
            if (pred(entry.Value)) {
                return entry.Value;
            }
        }

        return default;
    }

    public void ForEach(Action<TValue> act) {
        foreach (Entry entry in m_byFirst.Values) {
            act(entry.Value);
        }
    }

    public void Clear() {
        m_byFirst.Clear();
        m_bySecond.Clear();
    }
}
