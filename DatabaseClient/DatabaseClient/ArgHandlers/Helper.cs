using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public class Helper
    {
        public static Dictionary<Type, Func<string, object>> ParseDictionary { get; set; }

        public static Dictionary<Type, string> KeyNames { get; set; }

        static Helper()
        {
            ParseDictionary = new Dictionary<Type, Func<string, object>>
            {
                {typeof(int), (x) => int.Parse(x)},
                {typeof(decimal), (x) => decimal.Parse(x)},
                {typeof(string), (x) => (x)},
                {typeof(DateTime), (x) => DateTime.Parse(x)},
                {typeof(double), (x) => double.Parse(x)},
                {typeof(bool), (x) => bool.Parse(x)}
            };

            KeyNames = new Dictionary<Type, string>
            {
                { typeof(Crime), "CrimeId" },
                { typeof(Evidence), "EvidenceId" },
                { typeof(Investigation), "CaseId" },
                { typeof(Officer), "OfficerId" },
                { typeof(PoliceDivision), "DivisionId" },
                { typeof(SuspectCrime), "Id" },
                { typeof(Suspect), "SuspectId" },
                { typeof(TaskForce), "TaskForceId" },
                { typeof(Vehicle), "VehicleId" }
            };
        }

        public static void Print(object o)
        {
            foreach (var info in o.GetType().GetProperties())
            {
                if (IsNotNavigation(info, o.GetType()))
                {
                    Console.Write($"{info.Name}: ");
                    Console.Write(info.GetValue(o) + " ");
                }
            }
            Console.WriteLine();
        }

        public static bool IsSettable(PropertyInfo info, Type type)
        {

            return IsNotNavigation(info, type) && KeyNames[type] != info.Name;
        }

        public static bool IsNotNavigation(PropertyInfo info, Type type)
        {

            return Helper.ParseDictionary.ContainsKey(info.PropertyType) || info.PropertyType.Name.Contains("Nullable");
        }

        public static Type GetEntityType(CrimesEntities db, string typeName)
        {
            var type = db.GetType();
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                var propertyType = prop.PropertyType;
                if (propertyType.GenericTypeArguments.Length > 0)
                {
                    var genericType = propertyType.GenericTypeArguments[0];
                    if (genericType.Name == typeName)
                    {
                        return genericType;
                    }
                }
            }
            return null;
        }
    }
}
