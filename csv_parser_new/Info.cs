namespace Csv_parser_new
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Info
    {
        public string First { get; set; }

        public string Second { get; set; }

        public string Third { get; set; }
    }

    public class Program
    {
        public static bool Parser(ArrayList arrFileText, out List<string> arrParsed)
        {
            arrParsed = new List<string>();
            foreach (string textLine in arrFileText)
            {
                bool quoteIsStarted = false,    // для текста в кавычках
                    textIsStarted = false,      // для текста без кавычек
                    isSeparated = false;         // закончился ли блок текста
                int startIndex = -1, endIndex = -1;
                for (int i = 0; i < textLine.Length - 1; i++)
                {
                    if (!quoteIsStarted && textLine[i] == '\"' && char.IsLetterOrDigit(textLine[i + 1]))
                    {
                        startIndex = i + 1;
                        quoteIsStarted = true;
                        continue;
                    }

                    if (char.IsLetterOrDigit(textLine[i]) && textLine[i + 1] == '\"')
                    {
                        endIndex = i;
                        quoteIsStarted = false;
                        continue;
                    }

                    if (!quoteIsStarted && !textIsStarted && char.IsUpper(textLine[i]))
                    {
                        startIndex = i;
                        textIsStarted = true;
                        continue;
                    }

                    if (!quoteIsStarted && textLine[i] == ',')
                    {
                        isSeparated = true;
                    }

                    if (isSeparated && textIsStarted)
                    {
                        endIndex = i - 1;
                        textIsStarted = false;
                    }

                    if (isSeparated)
                    {
                        arrParsed.Add(textLine.Substring(startIndex, endIndex - startIndex + 1));
                        isSeparated = false;
                    }
                }

                if (arrParsed.Count > 1)
                {
                    if (endIndex - startIndex < 0)
                    {
                        endIndex = textLine.Length - 1;
                    }

                    arrParsed.Add(textLine.Substring(startIndex, endIndex - startIndex + 1));
                }
                else
                {
                    Console.WriteLine("Невозможно распарсить {0}-ю строку, проверьте корректность файла.", arrFileText.IndexOf(textLine) + 1);
                }
            }

            if (arrParsed.Count <= 1)
            {
                Console.WriteLine("Отсутствуют разделители!\n" +
                    "Проверьте корректность файла!");
                return false;
            }

            return true;
        }

        public static List<Info> Union(List<string> arrParsed)
        {
            List<Info> by_3_items = new List<Info>();
            for (int i = 0; i < arrParsed.Count - 2; i = i + 3)
            {
                by_3_items.Add(new Info
                {
                    First = arrParsed[i],
                    Second = arrParsed[i + 1],
                    Third = arrParsed[i + 2]
                });
            }

            return by_3_items;
        }

        public static List<Info> Sorter(List<Info> by_3_items, int inputSortProperty)
        {
            IEnumerable<Info> sortedList = null;
            switch (inputSortProperty)
            {
                case 1:
                    sortedList = from info in by_3_items orderby info.First select info;
                    break;
                case 2:
                    sortedList = from info in by_3_items orderby info.Second select info;
                    break;
                case 3:
                    sortedList = from info in by_3_items orderby info.Third select info;
                    break;
            }

            return sortedList.ToList();
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу. Пример: \"C:\\\\Users\\file.csv\"");
            string path = Console.ReadLine();
            Console.WriteLine("Выберите столбец для сортироки. Если сортировка не нужна, введите 0.");
            int inputSortProperty = int.Parse(Console.ReadLine());
            ArrayList arrFileText = new ArrayList();
            try
            {
                StreamReader str = new StreamReader(path);
                string line = string.Empty;
                while (line != null)
                {
                    line = str.ReadLine();
                    if (line != null)
                    {
                        arrFileText.Add(line);
                    }
                }

                str.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            List<string> arrParsed = new List<string>();
            bool isParsed = Parser(arrFileText, out arrParsed);     // вывод списка, если удалось распарсить
            if (isParsed)
            {
                var list = Union(arrParsed);
                int maxLength_1 = list.Max(s => s.First.Length);
                int maxLength_2 = list.Max(s => s.Second.Length);
                if (inputSortProperty >= 1 && inputSortProperty <= 3)
                {
                    list = Sorter(list, inputSortProperty);
                }
                else
                {
                    Console.WriteLine("Неверно выбран столбец для сортировки.\n" +
                        "Список не будет отсортирован.\n");
                }

                foreach (Info s in list)
                {
                    Console.WriteLine(
                        "{0} \t{1} \t{2}",
                        s.First.PadRight(maxLength_1),
                        s.Second.PadRight(maxLength_2),
                        s.Third);
                }
            }
        }
    }
}
