using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace fodorr_dolgozat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string connstring = "server=localhost;user id=root;password=;database=fodorr;Charset=utf8;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void lekerdez_Click(object sender, RoutedEventArgs e)
        {
            lbAdatok.Items.Clear();
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM filmek", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string sor = $"{reader["filmazon"]};{reader["cim"]};{reader["ev"]};{reader["szines"]};{reader["mufaj"]};{reader["hossz"]}";
                    lbAdatok.Items.Add(sor);
                }
            }
        }

        private void lbAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] adatok = lbAdatok.SelectedItem.ToString().Split(';');
            lbFilmAzon.Content = adatok[0];
            tb1.Text = adatok[1];
            tb2.Text = adatok[2];
            tb3.Text = adatok[3];
            tb4.Text = adatok[4];
            tb5.Text = adatok[5];
        }

        private void modosit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connstring))
                {
                    conn.Open();

                    string sql = $"UPDATE filmek SET " +
                                 $"cim = '{tb1.Text}', " +
                                 $"ev = {int.Parse(tb2.Text)}, " +
                                 $"szines = '{tb3.Text}', " +
                                 $"mufaj = '{tb4.Text}', " +
                                 $"hossz = {int.Parse(tb5.Text)} " +
                                 $"WHERE filmazon = '{lbFilmAzon.Content}'";

                    var cmd = new MySqlCommand(sql, conn);
                    int sorok = cmd.ExecuteNonQuery();

                    MessageBox.Show($"{sorok} sor módosítva.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt: " + ex.Message);
            }
        }
    }
}
