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

namespace GofromirColor
{
    public partial class Form2 : Form
    {

        SqlConnection sqlConnection; // Для подключения к БД. Обьект обьявляем как поле класса  /*
        
        string connectionString;    // Для передаваемых параметров с Form1
        string color_lbl;

        SqlDataAdapter adapter;
        DataSet dataSet;

        /*
         * + вызов Form2 из Form1
         *  Form2 form = new Form2(this.connectionString, this.txtGRPanton.Text);
            form.ShowDialog();  // ShowDialog - блокирует фокус на второй форме      
        */



        public Form2(string connectionString, string color_lbl)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.color_lbl = color_lbl;

            // Устанавливается полное выделение строки и запрет на ручное добавление новых строк
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

        }

        private void Form2_Load(object sender, EventArgs e) //  async 
        {
    
            for (int i=0; i<dataGridView1.ColumnCount; i++)
            {
                if (i==1) dataGridView1.Columns[i].Width = 150;
                else
                    dataGridView1.Columns[i].Width = 50;
            }

            sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

            sqlConnection.Open();    // Открываем БД в асинхронном режиме  // await + OpenAsync


            adapter = new SqlDataAdapter("SELECT * FROM [Colors] WHERE [Color]=@Color", sqlConnection); // Заполнение DATAGRID
            adapter.SelectCommand.Parameters.AddWithValue("Color", color_lbl);
            

            // SqlCommand command = new SqlCommand("SELECT * FROM [Products] WHERE [Name]=@Name", sqlConnection);    // Выбираем все из Products, sqlConnection - для определения куда отпавлять запрос
            // command.Parameters.AddWithValue("Name", textBox1.Text); // Выбираем из таблицы только строку в которой Name=молоко - Введенное значение в textBox1.Text


            dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            dataGridView1.Columns[1].ReadOnly = true;     // Color=1


            DataRow row = dataSet.Tables[0].NewRow(); // добавляем новую строку в DataTable
            dataSet.Tables[0].Rows.Add(row);
            
            
            

            /*  Эксперимент не удался
            DataTable dt = dataSet.Tables[0];
            // добавим новую строку
            DataRow newRow = dt.NewRow();
            newRow["Color"] = "Alice";
            //newRow["Age"] = 24;
            dt.Rows.Add(newRow);
            */


            //  dataGridView1.Columns[0].Width = 50;
            //  dataGridView1.Columns[1].Width = 150;


            // TODO: данная строка кода позволяет загрузить данные в таблицу "col1DataSet.Colors". При необходимости она может быть перемещена или удалена.
            this.colorsTableAdapter.Fill(this.col1DataSet.Colors);
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            // Не сохранились данный в БД
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dataSet);
            // альтернативный способ - обновление только одной таблицы
            //adapter.Update(dt);
            // заново получаем данные из бд
            // очищаем полностью DataSet
            dataSet.Clear();
            // перезагружаем данные
            adapter.Fill(dataSet);


        }
    }
}
