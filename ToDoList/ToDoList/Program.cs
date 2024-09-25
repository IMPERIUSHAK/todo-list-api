using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapGet("/", () => "API для задач. Используйте /api/tasks для работы простмотра задач.");

//запрос и обновление данных
app.MapGet("/api/tasks", async (ApplicationContext db) =>
{

    await db.ReorderTaskId();

    return await db.Tasks.ToListAsync();
});

// Получение задачи по id
app.MapGet("/api/tasks/{id:int}", async (int id, ApplicationContext db) =>
{
    TaskStr? TaskItem = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    if (TaskItem == null) return Results.NotFound(new { message = "Задача не найдена" });

    return Results.Json(TaskItem);
});

//Удаление задачи по id
app.MapDelete("/api/tasks/{id:int}", async (int id, ApplicationContext db) =>
{
    TaskStr? TaskItem = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

    if (TaskItem == null) return Results.NotFound(new { message = "Task Not Found" });

    db.Tasks.Remove(TaskItem);
    await db.ReorderTaskId();
    await db.SaveChangesAsync();
    return Results.Json(db.Tasks);
});
//Добавление данных
app.MapPost("/api/tasks/", async (TaskStr TaskItem, ApplicationContext db) =>
{
    await db.Tasks.AddAsync(TaskItem);
    await db.SaveChangesAsync();
    return Results.Json(db.Tasks);
});
//Загрузка данных
app.MapPost("/api/tasks/load", async (List<TaskStr> tasks, ApplicationContext db) =>
{

    await db.Tasks.AddRangeAsync(tasks);
    await db.ReorderTaskId();
    await db.SaveChangesAsync();           
    return Results.Json(db.Tasks);            
});

//изменение списка дел
app.MapPut("/api/tasks/", async (TaskStr EditedTask, ApplicationContext db) =>
{

    var TaskItm = await db.Tasks.FirstOrDefaultAsync(t => t.Id == EditedTask.Id);

    if (TaskItm == null) return Results.NotFound(new { message = "Task Not Found" });
    
    //если поле для изменения списка пустой то пользователь может ее меняет только состояние
    if (!string.IsNullOrEmpty(EditedTask.Value))
    {
        TaskItm.Value = EditedTask.Value;
    }
    TaskItm.IsCompleted = EditedTask.IsCompleted;
    
    await db.SaveChangesAsync();
    return Results.Json(db.Tasks);
});

app.Run();
