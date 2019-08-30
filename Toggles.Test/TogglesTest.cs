using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Toggles.Test
{
    public class TogglesTest
    {
        public TogglesTest()
        {
            Instance = new Toggles();
            Interface = Instance;
        }

        private Toggles Instance { get; }
        private IToggles Interface { get; }

        private readonly string Toggle = "Toggle";

        private void SetOnlyToggleDefault(string toggle, bool value) =>
            Instance.CacheToggleValues(toggle, new Dictionary<string, bool> {{toggle, value}});

        private void SetToggleDefaultAndValues(string toggle, bool defaultValue, Dictionary<string, bool> values)
        {
            values.Add(toggle, defaultValue);
            Instance.CacheToggleValues(toggle, values);
        }

        [Fact]
        public void Null_toggle_is_false() => Interface.IsEnabled(null).Should().BeFalse();

        [Fact]
        public void Empty_toggle_is_false() => Interface.IsEnabled(string.Empty).Should().BeFalse();

        [Fact]
        public void Whitespace_only_toggle_is_false() => Interface.IsEnabled(" ").Should().BeFalse();

        [Fact]
        public void Unknown_toggle_is_false() => Interface.IsEnabled("unknown").Should().BeFalse();

        [Fact]
        public void Toggle_default_is_returned_if_no_identifiers_are_specified()
        {
            SetOnlyToggleDefault(Toggle, true);
            Interface.IsEnabled(Toggle).Should().BeTrue();
        }

        [Fact]
        public void Toggle_default_is_returned_if_unknown_identifiers_are_specified()
        {
            SetOnlyToggleDefault(Toggle, true);
            Interface.IsEnabled(Toggle, new[] {"unknown 1", "unknown 2"}).Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Toggle_value_of_identifier_takes_precedence_over_default(bool value)
        {
            SetToggleDefaultAndValues(Toggle, !value, new Dictionary<string, bool> {{"identifier", value}});
            Interface.IsEnabled(Toggle, new[] {"identifier"}).Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Toggle_value_of_first_identifier_takes_precedence(bool value)
        {
            SetToggleDefaultAndValues(Toggle, !value, new Dictionary<string, bool>
            {
                {"identifier1", value},
                {"identifier2", !value}
            });

            Interface.IsEnabled(Toggle, new[] {"identifier1", "identifier2"}).Should().Be(value);
        }
    }
}