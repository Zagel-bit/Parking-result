using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Parking
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Parking"].ConnectionString);
            sqlConnection.Open();

            
                SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT klient_id, klient_name, klient_phone, klient_car_name, klient_car_number FROM klient", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            
            dataGridView1.DataSource = ds.Tables[0];

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ФИО";
            dataGridView1.Columns[2].HeaderText = "Номер телефона";
            dataGridView1.Columns[3].HeaderText = "Марка машины";
            dataGridView1.Columns[4].HeaderText = "Номер машины";

        }

        private void Button1_Click(object sender, EventArgs e) // Кнопка добавления данных в таблицу
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO [klient] (klient_name, klient_phone, klient_car_name, klient_car_number) " +
                "Values (N'" + textBox1.Text + "',N'" + maskedTextBox1.Text + "',N'" + textBox3.Text + "',N'" + maskedTextBox2.Text + "')", sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT klient_id, klient_name, klient_phone, klient_car_name, klient_car_number FROM klient", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void Button2_Click(object sender, EventArgs e) //Кнопка для изменения данных в таблице
        {
           SqlCommand command = new SqlCommand(
               "UPDATE [klient] SET [klient_name]=N'" + textBox1.Text + "', [klient_phone]=N'" + maskedTextBox1.Text + "', [klient_car_name]=N'" + textBox3.Text + "', [klient_car_number]=N'" + maskedTextBox2.Text + "' where [klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'",sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT klient_id, klient_name, klient_phone, klient_car_name, klient_car_number FROM klient", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void Button3_Click(object sender, EventArgs e) // Кнопка удаления данных из таблицы
        {
            
            try
            {
                if (MessageBox.Show("Стереть всю информацию клиента?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
                {

                    SqlCommand command = new SqlCommand(
               "delete from [parking] where [parking_klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
                    SqlCommand command1 = new SqlCommand(
               "delete from [card] where [card_klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
                    SqlCommand command2 = new SqlCommand(
               "delete from [product] where [product_klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
                    SqlCommand command3 = new SqlCommand(
               "delete from [service] where [service_klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
                    SqlCommand command4 = new SqlCommand(
               "delete from [klient] where [klient_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
                    
                    command.ExecuteNonQuery().ToString();
                    command1.ExecuteNonQuery().ToString();
                    command2.ExecuteNonQuery().ToString();
                    command3.ExecuteNonQuery().ToString();
                    command4.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }

            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT klient_id, klient_name, klient_phone, klient_car_name, klient_car_number FROM klient", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
