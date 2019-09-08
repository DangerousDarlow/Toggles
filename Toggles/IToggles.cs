using System.Collections.Generic;

namespace Toggles
{
    public interface IToggles
    {
        bool IsEnabled(string toggle, IEnumerable<string> identifiers = null);
    }
}