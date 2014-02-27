using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

namespace Introduction
{
    [UseReporter(typeof(DiffReporter))]
    public class PayStationTests
    {
        [Fact]
        public void PaidForOneHour_ClassicWay()
        {
            var payStation = new PayStation();

            payStation.InsertCoin(1);
            payStation.InsertCoin(0.5m);
            var display = payStation.ReadDisplay();

            Assert.Equal("Paid:\t1,50 EUR\r\n" +
                         "Parking Time:\t1 Hour", display);
        }

        [Fact]
        public void PaidForOneHour_ApprovalWay_Manual()
        {
            var payStation = new PayStation();

            payStation.InsertCoin(1);
            payStation.InsertCoin(0.5m);
            var display = payStation.ReadDisplay();

            Assert.Equal("", display);
        }

        [Fact]
        public void PaidForOneHour_ApprovalWay_WithTool()
        {
            var payStation = new PayStation();

            payStation.InsertCoin(1);
            payStation.InsertCoin(0.5m);
            var display = payStation.ReadDisplay();

            Approvals.Verify(display);
        }
    }

    //TODO: implementazione di partenza non completa
    public class PayStation
    {
        private const decimal rate = 1.5m;
        private decimal total;
        private int hours;

        public void InsertCoin(decimal eur)
        {
            total += eur;
            hours = (int)Math.Floor(total / rate);
        }

        public string ReadDisplay()
        {
            return String.Format("{0} - {1}", total, hours);
        }
    }
}