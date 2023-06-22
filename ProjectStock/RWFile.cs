using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectStock
{
    public static class RWFile
    {
        public static readonly string file_name = "data.json";

        public static void WriteFile(List<Pallet> _pallets)
        {
            try
            {
                using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate))
                    JsonSerializer.Serialize<List<Pallet>>(fs, _pallets);
            }
            catch (Exception e) { Console.WriteLine("Ошибка: " + e.Message); }
            finally { Console.WriteLine("Для возвращения в меню нажмите любую кнопку..."); }
            Console.WriteLine("Файл успешно сохранен.");
            Console.ReadLine();
        }

        public static List<Pallet>? ReadFile()
        {
            List<Pallet>? pallets = null;
            try
            {
                using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate))
                    pallets = JsonSerializer.Deserialize<List<Pallet>>(fs);


            }
            catch (Exception e) { Console.WriteLine("Ошибка: " + e.Message); }
            Console.WriteLine("Файл успешно загружен.");
            return pallets;
        }
    }
}
