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

namespace Parking
{
    public partial class Form5 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Parking"].ConnectionString);
            sqlConnection.Open();
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parkingDataSet.klient". При необходимости она может быть перемещена или удалена.
            this.klientTableAdapter.Fill(this.parkingDataSet.klient);

            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.product_id, b.klient_name, a.product_name, a.product_price FROM [product] a, [klient] b WHERE a.product_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "ФИО клиента";
            dataGridView1.Columns[2].HeaderText = "Наименование товара";
            dataGridView1.Columns[3].HeaderText = "Стоимость товара";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
              "INSERT INTO [product] (product_name, product_price, product_klient_id) Values (N'" + textBox1.Text + "',N'" + maskedTextBox1.Text + "'," + comboBox2.SelectedValue + ")", sqlConnection);
            command.ExecuteNonQuery().ToString();


            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.product_id, b.klient_name, a.product_name, a.product_price FROM [product] a, [klient] b WHERE a.product_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
               "UPDATE [product] SET [product_name]=N'" + textBox1.Text + "',[product_price]=N'" + maskedTextBox1.Text + "', [product_klient_id]=" + comboBox2.SelectedValue + " where [product_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);
            command.ExecuteNonQuery().ToString();
            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.product_id, b.klient_name, a.product_name, a.product_price FROM [product] a, [klient] b WHERE a.product_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Удалить продажу для клиента?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                           == DialogResult.Yes)
                {

                    SqlCommand command = new SqlCommand(
               "delete from [product] where [product_id]='" + Convert.ToString(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value) + "'", sqlConnection);

                    command.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Непредвиденная ошибка!");
            }

            SqlDataAdapter adapter = new SqlDataAdapter(
               "SELECT a.product_id, b.klient_name, a.product_name, a.product_price FROM [product] a, [klient] b WHERE a.product_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox2.FindString(comboBox2.Text);
        }
    }
}
