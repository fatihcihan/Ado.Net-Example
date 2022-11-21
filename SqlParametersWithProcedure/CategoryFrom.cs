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
    public partial class CategoryFrom : Form
    {
        SqlConnection sqlConnection = new SqlConnection("Server=localhost;Database=Northwind;UID=sa;Password=1");
        public CategoryFrom()
        {
            InitializeComponent();
        }

        private void CategoryFrom_Load(object sender, EventArgs e)
        {
            GetCategories();
        }

        private void GetCategories()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("procedureCategoryList", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("procedureSaveCategory", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@categoryName", txtCategoryName.Text);
            sqlCommand.Parameters.AddWithValue("@description", txtDescription.Text);
            sqlConnection.Open();
            int affectedLine = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (affectedLine > 0)
            {
                MessageBox.Show("Category Added");
                GetCategories();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                SqlCommand sqlCommand = new SqlCommand("procedureDeleteCategory", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                int categoryId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CategoryID"].Value);
                sqlCommand.Parameters.AddWithValue("@categoryId", categoryId);
                sqlConnection.Open();
                int affectedLine = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                if (affectedLine > 0)
                {
                    MessageBox.Show("Deleted Category");
                    GetCategories();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("procedureUpdateCategory", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            DataGridViewRow dataGridViewRow = dataGridView1.CurrentRow;
            sqlCommand.Parameters.AddWithValue("@categoryId", dataGridViewRow.Cells["CategoryId"].Value);
            sqlCommand.Parameters.AddWithValue("@categoryName", dataGridViewRow.Cells["CategoryName"].Value);
            sqlCommand.Parameters.AddWithValue("@description", dataGridViewRow.Cells["Description"].Value);
            //sqlCommand.Parameters.AddWithValue("@categoryId", dataGridView1.CurrentRow.Cells["CategoryId"].Value);
            //sqlCommand.Parameters.AddWithValue("@categoryName", dataGridView1.CurrentRow.Cells["CategoryName"].Value);
            //sqlCommand.Parameters.AddWithValue("@description", dataGridView1.CurrentRow.Cells["Description"].Value);
            sqlConnection.Open();
            int affectedLine = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            if (affectedLine > 0)
            {
                MessageBox.Show("Updated Category");
                GetCategories();
            }
            else
            {
                MessageBox.Show("Error");
            }

        }
    }
}
