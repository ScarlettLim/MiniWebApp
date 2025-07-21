using WebApp1.Models;

namespace WebApp1.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Projects.Any())
            {
                var projects = new Project[]
                {
                new Project{Name="Project A"},
                new Project{Name="Project B"},
                };

                foreach (var p in projects)
                {
                    context.Projects.Add(p);
                }
                context.SaveChanges();

                var tasks = new ProjectTask[]
                {
                new ProjectTask{Name="Task 1", ProjectId=1},
                new ProjectTask{Name="Task 2", ProjectId=1},
                new ProjectTask{Name="Task 3", ProjectId=2},
                new ProjectTask{Name="Task 4", ProjectId=2},
                };

                foreach (var t in tasks)
                {
                    context.ProjectTasks.Add(t);
                }
                context.SaveChanges();
            }
        }
    }
}
