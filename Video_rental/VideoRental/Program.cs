using System.Collections;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> lab3

using CRUD;

using VideoRentalModels;

<<<<<<< HEAD
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using Update = CRUD.Update;

=======
>>>>>>> lab3
namespace VideoRental
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoRentalContext dbContext = new VideoRentalContext();
            Read read = new Read();
            bool isNotExit = true;


            while (isNotExit)
            {
                Console.WriteLine("Выберите пункт меню");
                Console.WriteLine("1. Чтение данных из таблицы");
                Console.WriteLine("2. Добавление данных в таблицу");
                Console.WriteLine("3. Обновление записи в таблице");
                Console.WriteLine("4. Удаление записи из таблицы");
                Console.WriteLine("5. Представления");
                Console.WriteLine("6. Добавить данные в таблицы Genres, Positions и Staff");
                Console.WriteLine("7. Запрос на выборку по полю в таблице Genres");
                Console.WriteLine("8. Запрос на поиск минимальной цены в таблице Prices");
                Console.WriteLine("9. Запрос на выборку данных из двух полей двух таблиц, связанных \n между собой отношением «один-ко-многим» (Staff и Positions)");
                Console.WriteLine("10. Запрос на выборку данных из двух таблиц, связанных между собой \n отношением «один-ко-многим» и отфильтрованным по некоторому \n" +
                                  "условию, налагающему ограничения на значения одного или нескольких полей (Staff и Positions)");


                Console.WriteLine(" Выход");
                int punct = int.Parse(Console.ReadLine());
                switch (punct)
                {
                    case 1:
                        Select(dbContext);
                        break;
                    case 2:

                        break;
                    case 3:
                        Update(dbContext);
                        break;
                    case 4:
                        Delete(dbContext);
                        break;
                    case 5:
                        Print(
                            "DiskId \t DiskTitle \t CreationYear \t Manufacturer \t Country \t MainActor \t Recording \t GenreTitle \t TypeTitle",
                            read.ViewAllDisks(dbContext));
                        break;

                    case 6:

                        // Создаем и добавляем 10 жанров кинофильмов на русском языке
                        var genresToAdd = new List<Genre>
                            {
                                new Genre
                                {
                                    Title = "Боевик",
                                    Description = "Фильмы в жанре боевика"
                                },
                                new Genre
                                {
                                    Title = "Комедия",
                                    Description = "Комедийные фильмы"
                                },
                                new Genre
                                {
                                    Title = "Драма",
                                    Description = "Драматические фильмы"
                                },
                                new Genre
                                {
                                    Title = "Ужасы",
                                    Description = "Фильмы ужасов"
                                },
                                new Genre
                                {
                                    Title = "Научная фантастика",
                                    Description = "Фильмы научной фантастики"
                                },
                                new Genre
                                {
                                    Title = "Приключения",
                                    Description = "Фильмы в жанре приключений"
                                },
                                new Genre
                                {
                                    Title = "Романтика",
                                    Description = "Фильмы о любви и романтике"
                                },
                                new Genre
                                {
                                    Title = "Фэнтези",
                                    Description = "Фильмы в жанре фэнтези"
                                },
                                new Genre
                                {
                                    Title = "Триллер",
                                    Description = "Триллеры и напряженные фильмы"
                                },
                                new Genre
                                {
                                    Title = "Мистика",
                                    Description = "Фильмы с элементами мистики"
                                }
                            };

                        dbContext.Genres.AddRange(genresToAdd);
                        dbContext.SaveChangesAsync();

                        var positionsToAdd = new List<Position>
                        {
                            new Position { Title = "Менеджер" },
                            new Position { Title = "Разработчик" },
                            new Position { Title = "Тестировщик" },
                            new Position { Title = "Дизайнер" }
                        };
                        dbContext.Positions.AddRange(positionsToAdd);
                        dbContext.SaveChangesAsync();

                        // Добавление данных в таблицу Staffs
                        var staffToAdd = new List<Staff>
                        {
                            new Staff { Surname = "Иванов", Name = "Петр", PositionId = 1, DateOfEmployment = DateTime.Parse("2022-01-15") },
                            new Staff { Surname = "Петров", Name = "Иван", PositionId = 2, DateOfEmployment = DateTime.Parse("2021-11-10") },
                            new Staff { Surname = "Сидоров", Name = "Алексей", PositionId = 3, DateOfEmployment = DateTime.Parse("2022-03-20") },
                            new Staff { Surname = "Козлов", Name = "Дмитрий", PositionId = 4, DateOfEmployment = DateTime.Parse("2022-02-05") }
                        };
                        dbContext.Staff.AddRange(staffToAdd);
                        dbContext.SaveChangesAsync();
                        break;

                    case 7:

                        Console.WriteLine("Введите название жанра для поиска");

                        var filteredGenres = dbContext.Genres.Where(g => g.Title.Contains(Console.ReadLine().ToLower())).ToList();

                        foreach (var genre in filteredGenres)
                        {
                            Console.WriteLine(genre.Title + " - " + genre.Description);
                        }
                        break;

                    case 8:
                        decimal? minPrice = dbContext.Pricelists.Min(p => p.Price);

                        Console.WriteLine($"Минимальная цена: {minPrice}");
                        break;

                    case 9:
                        var staffAndPositionTitles = dbContext.Staff
                            .Join(
                                dbContext.Positions,
                                staff => staff.PositionId,
                                position => position.PositionId,
                                (staff, position) => new
                                {
                                    StaffSurname = staff.Surname,
                                    StaffName = staff.Name,
                                    PositionTitle = position.Title
                                })
                            .ToList();

                        foreach (var item in staffAndPositionTitles)
                            Console.WriteLine($"Surname: {item.StaffSurname}, Name: {item.StaffName}, Position: {item.PositionTitle}");

                        break;

                    case 10:
                        var filteredStaff = dbContext.Staff
                            .Join(
                                dbContext.Positions,
                                staff => staff.PositionId,
                                position => position.PositionId,
                                (staff, position) => new
                                {
                                    Staff = staff,
                                    PositionTitle = position.Title
                                })
                            .Where(result => result.PositionTitle == "Разработчик")
                            .ToList();

                        foreach (var item in filteredStaff)
                        {
                            Console.WriteLine($"Surname: {item.Staff.Surname}, Name: {item.Staff.Name}, Position: {item.PositionTitle}");
                        }
                        break;


                    case 11:
                        isNotExit = false;
                        break;
                    default:
                        Console.WriteLine("Выбран неверный пункт меню");
                        break;
                }
            }
        }

        static void Print(string sqltext, IEnumerable items)
        {
            Console.WriteLine(sqltext);
            foreach (var item in items)
                Console.WriteLine(item.ToString());

            Console.WriteLine();
            Console.ReadKey();
        }

        static void Select(VideoRentalContext db)
        {
            Read read = new Read();
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("\nВыберите пункт меню");
                Console.WriteLine("1.Вывести значение из таблицы Clientele");
                Console.WriteLine("2.Вывести значение из таблицы Disks");
                Console.WriteLine("3.Вывести значение из таблицы Genres");
                Console.WriteLine("4.Вывести значение из таблицы Positions");
                Console.WriteLine("5.Вывести значение из таблицы Pricelist");
                Console.WriteLine("6.Вывести значение из таблицы Producers");
                Console.WriteLine("7.Вывести значение из таблицы Staff");
                Console.WriteLine("8.Вывести значение из таблицы Taking");
                Console.WriteLine("9.Вывести значение из таблицы Types");
                Console.WriteLine("10.Выход из меню выбора");

                int point = int.Parse(Console.ReadLine());
                switch (point)
                {
                    case 1:
                        Print("ClientId \t Surname \t Name \t Middlename \t Addres \t Phone \t Passport",
                            read.ReadClienteles(db));
                        break;
                    case 2:
                        Print(
                            "DiskId \t Title \t CreationYear \t Producer \t MainActor \t Recording \t GenreId \t DiskType",
                            read.ReadDisks(db));
                        break;
                    case 3:
                        Print("GenreId \t Title \t Description", read.ReadGenres(db));
                        break;
                    case 4:
                        Print("PositionId \t Title", read.ReadPositions(db));
                        break;
                    case 5:
                        Print("PriceId \t DiskId \t Price", read.ReadPricelist(db));
                        break;
                    case 6:
                        Print("ProduceId \t Manufacturer \t Country", read.ReadProducers(db));
                        break;
                    case 7:
                        Print("StaffId \t Surname \t Name \t Middlename \t PositionId \t DateOfEmployment",
                            read.ReadStaff(db));
                        break;
                    case 8:
                        Print(
                            "TakeId \t ClientId \t DiskId \t DateOfCapture \t ReturnDate \t PaymentMark \t RefundMark \t StaffId",
                            read.ReadTaking(db));
                        break;
                    case 9:
                        Print("TypeId \t Title \t Description", read.ReadTypes(db));
                        break;
                    case 10:
                        isNotExit = false;
                        break;
                    default:
                        Console.WriteLine("Выбран неверный пункт меню");
                        break;
                }
            }
        }

        static void Delete(VideoRentalContext db)
        {
            Delete deleter = new Delete();
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("\nВыберите пункт меню");
                Console.WriteLine("1.Удалить значение из таблицы Clientele");
                Console.WriteLine("2.Удалить значение из таблицы Disks");
                Console.WriteLine("3.Удалить значение из таблицы Genres");
                Console.WriteLine("4.Удалить значение из таблицы Positions");
                Console.WriteLine("5.Удалить значение из таблицы Pricelist");
                Console.WriteLine("6.Удалить значение из таблицы Producer");
                Console.WriteLine("7.Удалить значение из таблицы Staff");
                Console.WriteLine("8.Удалить значение из таблицы Taking");
                Console.WriteLine("9.Удалить значение из таблицы Type");
                Console.WriteLine("10.Выход из приложения");
                int point = int.Parse(Console.ReadLine());
                int id;
                switch (point)
                {
                    case 1:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveClient(db, id);
                        break;
                    case 2:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveDisk(db, id);
                        break;
                    case 3:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveGenre(db, id);
                        break;
                    case 4:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemovePosition(db, id);
                        break;
                    case 5:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemovePrice(db, id);
                        break;
                    case 6:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveProducer(db, id);
                        break;
                    case 7:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveStaff(db, id);
                        break;
                    case 8:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveTaking(db, id);
                        break;
                    case 9:
                        Console.WriteLine("Введите ID");
                        id = int.Parse(Console.ReadLine());
                        deleter.RemoveType(db, id);
                        break;
                    case 10:
                        isNotExit = false;
                        break;
                    default:
                        Console.WriteLine("Выбран неверный пункт");
                        break;
                }
            }
        }

        static void Update(VideoRentalContext db)
        {
            Update updater = new Update();
            bool isNotExit = true;
            while (isNotExit)
            {
                Console.WriteLine("\nВыберите пункт меню");
                Console.WriteLine("1. Изменить значение в таблице Genres");
                Console.WriteLine("2. Изменить значение в таблице Clientele");
                Console.WriteLine("3. Изменить значение в таблице Disks");
                Console.WriteLine("4. Изменить значение в таблице Positions");
                Console.WriteLine("5. Изменить значение в таблице Pricelist");
                Console.WriteLine("6. Изменить значение в таблице Producer");
                Console.WriteLine("7. Изменить значение в таблице Staff");
                Console.WriteLine("8. Изменить значение в таблице Taking");
                Console.WriteLine("9. Изменить значение в таблице Type");
                Console.WriteLine("10. Выход из приложения");

                int point = int.Parse(Console.ReadLine());
                int id;

                switch (point)
                {
                    case 1:
                        Console.WriteLine("Введите id");
                        id = int.Parse(Console.ReadLine());
                        var updGenre = db.Genres.Find(id);

                        Console.WriteLine("Введите новое название:");
                        updGenre.Title = Console.ReadLine();
                        Console.WriteLine("Введите новое описание:");
                        updGenre.Description = Console.ReadLine();

                        updater.UpdateGenre(db, updGenre);
                        db.SaveChangesAsync();
                        break;

                    case 2:
                        Console.WriteLine("Введите id");
                        id = int.Parse(Console.ReadLine());
                        var updClient = db.Clienteles.Find(id);

                        Console.WriteLine("Введите новое имя:");
                        updClient.Name = Console.ReadLine();
                        Console.WriteLine("Введите новую фамилию:");
                        updClient.Surname = Console.ReadLine();
                        Console.WriteLine("Введите новое отчество:");
                        updClient.Middlename = Console.ReadLine();
                        Console.WriteLine("Введите новый адрес:");
                        updClient.Addres = Console.ReadLine();
                        Console.WriteLine("Введите новый номер телефона:");
                        updClient.Phone = Console.ReadLine();
                        Console.WriteLine("Введите новый номер паспорта:");
                        updClient.Passport = Console.ReadLine();

                        updater.UpdateClientele(db, updClient);
                        db.SaveChangesAsync();
                        break;

                    case 10:
                        isNotExit = false;
                        break;
                }
            }
        }
    }
}
