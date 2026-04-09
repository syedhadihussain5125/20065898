using System;
using System.Collections.Generic;

internal sealed record Element(int AtomicNumber, string Name, string Class, string Info);

internal static class Program
{
    private static readonly Dictionary<int, Element> Elements = new()
    {
        [1] = new Element(1, "Hydrogen", "Nonmetal", "Hydrogen is the lightest element and is widely used in fuel cells and ammonia production."),
        [2] = new Element(2, "Helium", "Noble Gas", "Helium is an inert gas used in balloons, cryogenics, and scientific instruments."),
        [3] = new Element(3, "Lithium", "Alkali Metal", "Lithium is a soft metal best known for use in rechargeable batteries."),
        [4] = new Element(4, "Beryllium", "Alkaline Earth Metal", "Beryllium is a lightweight metal used in aerospace parts and precision tools."),
        [5] = new Element(5, "Boron", "Metalloid", "Boron is used in glass, detergents, and as a dopant in semiconductors."),
        [6] = new Element(6, "Carbon", "Nonmetal", "Carbon forms the basis of organic life and appears as graphite and diamond."),
        [7] = new Element(7, "Nitrogen", "Nonmetal", "Nitrogen makes up most of Earth's atmosphere and is vital for proteins and DNA."),
        [8] = new Element(8, "Oxygen", "Nonmetal", "Oxygen supports respiration and combustion and is essential for life."),
        [9] = new Element(9, "Fluorine", "Halogen", "Fluorine is highly reactive and used in toothpaste compounds and industrial chemicals."),
        [10] = new Element(10, "Neon", "Noble Gas", "Neon emits a bright glow in electric discharge tubes and signage."),
        [11] = new Element(11, "Sodium", "Alkali Metal", "Sodium is a reactive metal commonly found in table salt compounds."),
        [12] = new Element(12, "Magnesium", "Alkaline Earth Metal", "Magnesium is important in alloys, medicine, and chlorophyll."),
        [13] = new Element(13, "Aluminium", "Post-transition Metal", "Aluminium is lightweight, corrosion-resistant, and used in transport and packaging."),
        [14] = new Element(14, "Silicon", "Metalloid", "Silicon is key to electronics and also abundant in sand and rocks."),
        [15] = new Element(15, "Phosphorus", "Nonmetal", "Phosphorus is essential in DNA, ATP, and fertilizers."),
        [16] = new Element(16, "Sulfur", "Nonmetal", "Sulfur is used in sulfuric acid production and occurs in proteins."),
        [17] = new Element(17, "Chlorine", "Halogen", "Chlorine is used in water treatment, disinfectants, and PVC production."),
        [18] = new Element(18, "Argon", "Noble Gas", "Argon is an inert gas used in lighting and welding atmospheres."),
        [19] = new Element(19, "Potassium", "Alkali Metal", "Potassium is essential for nerve function and found in many minerals."),
        [20] = new Element(20, "Calcium", "Alkaline Earth Metal", "Calcium is important for bones, teeth, and many industrial compounds."),
        [21] = new Element(21, "Scandium", "Transition Metal", "Scandium is used in high-performance alloys and certain lighting applications."),
        [22] = new Element(22, "Titanium", "Transition Metal", "Titanium is strong, light, and highly corrosion-resistant."),
        [23] = new Element(23, "Vanadium", "Transition Metal", "Vanadium improves steel strength and is used in specialty alloys."),
        [24] = new Element(24, "Chromium", "Transition Metal", "Chromium provides corrosion resistance and shine in stainless steel and plating."),
        [25] = new Element(25, "Manganese", "Transition Metal", "Manganese is important in steel production and battery chemistry."),
        [26] = new Element(26, "Iron", "Transition Metal", "Iron is a major structural metal and central in hemoglobin."),
        [27] = new Element(27, "Cobalt", "Transition Metal", "Cobalt is used in magnets, alloys, and lithium-ion battery cathodes."),
        [28] = new Element(28, "Nickel", "Transition Metal", "Nickel is used in alloys, stainless steel, and rechargeable batteries."),
        [29] = new Element(29, "Copper", "Transition Metal", "Copper is an excellent electrical conductor used in wiring and electronics."),
        [30] = new Element(30, "Zinc", "Transition Metal", "Zinc is used for galvanizing steel and in biological enzyme functions.")
    };

    private static void Main()
    {
        Console.WriteLine("Hi there! Happy to help!");

        while (true)
        {
            Console.Write("Provide atomic number of the element: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out var atomicNumber))
            {
                Console.WriteLine("Please provide a valid number from 1 to 30.");
                continue;
            }

            if (!Elements.TryGetValue(atomicNumber, out var element))
            {
                Console.WriteLine("Element data is available only for atomic numbers 1 to 30.");
            }
            else
            {
                Console.WriteLine($"Atomic Number: {element.AtomicNumber}");
                Console.WriteLine($"Name: {element.Name}");
                Console.WriteLine($"Class: {element.Class} ({element.Info})");
            }

            Console.Write("Do you want to know more elements [y/n]? ");
            var answer = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

            if (answer == "n")
            {
                Console.WriteLine("Thanks !");
                break;
            }

            if (answer != "y")
            {
                Console.WriteLine("Invalid choice. Continuing by default.");
            }
        }
    }
}