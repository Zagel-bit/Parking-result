using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration; //Подключение базы
using System.Data.SqlClient; //подключение работы с sql


using Excel = Microsoft.Office.Interop.Excel;

namespace Parking
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Parking"].ConnectionString);
            sqlConnection.Open();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2();
            F2.ShowDialog();


        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Form3 F3 = new Form3();
            F3.ShowDialog();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Form4 F4 = new Form4();
            F4.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 F5 = new Form5();
            F5.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form6 F6 = new Form6();
            F6.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Возникли проблемы? \nТелефон системного администратора \n+7(999)-123-11-22");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.parking_id, a.parking_type, a.parking_summ, b.klient_name FROM [parking] a, [klient] b WHERE a.parking_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Тип парковочного места";
            dataGridView1.Columns[2].HeaderText = "Итоговая сумма по продажи";
            dataGridView1.Columns[3].HeaderText = "ФИО клиента";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.service_id, a.service_name, a.service_price, b.klient_name FROM [service] a, [klient] b WHERE a.service_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название предоставленной услуги";
            dataGridView1.Columns[2].HeaderText = "Цена предоставленной услуги";
            dataGridView1.Columns[3].HeaderText = "ФИО клиента";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT a.product_id, a.product_name, a.product_price, b.klient_name FROM [product] a, [klient] b WHERE a.product_klient_id = b.klient_id", sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Название проданного товара";
            dataGridView1.Columns[2].HeaderText = "Цена проданного товара";
            dataGridView1.Columns[3].HeaderText = "ФИО клиента";
        }

        private void button10_Click(object sender, EventArgs e)
        {

            //Приложение
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            //Книга.
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //Таблица.
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
            Excel.Worksheet wsh = ExcelApp.ActiveSheet;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j < dataGridView1.ColumnCount; j++)
                {
                    ExcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                }
            }
            for (int coll = 2; coll <= dataGridView1.ColumnCount; coll++)
            {
                ExcelApp.Cells[1, coll] = dataGridView1.Columns[coll - 1].HeaderCell.Value;
                ExcelApp.Columns.AutoFit();
            }
            //Вызываем нашу созданную эксельку.
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;


        }
    }
}
