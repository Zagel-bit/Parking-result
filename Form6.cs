using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking
{
    public partial class Form6 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Parking"].ConnectionString);
            sqlConnection.Open();
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parkingDataSet.klient". При необходимости она может быть перемещена или удалена.
            this.klientTableAdapter.Fill(this.parkingDataSet.klient);

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            // Display the date as "19/10/2000 20:10:00".  
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            // Display the date as "19/10/2000 20:10:00".  
            dateTimePicker2.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            


            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ФИО клиента";
            dataGridView1.Columns[2].HeaderText = "Название парковочного места";
            dataGridView1.Columns[3].HeaderText = "Цена по тарифу";
            dataGridView1.Columns[4].HeaderText = "Стоимость";
            dataGridView1.Columns[5].HeaderText = "Время въезда";
            dataGridView1.Columns[6].HeaderText = "Время выезда";
            dataGridView1.Columns[7].HeaderText = "Марка машины";
            dataGridView1.Columns[8].HeaderText = "Номер машины";
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand(
                 "SELECT a.card_name FROM [card] a, [klient] b WHERE b.klient_name = N'"+ comboBox2.Text +"' and a.card_klient_id = b.klient_id", sqlConnection);
            
            label7.Text = (string)sqlCommand.ExecuteScalar();
            if (label7.Text != "")
            {
                label8.Visible = true;
            }
            else
            {
                label8.Visible= false;
            }
        }

        private void СomboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox3.SelectedIndex;
            if (comboBox3.SelectedIndex == 0) { 
                textBox1.Text = "Крытый паркинг";
                textBox2.Text = "5";
                dateTimePicker2.Enabled = true; 
            }       //За минуту

            if (comboBox3.SelectedIndex == 1) {
                textBox1.Text = "Крытый паркинг";
                textBox2.Text = "250";
                dateTimePicker2.Enabled = true; 
            }     //За час

            if (comboBox3.SelectedIndex == 2) {
                textBox1.Text = "Крытый паркинг";
                textBox2.Text = "2000";
                dateTimePicker2.Enabled = true; 
            }    //За сутки

            if (comboBox3.SelectedIndex == 3)                                //Гостевой проверка на гостевую карту
            {
                try
                {
                    dateTimePicker2.Enabled = false;
                    if (label7.Text == "Гостевая карта")
                    {

                        textBox1.Text = "Открытый паркинг";
                        textBox2.Text = "0";

                        DateTime date_start = dateTimePicker1.Value;
                        DateTime date_end = date_start.AddHours(1);

                        dateTimePicker2.Value = date_end;
                    }
                   
                    else { 
                        textBox1.Text = "Открытый паркинг";
                        textBox2.Text = "Ошибка! У клиента нет гостевой карты.";
                        dateTimePicker2.Enabled = false; }
                }
                catch (Exception)
                {
                    MessageBox.Show("Непредвиденная ошибка!");
                }
            }      

            if (comboBox3.SelectedIndex == 4) {                 //За месяц в любое время вип место проверка на клубную карту
                try
                {
                    dateTimePicker2.Enabled = false;
                    if (label7.Text == "Клубная карта")
                    {
                        textBox1.Text = "Подземный паркинг" ;
                        textBox2.Text = "10000";
                        
                        DateTime date_start = dateTimePicker1.Value;
                        DateTime date_end = date_start.AddMonths(1);

                        dateTimePicker2.Value = date_end;
                    }
                    else {
                        textBox1.Text = "Подземный паркинг";
                        textBox2.Text = "Ошибка! У клиента нет клубной карты.";
                        
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Непредвиденная ошибка!");
                }
            }      
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker2.Enabled = false;
                if (textBox2.Text == "10000")
                {
                    DateTime date_start = dateTimePicker1.Value;
                    DateTime date_end = date_start.AddMonths(1);

                    dateTimePicker2.Value = date_end;

                }
                else if (textBox2.Text == "0")
                {

                    textBox1.Text = "Открытый паркинг";
                    textBox2.Text = "0";

                    DateTime date_start = dateTimePicker1.Value;
                    DateTime date_end = date_start.AddHours(1);

                    dateTimePicker2.Value = date_end;
                }

                else
                {
                    dateTimePicker2.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "10000")
            {
                int summ = 10000;

                SqlCommand command = new SqlCommand(
                  "INSERT INTO [parking] (parking_type, parking_price, parking_start, parking_summ, parking_stop, parking_klient_id) Values (N'" + textBox1.Text + "',N'" + textBox2.Text + "',N'" + dateTimePicker1.Text + "', N'" + summ + "' ,N'" + dateTimePicker2.Text + "',N'" + comboBox2.SelectedValue + "')", sqlConnection);
                
                command.ExecuteNonQuery().ToString();


                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                SqlCommand command = new SqlCommand(
                  "INSERT INTO [parking] (parking_type, parking_price, parking_start, parking_stop, parking_klient_id) Values (N'" + textBox1.Text + "',N'" + textBox2.Text + "',N'" + dateTimePicker1.Text + "',N'" + dateTimePicker2.Text + "',N'" + comboBox2.SelectedValue + "')", sqlConnection);
                
                command.ExecuteNonQuery().ToString();


                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
               "UPDATE [parking] SET [parking_type]=N'" + textBox1.Text + "',[parking_price]=N'" + textBox2.Text + "',[parking_start]=N'"+ dateTimePicker1.Text+ "', [parking_stop]=N'" + dateTimePicker2.Text+"', [parking_klient_id]=" + comboBox2.SelectedValue + " where [parking_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить продажу парковочного места для клиента?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
                {

                    SqlCommand command = new SqlCommand(
               "delete from [parking] where [parking_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);

                    command.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }

            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button4_Click(object sender, EventArgs e)
        {

            DateTime dateTime = new DateTime();
            dateTime = DateTime.Now;

            SqlCommand sqlCommand = new SqlCommand(
                "SELECT parking_start FROM [parking] WHERE [parking_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            string dateTime1 = (string)sqlCommand.ExecuteScalar();
            
            DateTime dateTime2 = DateTime.Parse( dateTime1);

            TimeSpan timeSpan = dateTime.Subtract(dateTime2);

            //DateTime dateTime1 = (DateTime)sqlCommand.ExecuteScalar(); пока не удалять

            SqlCommand sqlCommand2 = new SqlCommand(
               "SELECT parking_price FROM [parking] WHERE [parking_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            string price = (string)sqlCommand2.ExecuteScalar();
            
            int price_int = Convert.ToInt32(price);

            int a = timeSpan.Days;
            int a1 = timeSpan.Days;
            int a2 = timeSpan.Days;
            int b = timeSpan.Hours;
            int b1 = timeSpan.Hours;
            int c = timeSpan.Minutes;
            int result = 0;
            int summ = 0;
            a = a * 24 * 60; //Получаем минуты за количество "а" дней
            b = b * 60; //Получаем минуты за количество "b" часов
                        //Минуты за минуты не пишем

            a1 = a1 * 24; // Получаем часы за количество "а" дней
                          // Получаем часы за часы
                          // Получаем не получаем минуты
            
                          //Получаем дни за дни
            
            if (price_int == 5)
            {
                result = a + b + c;
                summ = result * price_int;
            }
            else if (price_int == 250)
            {
                result = a1 + b1;
                summ = result * price_int;
            }
            else if (price_int == 2000)
            {
                result = a2;
                summ = result * price_int;
            }
            else if (price_int == 0)
            {
                summ = 0;
            }
            else if (price_int == 10000)
            {
                summ = 10000;
            }



            SqlCommand command = new SqlCommand(
               "UPDATE [parking] SET [parking_summ]=N'" + summ + "',[parking_stop]=N'" + dateTime + "' where [parking_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.parking_id, b.klient_name, a.parking_type, a.parking_price, a.parking_summ, a.parking_start, a.parking_stop, b.klient_car_name, b.klient_car_number FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];







        }
    }
}
