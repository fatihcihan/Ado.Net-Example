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

namespace Disconnected.Arch
{
    public partial class ProductForm : Form
    {
        SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=Northwind;UID=sa;Password=1;");

        public ProductForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetProducts();
        }

        private void GetProducts()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from Products", sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            productsDataGrid.DataSource = dataTable;
            productsDataGrid.Columns["ProductId"].Visible = false;
            productsDataGrid.Columns["SupplierId"].Visible = false;
            productsDataGrid.Columns["CategoryId"].Visible = false;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal price = numPrice.Value;
            decimal unitsInStock = numUnitsInStock.Value;

            if (productName == "" || price.Equals(null) || unitsInStock.Equals(null))
            {
                MessageBox.Show("Please fill in all fields");
            }

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = string.Format("insert Products (ProductName,UnitPrice,UnitsInStock) values('{0}',{1},{2})",
                                                    productName, price, unitsInStock);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            var affectedLine = sqlCommand.ExecuteNonQuery();
            if (affectedLine > 0)
            {
                MessageBox.Show("Product Added");
                GetProducts();
            }
            else
            {
                MessageBox.Show("Failed");
            }

            sqlConnection.Close();

        }

        private void btnCategoryForm_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.ShowDialog();

        }

        private void productsDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //datagridview'den secili satiri aliyoruz
            txtProductName.Text = productsDataGrid.CurrentRow.Cells["ProductName"].Value.ToString();
            txtProductName.Tag = productsDataGrid.CurrentRow.Cells["ProductId"].Value;
            numPrice.Value = (decimal)productsDataGrid.CurrentRow.Cells["UnitPrice"].Value;
            numUnitsInStock.Value = Convert.ToDecimal(productsDataGrid.CurrentRow.Cells["UnitsInStock"].Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = string.Format("update Products set ProductName='{0}', UnitPrice={1}, UnitsInStock={2} where ProductId={3}",
                                                    txtProductName.Text, numPrice.Value, numUnitsInStock.Value, txtProductName.Tag);

            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            try
            {
                var affectedLine = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                if (affectedLine > 0)
                {
                    MessageBox.Show("Product Updated");
                    GetProducts();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
                sqlConnection.Close();
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (productsDataGrid.CurrentRow != null)
            {
                int id = Convert.ToInt32(productsDataGrid.CurrentRow.Cells["ProductId"].Value);
                SqlCommand sqlCommand = new SqlCommand(string.Format("delete Products where ProductId={0}", id), sqlConnection);
                sqlConnection.Open();
                int affectedLine = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                if (affectedLine > 0)
                {
                    MessageBox.Show("Product deleted");
                    GetProducts();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }

        }
    }
}
