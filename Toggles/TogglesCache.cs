using System.Collections.Generic;

namespace Toggles
{
    public class TogglesCache : IToggles
    {
        private readonly Dictionary<string, Dictionary<string, bool>> _cache =
            new Dictionary<string, Dictionary<string, bool>>();

        private readonly ITogglesRemoteSync _sync;

        public TogglesCache(ITogglesRemoteSync sync)
        {
            if (sync != null)
            {
                _sync = sync;
                _sync.ToggleUpdate += UpdateToggle;
                _sync.StartConsuming();
            }
        }

        public bool IsEnabled(string toggle, IEnumerable<string> identifiers = null)
        {
            if (string.IsNullOrWhiteSpace(toggle))
                return false;

            if (!_cache.ContainsKey(toggle))
                return false;

            var values = _cache[toggle];
            if (values == null || values.Count == 0)
                return false;

            if (identifiers != null)
                foreach (var identifier in identifiers)
                {
                    if (!values.ContainsKey(identifier))
                        continue;

                    return values[identifier];
                }

            return values.ContainsKey(toggle) && values[toggle];
        }

        public void CacheToggleValues(string toggle, Dictionary<string, bool> values)
        {
            if (string.IsNullOrWhiteSpace(toggle))
                return;

            if (values == null || values.Count == 0)
                return;

            _cache[toggle] = values;
        }

        public void UpdateToggle(Toggle toggle)
        {
            if (_cache.ContainsKey(toggle.Name) && _cache[toggle.Name] != null)
                _cache[toggle.Name][toggle.Name] = toggle.Enabled;
            else
                _cache[toggle.Name] = new Dictionary<string, bool> {{toggle.Name, toggle.Enabled}};
        }
    }
}