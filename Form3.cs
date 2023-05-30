using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Parking
{
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Parking"].ConnectionString);
            sqlConnection.Open();
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parkingDataSet.klient". При необходимости она может быть перемещена или удалена.
            this.klientTableAdapter.Fill(this.parkingDataSet.klient);
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.card_id, a.card_name, a.card_klient_id, b.klient_name FROM [card] a, [klient] b WHERE a.card_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название карты";
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "ФИО клиента";
        }

        private void ComboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox2.FindString(comboBox2.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
               "INSERT INTO [card] (card_name, card_klient_id) Values (N'" + comboBox1.Text + "'," + comboBox2.SelectedValue + ")", sqlConnection);
            command.ExecuteNonQuery().ToString();


            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.card_id, a.card_name, a.card_klient_id, b.klient_name FROM [card] a, [klient] b WHERE a.card_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
               "UPDATE [card] SET [card_name]=N'" + comboBox1.Text + "', [card_klient_id]=" + comboBox2.SelectedValue + " where [card_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.card_id, a.card_name, a.card_klient_id, b.klient_name FROM [card] a, [klient] b WHERE a.card_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить карту клиента?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
                {

                    SqlCommand command = new SqlCommand(
               "delete from [card] where [card_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);

                    command.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }

            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.card_id, a.card_name, a.card_klient_id, b.klient_name FROM [card] a, [klient] b WHERE a.card_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
