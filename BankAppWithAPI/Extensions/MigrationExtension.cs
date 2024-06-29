namespace BankAppWithAPI.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

            context.Database.Migrate();
        } 
    }
}
