//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder();

//// получаем строку подключения из файла конфигурации
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//// добавляем контекст ApplicationContext в качестве сервиса в приложение
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

//var app = builder.Build();


//app.Run();