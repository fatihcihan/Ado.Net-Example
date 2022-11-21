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

namespace SqlParametersWithProcedure
{
    public partial class ProductForm : Form
    {

        SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=Northwind;UID=sa;Password=1;");

        public ProductForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "Insert Products(ProductName,UnitPrice,UnitsInStock) values(@productName,@unitPrice,@unitsInStock)";
            sqlCommand.Parameters.AddWithValue("@productName", txtProductName.Text);
            sqlCommand.Parameters.AddWithValue("@unitPrice", numPrice.Value);
            sqlCommand.Parameters.AddWithValue("@unitsInStock", numUnitsInStock.Value);
            sqlConnection.Open();
            int affectedLine = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (affectedLine > 0)
                MessageBox.Show("Product Added");
            else
                MessageBox.Show("Error");
        }

        private void GetProducts()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from Products", sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetProducts();
        }
    }
}
