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
      while (count<linesPerPage && ((line = myReader.ReadLine()) != null))
     {
         // calculate the next line position based on - Рассчитать следующую позицию строки на основе высоты шрифта в соответствии с печатающим устройством
         // the height of the font according to the printing device
         yPosition = topMargin + (count* printFont.GetHeight(ev.Graphics));
 
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
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Подготовка информационного поля tabpage1  
            lblErrT0.Text = "Внимание! ";
            lblErrT0.Visible = false;

            tabPage3.Parent = null;     // Скрываем элемент "Корректировка рецепта"

    


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

            // Определяем св-ва txtGRPanton. Для выпадающего списка
            txtGRPanton.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtGRPanton.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

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



        private async void btnSearch_Click(object sender, EventArgs e)
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



                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                SqlCommand command = new SqlCommand("SELECT [Color] FROM [Colors] WHERE [Color] LIKE '%' + @a + '%'", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос
                command.Parameters.AddWithValue("a", txtGRPanton.Text);

                try
                {
                    sqlReader = await command.ExecuteReaderAsync();
                    while (await sqlReader.ReadAsync())
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {            
           if (tf_listbox)  //   Если f_listbox = false, то строку выполнения пропускаем  
                txtGRPanton.Text = listBox1.SelectedItem.ToString();
        }

        private async void btnCalculate_Click(object sender, EventArgs e)
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
                if (txtGRWeight.Text == "0,0" || txtGRWeight.Text == "0," || txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
                {
                    lblErrT0.Text += "Заполните поле 'Вес, кг'. ";                  
                }
            }
            // Проверка только txtGRWeight (вес)
            else if (txtGRWeight.Text == "0,0" || txtGRWeight.Text == "0," || txtGRWeight.Text == "0" || txtGRWeight.Text == "") // Условное ИЛИ
            {
                lblErrT0.Text += "Заполните поле 'Вес, кг'. ";
                lblErrT0.Visible = true;
            }                
         
            else
            {

                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                                              
                SqlCommand command = new SqlCommand("SELECT * FROM [Colors] WHERE [Color]=@Color", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос                
                command.Parameters.AddWithValue("Color", txtGRPanton.Text); 

                try
                {
                    sqlReader = await command.ExecuteReaderAsync();
                    while (await sqlReader.ReadAsync()) // Заполнение данных, для размешивания
                    {
                        //float f_test = sqlReader.GetFloat(4);   // ExtenderProc - 4 колонка. Эксперимент с получение дробной части.
                        //f_test = f_test * Convert.ToSingle(textBox28.Text);
                                                                                                                     
                        
                        listBox1.Items.Add("Рецепт изготовления " + txtGRPanton.Text  + " на " + txtGRWeight.Text + " кг.");                       
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Рецепт - " + Convert.ToString(sqlReader.GetValue(2)));      // По белому/ По бурому
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Экстендер(лак) - " + Convert.ToString(sqlReader.GetValue(3)));      // Экстендер
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Состав:");      
                        listBox1.Items.Add("");
                        if (sqlReader.GetFloat(4) != 0)
                            listBox1.Items.Add("Экстендер - " + Convert.ToString(sqlReader.GetFloat(4) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(5) != 0)
                            listBox1.Items.Add("Желтая - " + Convert.ToString(sqlReader.GetFloat(5) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(6) != 0)
                            listBox1.Items.Add("Красная - " + Convert.ToString(sqlReader.GetFloat(6) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(7) != 0)
                            listBox1.Items.Add("Рубин - " + Convert.ToString(sqlReader.GetFloat(7) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(8) != 0)
                            listBox1.Items.Add("Радомин - " + Convert.ToString(sqlReader.GetFloat(8) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(9) != 0)
                            listBox1.Items.Add("Оранжевая - " + Convert.ToString(sqlReader.GetFloat(9) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(10) != 0)
                            listBox1.Items.Add("Пинк - " + Convert.ToString(sqlReader.GetFloat(10) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(11) != 0)
                            listBox1.Items.Add("Фиолетовая - " + Convert.ToString(sqlReader.GetFloat(11) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(12) != 0)
                            listBox1.Items.Add("Синяя - " + Convert.ToString(sqlReader.GetFloat(12) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(13) != 0)
                            listBox1.Items.Add("Зеленая - " + Convert.ToString(sqlReader.GetFloat(13) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(14) != 0)
                            listBox1.Items.Add("Черная - " + Convert.ToString(sqlReader.GetFloat(14) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(15) != 0)
                            listBox1.Items.Add("Белая - " + Convert.ToString(sqlReader.GetFloat(15) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
                        if (sqlReader.GetFloat(16) != 0)
                            listBox1.Items.Add("Вода - " + Convert.ToString(sqlReader.GetFloat(16) / 100 * Convert.ToSingle(txtGRWeight.Text)) + " кг");
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

            }
        }

        private async void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 2) tabControl1.SelectedIndex = 0; // Не пускаем в раздел. SelectedIndex == 2 - Корректировка рецепта. Переводим в раздел откуда берутся входные данные.
            if (tabControl1.SelectedIndex == 0) // Вкладка "Готовить рецепт"
            {
                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                SqlCommand command = new SqlCommand("SELECT * FROM [Colors]", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос
                // Создаем коллекцию
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                
                try
                {
                    sqlReader = await command.ExecuteReaderAsync();
                    while (await sqlReader.ReadAsync())
                    {
                      // Заполняем MyCollection
                        MyCollection.Add(Convert.ToString(sqlReader["Color"]));
                    }
                    txtGRPanton.AutoCompleteCustomSource = MyCollection;
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
                        tabControl1.SelectedIndex = 2;  // Переносим на вкладку

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
                        txtKRExtenderKg.Text = Convert.ToString(Convert.ToSingle(sqlReader["ExtenderProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRYellow.Text = Convert.ToString(Convert.ToSingle(sqlReader["YellowProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRRed.Text = Convert.ToString(Convert.ToSingle(sqlReader["RedProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRRubin.Text = Convert.ToString(Convert.ToSingle(sqlReader["RubinProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRRadomin.Text = Convert.ToString(Convert.ToSingle(sqlReader["RadominProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKROrange.Text = Convert.ToString(Convert.ToSingle(sqlReader["OrangeProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRPink.Text = Convert.ToString(Convert.ToSingle(sqlReader["PinkProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRViolet.Text = Convert.ToString(Convert.ToSingle(sqlReader["VioletProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRBlue.Text = Convert.ToString(Convert.ToSingle(sqlReader["BlueProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRGreen.Text = Convert.ToString(Convert.ToSingle(sqlReader["GreenProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRBlack.Text = Convert.ToString(Convert.ToSingle(sqlReader["BlackProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRWhite.Text = Convert.ToString(Convert.ToSingle(sqlReader["WhiteProc"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
                        txtKRWater.Text = Convert.ToString(Convert.ToSingle(sqlReader["Water"]) / 100 * Convert.ToSingle(txtKRWeight.Text));
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
            float ft_txtKRExtenderKg = 0;
            float ft_txtKRYellow = 0;
            float ft_txtKRRed = 0;
            float ft_txtKRRubin = 0;
            float ft_txtKRRadomin = 0;
            float ft_txtKROrange = 0;
            float ft_txtKRPink = 0;
            float ft_txtKRViolet = 0;
            float ft_txtKRBlue = 0;
            float ft_txtKRGreen = 0;
            float ft_txtKRBlack = 0;
            float ft_txtKRWhite = 0;
            float ft_txtKRWater = 0;
                        
            if (Convert.ToSingle(txtKRViscosity.Text)<=0)
            {
                MessageBox.Show("Введите полученную вязкость!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            float summaKR = (float)Convert.ToDouble(txtKRExtenderKg.Text) + (float)Convert.ToDouble(txtKRYellow.Text) 
                            + (float)Convert.ToDouble(txtKRRed.Text) + (float)Convert.ToDouble(txtKRRubin.Text) 
                            + (float)Convert.ToDouble(txtKRRadomin.Text) + (float)Convert.ToDouble(txtKROrange.Text) 
                            + (float)Convert.ToDouble(txtKRPink.Text) + (float)Convert.ToDouble(txtKRViolet.Text) 
                            + (float)Convert.ToDouble(txtKRBlue.Text) + (float)Convert.ToDouble(txtKRGreen.Text)
                            + (float)Convert.ToDouble(txtKRBlack.Text) + (float)Convert.ToDouble(txtKRWhite.Text)
                            + (float)Convert.ToDouble(txtKRWater.Text);
            
            // На 0 делить нельзя. 0 присвоен при объявлении.
            if (Convert.ToDouble(txtKRExtenderKg.Text) > 0) 
                ft_txtKRExtenderKg = (float)Convert.ToDouble(txtKRExtenderKg.Text) / summaKR * 100;           
            if (Convert.ToDouble(txtKRYellow.Text) > 0)
                ft_txtKRYellow = (float)Convert.ToDouble(txtKRYellow.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRRed.Text) > 0)
                ft_txtKRRed = (float)Convert.ToDouble(txtKRRed.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRRubin.Text) > 0)
                ft_txtKRRubin = (float)Convert.ToDouble(txtKRRubin.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRRadomin.Text) > 0)
                ft_txtKRRadomin = (float)Convert.ToDouble(txtKRRadomin.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKROrange.Text) > 0)
                ft_txtKROrange = (float)Convert.ToDouble(txtKROrange.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRPink.Text) > 0)
                ft_txtKRPink = (float)Convert.ToDouble(txtKRPink.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRViolet.Text) > 0)
                ft_txtKRViolet = (float)Convert.ToDouble(txtKRViolet.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRBlue.Text) > 0)
                ft_txtKRBlue = (float)Convert.ToDouble(txtKRBlue.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRGreen.Text) > 0)
                ft_txtKRGreen = (float)Convert.ToDouble(txtKRGreen.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRBlack.Text) > 0)
                ft_txtKRBlack = (float)Convert.ToDouble(txtKRBlack.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRWhite.Text) > 0)
                ft_txtKRWhite = (float)Convert.ToDouble(txtKRWhite.Text) / summaKR * 100;
            if (Convert.ToDouble(txtKRWater.Text) > 0)
                ft_txtKRWater = (float)Convert.ToDouble(txtKRWater.Text) / summaKR * 100;
            
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
                command.Parameters.AddWithValue("Viscosity", (float)Convert.ToDouble(txtKRViscosity.Text));
                command.Parameters.AddWithValue("Extender", txtKRExtenderType.Text);
                command.Parameters.AddWithValue("WhiteBlack", txtKRWhiteBlack.Text);

                command.ExecuteNonQuery();
            }

            MessageBox.Show("Рецепт корректированного пантона внесен в БД. Имя - " + txtKRPantonNew.Text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tabControl1.SelectedIndex = 0;      // Переносим на вкладку "Готовить рецепт"
            tabPage3.Parent = null;             // Скрываем элемент "Корректировка рецепта"


        }
    }

}
