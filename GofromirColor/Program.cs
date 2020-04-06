using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GofromirColor
{
    class Proverka_vvoda
    {
        // Правильными символами считаются цифры,
        // запятая, <Enter> и <Backspace>.
        // Будем считатьать правильным символом
        // также точку, на заменим ее запятой.
        // Остальные символы запрещены.
        // Чтобы запрещенный символ не отображался 
        // в поле редактирования,присвоим 
        // значение true свойству Handled параметра e

        public char chr;
        public string chr_str;
        public Proverka_vvoda(char x, string x1) { chr = x; chr_str = x1; }
        
        public char Conv()
        {
            if (( chr>= '0') && (chr <= '9'))   // цифра
            {             
                return chr;
            }

            if (chr == '.')   // точку заменим запятой
            {
                chr = ',';
            }

            if (chr == ',')   // запятая уже есть в поле редактирования
            {
                if (chr_str.IndexOf(',') != -1)   // -1 - символ в строке не найден
                {
                    chr = ' ';  // Распознаем символ и начатие не обрабатываем. e.Handled = true;
                }
                return chr;
            }

            if (Char.IsControl(chr)) // Разрешаем ввод  <Backspace>. <Esc>,<Enter> - не разрешаем
            {
                if (chr == (char)Keys.Back)
                    return chr;
            }

            // остальные символы запрещены
            chr = ' ';  // Распознаем символ и начатие не обрабатываем. e.Handled = true;
            return chr;
        }

    }

    /*class Proverka_text
    {
        public string str_text;
        public Proverka_text(string x) { str_text = x; }
        public string Str_text()
        {
            string a = "аб";
            str_text = str_text + a;
            return str_text;
        }
    }*/


    /*

        }
     */




    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
