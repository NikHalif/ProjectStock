﻿// See https://aka.ms/new-console-template for more information
using ConsoleTables;
using ConsoleTools;
using ProjectStock;

List<Pallet> pallets = new List<Pallet>();

ShowMenu();

/// Отобразить главное меню 
void ShowMenu(string title = "Меню:")
{
    var menu = new ConsoleMenu(args, level: 0)
      .Add("Добавление паллеты", MenuAddPallet)
      .Add("Редактирование паллет", (thisMenu) => { MenuAllPallets(); thisMenu.CloseMenu(); })
      .Add("Вывод отсортированных паллет по ТЗ", () => MenuFilterPallets())
      //.Add("Загрузка данных из файла", () => MenuAllPallets())
      //.Add("Сохранение данных в файл", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!")
      .Add("Выход", () => Environment.Exit(0))
      .Configure(config =>
      {
          config.WriteHeaderAction = () => Console.WriteLine(title);
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.EnableWriteTitle = false;
          config.EnableBreadcrumb = false;
      });

    menu.Show();
}

/// Отобразить все паллеты 
void MenuFilterPallets(string title = "Список паллет:")
{
    var table = PalletsToTable(pallets);
    table.Write();

    Console.WriteLine("Для возвращения в меню нажмите любую кнопку...");
    Console.ReadKey();
    ShowMenu();
}

/// Формирование таблицы из списка паллет
ConsoleTable PalletsToTable(List<Pallet> _pallets)
{
    var table = new ConsoleTable("ID", "Ширина, см", "Длина, см", "Объем, см^3", "Масса, кг", "Кол-во коробок", "Срок годности");
    foreach (var pallet in pallets)
        table.AddRow(pallet.ID, pallet.Width, pallet.Depth, pallet.GetVolumePallet(), pallet.GetMass(), pallet.Boxes.Count, pallet.GetDateExpiration());
    return table;
}
/// Отобразить все паллеты 
void MenuAllPallets(string title = "Список паллет:")
{
    var menu_pallets = new ConsoleMenu(args, level: 1)
    .Add("Назад", ConsoleMenu.Close);

    foreach (var pallet in pallets)
    {
        menu_pallets.Add(pallet.ID.ToString(), () => MenuEditPallet(pallet.ID));
    }
    if(pallets.Count > 5) menu_pallets.Add("Назад", ConsoleMenu.Close);
    menu_pallets.Configure(config =>
        {
            config.WriteHeaderAction = () => Console.WriteLine(title);
            config.Selector = "--> ";
            config.EnableFilter = false;
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = false;
        }).Show();
}

/// Добавление паллеты 
void MenuAddPallet()
{
    Console.WriteLine("Укажите ID паллеты:");
    var id_text = Console.ReadLine();
    ulong id;
    while (!ulong.TryParse(id_text?.Trim(), out id))
    {
        Console.Clear();
        Console.WriteLine($"ID должен быть числом от {ulong.MinValue} до {ulong.MaxValue}.\nУкажите корректный ID паллеты:");
        id_text = Console.ReadLine();
    }

    if (pallets.Find(pallet => pallet.ID == id) != null)
        ShowMenu($"Паллета с ID {id} уже есть в списке.\nУкажите индивидуальный ID или отредактируйте нужную паллету через главное меню:");

    Console.WriteLine("Укажите ширину паллеты в сантиметрах:");
    var width_text = Console.ReadLine();
    float width;
    while (!float.TryParse(width_text?.Trim(), out width) || width < 0)
    {
        Console.Clear();
        Console.WriteLine($"Ширина должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную ширину паллеты в сантиметрах:");
        width_text = Console.ReadLine();
    }

    Console.WriteLine("Укажите длину паллеты в сантиметрах:");
    var depth_text = Console.ReadLine();
    float depth;
    while (!float.TryParse(depth_text?.Trim(), out depth) || width < 0)
    {
        Console.Clear();
        Console.WriteLine($"Длина должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную длину паллеты в сантиметрах:");
        depth_text = Console.ReadLine();
    }
    pallets.Add(new Pallet(id, width, depth));
    MenuResultAddPallet(id);
}

/// Меню выбора действия после добавления панели
void MenuResultAddPallet(ulong pallet_id)
{
    new ConsoleMenu(args, level: 1)
        .Add("Редактировать паллету", (thisMenu) => { MenuEditPallet(pallet_id); thisMenu.CloseMenu(); })
        .Add("Вернуться в главное меню", ConsoleMenu.Close)
        .Configure(config =>
        {
            config.WriteHeaderAction = () => Console.WriteLine("Паллета добавлена!\nВыберите действие:");
            config.Selector = "--> ";
            config.EnableFilter = false;
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = false;
        })
        .Show();
}

/// Добавление коробки к паллете с ID
void MenuAddBox(ulong pallet_id, ulong? box_id = null)
{
    var pallet = pallets.Find(pallet => pallet.ID == pallet_id);
    if (pallet == null) { ShowMenu("Ошибка!\nID паллеты для добавления коробки не найден.\nМеню:"); return; }

    ulong id;
    if (box_id == null)
    {
        Console.WriteLine("Укажите ID коробки:");
        var id_text = Console.ReadLine();
        while (!ulong.TryParse(id_text?.Trim(), out id))
        {
            Console.Clear();
            Console.WriteLine($"ID должен быть числом от {ulong.MinValue} до {ulong.MaxValue}.\nУкажите корректный ID коробки:");
            id_text = Console.ReadLine();
        }

        if (pallet.Find(boxes => boxes.ID == id) != null)
            MenuEditPallet(pallet_id, $"Коробка с ID {id} уже есть в списке.\nУкажите индивидуальный ID или отредактируйте нужную коробку:");
    }
    else id = (ulong)box_id;

    Console.WriteLine("Укажите ширину коробки в сантиметрах:");
    var width_text = Console.ReadLine();
    float width;
    while (!float.TryParse(width_text?.Trim(), out width) || width < 0)
    {
        Console.Clear();
        Console.WriteLine($"Ширина должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную ширину коробки в сантиметрах:");
        width_text = Console.ReadLine();
    }

    Console.WriteLine("Укажите высоту коробки в сантиметрах:");
    var height_text = Console.ReadLine();
    float height;
    while (!float.TryParse(height_text?.Trim(), out height) || height < 0)
    {
        Console.Clear();
        Console.WriteLine($"Высота должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную высоту коробки в сантиметрах:");
        height_text = Console.ReadLine();
    }

    Console.WriteLine("Укажите глубину коробки в сантиметрах:");
    var depth_text = Console.ReadLine();
    float depth;
    while (!float.TryParse(depth_text?.Trim(), out depth) || depth < 0)
    {
        Console.Clear();
        Console.WriteLine($"Глубина должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную глубину коробки в сантиметрах:");
        depth_text = Console.ReadLine();
    }

    Console.WriteLine("Укажите массу коробки в килограммах:");
    var mass_text = Console.ReadLine();
    float mass;
    while (!float.TryParse(mass_text?.Trim(), out mass) || mass < 0)
    {
        Console.Clear();
        Console.WriteLine($"Масса должна быть числом от 0 до {float.MaxValue}.\nУкажите корректную массу коробки в килограммах:");
        mass_text = Console.ReadLine();
    }
    // DateTime? date_production = null, DateTime? date_expiration = null

    Console.WriteLine("Укажите дату производства:");
    var date_production_text = Console.ReadLine();
    DateTime date_production;
    while (!DateTime.TryParse(date_production_text?.Trim(), out date_production))
    {
        Console.Clear();
        Console.WriteLine($"Некорректная дата. Пример формата: 22.06.2023.\nУкажите корректную массу коробки в килограммах:");
        date_production_text = Console.ReadLine();
    }

    try
    {
        pallet.Add(new Box(id, width, height, depth, mass, date_production));
    }
    catch (ArgumentException e)
    {
        MenuEditPallet(pallet_id, $"Ошибка добавления коробки:\n{e.Message}\nВыберите действие:");
    }
    MenuEditPallet(pallet_id, $"Коробка {id} добавлена на паллету {pallet_id}\nВыберите действие:");
}

/// Меню редактирования паллеты по её ID
void MenuEditPallet(ulong id, string title = "")
{
    var pallet = pallets.Find(pallet => pallet.ID == id);
    if (pallet == null) { ShowMenu("Ошибка!\nID паллеты не найден.\nМеню:"); return; }

    new ConsoleMenu(args, level: 1)
        .Add("Добавить коробку", (thisMenu) => { thisMenu.CloseMenu(); MenuAddBox(id); })
        .Add("Редактировать коробки", () => MenuEditAllBox(id))
        .Add("Удалить паллету", (thisMenu) => { pallets.Remove(pallet); ShowMenu("Паллета удалена!\nГлавное меню:"); thisMenu.CloseMenu(); })
        .Add("Назад", ConsoleMenu.Close)
        .Configure(config =>
        {
            config.WriteHeaderAction = () => { if (title.Trim() == "") Console.WriteLine($"ID Паллеты:{id}\nВыберите действие:"); else Console.WriteLine(title); };
            config.Selector = "--> ";
            config.EnableFilter = false;
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = false;
        })
        .Show();
}

/// Вывод всех коробок паллеты по ID паллеты
void MenuEditAllBox(ulong id, string title = "Список коробок:")
{
    var pallet = pallets.Find(pallet => pallet.ID == id);
    if (pallet == null) { ShowMenu("Ошибка!\nID паллеты не найден.\nМеню:"); return; }

    var menu_boxes = new ConsoleMenu(args, level: 2)
    .Add("Назад", ConsoleMenu.Close);

    foreach (var box in pallet.Boxes)
    {
        menu_boxes.Add(box.ID.ToString(), () => MenuEditBox(box.ID, pallet.ID));
    }
    if(pallet.Boxes.Count > 5) menu_boxes.Add("Назад", ConsoleMenu.Close);
    menu_boxes.Configure(config =>
        {
            config.WriteHeaderAction = () => Console.WriteLine(title);
            config.Selector = "--> ";
            config.EnableFilter = false;
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = false;
        }).Show();
}

/// Меню редактирования коробки
void MenuEditBox(ulong box_id, ulong pallet_id, string title = "")
{
    var pallet = pallets.Find(pallet => pallet.ID == pallet_id);
    if (pallet == null) { ShowMenu("Ошибка!\nID паллеты не найден.\nМеню:"); return; }
    var box = pallet.Find(b => b.ID == box_id);
    if (box == null) { MenuEditAllBox(pallet_id, "Ошибка!\nID коробки не найден.\nМеню:"); return; }

    new ConsoleMenu(args, level: 3)
        .Add("Пересоздать коробку", (thisMenu) => { MenuAddBox(pallet_id, box_id); thisMenu.CloseMenu(); })
        .Add("Удалить коробку", (thisMenu) => { pallet.Remove(box); MenuEditAllBox(pallet_id,"Коробка удалена!\nМеню коробок:"); thisMenu.CloseMenu(); })
        .Add("Назад", ConsoleMenu.Close)
        .Configure(config =>
        {
            config.WriteHeaderAction = () => { if (title.Trim() == "") Console.WriteLine($"ID паллеты:{pallet_id}, ID коробки: {box_id}\nВыберите действие:"); };
            config.Selector = "--> ";
            config.EnableFilter = false;
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = false;
        })
        .Show();
}