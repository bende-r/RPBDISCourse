using Microsoft.EntityFrameworkCore;

using Services;

using VideoRentalModels;

using Type = VideoRentalModels.Type;

namespace VideoRentalWebApplication;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        string connection = Configuration.GetConnectionString("SQLConnection");
        services.AddDbContext<VideoRentalContext>(options => options.UseSqlServer(connection));

        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddSession();

        services.AddScoped<ICaсheDisk, CacheDisk>();
        services.AddScoped<ICacheType, CacheType>();
        services.AddScoped<ICacheClientele, CacheClientele>();
        services.AddScoped<ICacheGenre, CacheGenre>();
        services.AddScoped<ICachePosition, CachePosition>();
        services.AddScoped<ICachePricelist, CachePricelist>();
        services.AddScoped<ICacheProducer, CacheProducer>();
        services.AddScoped<ICacheStaff, CacheStaff>();
        services.AddScoped<ICacheTaking, CacheTaking>();

        services.AddRazorPages();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // добавляем поддержку статических файлов
        app.UseStaticFiles();
        // добавляем поддержку сессий
        app.UseSession();
        //Запоминание в Session значений, введенных в форме

        app.Map("/info", Info);
        app.Map("/clientele", Clientele);
        app.Map("/disks", Disks);
        app.Map("/genres", Genres);
        app.Map("/positions", Positions);
        app.Map("/pricelist", Pricelists);
        app.Map("/producers", Producers);
        app.Map("/staff", Staff);
        app.Map("/takings", Takings);
        app.Map("/types", Types);
        app.Map("/genres1", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                //обращение к сервису
                ICacheGenre cachedTanksService = context.RequestServices.GetService<ICacheGenre>();
                IEnumerable<Genre> deliveredResource = cachedTanksService.GetGenres("genre20");
                string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
                                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                                    "<BODY><H1>Список емкостей</H1>" +
                                    "<TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>Код</TH>";
                HtmlString += "<TH>Материал</TH>";
                HtmlString += "<TH>Тип</TH>";
                HtmlString += "<TH>Объем</TH>";
                HtmlString += "</TR>";
                foreach (var delivered in deliveredResource)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + delivered.Title + "</TD>";
                    HtmlString += "<TD>" + delivered.Description + "</TD>";

                    HtmlString += "</TR>";
                }

                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/tanks'>Емкости</A></BR>";
                HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                HtmlString += "</BODY></HTML>";

                // Вывод данных
                await context.Response.WriteAsync(HtmlString);
            });
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();  // Добавляем маршрутизацию для RazorPages
        });
    }

    private static void Info(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            string httpString = "<html>" +
                                "<head>" +
                                "<title>Информация о клиенте</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body align='middle'>" +
                                "<div> Сервер: " + context.Request.Host + "</div>" +
                                "<div> Путь: " + context.Request.PathBase + "</div>" +
                                "<div> Протокол: " + context.Request.Protocol + "</div>" +
                                "<div><a href='/'>Главная</a></div>" +
                                "</body>" +
                                "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Genres(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheGenre cacheGenre = context.RequestServices.GetService<ICacheGenre>();
            IEnumerable<Genre> genres = cacheGenre.GetGenres("genre20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Genres</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Genres'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Название </td>" +
                                "<td>Описание</td>" +
                                "</tr>";

            foreach (Genre genre in genres)
            {
                httpString += "<tr>";
                httpString += $"<td>{genre.GenreId}</td>";
                httpString += $"<td>{genre.Title}</td>";
                httpString += $"<td>{genre.Description}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Clientele(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheClientele cacheClientele = context.RequestServices.GetService<ICacheClientele>();
            IEnumerable<Clientele> clienteles = cacheClientele.GetClientele("clientele20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Clientele</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Clientele'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Фамилия </td>" +
                                "<td>Имя</td>" +
                                "<td>Отчество</td>" +
                                "<td>Адрес</td>" +
                                "<td>Номер телефона</td>" +
                                "<td>Номер паспорта</td>" +
                                "</tr>";

            foreach (Clientele clientele in clienteles)
            {
                httpString += "<tr>";
                httpString += $"<td>{clientele.ClientId}</td>";
                httpString += $"<td>{clientele.Surname}</td>";
                httpString += $"<td>{clientele.Name}</td>";
                httpString += $"<td>{clientele.Middlename}</td>";
                httpString += $"<td>{clientele.Addres}</td>";
                httpString += $"<td>{clientele.Phone}</td>";
                httpString += $"<td>{clientele.Passport}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";
            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Disks(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICaсheDisk caсheDisk = context.RequestServices.GetService<ICaсheDisk>();
            IEnumerable<Disk> disks = caсheDisk.GetDisks("disk20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Genres</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Genres'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Название </td>" +
                                "<td>Год выпуска</td>" +
                                "<td>Продюссер</td>" +
                                "<td>Главный актёр</td>" +
                                "<td>Дата записи </td>" +
                                "<td>Жанр </td>" +
                                "<td>Тип диска</td>" +
                                "</tr>";

            foreach (Disk disk in disks)
            {
                httpString += "<tr>";
                httpString += $"<td>{disk.DiskId}</td>";
                httpString += $"<td>{disk.Title}</td>";
                httpString += $"<td>{disk.CreationYear}</td>";
                httpString += $"<td>{disk.Producer}</td>";
                httpString += $"<td>{disk.MainActor}</td>";
                httpString += $"<td>{disk.Recording}</td>";
                httpString += $"<td>{disk.Genre}</td>";
                httpString += $"<td>{disk.DiskType}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Positions(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICachePosition cachePosition = context.RequestServices.GetService<ICachePosition>();
            IEnumerable<Position> positions = cachePosition.GetPositions("position20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Positions</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Positions'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Название </td>" +
                                "</tr>";

            foreach (Position position in positions)
            {
                httpString += "<tr>";
                httpString += $"<td>{position.PositionId}</td>";
                httpString += $"<td>{position.Title}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Pricelists(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICachePricelist caсhcaPricelist = context.RequestServices.GetService<ICachePricelist>();
            IEnumerable<Pricelist> pricelists = caсhcaPricelist.GetPricelist("pricelist20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Pricelist</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Pricelist'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Диск </td>" +
                                "<td>Цена</td>" +
                                "</tr>";

            foreach (Pricelist pricelist in pricelists)
            {
                httpString += "<tr>";
                httpString += $"<td>{pricelist.PriceId}</td>";
                httpString += $"<td>{pricelist.DiskId}</td>";
                httpString += $"<td>{pricelist.Price}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Producers(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheProducer cacheProducer = context.RequestServices.GetService<ICacheProducer>();
            IEnumerable<Producer> producers = cacheProducer.GetProducers("producer20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Producers</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Producers'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Производитель </td>" +
                                "<td>Страна</td>" +
                                "</tr>";

            foreach (Producer producer in producers)
            {
                httpString += "<tr>";
                httpString += $"<td>{producer.ProduceId}</td>";
                httpString += $"<td>{producer.Manufacturer}</td>";
                httpString += $"<td>{producer.Country}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Staff(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheStaff cacheStaff = context.RequestServices.GetService<ICacheStaff>();
            IEnumerable<Staff> staffs = cacheStaff.GetStaff("staff20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Staff</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Staff'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Фамилия </td>" +
                                "<td>Имя </td>" +
                                "<td>Отчество </td>" +
                                "<td>Должность </td>" +
                                "<td>Дата найма </td>" +
                                "</tr>";

            foreach (Staff staff in staffs)
            {
                httpString += "<tr>";
                httpString += $"<td>{staff.StaffId}</td>";
                httpString += $"<td>{staff.Surname}</td>";
                httpString += $"<td>{staff.Name}</td>";
                httpString += $"<td>{staff.Middlename}</td>";
                httpString += $"<td>{staff.Position}</td>";
                httpString += $"<td>{staff.DateOfEmployment}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Takings(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheTaking cacheTaking = context.RequestServices.GetService<ICacheTaking>();
            IEnumerable<Taking> takings = cacheTaking.GetTakings("taking20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Taking</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Taking'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Клиент </td>" +
                                "<td>Диск </td>" +
                                "<td>Дата взятия </td>" +
                                "<td>Дата возврата </td>" +
                                "<td>Отметка об оплате </td>" +
                                "<td>Отметка о возврате </td>" +
                                "<td>Сотрудник, выдавший диск </td>" +
                                "</tr>";

            foreach (Taking taking in takings)
            {
                httpString += "<tr>";
                httpString += $"<td>{taking.TakeId}</td>";
                httpString += $"<td>{taking.ClientId}</td>";
                httpString += $"<td>{taking.DiskId}</td>";
                httpString += $"<td>{taking.DateOfCapture}</td>";
                httpString += $"<td>{taking.ReturnDate}</td>";
                httpString += $"<td>{taking.PaymentMark}</td>";
                httpString += $"<td>{taking.RefundMark}</td>";
                httpString += $"<td>{taking.StaffId}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }

    private static void Types(IApplicationBuilder app)
    {
        app.Run(async (context) =>
        {
            ICacheType cacheType = context.RequestServices.GetService<ICacheType>();
            IEnumerable<Type> types = cacheType.GetTypes("type20");

            string httpString = "<html>" +
                                "<head>" +
                                "<title>Таблица Types</title>" +
                                "<style>" +
                                "div { font-size: 24; }" +
                                "table { font-size: 20; }" +
                                "</style>" +
                                "</head>" +
                                "<meta charset='utf-8'/>" +
                                "<body>" +
                                "<div align='center'>Таблица 'Typess'</div>" +
                                "<div align='center'>" +
                                "<table border=1>" +
                                "<tr>" +
                                "<td>ID </td>" +
                                "<td>Название </td>" +
                                "<td>Описание </td>" +
                                "</tr>";

            foreach (Type type in types)
            {
                httpString += "<tr>";
                httpString += $"<td>{type.TypeId}</td>";
                httpString += $"<td>{type.Title}</td>";
                httpString += $"<td>{type.Description}</td>";
                httpString += "</tr>";
            }

            httpString += "</table>";

            httpString += "<div align='center'><a href='/'>Главная</a></div>";
            httpString += "</body>" +
                          "</html>";

            await context.Response.WriteAsync(httpString);
        });
    }
}