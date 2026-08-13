namespace PetReporting
{
    public class PetReportWriter
    {
        public void WriteReport(IEnumerable<Pet> pets, string filename)
        {
            var entries = new List<string>();
            entries.Add("Owner Name, Date Joined Practice, Number of Visits, Number Of Lives");

            foreach(var pet in pets)
            {
                var entry = $"{pet.Owner.FirstName} {pet.Owner.LastName}," +
                            $"{pet.JoinedPractice:yyyy-MM-dd}," +
                            $"{pet.NumberOfVisits},"+
                            $"{pet.GetExtraFields()}";
                
                entries.Add(entry);
            }

            File.WriteAllLines(filename, entries);
        }
    }
}