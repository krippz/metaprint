using System.Collections.Generic;
using System.Linq;
using metaprint.Extensions;

namespace metaprint.Config
{
    public class Settings
    {
        public IEnumerable<string> Extensions { get; private set; }

        public Settings(IEnumerable<string> fileExtensions)
        {
            var safeExt = fileExtensions.ToList();
            if (safeExt.IsNullOrEmpty())
            {
                SetDefaults();
            }
            else
            {
                Extensions = safeExt.Select(i => i.StartsWith(".") ? i.ToLowerInvariant() : "." + i.ToLowerInvariant());
            }
        }

        private void SetDefaults()
        {
            Extensions = new List<string>
            {
                ".dll",
                ".exe"
            };
        }
    }
}