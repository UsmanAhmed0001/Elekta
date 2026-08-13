namespace PetReporting
{
    public class Cat : Pet
    {
        public int? numberOfLives {get; set;}
        public override string GetExtraFields() => numberOfLives?.ToString() ?? string.Empty;
    }
}