using System.ComponentModel.Design;

namespace KontorNord
{
    internal class Program
    {
        static void Main(string[] args)
        {

            bool isRunning = true;

            while (isRunning)
            {
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("===== Welcome, {workerID} ====="); // Det første man ser efter log-in - Indsæt $ når workerID er sat op
				Console.ResetColor();
				Console.WriteLine(""); // Lidt spacing


				showMenu(); // Kalder Menu-Metode, der viser Menu Options


				string input = Console.ReadKey(true).KeyChar.ToString(); // Denne konvetering betyder, at consollen venter på én tast, og derefter konvetere den tallet til string > Brug den i nedenstående Switch
				switch (input) // Hvis brugeren trykker på en tast (1-4), så kører/kalder vi på den givne metode - While-Loop sikrer gentagelse så længe isRunning = true;. Switch vælger metoden - While-Loop styrer gentagelsen
				{
					case "1":
						showMeetings();
						break;

					case "2":
						bookMeetings();
						break;

					case "3":
						cancelMeetings();
                        break;

                    case "4":
                        isRunning = false;
                        /*     exitProgram(); Hvis vi beslutter os for at benytte exitProgram-Metoden nederst i Programmet */
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please input a valid number (1-4)");
                        Console.ReadKey(true); // Betyder: "Hvis ikke denne tast i consollen"
                        Console.Clear(); // Rydder fejl-beskeden ved fejl-input
                        break;
				}
			}
        }

        static void showMenu() // Metoden, der viser vores menu efter log-in
        {
			Console.WriteLine("1) Show current, past and/or future meetings via our Calendar");
            Console.WriteLine("2) Book a meeting");
            Console.WriteLine("3) Cancel a meeting");
            Console.WriteLine("4) Exit");
        }

        static void showMeetings() // Metoden, der viser kalendervisning
        {
            Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

            Console.WriteLine("Menu is coming... soonTM");
            Console.WriteLine("");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Blue;
            returnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu (showMenu)
            Console.ResetColor(); // Ikke nødvendig, men god skik
		}

        static void bookMeetings() // Metoden, der tillader brugeren at booke møder
        {
			Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

			Console.WriteLine("You can book meetings... soonTM");
			Console.WriteLine("");
			Console.WriteLine("");

			Console.ForegroundColor = ConsoleColor.Blue;
			returnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu (showMenu)
			Console.ResetColor(); // Ikke nødvendig, men god skik

		}

        static void cancelMeetings() // Metoden, der tillader brugeren at aflyse møder
        {
			Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

			Console.WriteLine("You can cancel meetings... soonTM");
			Console.WriteLine("");
			Console.WriteLine("");

			Console.ForegroundColor = ConsoleColor.Blue;
			returnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu (showMenu)
			Console.ResetColor(); // Ikke nødvendig, men god skik
		}

        static void returnToMenu() // Return-Metode, der bliver kaldt i andre menu-metoder, så brugeren kan komme tilbage til Menu (showMenu)
        {
            Console.WriteLine();
            Console.WriteLine("Press 'M' to return to menu");

            while (true)
            {
                var key = Console.ReadKey(true).KeyChar; // Var, da C# sagtens selv kan aflæse en nem 'char'

                if (char.ToUpper(key) == 'M') // ToUpper - "Fejlsikring" i fald af, at brugeren ikke skriver stort 'M' (Også kaldet Case-Insensitive)
                {
                    Console.Clear();
                    break; // Ligesome switch, så siger dette break: "Hey, stop loopet", og sender os ud af loopet
                }
                else
                {
                    Console.WriteLine("Wrong key Dawg - Press 'M'");
                }
            }
        }

      /*  static void exitProgram() // Det her er lidt ringe, men den virker. Dog, foretrækker jeg isRunning = false i Case "4"
        {
            Environment.Exit(0); // Exit the program
        }
      */
       
        
    }
}
