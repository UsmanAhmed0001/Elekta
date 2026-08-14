# Changes Made and Why

## 1. Domain Model —> Pet : Owner
`Pet` inherited from `Owner`, violating the is-a rule of inheritance. A Pet is not an Owner.

```csharp
// Before
public class Pet : Owner { }

// After
public abstract class Pet
{
    public Owner Owner { get; set; } = new Owner();
}
```

***Why?*** Composition should be favoured over inheritance. A pet HAS an owner, it is not an owner.

---

## 2. Made Pet Abstract
`Pet` could be instantiated directly with `new Pet()`. A plain pet does not exist in real life.

```csharp
// Before
public class Pet : Owner { }

// After
public abstract class Pet { }
```

***Why?*** Correct OOP design which enforces that only specific concrete types of pets can be created.

---

## 3. Separate Classes —> Separation of Concerns
All classes were into a single `Program.cs`. Now Split into `Owner.cs`, `Pet.cs`, `Dog.cs`, `Cat.cs`, `PetReportWriter.cs`.  
***Why?*** A reviewer scanning the project can understand the structure instantly without opening any files.

---

## 4. Single Responsibility Principle —> PetReportWriter
`printReport` was living on `Pet`, giving it three jobs: model an animal, format CSV, write to disk.

```csharp
// Before — report method sitting on the domain class
public class Pet : Owner
{
    public void printReport(IEnumerable<Pet> pets, string filename) { }
}

// After — dedicated class with one job
public class PetReportWriter
{
    public void WriteReport(IEnumerable<Pet> pets, string filename) { }
}
```

***Why?*** If the report format changes, only `PetReportWriter` is touched. `Pet` is never touched.

---

## 5. Open/Closed Principle —> Replaced `is Cat` Check
Adding a new animal type required editing `printReport`, violating the Open/Closed Principle.

```csharp
// Before — must edit this every time a new animal is added
if (p is Cat)
{
    var cat = p as Cat;
    entry += "," + cat.numberOfLives;
}

// After — each pet answers for itself
// Pet.cs
public virtual string GetExtraFields() => string.Empty;

// Cat.cs
public override string GetExtraFields() =>
    NumberOfLives?.ToString() ?? string.Empty;

// PetReportWriter.cs
entry += "," + pet.GetExtraFields();
```

***Why?*** Open for extension, closed for modification. Adding a new animal requires only a new class, zero changes to `PetReportWriter`.

---

## 6. Encapsulating Public Fields —> Properties
All data was exposed as public fields with inconsistent naming.

```csharp
// Before
public string Firstname;
public int numberofVisits;

// After
public string FirstName { get; set; } = string.Empty;
public int NumberOfVisits { get; set; }
```

***Why?*** Properties support encapsulation.

---

## 7. DRY Principle
`CostPerVisit` was declared in both `Dog` and `Cat`.

```csharp
// Before — duplicated in two places
public class Dog : Pet { public double CostPerVisit; }
public class Cat : Pet { public double CostPerVisit; }

// After — declared once on the parent
public abstract class Pet
{
    public double CostPerVisit { get; set; }
}
```

***Why?*** Don't Repeat Yourself. Moved to `Pet` so it is declared exactly once. Duplicated code means duplicated bugs.

---

## 8. Fixed CSV Defects
- Missing comma between owner name and date produced malformed rows.
- `DateTime.ToString()` produces different output on different machines (UK vs US locale).

```csharp
// Before — no comma, culture-dependent date
var entry = string.Join(" ", p.Firstname, p.Lastname) + p.joinedPractice + "," + p.numberofVisits;

// After — fixed delimiter, fixed format
var entry = $"{pet.Owner.FirstName} {pet.Owner.LastName}," +
            $"{pet.JoinedPractice:yyyy-MM-dd}," +
            $"{pet.NumberOfVisits}," +
            $"{pet.GetExtraFields()}";
```

***Why?*** The CSV now produces identical, correctly delimited output on every machine regardless of locale.

---

## Suggestions Given More Time

- **Split into two projects** — `src/PetReporting` (class library) and `tests/PetReporting.Tests` (test project). Production code should not live in a test project.
- **More test cases** — empty pet list, pet without an owner name, CSV field escaping (e.g. owner named `"Lucy, Jr"` would break the CSV).
- **CostPerVisit** — declared but never used anywhere. Need to clarify the reason with the team — dead code is a maintenance burden.
- **Improve test assertions** — replace the single line count check with focused, descriptively named tests:
```csharp
  WriteReport_CreatesCorrectNumberOfLines
  WriteReport_FirstLineIsHeader
  WriteReport_DogLineHasNoNumberOfLives
  WriteReport_CatLineIncludesNumberOfLives
```