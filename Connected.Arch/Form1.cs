using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connected.Arch
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetProducts();
            GetCategories();
        }


        public void GetProducts()
        {

            // Sql Auth.
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = "Server=localhost;Database=Northwind;UID=sa;Password=1;";

            // Windows Auth.
            //sqlConnection.ConnectionString = "Server=localhost;Database=Northwind;Integrated_Security=true;";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "Select * from Products";
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                var productName = sqlDataReader["ProductName"];
                var unitPrice = sqlDataReader["UnitPrice"];
                var quantityPerUnit = sqlDataReader["QuantityPerUnit"];

                listProcudts.Items.Add(string.Format("{0}-{1}-{2}", productName, unitPrice, quantityPerUnit));
            }
            sqlConnection.Close();
        }

        public void GetCategories()
        {
            SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=Northwind;UID=sa;Password=1;");

            SqlCommand sqlCommand = new SqlCommand("Select * from Categories", sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                var categoryName = sqlDataReader["CategoryName"];
                var description = sqlDataReader["Description"];

                listCategories.Items.Add(string.Format("{0}-{1}", categoryName, description));
            }
            sqlConnection.Close();
        }
    }
}
