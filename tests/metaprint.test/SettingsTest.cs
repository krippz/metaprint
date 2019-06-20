using Xunit;
using Config;
using System.Collections.Generic;

namespace Metaprint.Test
{
    public class SettingsTest
    {
        [Fact]
        public void ShouldAddDotToExtensionIfAbsent()
        {
            var extensions = new List<string>() { "dll", "exe" };

            var expected = new List<string>() { ".dll", ".exe" };

            var settings = new Settings(extensions);

            var actual = settings.Extensions;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldNormalizeMixedExtensions()
        {
            var extensions = new List<string>() { "dll", ".exe", "1", "JSON" };

            var expected = new List<string>() { ".dll", ".exe", ".1", ".json" };

            var settings = new Settings(extensions);

            var actual = settings.Extensions;

            Assert.Equal(expected, actual);
        }
    }
}