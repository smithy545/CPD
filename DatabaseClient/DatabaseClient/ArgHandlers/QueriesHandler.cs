using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseClient.ArgHandlers
{
    public class QueriesHandler : ArgHandler
    {
        public static QueriesHandler Singleton { get; set; }

        static QueriesHandler()
        {
            Singleton = new QueriesHandler();
            Singleton.Handlers = new Dictionary<string, Func<List<string>, Task<bool>>>
            {
                {"all",  Singleton.All},
                {"evidenceforcrime",  Singleton.EvidenceForCrime},
                {"guiltysuspects", Singleton.SuspectsGuilty },
                {"numcrimeslocation", Singleton.NumCrimeLocation },
                {"help", Singleton.Help},
                {"taskforce", Singleton.CrimeOfficers }
            };
        }

        /// <summary>
        /// gets all the entities of center type
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<bool> All(List<string> args)
        {
            using (var db = new CrimesEntities())
            {
                var dbSet = await ((IEnumerable)db.GetType().GetProperty(args[0]).GetValue(db)).AsQueryable().ToListAsync();
                foreach (var value in dbSet)
                {
                    Helper.Print(value);
                }
            }

            return true;
        }

        /// <summary>
        /// finds all the evidence for a specific crime
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<bool> EvidenceForCrime(List<string> args)
        {
            using (var db = new CrimesEntities())
            {
                var crimeId = int.Parse(args[0]);
                var evidence = await db.Evidences.Where(x => x.CrimeId == crimeId).ToListAsync();
                foreach (var piece in evidence)
                {
                    Helper.Print(piece);
                }
            }

            return true;
        }

        /// <summary>
        /// finds all the suspects who have been given 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true</returns>
        public async Task<bool> SuspectsGuilty(List<string> args)
        {
            using (var db = new CrimesEntities())
            {
                var suspects =
                    await db.Suspects.Where(x => x.SuspectCrimes.Where(y => y.IsGuilty).ToList().Count > 0).ToListAsync();
                suspects.ForEach(x => Helper.Print(x));
            }
            return true;
        }

        /// <summary>
        /// finds the number of crimes in a given location
        /// </summary>
        /// <param name="args">args[0] is crime type args[1] is location</param>
        /// <returns>true</returns>
        public async Task<bool> NumCrimeLocation(List<string> args)
        {
            var crime = args[0];
            var location = args[1];
            using (var db = new CrimesEntities())
            {
                var crimes = await db.Crimes.Where(x => x.CrimeType == crime && location == x.Location).ToListAsync();
                Console.WriteLine(crimes.Count);
            }
            return true;
        }

        /// <summary>
        /// finds all the cars associated with a specific suspect
        /// </summary>
        /// <param name="args">the suspect </param>
        /// <returns>true</returns>
        public async Task<bool> SuspectCars(List<string> args)
        {
            var suspectId = int.Parse(args[0]);
            using (var db = new CrimesEntities())
            {
                var cars = await db.Vehicles.Where(x => x.SuspectId == suspectId).ToListAsync();
                cars.ForEach(x => Helper.Print(x));
            }

            return true;
        }

        /// <summary>
        /// finds all the officers in a task for for a crime
        /// </summary>
        /// <param name="args">the crime id</param>
        /// <returns>true</returns>
        public async Task<bool> CrimeOfficers(List<string> args)
        {
            var crimeId = int.Parse(args[0]);
            using (var db = new CrimesEntities())
            {
                var officerIds =
                    (await db.TaskForces.Where(x => x.CrimeId == crimeId).ToListAsync()).Select(x => x.OfficerId)
                        .ToList();
                var officers = await db.Officers.Where(x => officerIds.Contains(x.OfficerId)).ToListAsync();
                officers.ForEach(x => Helper.Print(x));
            }

            return true;
        }
    }
}
