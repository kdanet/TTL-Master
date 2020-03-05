using System;
using System.IO;

namespace Tethering
{
    class Program
    {
        static int Menu()
        {
            Console.WriteLine(" Что вы хотите сделать?");
            Console.WriteLine(" 1) Обойти блокировку\n 2) Посмотреть значение TTL\n 3) Создать резервную копию\n" +
                " 4) Восстановить из резервной копии\n 5) Получить список резервных копий\n 6) Выход\n");
            int choice;
            bool check = int.TryParse(Console.ReadLine(), out choice);
            if (check == false) return 0;
            if (choice == 1)
            {
                Console.Clear();
                Console.WriteLine(" Введите номер выбранного пункта:");
                Console.WriteLine(" 1) Раздача с Android или IOS по Wi-Fi");
                Console.WriteLine(" 2) Раздача с Android или IOS по Wi-Fi по USB");
                Console.WriteLine(" 3) Раздача с других OS по Wi-Fi");
                Console.WriteLine(" 4) Раздача с других OS по USB");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" 5) Выход в меню...\n");
                Console.ResetColor();
                int x = int.Parse(Console.ReadLine());
                if (x == 1) { logic.Save(65); return 2; }
                else if (x == 2) { logic.Save(64); return 2; }
                else if (x == 3) { logic.Save(130); return 2; }
                else if (x == 4) { logic.Save(129); return 2; }
                else if (x == 5) { return 0; }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Неверный Ввод");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (choice == 2)
            {
                try
                {
                    int x = logic.Get();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n Текущее значение TTL: {0}", x);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Ошибка: Запустите Приложение от Имени Администратора.");
                    Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
            else if (choice == 3)
            {
                logic.Backup();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Резервная копия успешно создана.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (choice == 4)
            {
                Console.Clear();
                var y = logic.GetBackupList();
                if (y.Count == 0)
                {
                    Console.WriteLine("Список резервных копий пуст...");
                    return 0;
                }
                foreach (var item in y)
                {
                    Console.WriteLine(" ID: " + item.id);
                    Console.WriteLine(" Значение TTL: " + item.TTLValue);
                    Console.WriteLine(" Дата создания: " + item.Date);
                    Console.WriteLine("---------------------------------------------------------");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n\nВведите ID резервной копии,которую хотите восстановить (-1,чтобы выйти в меню)");
                Console.ForegroundColor = ConsoleColor.White;
                int id = int.Parse(Console.ReadLine());
                if (id == -1) return 0;
                logic.RestoreBackup(id);
                return 2;
            }
            else if (choice == 5)
            {
                Console.Clear();
                var y = logic.GetBackupList();
                if (y.Count == 0)
                {
                    Console.WriteLine("Список резервных копий пуст...");
                    return 0;
                }
                foreach (var item in y)
                {
                    Console.WriteLine(" ID: " + item.id);
                    Console.WriteLine(" Значение TTL: " + item.TTLValue);
                    Console.WriteLine(" Дата создания: " + item.Date);
                    Console.WriteLine("------------------------------------------------");
                }
            }
            else if (choice == 6)
            {
                return 1;
            }
            return 0;
        }
        static BusinnesLogic logic;
        static void Main(string[] args)
        {


            logic = new BusinnesLogic(new DataSource());
            var file=File.Open("log.txt", FileMode.OpenOrCreate);
            using (var stream = new StreamReader(file))
            using (var wr = new StreamWriter(file))
            {
                string str = stream.ReadLine();
                if (str==null)
                {
                    logic.Save(65);
                    wr.Write("1");
                }
                
            }
            logic.Save(65);
            while (true)
            {
                var text = new WenceyWang.FIGlet.AsciiArt("by    kdanet");
                text.ToString(); //Get string
                var result = text.Result; //Get every Single line
                foreach (var el in result) Console.WriteLine(el);
                text = new WenceyWang.FIGlet.AsciiArt("          4pda    ");
                text.ToString();
                result = text.Result;
                Console.ForegroundColor = ConsoleColor.Blue;
                foreach (var el in result) Console.WriteLine(el);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\n");
                int exit = Menu();
                //0-продолжить в штатном режиме
                //1-завершить работу программы
                //2-перезагрузить ПК
                if (exit == 1) break;
                else if (exit == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n Перезагрузить компьютер сейчас? Y/N");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        System.Diagnostics.Process.Start("shutdown", "/r /t 000");
                    }
                    else Console.Clear();
                }
                Console.WriteLine("\nНажмите Любую Кнопку для продолжения...");
                Console.ReadLine();
                Console.Clear();

            }


        }
    }
}
