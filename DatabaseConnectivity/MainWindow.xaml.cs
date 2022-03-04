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
using System.Data.SqlClient;
using System.Data;

namespace DatabaseConnectivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=YY233879;Initial Catalog=WpfDB;Integrated Security=True");
            LoadData();

            textBox.TextChanged += OnTextChanged;
            button.IsEnabled = false;
        }

        public void LoadData()
        {
            SqlCommand cmd = new SqlCommand("select * from WpfTable",connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            connection.Close();
            dataGrid.ItemsSource = dt.DefaultView;
            var x = dt.Rows;
            int size = dt.Rows.Count;
            if (size != 0)
            {
                label.Content = x[size - 1][1];
            }
            else
            {
                label.Content = "No Data Inserted";
            }
            
            
        }

        private void Insert_Data(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO WpfTable VALUES (@Info)",connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Info",textBox.Text);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            LoadData();
        }
        
        private void Clear_Data(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM WpfTable", connection);
        //    cmd.CommandType = CommandType.Text;
            connection.Open();
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("DBCC CHECKIDENT ('WpfTable', RESEED, 0)", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            LoadData();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text!="")
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }
    }
}
