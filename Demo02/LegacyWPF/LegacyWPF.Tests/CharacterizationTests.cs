using System;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using Xunit;

namespace LegacyWPF.Tests
{
    [UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
    public class CharacterizationTests
    {
        [Fact]
        public void InitialState()
        {
            WpfApprovals.Verify(() => new MainWindow());
        }

        [Fact]
        public void SearchContainerWithFullInfo()
        {
            WpfApprovals.Verify(() =>
            {
                var window = new MainWindow();
                window.SearchText = "MEDU7485729";
                window.DoSearch();
                return window;
            });
        }

        [Fact]
        public void SearchContainerWithNoShips()
        {
            WpfApprovals.Verify(() =>
            {
                var window = new MainWindow();
                window.SearchText = "TRLU5539435";
                window.DoSearch();
                return window;
            });
        }

        [Fact]
        public void SearchUnknownContainer()
        {
            WpfApprovals.Verify(() =>
            {
                var window = new MainWindow();
                window.SearchText = "MEDU9000123";
                window.DoSearch();
                return window;
            });
        }

        [Fact]
        public void SearchEmptyInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var window = new MainWindow();
                window.SearchText = String.Empty;
                window.DoSearch();
            });
        }
    }
}