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

// Удалено!

namespace GofromirColor
{
    public partial class Form1 : Form
    {
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



        
        SqlConnection sqlConnection; // Для подключения к БД. Обьект обьявляем как поле класса  /*
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ida\Source\Repos\GitHub\GofromirColor\Properties\col1.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            lbl_listbox1.Visible = false;

            // Текстовые поля с отображением цвета не доступны для работы. 11 шт.
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
            SqlCommand command = new SqlCommand("SELECT * FROM [Colors$]", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос

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

        private async void button1_Click(object sender, EventArgs e)  // Сохранить асинхронно
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

            {
                textBox13.Text = textBox13.Text + "/" + Convert.ToString(comboBox1.SelectedItem) + "/" + Convert.ToString(comboBox2.SelectedItem);
                //SqlConnection sqlConnection; обьявлено в начале
                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение
                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlCommand command = new SqlCommand("INSERT INTO [Colors$] (Color, ExtenderProc, YellowProc, RedProc, RubinProc, RadominProc, OrangeProc, PinkProc, VioletProc, BlueProc, GreenProc, BlackProc, WhiteProc, Water, Viscosity, Extender, WhiteBlack) " +
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

            }
        }

        // НАЧАЛО. ПРОВЕРКА 14 ПОЛЕЙ НА ТО, ЧЧТО ВВЕДЕНЫ ТОЛЬКО ЦИФРЫ С ПЛАВАЮЩЕЙ ЗАПЯТОЙ
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
            Proverka_vvoda proverka = new Proverka_vvoda(e.KeyChar, textBox28.Text);

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
                SqlCommand command = new SqlCommand("SELECT [Color] FROM [Colors$] WHERE [Color] LIKE '%' + @a + '%'", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос
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

            if (txtGRPanton.Text == "")
            {
                MessageBox.Show("Заполните поле 'Пантон'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox28.Text == "0,0")
            {
                MessageBox.Show("Заполните поле 'Вес, кг'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                                              
                SqlCommand command = new SqlCommand("SELECT * FROM [Colors$] WHERE [Color]=@Color", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос                
                command.Parameters.AddWithValue("Color", txtGRPanton.Text); 
                
                // Выборка по 1 нужной строке
                // SqlCommand command = new SqlCommand("SELECT * FROM [Products] WHERE [Name]=@Name", sqlConnection);    // Выбираем все из Products, sqlConnection - для определения куда отпавлять запрос
                // command.Parameters.AddWithValue("Name", textBox1.Text); // Выбираем из таблицы только строку в которой Name=молоко - Введенное значение в textBox1.Text

                try
                {
                    sqlReader = await command.ExecuteReaderAsync();
                    while (await sqlReader.ReadAsync()) // Заполнение данных, для размешивания
                    {
                        //float f_test = sqlReader.GetFloat(4);   // ExtenderProc - 4 колонка. Эксперимент с получение дробной части.
                        //f_test = f_test * Convert.ToSingle(textBox28.Text);
                                                                                                                     
                        
                        listBox1.Items.Add("Рецепт изготовления " + txtGRPanton.Text  + " на " + textBox28.Text + " кг.");                       
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Рецепт - " + Convert.ToString(sqlReader.GetValue(2)));      // По белому/ По бурому
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Экстендер(лак) - " + Convert.ToString(sqlReader.GetValue(3)));      // Экстендер
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Состав:");      
                        listBox1.Items.Add("");
                        if (sqlReader.GetFloat(4) != 0)
                            listBox1.Items.Add("Экстендер - " + Convert.ToString(sqlReader.GetFloat(4) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(5) != 0)
                            listBox1.Items.Add("Желтая - " + Convert.ToString(sqlReader.GetFloat(5) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(6) != 0)
                            listBox1.Items.Add("Красная - " + Convert.ToString(sqlReader.GetFloat(6) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(7) != 0)
                            listBox1.Items.Add("Рубин - " + Convert.ToString(sqlReader.GetFloat(7) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(8) != 0)
                            listBox1.Items.Add("Радомин - " + Convert.ToString(sqlReader.GetFloat(8) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(9) != 0)
                            listBox1.Items.Add("Оранжевая - " + Convert.ToString(sqlReader.GetFloat(9) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(10) != 0)
                            listBox1.Items.Add("Пинк - " + Convert.ToString(sqlReader.GetFloat(10) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(11) != 0)
                            listBox1.Items.Add("Фиолетовая - " + Convert.ToString(sqlReader.GetFloat(11) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(12) != 0)
                            listBox1.Items.Add("Синяя - " + Convert.ToString(sqlReader.GetFloat(12) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(13) != 0)
                            listBox1.Items.Add("Зеленая - " + Convert.ToString(sqlReader.GetFloat(13) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(14) != 0)
                            listBox1.Items.Add("Черная - " + Convert.ToString(sqlReader.GetFloat(14) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(15) != 0)
                            listBox1.Items.Add("Белая - " + Convert.ToString(sqlReader.GetFloat(15) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
                        if (sqlReader.GetFloat(16) != 0)
                            listBox1.Items.Add("Вода - " + Convert.ToString(sqlReader.GetFloat(16) / 100 * Convert.ToSingle(textBox28.Text)) + " кг");
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
            if (tabControl1.SelectedIndex == 0) // Вкладка "Готовить рецепт"
            {
                sqlConnection = new SqlConnection(connectionString);    // Устанавливаем соединение

                await sqlConnection.OpenAsync();    // Открываем БД в асинхронном режиме
                SqlDataReader sqlReader = null; // SQLDataReader позволяет получать таблицу в таблицном виде
                SqlCommand command = new SqlCommand("SELECT * FROM [Colors$]", sqlConnection);    // Выбираем все из Colors, sqlConnection - для определения куда отпавлять запрос
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
    }

}
