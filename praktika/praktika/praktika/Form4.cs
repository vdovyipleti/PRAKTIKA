using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace praktika
{
    public partial class Form4 : Form
    {
        int id;
        int selectedRow;
        SqlConnection sqlConnection = null;
        public Form4()
        {
            InitializeComponent();
        }

        private void ReadSingleRowData(DataGridView dataGridView, IDataRecord dataRecord)
        {
            dataGridView.Rows.Add(
                dataRecord.GetInt32(0),
                dataRecord.GetString(1),
                dataRecord.GetString(2),
                dataRecord.GetString(3),
                dataRecord.GetString(4),
                dataRecord.GetDecimal(5),
                dataRecord.GetInt32(6));

        }
        /// <summary>
        /// Метод заполнения таблицы
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="queryString"></param>
        private void RefreshDataGridData(DataGridView dataGridView)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
            dataGridView.Rows.Clear();
            string queryString =
                "SELECT [id_книги],[Название],[Первый_автор]  ,[Издательство] ,[Место_издания] ,[Количество_страниц] ,[Цена] " +
                "FROM [Книги] ";
            SqlCommand sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
                ReadSingleRowData(dataGridView, reader);
            reader.Close();
            sqlConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            this.Hide();
            form.ShowDialog();
            this.Show();

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["praktikaZvereva"].ConnectionString);
            sqlConnection.Open(); //открыли подключение к бд
            RefreshDataGridData(dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string searchQuery = "SELECT * FROM Книги " +
                    "WHERE id_книги LIKE '%" + textBox1.Text + "%' OR " +
                    "Название LIKE '%" + textBox1.Text + "%' OR " +
                    "Первый_автор LIKE '%" + textBox1.Text + "%' OR " +
                    "Издательство LIKE '%" + textBox1.Text + "%' OR " +
                    "Место_издания  LIKE '%" + textBox1.Text + "%' OR " +
                    "Цена LIKE '%" + textBox1.Text + "%' OR " +
                    "Количество_страниц LIKE '%" + textBox1.Text + "%'";
            SqlCommand sqlCommand = new SqlCommand(searchQuery, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
                ReadSingleRowData(dataGridView1, reader);
            reader.Close();
        }

    }
}
