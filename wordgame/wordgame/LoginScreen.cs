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

namespace wordgame
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private int userNumber;
        private Boolean userAccess = false;

        private string _name;

        public string playerName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private int userCounter()
        {
            SqlConnection baglanti = new SqlConnection();
            baglanti.ConnectionString = "Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True";
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT MAX(id) as userNumber FROM users";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            dr.Read();
            userNumber = (int)dr["userNumber"];
            baglanti.Close();
            return userNumber;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int counter = 1; counter <= userCounter(); counter++)
            {
                SqlConnection baglanti = new SqlConnection();
                baglanti.ConnectionString = "Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True";
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT * FROM users where id = " + counter;
                komut.Connection = baglanti;
                komut.CommandType = CommandType.Text;

                SqlDataReader dr;
                baglanti.Open();
                dr = komut.ExecuteReader();
                dr.Read();
                if (textBox1.Text == dr["userName"].ToString() && textBox2.Text == dr["userPass"].ToString())
                {
                    userAccess = true;
                    _name = dr["userName"].ToString();
                }
                else
                {
                    if (userAccess != true)
                    {
                        userAccess = false;
                    }
                }
                baglanti.Close();
            }
            if (userAccess == true)
            {
                MessageBox.Show("Kullanıcı adı ve şifre doğru oyun sayfasına yönlendiriliyorsunuz..");
                GameScreen gameScreen = new GameScreen();
                gameScreen.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış lütfen tekrar deneyin..");
            }
            reset();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void LoginScreen_Load(object sender, EventArgs e)
        {

        }

        private void reset()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string userName = textBox3.Text;
            string userPass = textBox4.Text;
            SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True");
            string komut = "insert into users(userName, userPass) values ('" + userName + "','" + userPass + "')";
            SqlCommand sorgu = new SqlCommand(komut, baglanti);
            try
            {
                baglanti.Open();
                sorgu.ExecuteNonQuery();
                MessageBox.Show("Kayıt Eklendi");
                baglanti.Close();
            }
            catch
            {
                MessageBox.Show("Ekleme işlemi Yapılamadı");
            }
            reset();
        }
    }
}
