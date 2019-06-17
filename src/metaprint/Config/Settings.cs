using System.Collections.Generic;
using System.Linq;
using Extensions;

namespace Config
{
    public class Settings
    {
        public IEnumerable<string> Extensions { get; set; }

        public Settings()
        {
            SetDefaults();
        }
        public Settings(IEnumerable<string> fileExtensions)
        {
            var safeExt = fileExtensions.ToList();
            if (safeExt.IsNullOrEmpty())
            {
                SetDefaults();
            }
            else
            {
                Extensions = safeExt.Select(i =>
                {
                    return i.StartsWith('.') ? i.ToLowerInvariant() : "." + i.ToLowerInvariant();
                });
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