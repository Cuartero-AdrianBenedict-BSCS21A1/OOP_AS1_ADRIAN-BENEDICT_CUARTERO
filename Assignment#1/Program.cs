using System;
using System.Collections.Generic;

public enum Kind { Dog, Cat, Lizard, Bird }
public enum Gender { Male, Female }

public abstract class Pet
{
    public Gender Gender { get; set; }
    public string Name { get; set; }
    public string Owner { get; set; }

    protected Pet(Gender gender, string name, string owner)
    {
        Gender = gender;
        Name = name;
        Owner = owner;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: Name = {Name}, Gender = {Gender}, Owner = {Owner}";
    }
}

public class Dog : Pet
{
    public string Breed { get; set; }

    public Dog(Gender gender, string name, string breed, string owner)
        : base(gender, name, owner)
    {
        Breed = breed;
    }

    public override string ToString()
    {
        return base.ToString() + $", Breed = {Breed}";
    }
}

public class Cat : Pet
{
    public bool Longhair { get; set; }

    public Cat(Gender gender, string name, bool longhair, string owner)
        : base(gender, name, owner)
    {
        Longhair = longhair;
    }

    public override string ToString()
    {
        return base.ToString() + $", Longhair = {Longhair}";
    }
}

public class Lizard : Pet
{
    public bool CanCrawl { get; set; }

    public Lizard(Gender gender, string name, bool canCrawl, string owner)
        : base(gender, name, owner)
    {
        CanCrawl = canCrawl;
    }

    public override string ToString()
    {
        return base.ToString() + $", CanCrawl = {CanCrawl}";
    }
}

public class Bird : Pet
{
    public bool WillFly { get; set; }

    public Bird(Gender gender, string name, bool willFly, string owner)
        : base(gender, name, owner)
    {
        WillFly = willFly;
    }

    public override string ToString()
    {
        return base.ToString() + $", WillFly = {WillFly}";
    }
}

public class Program
{
    private static List<Pet> pets = new List<Pet>();

    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("Welcome to the Pet shop!");
            Console.WriteLine("*******************************");
            Console.WriteLine("Please select from the choices below:");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. - Add a Pet");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("2. - List of all Pets");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("3. - Exit application");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Please choose your desired function: ");

            var choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    AddPet();
                    break;
                case "2":
                    ListAllPets();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option! Please try again.");
                    break;
            }
        }
    }

    private static void AddPet()
    {
        try
        {
            Kind kind = GetPetKind();
            Gender gender = GetGender();
            string name = GetName();
            string owner = GetOwner();

            Pet pet = CreatePet(kind, gender, name, owner);

            if (pet != null)
            {
                Console.WriteLine($"Your inputted details about the pet: \n{pet}");
                if (ConfirmAddition())
                {
                    pets.Add(pet);
                    Console.WriteLine($"The pet: {pet} has been added successfully!");
                }
                else
                {
                    Console.WriteLine("The user has cancelled.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"System error: {ex.Message}");
        }
    }

    private static void ListAllPets()
    {
        if (pets.Count == 0)
        {
            Console.WriteLine("There are no pets that are currently in the list.");
            return;
        }

        foreach (var pet in pets)
        {
            Console.WriteLine(pet);
        }
    }

    private static Kind GetPetKind()
    {
        Console.Write("Enter the kind of pet (Dog, Cat, Lizard, Bird): ");
        if (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out Kind kind))
        {
            throw new ArgumentException("Invalid pet kind. Please enter Dog, Cat, Lizard, or Bird.");
        }
        return kind;
    }

    private static Gender GetGender()
    {
        Console.Write("Enter the gender (Male, Female): ");
        if (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out Gender gender))
        {
            throw new ArgumentException("Invalid gender. Please enter Male or Female.");
        }
        return gender;
    }

    private static string GetName()
    {
        Console.Write("Enter the name of the pet: ");
        var name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Pet name cannot be empty.");
        }
        return name;
    }

    private static string GetOwner()
    {
        Console.Write("Enter the owner of the pet: ");
        var owner = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new ArgumentException("Owner cannot be empty.");
        }
        return owner;
    }

    private static Pet CreatePet(Kind kind, Gender gender, string name, string owner)
    {
        switch (kind)
        {
            case Kind.Dog:
                Console.Write("Enter the breed of the dog: ");
                var breed = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(breed))
                {
                    throw new ArgumentException("Breed cannot be empty.");
                }
                return new Dog(gender, name, breed, owner);

            case Kind.Cat:
                Console.Write("Is the cat longhaired (yes/no)? ");
                if (!TryParseYesNo(Console.ReadLine()?.Trim(), out bool isLonghaired))
                {
                    throw new ArgumentException("Invalid longhaired status. Please enter yes or no.");
                }
                return new Cat(gender, name, isLonghaired, owner);

            case Kind.Lizard:
                Console.Write("Can the lizard crawl (yes/no)? ");
                if (!TryParseYesNo(Console.ReadLine()?.Trim(), out bool canCrawl))
                {
                    throw new ArgumentException("Invalid crawl status. Please enter yes or no.");
                }
                return new Lizard(gender, name, canCrawl, owner);

            case Kind.Bird:
                Console.Write("Can the bird fly (yes/no)? ");
                if (!TryParseYesNo(Console.ReadLine()?.Trim(), out bool canFlyBird))
                {
                    throw new ArgumentException("Invalid fly status. Please enter yes or no.");
                }
                return new Bird(gender, name, canFlyBird, owner);

            default:
                throw new ArgumentException("Unexpected pet kind.");
        }
    }

    private static bool TryParseYesNo(string input, out bool result)
    {
        input = input?.Trim().ToLower();
        if (input == "yes")
        {
            result = true;
            return true;
        }
        if (input == "no")
        {
            result = false;
            return true;
        }
        result = false;
        return false;
    }

    private static bool ConfirmAddition()
    {
        Console.Write("Do you want to proceed? (y/n): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();
        return confirmation == "y";
    }
}
