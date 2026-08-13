namespace PetReporting
{
    public abstract class Pet
    {
        public string Name {get; set;} = string.Empty;
        public Owner Owner {get; set; } = new Owner();
        public int NumberOfVisits {get; set;}
        public DateTime JoinedPractice {get; set;}
        public double CostPerVisit{ get; set; }

        public virtual string GetExtraFields() => string.Empty;
    }
}