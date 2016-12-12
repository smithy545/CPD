using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public class CreateHandler : ArgHandler
    {
        public static CreateHandler Singleton { get; set; }

        static CreateHandler()
        {
            Singleton = new CreateHandler();
            var dic = new Dictionary<string, Func<List<string>, Task<bool>>>();
            Singleton.Handlers = dic;
            dic.Add("help", Help);
            dic.Add("init", Singleton.Create);
        }

        public static async Task<bool> Help(List<string> args)
        {
            using (var db = new CrimesEntities())
            {
                Console.WriteLine("List of valid types to create");
                foreach (var prop in db.GetType().GetProperties())
                {
                    if (prop.PropertyType.Name.Contains("DbSet"))
                    {
                        Console.WriteLine(prop.PropertyType.GetGenericArguments()[0].Name);
                    }
                }
            }
            return true;
        }

        public async Task<bool> Create(List<string> args)
        {
            var typeName = args[0];
            Type entityType = null;
            using (var db = new CrimesEntities())
            {
                entityType = Helper.GetEntityType(db, typeName);

                if (entityType == null)
                {
                    Console.WriteLine("Invalid type name");
                    return await Help(args);
                }

                var entity = entityType.GetConstructor(new Type[0]).Invoke(new object[0]);
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
                        Console.Write($"{prop.Name}: ");
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
                var addTo = db.GetType().GetProperty($"{entityType.Name}s");
                var dbSet = addTo.GetValue(db);
                var method = dbSet.GetType().GetMethod("Add");
                method.Invoke(dbSet, new [] {entity});
                try
                {
                    await db.SaveChangesAsync();
                    var keyName = Helper.KeyNames[entityType];
                    var newId = (int) entity.GetType().GetProperty(keyName).GetValue(entity);
                    Console.WriteLine($"Successfully initialized new {typeName} and {keyName} of {newId}");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error saving to database. Please try again");
                }

            }
            return true;
        }
    }
}
