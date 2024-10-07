using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Homework_SysPr_6._1
{
    public class DatabaseUser
    {
        private DbContextOptions<ApplicationContext> GetConnectionOptions()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            return optionsBuilder.UseSqlServer(connectionString).Options;
        }

        public void EnsurePopulate()
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                List<User> users = new List<User>
                {
                    new User { Name = "Alex", Email = "alex@mail.com"},
                    new User { Name = "Alice", Email = "alice@mail.com"},
                    new User { Name = "Bob", Email = "bob@mail.com" }
                };
                db.Users.AddRange(users);
                db.SaveChanges();
            }
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using (ApplicationContext db = new ApplicationContext(GetConnectionOptions()))
            {
               return await db.Users.ToListAsync();
            }
        }
        
        
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

    }
    

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return String.Format("Id - {0}\nName - {1}\nEmail - {2}\n", Id, Name, Email);
        }
    }

    public class Program
    {
        static async Task Main()
        {
            //Добавить коллекцию пользователь в базе данных и после считать всех пользователей из таблицы в коллекцию, 
            //используя асинхронные методы.Работать с базой данных можно через Ado.Net или EF.


            DatabaseUser db = new DatabaseUser();
            //db.EnsurePopulate();
            var allUsers = await db.GetAllUsersAsync();
            foreach (var user in allUsers)
            {
                //Console.WriteLine(user.Id);
                //Console.WriteLine(user.Name);
                //Console.WriteLine(user.Email);
                Console.WriteLine(user.ToString());
                Console.WriteLine();
            }
           
            
            Console.ReadLine();

        }

    }

    
}
