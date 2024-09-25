using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<TaskStr> Tasks { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated(); 
    }

    public async Task ReorderTaskId()
    {
        var tasks = await Tasks.OrderBy(t => t.Position).ToListAsync();
        for (int i = 0; i < tasks.Count; i++)
        {
            tasks[i].Position = i + 1;
        }
   
        await SaveChangesAsync();
    }


}
