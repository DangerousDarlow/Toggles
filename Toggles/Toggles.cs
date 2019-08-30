using System.Collections.Generic;

namespace Toggles
{
    public interface IToggles
    {
        bool IsEnabled(string toggle, IEnumerable<string> identifiers = null);
    }

    public class Toggles : IToggles
    {
        private readonly Dictionary<string, Dictionary<string, bool>> _cache =
            new Dictionary<string, Dictionary<string, bool>>();

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
            {
                foreach (var identifier in identifiers)
                {
                    if (!values.ContainsKey(identifier))
                        continue;

                    return values[identifier];
                }
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
    }
}