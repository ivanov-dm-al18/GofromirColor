using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;    // Для работы с базой данных
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;                // Добавляем для возможности печати
using System.Configuration;     // Для SQL connectionstring подключение через config

// Удалено!

namespace GofromirColor
{
    public partial class Form1 : Form
    {
        // Создаем переменные для определения изменения 14 полей. "Корректировка рецепта"
        string var_txtKRExtenderKg;
        string var_txtKRYellow;
        string var_txtKRRed;
        string var_txtKRRubin;
        string var_txtKRRadomin;
        string var_txtKROrange;
        string var_txtKRPink;
        string var_txtKRViolet;
        string var_txtKRBlue;
        string var_txtKRGreen;
        string var_txtKRBlack;
        string var_txtKRWhite;
        string var_txtKRWater;
        string var_txtKRViscosity;

        //  public Form2 form = new Form2();

        private StringReader myReader;      //  Добавляем для возможности печати

        public bool tf_listbox = false;

        // Добавляем для заполнения DataGridView
        SqlDataAdapter adapter;
        DataSet dataSet;

        // Для замены части запроса
        public string sql_query;




        protected void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPosition = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;
            Font printFont = this.listBox1.Font;
            SolidBrush myBrush = new SolidBrush(Color.Black);

            // Work out the number of lines per page, using the MarginBounds. - Вычисляем количество строк на странице, используя MarginBounds
            linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

            // Iterate over the string using the StringReader, printing each line. - Итерация над строкой с помощью StringReader, печатая каждую строку.
            while (count < linesPerPage && ((line = myReader.ReadLine()) != null))
            {
                // calculate the next line position based on - Рассчитать следующую позицию строки на основе высоты шрифта в соответствии с печатающим устройством
                // the height of the font according to the printing device
                yPosition = topMargin + (count * printFont.GetHeight(ev.Graphics));

                // draw the next line in the rich edit control - Нарисуйте следующую линию в элементе управления редактирования Rich

                ev.Graphics.DrawString(line, printFont, myBrush, leftMargin, yPosition, new StringFormat());
                count++;
            }

            // If there are more lines, print another page. - При наличии других строк распечатайте другую страницу.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;

            myBrush.Dispose();
        }




        public SqlConnection sqlConnection; // Для подключения к БД. Обьект обьявляем как поле класса  /*

        // 3-й варинат подключения + App.config
        //string connectionString => ConfigurationManager.ConnectionStrings["MainDatabase"].ConnectionString;     

        // 2-ой вариант подключения
        //string DataBaseFilePath => Path.Combine(Application.StartupPath, @"Properties\col1.mdf");
        //string connectionString => $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataBaseFilePath};Integrated Security=True";

        // 1ый вариант подключения
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\д\source\repos\GofromirColor\GofromirColor\Properties\col1.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen; // Запуск формы по центру экрана

            // Устанавливается полное выделение строки и запрет на ручное добавление новых строк
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //ClientSize = new Size(816, 454);                // Задаем жесткие размеры формы
            //FormBorderStyle = FormBorderStyle.FixedSingle;  // Границы формы фиксированные

            // Подготовка информационного поля tabpage1  
            lblErrT0.Text = "Внимание! ";
            lblErrT0.Visible = false;

            tabPage3.Parent = null;     // Скрываем элемент "Корректировка рецепта". Были проблемы с автозаполнением tabPage4.         

            // Текстовые поля с отображением цвета не доступны для работы. 11 шт. Вкладка "Новый рецепт".
            textBox14.Enabled = false;
            textBox15.Enabled = false;
            textBox16.Enabled = false;
            textBox17.Enabled = false;
            textBox18.Enabled = false;
            textBox19.Enabled = false;
            textBox20.Enabled = false;
            textBox22.Enabled = false;
            textBox23.Enabled = false;
            textBox24.Enabled = false;
            textBox25.Enabled = false;

            // Текстовые поля с отображением цвета не доступны для работы. 11 шт. Вкладка "Корректировка рецепта".
            textBox41.Enabled = false;
            textBox42.Enabled = false;
            textBox43.Enabled = false;
            textBox44.Enabled = false;
            textBox45.Enabled = false;
            textBox46.Enabled = false;
            textBox35.Enabled = false;
            textBox36.Enabled = false;
            textBox37.Enabled = false;
            textBox38.Enabled = false;
            textBox39.Enabled = false;
            
            // Заполняем Combobox2 - По белому/по бурому
            comboBox2.Items.Insert(0, "По белому");
            comboBox2.Items.Insert(1, "По бурому");

            // Заполняем Combobox1 - Вид экстендера
            comboBox1.Items.Insert(0, "150");
            comboBox1.Items.Insert(1, "151");
            comboBox1.Items.Insert(2, "152");
            comboBox1.Items.Insert(3, "153");
            comboBox1.Items.Insert(4, "OPV");

            // Определяем св-ва txtGRPanton + . Для выпадающего списка
            txtGRPanton.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtGRPanton.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            // Определяем св-ва txtSRPanton + . Для выпадающего списка
            txtSRPanton.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSRPanton.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            // Настраиваем dateTimePicker на странице "Статистика расходов"
            dateTimeIN.Format = DateTimePickerFormat.Custom;
            dateTimeIN.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dateTimeIN.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimeOUT.Format = DateTimePickerFormat.Custom;
            dateTimeOUT.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            dateTimeOUT.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);


            sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

            await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
            SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
            SqlCommand command = new SqlCommand("SELECT * FROM [Colors]", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос

            // Выборка по 1 нужной строке
            // SqlCommand command = new SqlCommand("SELECT * FROM [Products] WHERE [Name]=@Name", sqlConnection);    // Выбираем все из Products, sqlConnection - для определения куда отпавлять запрос
            // command.Parameters.AddWithValue("Name", textBox1.Text); // Выбираем из таблицы только строку в которой Name=молоко - Введенное значение в textBox1.Text

            // Создаем коллекцию
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();


            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    //listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "    " + Convert.ToString(sqlReader["Name"]) + "  " + Convert.ToString(sqlReader["Price"]));

                    //comboBox4.Items.Add(Convert.ToString(sqlReader["Color"]));   // Выгрузка колонки Color в comboBox4
                    // Заполняем MyCollection
                    MyCollection.Add(Convert.ToString(sqlReader["Color"]));
                }
                txtGRPanton.AutoCompleteCustomSource = MyCollection;
                txtKRPanton.AutoCompleteCustomSource = MyCollection;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            finally
            {
                if (sqlReader != null)  // если sqlReader не 0, то
                    sqlReader.Close();
            }

            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)     // Чтобы не потерять данные при выходе. Проверяем открыто ли соединение и закрываем его
                sqlConnection.Close();

        }

        private void button1_Click(object sender, EventArgs e)  // Сохранить async
        {
            // Cумма textBox1-12, 21. Должно быть 100%.
            float summa = (float)Convert.ToDouble(textBox1.Text) + (float)Convert.ToDouble(textBox2.Text) + (float)Convert.ToDouble(textBox3.Text) + (float)Convert.ToDouble(textBox4.Text) + (float)Convert.ToDouble(textBox5.Text) + (float)Convert.ToDouble(textBox6.Text) + (float)Convert.ToDouble(textBox7.Text) + (float)Convert.ToDouble(textBox8.Text) + (float)Convert.ToDouble(textBox9.Text) + (float)Convert.ToDouble(textBox10.Text) + (float)Convert.ToDouble(textBox11.Text) + (float)Convert.ToDouble(textBox12.Text) + (float)Convert.ToDouble(textBox21.Text);
            //textBox26.Text = Convert.ToString(summa);           
            if (summa != 100)
            {
                MessageBox.Show("Рецепт введен неверно. Сумма всех компонентов не равна 100%. Выполните корректировку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите экстендер.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите по какому слою краска (По белому/По бурому).", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox13.Text == "")
            {
                MessageBox.Show("Введите название пантона.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {   // Формирование пантона (Пантон/ЛАК/По белому)
                textBox13.Text = textBox13.Text + "/" + Convert.ToString(comboBox1.SelectedItem) + "/" + Convert.ToString(comboBox2.SelectedItem);

                // Проверяем есть ли пантон c таким именем в БД
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                {
                    sqlConnection.Open();           // Открываем БД
                    SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                    SqlCommand command = new SqlCommand("SELECT * FROM [Colors] WHERE [Color]=@Color", sqlConnection);    // Проверим есть ли в БД такой пантон
                    command.Parameters.AddWithValue("Color", textBox13.Text);
                    sqlReader = command.ExecuteReader(); // Читаем данные 
                    if (sqlReader.Read()) // Если есть пантон с таким именем, то выход из  процедуры.
                    {
                        MessageBox.Show("Пантон с таким именем уже существует в БД!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        sqlReader.Close();
                        return;
                    }
                }

                // Записываем рецепт в БД
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                {
                    sqlConnection.Open();    // Открываем БД
                    SqlCommand command = new SqlCommand("INSERT INTO [Colors] (Color, ExtenderProc, YellowProc, RedProc, RubinProc, RadominProc, OrangeProc, PinkProc, VioletProc, BlueProc, GreenProc, BlackProc, WhiteProc, Water, Viscosity, Extender, WhiteBlack) " +
                                                    "VALUES (@Color, @ExtenderProc, @YellowProc, @RedProc, @RubinProc, @RadominProc, @OrangeProc, @PinkProc, @VioletProc, @BlueProc, @GreenProc, @BlackProc, @WhiteProc, @Water, @Viscosity, @Extender, @WhiteBlack)", sqlConnection);
                    command.Parameters.AddWithValue("Color", textBox13.Text);   // Добавить со значением Color из textBox13.text
                    command.Parameters.AddWithValue("ExtenderProc", (float)Convert.ToDouble(textBox21.Text));
                    command.Parameters.AddWithValue("YellowProc", (float)Convert.ToDouble(textBox1.Text));
                    command.Parameters.AddWithValue("RedProc", (float)Convert.ToDouble(textBox2.Text));
                    command.Parameters.AddWithValue("RubinProc", (float)Convert.ToDouble(textBox3.Text));
                    command.Parameters.AddWithValue("RadominProc", (float)Convert.ToDouble(textBox4.Text));
                    command.Parameters.AddWithValue("OrangeProc", (float)Convert.ToDouble(textBox5.Text));
                    command.Parameters.AddWithValue("PinkProc", (float)Convert.ToDouble(textBox6.Text));
                    command.Parameters.AddWithValue("VioletProc", (float)Convert.ToDouble(textBox7.Text));
                    command.Parameters.AddWithValue("BlueProc", (float)Convert.ToDouble(textBox8.Text));
                    command.Parameters.AddWithValue("GreenProc", (float)Convert.ToDouble(textBox9.Text));
                    command.Parameters.AddWithValue("BlackProc", (float)Convert.ToDouble(textBox10.Text));
                    command.Parameters.AddWithValue("WhiteProc", (float)Convert.ToDouble(textBox11.Text));
                    command.Parameters.AddWithValue("Water", (float)Convert.ToDouble(textBox12.Text));
                    command.Parameters.AddWithValue("Viscosity", (float)Convert.ToDouble(textBox26.Text));
                    command.Parameters.AddWithValue("Extender", comboBox1.SelectedItem);
                    command.Parameters.AddWithValue("WhiteBlack", comboBox2.SelectedItem);

                    //ДОБАВИТЬ ПРОВЕРКУ, ЧТО РЕЦЕПТ С ТАКИМ ИМЕНЕМ СУЩЕСТВУЕТ!!!

                    command.ExecuteNonQuery();

                }

                /*
                     
                //SqlConnection sqlConnection; обьявлено в начале
                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение
                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме

                SqlCommand command = new SqlCommand("INSERT INTO [Colors] (Color, ExtenderProc, YellowProc, RedProc, RubinProc, RadominProc, OrangeProc, PinkProc, VioletProc, BlueProc, GreenProc, BlackProc, WhiteProc, Water, Viscosity, Extender, WhiteBlack) " +
                                                    "VALUES (@Color, @ExtenderProc, @YellowProc, @RedProc, @RubinProc, @RadominProc, @OrangeProc, @PinkProc, @VioletProc, @BlueProc, @GreenProc, @BlackProc, @WhiteProc, @Water, @Viscosity, @Extender, @WhiteBlack)", sqlConnection);
                command.Parameters.AddWithValue("Color", textBox13.Text);   // Добавить со значением Color из textBox13.text
                command.Parameters.AddWithValue("ExtenderProc", (float)Convert.ToDouble(textBox21.Text));
                command.Parameters.AddWithValue("YellowProc", (float)Convert.ToDouble(textBox1.Text));
                command.Parameters.AddWithValue("RedProc", (float)Convert.ToDouble(textBox2.Text));
                command.Parameters.AddWithValue("RubinProc", (float)Convert.ToDouble(textBox3.Text));
                command.Parameters.AddWithValue("RadominProc", (float)Convert.ToDouble(textBox4.Text));
                command.Parameters.AddWithValue("OrangeProc", (float)Convert.ToDouble(textBox5.Text));
                command.Parameters.AddWithValue("PinkProc", (float)Convert.ToDouble(textBox6.Text));
                command.Parameters.AddWithValue("VioletProc", (float)Convert.ToDouble(textBox7.Text));
                command.Parameters.AddWithValue("BlueProc", (float)Convert.ToDouble(textBox8.Text));
                command.Parameters.AddWithValue("GreenProc", (float)Convert.ToDouble(textBox9.Text));
                command.Parameters.AddWithValue("BlackProc", (float)Convert.ToDouble(textBox10.Text));
                command.Parameters.AddWithValue("WhiteProc", (float)Convert.ToDouble(textBox11.Text));
                command.Parameters.AddWithValue("Water", (float)Convert.ToDouble(textBox12.Text));
                command.Parameters.AddWithValue("Viscosity", (float)Convert.ToDouble(textBox26.Text));
                command.Parameters.AddWithValue("Extender", comboBox1.SelectedItem);
                command.Parameters.AddWithValue("WhiteBlack", comboBox2.SelectedItem);

                //ДОБАВИТЬ ПРОВЕРКУ, ЧТО РЕЦЕПТ С ТАКИМ ИМЕНЕМ СУЩЕСТВУЕТ!!!

                await command.ExecuteNonQueryAsync();

                if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)     // Чтобы не потерять данные при выходе. Проверяем открыто ли соединение и закрываем его
                    sqlConnection.Close();
                    */
            }
        }

        // НАЧАЛО. ПРОВЕРКА 14 ПОЛЕЙ НА ТО, ЧТО ВВЕДЕНЫ ТОЛЬКО ЦИФРЫ С ПЛАВАЮЩЕЙ ЗАПЯТОЙ
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox7.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox1.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox2.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox3.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox4.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox5.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox6.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox8.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox9.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox10.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox11.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox12.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox21.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox26.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtGRWeight.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }
        // КОНЕЦ. ПРОВЕРКА 14 ПОЛЕЙ НА ТО, ЧТО ВВЕДЕНЫ ТОЛЬКО ЦИФРЫ С ПЛАВАЮЩЕЙ ЗАПЯТОЙ + 28текст - вес, кг



        private void btnSearch_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            lbl_listbox1.Visible = true;
            lbl_listbox1.Text = "Выберите пантон:";

            tf_listbox = true;


            if (txtGRPanton.Text == "")
            {
                MessageBox.Show("Заполните поле 'Пантон'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение
                {
                    sqlConnection.Open();    // Открываем БД в асинхронном режиме
                    SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                    SqlCommand command = new SqlCommand("SELECT [Color] FROM [Colors] WHERE [Color] LIKE '%' + @a + '%'", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос
                    command.Parameters.AddWithValue("a", txtGRPanton.Text);

                    try
                    {
                        sqlReader = command.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            listBox1.Items.Add(Convert.ToString(sqlReader["Color"]));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    finally
                    {
                        if (sqlReader != null)  // если sqlReader не 0, то
                            sqlReader.Close();
                    }

                    if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)     // Чтобы не потерять данные при выходе. Проверяем открыто ли соединение и закрываем его
                        sqlConnection.Close();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tf_listbox)  //   Если f_listbox = false, то строку выполнения пропускаем  
                txtGRPanton.Text = listBox1.SelectedItem.ToString();
        }

        private void btnCalculate_Click(object sender, EventArgs e)   // async
        {
            tf_listbox = false;
            lbl_listbox1.Text = "Рецепт:";
            listBox1.Items.Clear(); // Очистка ListBox перед выводом новых данных
            lblErrT0.Text = "Внимание! ";   // Подготовка информационного lblErrT0
            lblErrT0.Visible = false;
            if (txtGRPanton.Text == "") // Проверка полей txtGRPanton + txtGRWeight (вес)
            {
                lblErrT0.Text += "Заполните поле 'Пантон'. ";
                lblErrT0.Visible = true;
                if (txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
                {
                    lblErrT0.Text += "Заполните поле 'Вес, кг'. ";
                }
            }
            // Проверка только txtGRWeight (вес)
            else if (txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
            {
                lblErrT0.Text += "Заполните поле 'Вес, кг'. ";
                lblErrT0.Visible = true;
            }

            else
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                {
                    sqlConnection.Open();    // Открываем БД
                    SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                    SqlCommand command = new SqlCommand("SELECT * FROM [Colors] WHERE [Color]=@Color", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос                
                    command.Parameters.AddWithValue("Color", txtGRPanton.Text);

                    // Проверка записи в БД
                    sqlReader = command.ExecuteReader(); // Читаем данные 
                    if (!sqlReader.Read()) // Если нет записей то true
                    {
                        lblErrT0.Text += "Введеный пантон отсутствует в БД.";
                        lblErrT0.Visible = true;
                    }
                    else
                    {
                        try
                        {
                            sqlReader.Close();  
                            sqlReader = command.ExecuteReader(); // Читаем данные ПОВТОРНО!
                            while (sqlReader.Read())
                            {
                                //float f_test = sqlReader.GetFloat(4);   // ExtenderProc - 4 колонка. Эксперимент с получение дробной части.
                                //f_test = f_test * Convert.ToSingle(textBox28.Text);


                                listBox1.Items.Add("Рецепт изготовления " + txtGRPanton.Text + " на " + txtGRWeight.Text + " кг.");
                                listBox1.Items.Add("");
                                listBox1.Items.Add("Рецепт - " + Convert.ToString(sqlReader.GetValue(2)));      // По белому/ По бурому
                                listBox1.Items.Add("");
                                listBox1.Items.Add("Экстендер(лак) - " + Convert.ToString(sqlReader.GetValue(3)));      // Экстендер
                                listBox1.Items.Add("");
                                listBox1.Items.Add("Состав:");
                                listBox1.Items.Add("");
                                if (sqlReader.GetFloat(4) != 0)
                                    listBox1.Items.Add("Экстендер - " + Convert.ToString(Math.Round(sqlReader.GetFloat(4) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(5) != 0)
                                    listBox1.Items.Add("Желтая - " + Convert.ToString(Math.Round(sqlReader.GetFloat(5) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(6) != 0)
                                    listBox1.Items.Add("Красная - " + Convert.ToString(Math.Round(sqlReader.GetFloat(6) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(7) != 0)
                                    listBox1.Items.Add("Рубин - " + Convert.ToString(Math.Round(sqlReader.GetFloat(7) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(8) != 0)
                                    listBox1.Items.Add("Радомин - " + Convert.ToString(Math.Round(sqlReader.GetFloat(8) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(9) != 0)
                                    listBox1.Items.Add("Оранжевая - " + Convert.ToString(Math.Round(sqlReader.GetFloat(9) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(10) != 0)
                                    listBox1.Items.Add("Пинк - " + Convert.ToString(Math.Round(sqlReader.GetFloat(10) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(11) != 0)
                                    listBox1.Items.Add("Фиолетовая - " + Convert.ToString(Math.Round(sqlReader.GetFloat(11) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(12) != 0)
                                    listBox1.Items.Add("Синяя - " + Convert.ToString(Math.Round(sqlReader.GetFloat(12) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(13) != 0)
                                    listBox1.Items.Add("Зеленая - " + Convert.ToString(Math.Round(sqlReader.GetFloat(13) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(14) != 0)
                                    listBox1.Items.Add("Черная - " + Convert.ToString(Math.Round(sqlReader.GetFloat(14) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(15) != 0)
                                    listBox1.Items.Add("Белая - " + Convert.ToString(Math.Round(sqlReader.GetFloat(15) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                if (sqlReader.GetFloat(16) != 0)
                                    listBox1.Items.Add("Вода - " + Convert.ToString(Math.Round(sqlReader.GetFloat(16) / 100 * Convert.ToSingle(txtGRWeight.Text), 3)) + " кг");
                                listBox1.Items.Add("");
                                listBox1.Items.Add("Заявленная вязкость - " + Convert.ToString(sqlReader.GetFloat(17)) + "с");

                                /*
                                //  Проверка 100%
                                float summa;
                                summa = sqlReader.GetFloat(4) + sqlReader.GetFloat(5) + sqlReader.GetFloat(6) + sqlReader.GetFloat(7) + sqlReader.GetFloat(8) + sqlReader.GetFloat(9) + sqlReader.GetFloat(10) + sqlReader.GetFloat(11) + sqlReader.GetFloat(12) + sqlReader.GetFloat(13) + sqlReader.GetFloat(14) + sqlReader.GetFloat(15) + sqlReader.GetFloat(16);

                                if (summa!=100)
                                { 
                                listBox1.Items.Add("Рецепт изготовления " + Convert.ToString(sqlReader.GetValue(1)) + " на " + textBox28.Text + " кг.");
                                listBox1.Items.Add("Сумма компонентов равна - " + Convert.ToString(summa));
                                listBox1.Items.Add("");
                                }
                                */

                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            if (sqlReader != null)  // если sqlReader не 0, то
                                sqlReader.Close();
                        }

                        if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)     // Чтобы не потерять данные при выходе. Проверяем открыто ли соединение и закрываем его
                            sqlConnection.Close();

                        sqlReader.Close();


                        // Обработка сохранения израсходованных материалов на изготовление рецепта

                        DialogResult result = MessageBox.Show("Сохранить изготовленный рецепт в БД статистики?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (result == DialogResult.Yes)
                        {
                            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                            //{
                                    sqlConnection.Open();    // Открываем БД был открыт               
                            // SqlCommand command = new SqlCommand("INSERT INTO Statictics VALUES " +
                                    // round(value, 3) - функция, округляет до 3 знаков после запятой. По математическим правилам.
                                    command = new SqlCommand("INSERT INTO Statictics VALUES " +
                                    " (GETDATE()," +
                                    " (SELECT Color FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT WhiteBlack FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT Extender FROM Colors WHERE [Color] = @Color)," +
                                    " @txtGRWeight," +
                                    " (SELECT round(ExtenderProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(YellowProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(RedProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(RubinProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(RadominProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(OrangeProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(PinkProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(VioletProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(BlueProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(GreenProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(BlackProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(WhiteProc*@Weight, 3) FROM Colors WHERE [Color] = @Color)," +
                                    " (SELECT round(Water*@Weight, 3) FROM Colors WHERE [Color] = @Color))", sqlConnection);

                                command.Parameters.AddWithValue("Color", txtGRPanton.Text);
                                command.Parameters.AddWithValue("txtGRWeight", Convert.ToDouble(txtGRWeight.Text)); // ToDouble - дает номальное преобразование
                                command.Parameters.AddWithValue("Weight", Convert.ToDouble(txtGRWeight.Text) / 100);

  
                                command.ExecuteNonQuery();
                            //}
                        }
                    }                    
                }               
            }           
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))    // Устанавливаем соединение
                {
                sqlConnection.Open();    // Открываем БД
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в табличном виде
                SqlCommand command = new SqlCommand("SELECT * FROM [Colors]", sqlConnection);       // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос                                                                                                 
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();     // Создаем коллекцию

                try
                {
                    sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        // Заполняем MyCollection
                        MyCollection.Add(Convert.ToString(sqlReader["Color"]));
                    }
                    txtGRPanton.AutoCompleteCustomSource = MyCollection;
                    txtSRPanton.AutoCompleteCustomSource = MyCollection;    // Для выбора рецепта в Статистике расходов

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    if (sqlReader != null)  // если sqlReader не 0, то
                        sqlReader.Close();
                }

                if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)     // Чтобы не потерять данные при выходе. Проверяем открыто ли соединение и закрываем его
                    sqlConnection.Close();
                }                  
                                    
            
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            string strText = "";
            foreach (object x in listBox1.Items)
            {
                strText = strText + x.ToString() + "\n";
            }

            myReader = new StringReader(strText);
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            lblErrT0.Text = "Внимание! ";   // Подготовка информационного lblErrT0
            lblErrT0.Visible = false;

            if (txtGRPanton.Text == "") // Проверка полей txtGRPanton + txtGRWeight (вес)
            {
                lblErrT0.Text += "Заполните поле 'Пантон'. ";
                lblErrT0.Visible = true;
                if (txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
                {
                    lblErrT0.Text += "Заполните поле 'Вес, кг'. ";
                }
            }
            // Проверка только txtGRWeight (вес)
            else if (txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
            {
                lblErrT0.Text += "Заполните поле 'Вес, кг'. ";
                lblErrT0.Visible = true;
            }

            else
            {
                // Проверяем есть ли пантон в БД
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                {
                    sqlConnection.Open();           // Открываем БД
                    SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                    SqlCommand command = new SqlCommand("SELECT * FROM [Colors] WHERE [Color]=@Color", sqlConnection);    // Проверим есть ли в БД такой пантон
                    command.Parameters.AddWithValue("Color", txtGRPanton.Text);
                    sqlReader = command.ExecuteReader(); // Читаем данные 
                    if (!sqlReader.Read()) // Если нет записей то true
                    {
                        lblErrT0.Text += "Введеный пантон отсутствует в БД.";
                        lblErrT0.Visible = true;
                    }
                    else
                    {
                        tabPage3.Parent = tabControl1;  // Показывает вкладка "Корректировать рецепт"
                        tabControl1.SelectedIndex = 3;  // Переносим на вкладку

                        // Заполняем текстовые поля данными из полученного рецепта
                        txtKRPanton.Text = txtGRPanton.Text;
                        txtKRPanton.Enabled = false;
                        txtKRWeight.Text = txtGRWeight.Text;
                        txtKRWeight.Enabled = false;
                        //object id = sqlReader.GetValue(0);
                        //object Extender = sqlReader["Extender"];
                        txtKRExtenderType.Text = Convert.ToString(sqlReader["Extender"]);
                        txtKRExtenderType.Enabled = false;
                        txtKRWhiteBlack.Text = Convert.ToString(sqlReader["WhiteBlack"]);
                        txtKRWhiteBlack.Enabled = false;
                        txtKRPantonNew.Enabled = false;     // Запрет ввода имени полученного пантона (Пантон/Р-вязкость)


                        // Выводим с пересчетом на вес замешанного пантона
                        txtKRExtenderKg.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["ExtenderProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRYellow.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["YellowProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRRed.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["RedProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRRubin.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["RubinProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRRadomin.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["RadominProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKROrange.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["OrangeProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRPink.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["PinkProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRViolet.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["VioletProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRBlue.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["BlueProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRGreen.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["GreenProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRBlack.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["BlackProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRWhite.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["WhiteProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRWater.Text = Convert.ToString(Math.Round(Convert.ToSingle(sqlReader["Water"]) / 100 * Convert.ToSingle(txtKRWeight.Text), 3));
                        txtKRViscosity.Text = Convert.ToString(sqlReader["Viscosity"]);

                        var_txtKRExtenderKg = txtKRExtenderKg.Text;
                        var_txtKRYellow = txtKRYellow.Text;
                        var_txtKRRed = txtKRRed.Text;
                        var_txtKRRubin = txtKRRubin.Text;
                        var_txtKRRadomin = txtKRRadomin.Text;
                        var_txtKROrange = txtKROrange.Text;
                        var_txtKRPink = txtKRPink.Text;
                        var_txtKRViolet = txtKRViolet.Text;
                        var_txtKRBlue = txtKRBlue.Text;
                        var_txtKRGreen = txtKRGreen.Text;
                        var_txtKRBlack = txtKRBlack.Text;
                        var_txtKRWhite = txtKRWhite.Text;
                        var_txtKRWater = txtKRWater.Text;
                        var_txtKRViscosity = txtKRViscosity.Text;


                    }
                    sqlReader.Close();
                }

                txtKRPantonNew.Text = txtKRPanton.Text + "/Р-" + txtKRViscosity.Text;





            }
        }

        private void txtKRViscosity_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRViscosity.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRExtenderKg_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRExtenderKg.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRYellow_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRYellow.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRRed_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRRed.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRRubin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRRubin.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRRadomin_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRRadomin.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKROrange_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKROrange.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRPink_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRPink.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRViolet_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRViolet.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRBlue_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRBlue.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRGreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRGreen.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRBlack_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRBlack.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRWhite_KeyPress(object sender, KeyPressEventArgs e)
        {
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRWhite.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }

        private void txtKRWater_KeyPress(object sender, KeyPressEventArgs e)
        {

            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, txtKRWater.Text);

            e.KeyChar = proverka.Conv();
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
            return;
        }


        //  Вкладка "Корректировка рецепта". В случае пустого поля, заменяем на 0 по потери фокуса. 
        //  Для того чтобы корректно вводились данные в БД.
        //  Проверка, изменено ли поле. Изменено - красное. Не изменено - белое.
        //  14 текстовых полей. 
        private void txtKRViscosity_Leave(object sender, EventArgs e)
        {
            if (txtKRViscosity.Text == String.Empty)
                txtKRViscosity.Text = "0";
            if (var_txtKRViscosity != txtKRViscosity.Text)
                txtKRViscosity.BackColor = Color.Red;
            else txtKRViscosity.BackColor = Color.White;
            txtKRPantonNew.Text = txtKRPanton.Text + "/Р-" + txtKRViscosity.Text;

        }

        private void txtKRExtenderKg_Leave(object sender, EventArgs e)
        {
            if (txtKRExtenderKg.Text == String.Empty)
                txtKRExtenderKg.Text = "0";
            if (var_txtKRExtenderKg != txtKRExtenderKg.Text)
                txtKRExtenderKg.BackColor = Color.Red;
            else txtKRExtenderKg.BackColor = Color.White;
        }

        private void txtKRYellow_Leave(object sender, EventArgs e)
        {
            if (txtKRYellow.Text == String.Empty)
                txtKRYellow.Text = "0";
            if (var_txtKRYellow != txtKRYellow.Text)
                txtKRYellow.BackColor = Color.Red;
            else txtKRYellow.BackColor = Color.White;

        }

        private void txtKRRed_Leave(object sender, EventArgs e)
        {
            if (txtKRRed.Text == String.Empty)
                txtKRRed.Text = "0";
            if (var_txtKRRed != txtKRRed.Text)
                txtKRRed.BackColor = Color.Red;
            else txtKRRed.BackColor = Color.White;

        }

        private void txtKRRubin_Leave(object sender, EventArgs e)
        {
            if (txtKRRubin.Text == String.Empty)
                txtKRRubin.Text = "0";
            if (var_txtKRRubin != txtKRRubin.Text)
                txtKRRubin.BackColor = Color.Red;
            else txtKRRubin.BackColor = Color.White;

        }

        private void txtKRRadomin_Leave(object sender, EventArgs e)
        {
            if (txtKRRadomin.Text == String.Empty)
                txtKRRadomin.Text = "0";
            if (var_txtKRRadomin != txtKRRadomin.Text)
                txtKRRadomin.BackColor = Color.Red;
            else txtKRRadomin.BackColor = Color.White;

        }

        private void txtKROrange_Leave(object sender, EventArgs e)
        {
            if (txtKROrange.Text == String.Empty)
                txtKROrange.Text = "0";
            if (var_txtKROrange != txtKROrange.Text)
                txtKROrange.BackColor = Color.Red;
            else txtKROrange.BackColor = Color.White;

        }

        private void txtKRPink_Leave(object sender, EventArgs e)
        {
            if (txtKRPink.Text == String.Empty)
                txtKRPink.Text = "0";
            if (var_txtKRPink != txtKRPink.Text)
                txtKRPink.BackColor = Color.Red;
            else txtKRPink.BackColor = Color.White;

        }

        private void txtKRViolet_Leave(object sender, EventArgs e)
        {
            if (txtKRViolet.Text == String.Empty)
                txtKRViolet.Text = "0";
            if (var_txtKRViolet != txtKRViolet.Text)
                txtKRViolet.BackColor = Color.Red;
            else txtKRViolet.BackColor = Color.White;

        }

        private void txtKRBlue_Leave(object sender, EventArgs e)
        {
            if (txtKRBlue.Text == String.Empty)
                txtKRBlue.Text = "0";
            if (var_txtKRBlue != txtKRBlue.Text)
                txtKRBlue.BackColor = Color.Red;
            else txtKRBlue.BackColor = Color.White;

        }

        private void txtKRGreen_Leave(object sender, EventArgs e)
        {
            if (txtKRGreen.Text == String.Empty)
                txtKRGreen.Text = "0";
            if (var_txtKRGreen != txtKRGreen.Text)
                txtKRGreen.BackColor = Color.Red;
            else txtKRGreen.BackColor = Color.White;

        }

        private void txtKRBlack_Leave(object sender, EventArgs e)
        {
            if (txtKRBlack.Text == String.Empty)
                txtKRBlack.Text = "0";
            if (var_txtKRBlack != txtKRBlack.Text)
                txtKRBlack.BackColor = Color.Red;
            else txtKRBlack.BackColor = Color.White;

        }

        private void txtKRWhite_Leave(object sender, EventArgs e)
        {
            if (txtKRWhite.Text == String.Empty)
                txtKRWhite.Text = "0";
            if (var_txtKRWhite != txtKRWhite.Text)
                txtKRWhite.BackColor = Color.Red;
            else txtKRWhite.BackColor = Color.White;

        }

        private void txtKRWater_Leave(object sender, EventArgs e)
        {
            if (txtKRWater.Text == String.Empty)
                txtKRWater.Text = "0";
            if (var_txtKRWater != txtKRWater.Text)
                txtKRWater.BackColor = Color.Red;
            else txtKRWater.BackColor = Color.White;

        }

        //  Вкладка "Новый рецепт". В случае пустого поля, заменяем на 0 по потери фокуса. Для того чтобы корректно вводились данные в БД.
        //  14 текстовых полей.
        private void textBox26_Leave(object sender, EventArgs e)
        {
            if (textBox26.Text == String.Empty)
                textBox26.Text = "0";
        }

        private void textBox21_Leave(object sender, EventArgs e)
        {
            if (textBox21.Text == String.Empty)
                textBox21.Text = "0";
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty)
                textBox1.Text = "0";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == String.Empty)
                textBox2.Text = "0";
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == String.Empty)
                textBox3.Text = "0";
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == String.Empty)
                textBox4.Text = "0";
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == String.Empty)
                textBox5.Text = "0";
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text == String.Empty)
                textBox6.Text = "0";
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text == String.Empty)
                textBox7.Text = "0";
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            if (textBox8.Text == String.Empty)
                textBox8.Text = "0";
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            if (textBox9.Text == String.Empty)
                textBox9.Text = "0";
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            if (textBox10.Text == String.Empty)
                textBox10.Text = "0";
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            if (textBox11.Text == String.Empty)
                textBox11.Text = "0";
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            if (textBox12.Text == String.Empty)
                textBox12.Text = "0";
        }

        private void btnKRSave_Click(object sender, EventArgs e)
        {
            // Переменные для пересчета
            double ft_txtKRExtenderKg = 0;
            double ft_txtKRYellow = 0;
            double ft_txtKRRed = 0;
            double ft_txtKRRubin = 0;
            double ft_txtKRRadomin = 0;
            double ft_txtKROrange = 0;
            double ft_txtKRPink = 0;
            double ft_txtKRViolet = 0;
            double ft_txtKRBlue = 0;
            double ft_txtKRGreen = 0;
            double ft_txtKRBlack = 0;
            double ft_txtKRWhite = 0;
            double ft_txtKRWater = 0;

            if (Convert.ToSingle(txtKRViscosity.Text) <= 0)
            {
                MessageBox.Show("Введите полученную вязкость!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double summaKR = Convert.ToDouble(txtKRExtenderKg.Text) + Convert.ToDouble(txtKRYellow.Text)
                            + Convert.ToDouble(txtKRRed.Text) + Convert.ToDouble(txtKRRubin.Text)
                            + Convert.ToDouble(txtKRRadomin.Text) + Convert.ToDouble(txtKROrange.Text)
                            + Convert.ToDouble(txtKRPink.Text) + Convert.ToDouble(txtKRViolet.Text)
                            + Convert.ToDouble(txtKRBlue.Text) + Convert.ToDouble(txtKRGreen.Text)
                            + Convert.ToDouble(txtKRBlack.Text) + Convert.ToDouble(txtKRWhite.Text)
                            + Convert.ToDouble(txtKRWater.Text);

            // На 0 делить нельзя. 0 присвоен при объявлении. Округляем до 3х знаком после запятой.
            if (Convert.ToDouble(txtKRExtenderKg.Text) > 0)
                ft_txtKRExtenderKg = Math.Round(Convert.ToDouble(txtKRExtenderKg.Text) / summaKR * 100, 3);                    
            if (Convert.ToDouble(txtKRYellow.Text) > 0)
                ft_txtKRYellow = Math.Round(Convert.ToDouble(txtKRYellow.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRRed.Text) > 0)
                ft_txtKRRed = Math.Round(Convert.ToDouble(txtKRRed.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRRubin.Text) > 0)
                ft_txtKRRubin = Math.Round(Convert.ToDouble(txtKRRubin.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRRadomin.Text) > 0)
                ft_txtKRRadomin = Math.Round(Convert.ToDouble(txtKRRadomin.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKROrange.Text) > 0)
                ft_txtKROrange = Math.Round(Convert.ToDouble(txtKROrange.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRPink.Text) > 0)
                ft_txtKRPink = Math.Round(Convert.ToDouble(txtKRPink.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRViolet.Text) > 0)
                ft_txtKRViolet = Math.Round(Convert.ToDouble(txtKRViolet.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRBlue.Text) > 0)
                ft_txtKRBlue = Math.Round(Convert.ToDouble(txtKRBlue.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRGreen.Text) > 0)
                ft_txtKRGreen = Math.Round(Convert.ToDouble(txtKRGreen.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRBlack.Text) > 0)
                ft_txtKRBlack = Math.Round(Convert.ToDouble(txtKRBlack.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRWhite.Text) > 0)
                ft_txtKRWhite = Math.Round(Convert.ToDouble(txtKRWhite.Text) / summaKR * 100, 3);
            if (Convert.ToDouble(txtKRWater.Text) > 0)
                ft_txtKRWater = Math.Round(Convert.ToDouble(txtKRWater.Text) / summaKR * 100, 3);

            // Запись пересчитанного рецепта в БД.
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
            {
                sqlConnection.Open();    // Открываем БД
                SqlCommand command = new SqlCommand("INSERT INTO [Colors] (Color, ExtenderProc, YellowProc, RedProc, RubinProc," +
                    " RadominProc, OrangeProc, PinkProc, VioletProc, BlueProc, GreenProc, BlackProc, WhiteProc, Water," +
                    " Viscosity, Extender, WhiteBlack) " +
                    "VALUES (@Color, @ExtenderProc, @YellowProc, @RedProc, @RubinProc, @RadominProc, @OrangeProc, @PinkProc, " +
                    "@VioletProc, @BlueProc, @GreenProc, @BlackProc, @WhiteProc, @Water, @Viscosity, @Extender, @WhiteBlack)", sqlConnection);
                command.Parameters.AddWithValue("Color", txtKRPantonNew.Text);
                command.Parameters.AddWithValue("ExtenderProc", ft_txtKRExtenderKg);
                command.Parameters.AddWithValue("YellowProc", ft_txtKRYellow);
                command.Parameters.AddWithValue("RedProc", ft_txtKRRed);
                command.Parameters.AddWithValue("RubinProc", ft_txtKRRubin);
                command.Parameters.AddWithValue("RadominProc", ft_txtKRRadomin);
                command.Parameters.AddWithValue("OrangeProc", ft_txtKROrange);
                command.Parameters.AddWithValue("PinkProc", ft_txtKRPink);
                command.Parameters.AddWithValue("VioletProc", ft_txtKRViolet);
                command.Parameters.AddWithValue("BlueProc", ft_txtKRBlue);
                command.Parameters.AddWithValue("GreenProc", ft_txtKRGreen);
                command.Parameters.AddWithValue("BlackProc", ft_txtKRBlack);
                command.Parameters.AddWithValue("WhiteProc", ft_txtKRWhite);
                command.Parameters.AddWithValue("Water", ft_txtKRWater);
                command.Parameters.AddWithValue("Viscosity", Convert.ToDouble(txtKRViscosity.Text));
                command.Parameters.AddWithValue("Extender", txtKRExtenderType.Text);
                command.Parameters.AddWithValue("WhiteBlack", txtKRWhiteBlack.Text);

                command.ExecuteNonQuery();
            }

            MessageBox.Show("Рецепт корректированного пантона внесен в БД. Имя - " + txtKRPantonNew.Text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabControl1.SelectedIndex = 0;      // Переносим на вкладку "Готовить рецепт"
            tabPage3.Parent = null;             // Скрываем элемент "Корректировка рецепта"


        }

        private void btnSRView_Click(object sender, EventArgs e)
        {

            // Настраиваем DataGridView1            
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.DataSource = null;    // Очищаем dataGridView1
           
            if (ckSRPanton.Checked && ckSRPeriod.Checked)    // Запрос с фильтром по дате и по имени
            {                                             
                if (txtSRPanton.Text == "")
                {
                    MessageBox.Show("Заполните поле 'Пантон'.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Проверяем есть ли пантон в БД
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                    {
                        sqlConnection.Open();           // Открываем БД
                        SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                        SqlCommand command = new SqlCommand("SELECT * FROM Statictics WHERE [Пантон]=@Color", sqlConnection);    // Проверим есть ли в БД такой пантон
                        command.Parameters.AddWithValue("Color", txtSRPanton.Text);
                        sqlReader = command.ExecuteReader(); // Читаем данные 
                        if (!sqlReader.Read()) // Если нет записей то true
                        {
                            MessageBox.Show("Введеный пантон отсутствует в БД.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            sqlReader.Close();  // Закрываем т.к. конфликт с adapter
                            adapter = new SqlDataAdapter("SELECT * FROM Statictics WHERE [Пантон]=@Color AND [Дата] BETWEEN @DateIn AND @DateOUT", sqlConnection);  // Заполнение DataGridView1
                            adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                            adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                            adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                            dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            dataGridView1.DataSource = dataSet.Tables[0];

                            // Продолжение запроса
                            sql_query = " WHERE[Пантон] = @Color AND[Дата] BETWEEN @DateIn AND @DateOUT";
                            /*
                            dataGridView1.Columns.Remove("id");     // Удаляем столбец ID
                            dataGridView1.Columns[0].Width = 75;    // Ширина даты
                            dataGridView1.Columns[1].Width = 100;   // Ширина Имя пантона + после форматирования, пропоционально увелициваются.

                            // Запретили сортировку по столбцам типа real. 4-17 во избежания ошибок.
                            for (int i = 4; i < dataGridView1.ColumnCount; i++)
                            {
                                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            }*/
                        }

                    }
                }                                                                                 
            }
            else if (ckSRPeriod.Checked)    // Запрос с фильтром по дате
            {

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                {
                    sqlConnection.Open();           // Открываем БД
                    adapter = new SqlDataAdapter("SELECT * FROM Statictics WHERE [Дата] BETWEEN @DateIn AND @DateOUT", sqlConnection);  // Заполнение DataGridView1
                    adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                    adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);

                    dataSet = new DataSet();
                    adapter.Fill(dataSet);                    
                    dataGridView1.DataSource = dataSet.Tables[0];

                    // Продолжение запроса
                    sql_query = " WHERE [Дата] BETWEEN @DateIn AND @DateOUT";

                    /*
                    dataGridView1.Columns.Remove("id");     // Удаляем столбец ID
                    dataGridView1.Columns[0].Width = 75;    // Ширина даты
                    dataGridView1.Columns[1].Width = 100;   // Ширина Имя пантона + после форматирования, пропоционально увелициваются.

                        // Запретили сортировку по столбцам типа real. 4-17 во избежания ошибок.
                        for (int i = 4; i < dataGridView1.ColumnCount; i++)
                        {
                            dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }*/

                    // Добавляем строку с суммой в столбце
                    //adapter = new SqlDataAdapter("SELECT * FROM Statictics WHERE [Дата] BETWEEN @DateIn AND @DateOUT", sqlConnection);  // Заполнение DataGridView1



                }
            }
            else if (ckSRPanton.Checked)    // Запрос с фильтром по пантону
            {
                if (txtSRPanton.Text == "")
                {
                    MessageBox.Show("Заполните поле 'Пантон'.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Проверяем есть ли пантон в БД
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))   // Устанавливаем соединение время жизни в пределах using
                    {
                        sqlConnection.Open();           // Открываем БД
                        SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                        SqlCommand command = new SqlCommand("SELECT * FROM Statictics WHERE [Пантон]=@Color", sqlConnection);    // Проверим есть ли в БД такой пантон
                        command.Parameters.AddWithValue("Color", txtSRPanton.Text);
                        sqlReader = command.ExecuteReader(); // Читаем данные 
                        if (!sqlReader.Read()) // Если нет записей то true
                        {
                            MessageBox.Show("Введеный пантон отсутствует в БД.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            sqlReader.Close();  // Закрываем т.к. конфликт с adapter
                            adapter = new SqlDataAdapter("SELECT * FROM Statictics WHERE [Пантон]=@Color", sqlConnection);  // Заполнение DataGridView1
                            adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                            dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            dataGridView1.DataSource = dataSet.Tables[0];
                            
                            // Продолжение запроса
                            sql_query = " WHERE [Пантон]=@Color";
                        }                        
                    }
                }
            }
            else
            {
                // Сообщение о том, что необходимо выбрать отбор.
                MessageBox.Show("Выберите параметры отбора.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Columns.Remove("id");     // Удаляем столбец ID
                dataGridView1.Columns[0].Width = 75;    // Ширина даты
                dataGridView1.Columns[1].Width = 100;   // Ширина Имя пантона + после форматирования, пропоционально увелициваются.

                // Запретили сортировку по столбцам типа real. 4-17 во избежания ошибок.
                for (int i = 4; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // Добавляем строку, заполняем ее автосуммой по столбцам
                // Строка суммы [Вес пантона, кг] - 5 колонка            
                adapter = new SqlDataAdapter("SELECT SUM([Вес пантона, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                DataSet dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                //DataRow row = dataSet.Tables[0].NewRow(); // добавляем новую строку в DataTable
                //dataSet.Tables[0].Rows.Add(row);
                dataSet.Tables[0].Rows.Add();  // Пустая строка
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][5] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса[Экстендер, кг] - 6 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Экстендер, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][6] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Желтая, кг] - 7 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Желтая, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][7] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Красная, кг] - 8 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Красная, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][8] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Рубин, кг] - 9 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Рубин, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][9] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Радомин, кг] - 10 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Радомин, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][10] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Оранжевая, кг] - 11 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Оранжевая, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][11] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Пинк, кг] - 12 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Пинк, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][12] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Фиолетовая, кг] - 13 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Фиолетовая, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][13] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Синяя, кг] - 14 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Пинк, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][14] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Зеленая, кг] - 15 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Зеленая, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][15] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Черная, кг] - 16 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Черная, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][16] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Белая, кг] - 17 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Белая, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][17] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Строка суммы веса [Вода, кг] - 18 колонка
                adapter = new SqlDataAdapter("SELECT SUM([Вода, кг]) as SUM_EX FROM Statictics" + sql_query, sqlConnection);  // Заполнение DataGridView1
                adapter.SelectCommand.Parameters.AddWithValue("Color", txtSRPanton.Text);
                adapter.SelectCommand.Parameters.AddWithValue("DateIn", dateTimeIN.Value);
                adapter.SelectCommand.Parameters.AddWithValue("DateOUT", dateTimeOUT.Value);
                dataSet_Sum = new DataSet();
                adapter.Fill(dataSet_Sum);
                dataSet.Tables[0].Rows[(dataSet.Tables[0].Rows.Count) - 1][18] = dataSet_Sum.Tables[0].Rows[0][0]; // Rows[строка][столбец]

                // Выделяем последнюю строку суммы жирным шрифтом
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Bold);
            }
            
            sql_query = ""; // Чистим часть запроса
            
        }

        // Номеруем строки // МОЖНО ID удаленный столбец переписать данными о номерах строк
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
            this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;
        }
    }
}

/*
 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Management;
using System.IO;


namespace Protection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string DiscSN_id = "SN193308905319";
        string CPUInfo_id = "BFEBFBFF000406C4";
        string VideoRam_id = "1073741824";


        private string CPUInfo()
        {
            var mds = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");  // Переменной присваивается неявный тип
            string processor_id = "";
            foreach (ManagementObject mo in mds.Get())    // Перебор массива значений mds.Get()
            {
                processor_id = mo["ProcessorId"].ToString();      // Возвращает значение ProcessorId в виде строки.
                break;
            }
            return processor_id;              
        }
                       
        private string DiscSN()
        {
            //("root\\CIMV2", "SELECT * FROM Win32_DiskDrive"); ---с правами доступа
            var mds = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");  // Переменной присваивается неявный тип
            string discSN = "";
            foreach (ManagementObject mo in mds.Get())    // Перебор массива значений mds.Get()
            {
                discSN = mo["SerialNumber"].ToString();      // Возвращает значение Disc SerialNumber  в виде строки.
                break;
            }
            return discSN;
        }

        private string VideoRam()
        {
            var mds = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");  // Переменной присваивается неявный тип
            string videoRam = "";
            foreach (ManagementObject mo in mds.Get())    // Перебор массива значений mds.Get()
            {
                videoRam = mo["AdapterRAM"].ToString();      // Возвращает значение AdapterRAM в виде строки.
                break;
            }
            return videoRam;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = CPUInfo();
            textBox3.Text = DiscSN();
            textBox4.Text = VideoRam();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (CPUInfo_id != CPUInfo() || DiscSN_id != DiscSN() || VideoRam_id != VideoRam())
             //   this.Close();

        }
    }
}


*/