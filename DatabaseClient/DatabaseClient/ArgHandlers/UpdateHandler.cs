using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public class UpdateHandler : ArgHandler
    {
        public static UpdateHandler Singleton { get; set; }
        static UpdateHandler()
        {
            Singleton = new UpdateHandler();
        }

        public async Task<bool> Handle(List<string> args)
        {
            using (var db = new CrimesEntities())
            {
                var typeName = args[0];
                var entityType = Helper.GetEntityType(db, typeName);
                var entities =  await ((IEnumerable)db.GetType().GetProperty(args[0] + "s").GetValue(db)).AsQueryable().ToListAsync();
                var propertyInfo = entityType.GetProperties().First(x => x.Name == Helper.KeyNames[entityType]);

                var entity = entities.FirstOrDefault(x => (int) propertyInfo.GetValue(x) == int.Parse(args[1]));
                if (entity == null)
                {
                    Console.WriteLine("No entity in the specified table with that id found");
                    return true;
                }
                Console.WriteLine("Enter values when prompted");
                foreach (var prop in entity.GetType().GetProperties())
                {
                    Type currentPropType;
                    if (prop.PropertyType.Name.Contains("Nullable"))
                    {
                        currentPropType = prop.PropertyType.GetGenericArguments()[0];
                    }
                    else
                    {
                        currentPropType = prop.PropertyType;
                    }
                    if (Helper.IsSettable(prop, entityType))
                    {
                        Console.Write($"{prop.Name} (previous value = {prop.GetValue(entity)}): ");
                        var entry = Console.ReadLine();
                        object value;
                        try
                        {
                            value = Helper.ParseDictionary[currentPropType].Invoke(entry);
                        }
                        catch (Exception)
                        {
                            value = null;
                        }
                        prop.SetValue(entity, value);
                        Console.WriteLine();
                    }
                }

                try
                {
                    await db.SaveChangesAsync();
                    Console.WriteLine("Successfully updated database");
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to update database. Please reenter data");
                }
            }
            return true;
        }
    }
}
