using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace praktika
{
    public partial class Form3 : Form
    {
        SqlConnection sqlConnection = null;
        bool See = true;
        private int _countTries = 0;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            textBox1.MaxLength = 50;
            textBox2.UseSystemPasswordChar = true;
            panel1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
            sqlConnection.Open();
            string login = textBox1.Text;
            string password = textBox2.Text;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            string queryClient = $" SELECT * FROM Читатели WHERE Логин = '{login}' and Пароль = '{password}';";
            SqlCommand sqlcommandCLient = new SqlCommand(queryClient, sqlConnection);

            sqlDataAdapter.SelectCommand = sqlcommandCLient;
            sqlDataAdapter.Fill(Table);
            if (Table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно зашли!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form4 form4 = new Form4();
                this.Hide();
                form4.ShowDialog();
                this.Show();

            }

            else
            {
                MessageBox.Show("Неверно введен пароль", "Ошибка входа");
                textBox1.Text = "";
                textBox2.Text = "";
               // Reset();
                _countTries++;
                panel1.Visible = true;
            }
            sqlConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (See)
                button2.BackColor = Color.AliceBlue ;
            else
                button2.BackColor = Color.Red;
            See = !See;
            textBox2.UseSystemPasswordChar = See;
            
        }

        private Bitmap drawImage(string txt, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            SolidBrush solid = new SolidBrush(Color.White);
            g.FillRectangle(solid, 0, 0, bmp.Width, bmp.Height);
            Font font = new Font("MS Reference Sans Serif", 30);
            solid = new SolidBrush(Color.Blue);
            g.DrawString(txt, font, solid, bmp.Width / 2  -  (txt.Length / 2) * font.Size,
                (bmp.Height / 2)  -  font.Size);

            int count = 0;
            Random rd = new Random();
            while (count < 1000)
            {
                solid = new SolidBrush(Color.YellowGreen);
                g.FillEllipse(solid, rd.Next(0, bmp.Width), rd.Next(0, bmp.Height), 4, 2);
                count++;
            }
            count = 0;
            while (count < 25)
            {
                g.DrawLine(new Pen(Color.Pink), rd.Next(0, bmp.Width), rd.Next(0, bmp.Height),
                    rd.Next(0, bmp.Width), rd.Next(0, bmp.Height));
                count++;
            }
            return bmp;
        }

        private string captchText;
        public String randString()
        {
            Random rd = new Random();
            int num = rd.Next(10000, 99999);
            string text = md5(num.ToString());
            text = text.ToUpper();
            text = text.Substring(0, 6);
            return text;
        }
        private void Reset()
        {
            captchText = this.randString();
            textBox3.Text = "";
            pictureBox1.BackgroundImage = drawImage(captchText, pictureBox1.Width, pictureBox1.Height);
        }

        public static string md5(String data)
        {
            return BitConverter.ToString(encryptData(data)).Replace(" — ", "").ToLower();
        }

        public static byte[] encryptData(String data)
        {
            MD5CryptoServiceProvider mD5Crypto = new MD5CryptoServiceProvider();
            byte[] hashedBytes;
            UTF8Encoding utf8 = new UTF8Encoding();
            hashedBytes = mD5Crypto.ComputeHash(utf8.GetBytes(data));
            return hashedBytes;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == captchText)
            {
                MessageBox.Show("Капча верна, теперь введмте заново логин и пароль", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _countTries = 0;
                return;
            }
            _countTries++;
            switch (_countTries)
            {
                case 2:
                    textBox3.Text = "";
                    MessageBox.Show("Не верно. Попробуйте еще раз через 3 мин.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Start();
                    panel1.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    break;
                case 3:
                    MessageBox.Show("Перезапуск приложения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    break;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval == 3000)
            {
                timer1.Stop();
                Reset();
                panel1.Enabled = true;

                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }

        }
    }
}
