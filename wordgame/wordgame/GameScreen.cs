using System;
using System.Collections;
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
    public partial class GameScreen : Form
    {
        public GameScreen()
        {
            InitializeComponent();
        }

        private int WordLength(string word)
        {
            return 0;
        }
        LoginScreen pName = new LoginScreen();
        private int counter = 1;
        private int questionCounter = 0;
        private string currentWord;
        private int currentWordLength;
        private int indexCounter = 0;
        private char randomLetter;
        private int questionScore = 0;
        private int totalScore = 0;
        private string playerName = "";
        ArrayList lstNumbers = new ArrayList();
        CountDownTimer timer = new CountDownTimer();

        //private string getPlayer()
        //{
        //    SqlConnection baglanti = new SqlConnection();
        //    baglanti.ConnectionString = "Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True";
        //    SqlCommand komut = new SqlCommand();
        //    komut.CommandText = "SELECT * FROM words where id = " + counter;
        //    komut.Connection = baglanti;
        //    komut.CommandType = CommandType.Text;

        //    SqlDataReader dr;
        //    baglanti.Open();
        //    dr = komut.ExecuteReader();
        //    dr.Read();
        //    currentWord = dr["answer"].ToString().ToUpper();
        //    currentWordLength = currentWord.Length;
        //    letterTextBoxAdd();
        //    label3.Text = dr["question"].ToString();
        //    baglanti.Close();
        //    return playerName;
        //}

        private void getWord()
        {
            letterBoxDelete();
            SqlConnection baglanti = new SqlConnection();
            baglanti.ConnectionString = "Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True";
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT * FROM words where id = " + counter;
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            dr.Read();
            currentWord = dr["answer"].ToString().ToUpper();
            currentWordLength = currentWord.Length;
            letterTextBoxAdd();
            label3.Text = dr["question"].ToString();
            baglanti.Close();
            counter++;
        }

        public ArrayList RandomNumbers(int max)
        {
            ArrayList lstNumbers = new ArrayList();
            Random rndNumber = new Random();

            int number = rndNumber.Next(1, max + 1);
            lstNumbers.Add(number);
            int count = 0;

            do
            {
                number = rndNumber.Next(1, max + 1);

                if (!lstNumbers.Contains(number))
                {
                    lstNumbers.Add(number);
                }

                count++;
            } while (count <= 10 * max);

            return lstNumbers;
        }

        private void letterBoxDelete()
        {
            for (int i = 0; i < currentWordLength; i++)
            {
                flowLayoutPanel1.Controls.Remove(flowLayoutPanel1.Controls.Find("txt_" + i, true)[0]);

                foreach (Button btn in flowLayoutPanel1.Controls.OfType<Button>())
                {
                    int controlIndex = int.Parse(btn.Name.Split('_')[1]);
                    if (controlIndex > i)
                    {
                        TextBox txt = (TextBox)flowLayoutPanel1.Controls.Find("txt_" + controlIndex, true)[0];
                        btn.Top = btn.Top - 25;
                        txt.Top = txt.Top - 25;
                    }
                }
            }
        }

        private void letterTextBoxAdd()
        {

            for (int i = 0; i < currentWordLength; i++)
            {
                TextBox textBox = new TextBox();
                int count = flowLayoutPanel1.Controls.OfType<TextBox>().ToList().Count;
                textBox.Location = new System.Drawing.Point(10, 25 * count);
                textBox.Size = new System.Drawing.Size(50, 50);
                textBox.Name = "txt_" + count;
                textBox.Font = new Font(textBox.Font.FontFamily, 36);
                flowLayoutPanel1.Controls.Add(textBox);
                textBox.Enabled = false;
            }
        }
        private void timerStart()
        {
            timer.SetTime(4, 0);

            timer.Start();

            timer.TimeChanged += () => label1.Text = timer.TimeLeftStr.Substring(1, 4);
            //textBox2.Text = timer.TimeLeftStr.Substring(1, 4);
        }

        private void timeCheck()
        {
            if (timer.TimeLeftStr.Substring(1, 4) == "0:00") 
            {
                gameOver();
            }
        }
        private void OyunSayfasi_Load(object sender, EventArgs e)
        {
            getWord();
            timerStart();
            questionScore = currentWordLength * 100;
            label7.Text = totalScore.ToString();
            label6.Text = questionScore.ToString();
            textBox1.Enabled = false;
            button3.Enabled = false;
            timerStart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = true;
            button2.Enabled = false;
            textBox1.Enabled = true;
            timer.Pause();
            questionCounter++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            questionScore -= 100;
            label6.Text = questionScore.ToString();
            if (indexCounter == 0)
            {
                lstNumbers = RandomNumbers(currentWordLength);
            }
            TextBox txt = (TextBox)flowLayoutPanel1.Controls.Find("txt_" + ((int)lstNumbers[indexCounter] - 1), true)[0];
            randomLetter = currentWord[((int)lstNumbers[indexCounter] - 1)];
            txt.Text = randomLetter.ToString();
            indexCounter++;
            if (indexCounter == (currentWordLength - 1))
            {
                indexCounter = 0;
                button2.Enabled = false;
            }
        }

        //4,5,6,7,8,9,10 harfli kelimelerin hepsinden ikişer tane yani toplam 14 tane kelime verilecek 
        private void questionReset()
        {
            questionScore = 0;
            textBox1.Text = "";
            indexCounter = 0;
        }
        private void newGameReset()
        {
            questionScore = 0;
            textBox1.Text = "";
            counter = 1;
            indexCounter = 0;
            questionScore = 0;
            totalScore = 0;
        }

        public void gameOver()
        {
            DateTime now = DateTime.Now;
            timer.Pause();
            MessageBox.Show(pName.playerName +" adlı kullanıcı olarak "+ now +" tarihinde oynadığınız oyunda yarışmayı " + totalScore + " TL kazanarak, " + timer.TimeLeftStr.Substring(1, 1) + "dk " + timer.TimeLeftStr.Substring(3, 2) + "sn süre artırarak tamamladınız! Tebrikler..");
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;
            newGameReset();
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.Show();
            this.Hide();
        }

        public void wrongAnswer()
        {
            MessageBox.Show("Cevabınız maalesef yanlış efenim.. " + (currentWordLength * 100) + " puanlık sorudan hiç puan alamadınız..");
            if ((counter-1) != 14)
            {
                getWord();
            }
            questionReset();
            questionScore = currentWordLength * 100;
            label6.Text = questionScore.ToString();
            timerStart();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            textBox1.Enabled = false;
            SqlConnection baglanti = new SqlConnection();
            baglanti.ConnectionString = "Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True";
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT * FROM words where id = " + (counter - 1);
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;

            SqlDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            dr.Read();

            if (textBox1.Text.Length != 0)
            {
                if (char.ToUpper(textBox1.Text[0]) + textBox1.Text.Substring(1).ToLower() == dr["answer"].ToString())
                {
                    MessageBox.Show("Doğru cevap efeniimm! " + questionScore + " TL kasanıza eklendi..");
                    if ((counter - 1) != 14)
                    {
                        getWord();
                    }
                    totalScore += questionScore;
                    label7.Text = totalScore.ToString();
                    questionReset();
                    questionScore = currentWordLength * 100;
                    label6.Text = questionScore.ToString();
                    timerStart();
                }
                else
                {
                    wrongAnswer();
                }
            }
            else
            {
                wrongAnswer();
            }
            baglanti.Close();
            if (questionCounter == 14)
            {
                gameOver();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeCheck();
        }

        private void reset2()
        {
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string questionValue = textBox2.Text;
            string answerValue = textBox3.Text;
            SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-SAQ5L7OC;Initial Catalog=words;Integrated Security=True");
            string komut = "insert into words(question, answer) values ('" + questionValue + "','" + answerValue + "')";
            SqlCommand sorgu = new SqlCommand(komut, baglanti);
            try
            {
                baglanti.Open();
                sorgu.ExecuteNonQuery();
                MessageBox.Show("Sorunuz Eklendi");
                baglanti.Close();
            }
            catch
            {
                MessageBox.Show("Ekleme işlemi Yapılamadı");
            }
            reset2();
        }
    }
}
