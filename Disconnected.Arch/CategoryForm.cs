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
    public partial class CategoryForm : Form
    {
        SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=Northwind;UID=sa;Password=1;");

        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            GetCategories();
        }

        private void GetCategories()
        {
            //SqlDataAdapter kendisi connection acip kapatiyor. Yalnizca listelerken gecerli (Disconnected Arch.)
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select * from Categories", sqlConnection);
            DataTable dataTable = new DataTable();  // Tek select sorgusu varsa (tek tablo) datatable isimizi gorur, join isleminde dataset kullanabiliriz. new DataSet();
            sqlDataAdapter.Fill(dataTable);
            categoriesDataGrid.DataSource = dataTable;
            categoriesDataGrid.Columns["CategoryId"].Visible = false;
            categoriesDataGrid.Columns["Picture"].Visible = false;

        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = string.Format("insert Categories(CategoryName,Description) values ('{0}','{1}')",
                                                    txtCategoryName.Text, txtDescription.Text);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            var affectedLine = sqlCommand.ExecuteNonQuery();
            if (affectedLine > 0)
            {
                MessageBox.Show("Category Added");
                GetCategories();
            }
            else
            {
                MessageBox.Show("Failed");
            }

        }
    }
}
