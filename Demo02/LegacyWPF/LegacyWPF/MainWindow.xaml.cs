using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace LegacyWPF
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<String, String> containerDescriptions = new Dictionary<String, String>
            {
                {"hc40","40' HIGH CUBE"},{"box40","40' BOX"},
                {"box20","20' BOX"},{"open40","40' OPEN TOP"},
                {"open20","20' OPEN TOP"},
            };

        private readonly Dictionary<String, String> shippingDescriptions = new Dictionary<String, String> { { "E", "Exp" }, { "I", "Imp" }, };

        public string SearchText
        {
            get { return txtSearch.Text; }
            set { txtSearch.Text = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter))
                return;

            DoSearch();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        public void DoSearch()
        {
            var sign = txtSearch.Text.Substring(0, 4).ToUpper();
            var number = Int32.Parse(txtSearch.Text.Substring(4, 6));
            var check = txtSearch.Text.Substring(10, 1);

            DataRow row;
            using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            using (var command = new SqlCeCommand("SELECT Sign, Number, Checkdigit, Type FROM Containers WHERE Sign=@Sign AND Number=@Number AND Checkdigit=@Checkdigit", connection))
            {
                command.Parameters.AddWithValue("Sign", sign);
                command.Parameters.AddWithValue("Number", number);
                command.Parameters.AddWithValue("Checkdigit", check);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    row = table.Rows.Count == 0 ? null : table.Rows[0];
                }
            }

            var container = row == null ? null : new Container(row["Sign"].ToString(),
                    Int32.Parse(row["Number"].ToString()), row["Checkdigit"].ToString(), containerDescriptions[row["Type"].ToString()],
                    Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images"), row["Type"] + ".gif"));
            if (container != null)
            {
                containerDetails.DataContext = container;
                containerDetails.Visibility = Visibility.Visible;
            }
            else
            {
                containerDetails.DataContext = null;
                containerDetails.Visibility = Visibility.Collapsed;
            }

            DataTable table1;
            using (var connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["default"].ConnectionString))
            using (var command = new SqlCeCommand("SELECT s.Type, s.Seal, s.Weight, s.ToPort, s.FromPort, s.Ship, s.Forwarder, s.BoardingDate, s.LandingDate FROM Shippings AS s INNER JOIN Containers AS c ON s.ContainerId = c.ContainerId WHERE c.Sign=@Sign AND c.Number=@Number AND c.Checkdigit=@Checkdigit", connection))
            {
                command.Parameters.AddWithValue("Sign", sign);
                command.Parameters.AddWithValue("Number", number);
                command.Parameters.AddWithValue("Checkdigit", check);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    table1 = new DataTable();
                    table1.Load(reader);
                }
            }
            var shippings = new List<Shipping>();
            foreach (DataRow row1 in table1.Rows)
            {
                var boardingDate = (Convert.IsDBNull(row1["BoardingDate"])) ? null : (DateTime?)Convert.ToDateTime(row1["BoardingDate"]);
                var landingDate = (Convert.IsDBNull(row1["LandingDate"])) ? null : (DateTime?)Convert.ToDateTime(row1["LandingDate"]);
                var port = row1["Type"].ToString() == "E" ? row1["ToPort"].ToString() : row1["FromPort"].ToString();
                var date = row1["Type"].ToString() == "E" ? boardingDate.GetValueOrDefault() : landingDate.GetValueOrDefault();
                var s = new Shipping(shippingDescriptions[row1["Type"].ToString()],
                    Int32.Parse(row1["Seal"].ToString()),
                    Int32.Parse(row1["Weight"].ToString()),
                    port, row1["Ship"].ToString(),
                    date, row1["Forwarder"].ToString());
                shippings.Add(s);
            }

            if (shippings.Count > 0)
            {
                lstShippings.ItemsSource = shippings;
                shippingsList.Visibility = Visibility.Visible;
            }
            else
            {
                lstShippings.DataContext = null;
                shippingsList.Visibility = Visibility.Collapsed;
            }
        }

        private void txtSearch_OnGotFocus(object sender, RoutedEventArgs e)
        {
            txtSearch.SelectAll();
        }
    }
}
