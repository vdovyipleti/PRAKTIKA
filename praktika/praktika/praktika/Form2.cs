using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace praktika
{
    public partial class Form2 : Form
    {
         SqlConnection sqlConnection = null;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
            }
            sqlConnection.Open();
            var id_читательского_билета = textBox6.Text;
            var ФИО_Читателя = textBox1.Text;
            var Номер_телефона = textBox2.Text;
            var Дата_рождения = textBox3.Text;
            var Логин = textBox4.Text;
            var Пароль = textBox5.Text;

            //        string addQueryClient = $"INSERT into  Читатели ( ФИО_Читателя, Номер_телефона, Дата_рождения,Логин,Пароль)" +
            //$"values ( '{ФИО_Читателя}', '{Номер_телефона}', '{Дата_рождения}', '{Логин}', '{Пароль})";

            //        SqlCommand command = new SqlCommand(addQueryClient, sqlConnection);
            //        command.ExecuteNonQuery();
            //        MessageBox.Show("Запись успешно создана!", "Успех");


            SqlCommand command = new SqlCommand(
               $"INSERT INTO [Читатели] ( id_читательского_билета, ФИО_Читателя, Номер_телефона, Дата_рождения,Логин,Пароль) VALUES (@id_читательского_билета,@ФИО_Читателя, @Номер_телефона, @Дата_рождения, @Логин, @Пароль)", sqlConnection);

            DateTime date = DateTime.Parse(textBox3.Text);
            command.Parameters.AddWithValue("id_читательского_билета", textBox6.Text);
            command.Parameters.AddWithValue("ФИО_Читателя", textBox1.Text);
            command.Parameters.AddWithValue("Номер_телефона", textBox2.Text);
            command.Parameters.AddWithValue("Дата_рождения", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("Логин", textBox4.Text);
            command.Parameters.AddWithValue("Пароль", textBox5.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Успех");


            sqlConnection.Close();
            this.Close();

            Form4 client = new Form4();
            this.Hide();
            client.ShowDialog();
            this.Show();


        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'а') && (e.KeyChar <= 'я')) //  строчные буквы  разрешены
                return;
            if ((e.KeyChar >= 'А') && (e.KeyChar <= 'Я')) //  прописные буквы разрешены
                return;
            if (e.KeyChar == (char)Keys.Space) // пробел разрешён
                return;
            if (e.KeyChar == (char)Keys.Back)  // BackSpase разрешён
                return;
            e.KeyChar = '\0';	// остальные символы запрещены (игнорировать)

        }

        

        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["praktikaZvereva"].ConnectionString);
            sqlConnection.Open(); //открыли подключение к бд


        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "(*.jpg)|*.jpg";
            openFileDialog1.Filter = "Image Files (*.JPG; *.PNG|*.JPG; *.PNG|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    pictureBox2.Image = new Bitmap(openFileDialog1.FileName);


                    File.Copy(textBox4.Text, Path.Combine(@"H:\praktika\praktika\Photo", Path.GetFileName(textBox4.Text)), true);

                    MessageBox.Show("Изображение успешно загружено в папку Photo!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch
                {
                    MessageBox.Show("Произошла ошибка: Изображение не может быть загружено", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
    }
}
