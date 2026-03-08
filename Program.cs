using System.ComponentModel.Design;
using System.Collections.Generic; // Tillader lister

namespace KontorNord
{
	internal class Program
	{
		static void Main(string[] args) // Jeg har lagt dette While-Loop i Main, da Main skal kunne styre hele programmet inde i sig selv. Kig på det som programmets "liv" eller livscyklus"
		{

			bool isRunning = true;

			while (isRunning)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("===== Welcome, {workerID} ====="); // Det første man ser efter log-in - Indsæt $ når workerID er sat op (interpolation)
				Console.ResetColor();
				Console.WriteLine(""); // Lidt spacing


				showMenu(); // Kalder Menu-Metode, der viser Menu Options
				int input;

				while (!int.TryParse(Console.ReadLine(), out input))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				switch (input) // Hvis brugeren trykker på en tast (1-4), så kører/kalder vi på den givne metode - While-Loop sikrer gentagelse så længe isRunning = true;. Switch vælger metoden - While-Loop styrer gentagelsen
				{
					case 1:
						showMeetings();
						break;

					case 2:
						bookMeetings();
						break;

					case 3:
						cancelMeetings();
						break;

					case 4:
						isRunning = false;
						/*     exitProgram(); Hvis vi beslutter os for at benytte exitProgram-Metoden nederst i Programmet */
						break;

					default:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Please input a valid number (1-4)");
						Console.ResetColor();
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

			Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

		}

		static void showMeetings() // Metoden, der viser kalendervisning
		{
			Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

			Console.WriteLine("Stuff is coming... soonTM");
			Console.WriteLine("");
			Console.WriteLine("");

			Console.ForegroundColor = ConsoleColor.Blue;
			returnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu (showMenu)
			Console.ResetColor(); // Ikke nødvendig, men god skik
		}

		static void bookMeetings() // Metoden, der tillader brugeren at booke møder
		{
			Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

			string day = SelectDay();

			string startTime = SelectStartTime();

			string endTime = SelectEndTime(startTime);

		    List<string> participants = AddParticipants();

			string note = AddNote();

			MeetingConfirmation(day, startTime, endTime, participants, note);

			// I slutningen skal jeg lave en: "Møde er nu booked"
			// Kalder så: MødeBekræftelse()

			Console.ForegroundColor = ConsoleColor.Blue;
			returnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu (showMenu)
			Console.ResetColor(); // Ikke nødvendig, men god skik

		}

		static string SelectDay() // Medetoder, der hører til en anden metode. Her vælger brugeren hvilken dag, de vil book et møde på
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================");
            Console.WriteLine("       BOOK ET MØDE");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");
			Console.WriteLine("Vælg en dag:");
			Console.WriteLine("1) Mandag");
			Console.WriteLine("2) Tirsdag");
			Console.WriteLine("3) Onsdag");
			Console.WriteLine("4) Torsdag");
			Console.WriteLine("5) Fredag");
            Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

			int valg; // Sikrer, at hvis brugen IKKE taster et tal(int), så får brugeren en fejl besked - C# prøver at konvetere string til int - Lokal Variable til metoden
			while (!int.TryParse(Console.ReadLine(), out valg)) // Out = At tasten bliver sendt ud i variablen: "valg"
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Indtast venligst et tal:");
				Console.ResetColor();
			}

			switch (valg) // Her bruger jeg ikke "break", da jeg har "return" i min metode. "Return", får koden til at stoppe automatisk (brugte switch, da det var min egen logik)
			{
				case 1: return "Mandag";
				case 2: return "Tirsdag";
				case 3: return "Onsdag";
				case 4: return "Torsdag";
				case 5: return "Fredag";
				default: return "Mandag";
			}
		}


		static string SelectStartTime()
		{
			Console.Clear();

			List<string> tider = new List<string>(); // Benytter en liste for at undgå 18 forskellige cases i en switch

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("============================");
			Console.WriteLine("       STARTTIDSPUNKT");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");

			int mulighed = 1; // Mulighed, altså sætter tal foran mulighederne 1-xx

			for (int tid = 8; tid <= 17; tid++) // For Loop, der kan generate forskellige tidspunkter
			{
				Console.WriteLine($"{mulighed}) {tid:D2}:00"); // :D2 = vi får vist 2 cifre. Så i stedet for 8:00, får brugeren vist 08:00 etc etc
				tider.Add($"{tid:D2}:00");
				mulighed++; // Plusser med 1 for hver mulighed (en tæller)

				if (tid != 17) // Stopper loopet ved 17:00 (dagen slutter, så brugeren får ikke vist 17:30++)
				{
					Console.WriteLine($"{mulighed}) {tid:D2}:30");
					tider.Add($"{tid:D2}:30");
					mulighed++; // Plusser med 1 for hver mulighed (en tæller)
				}
			}

            Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

			int valg; // Sikrer, at hvis brugen IKKE taster et tal(int), så får brugeren en fejl besked - C# prøver at konvetere string til int - Lokal Variable til metoden
			while (!int.TryParse(Console.ReadLine(), out valg))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Indtast venligst et tal: ");
				Console.ResetColor();
			}
			

			return tider[valg - 1]; // Returnerer tiden fra listen (minus 1, da computer(index) starter fra 0 (tak Isa <3)

		}

		static string SelectEndTime(string startTime) // Basically en kopi af ovenstående metode, men bare som slut-tidspunkt i stedet. Vil gemme som StartTime - EndTime
		{
			Console.Clear();

			List<string> tider = new List<string>();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("============================");
			Console.WriteLine("       SLUTTIDSPUNKT");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");

			int mulighed = 1;

			for (int tid = 8; tid <= 17; tid++)
			{
				tider.Add($"{tid:D2}:00");

				if (tid != 17)
				{
					tider.Add($"{tid:D2}:30");
				}
			}

			int startIndex = tider.IndexOf(startTime);

			for (int i = startIndex + 1; i < tider.Count; i++)
			{
				Console.WriteLine($"{i - startIndex}) {tider[i]}");
			}

			Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

			int valg; // Sikrer, at hvis brugen IKKE taster et tal(int), så får brugeren en fejl besked - C# prøver at konvetere string til int - Lokal Variable til metoden
			while (!int.TryParse(Console.ReadLine(), out valg))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Indtast venligst et tal: ");
				Console.ResetColor();
			}

			return tider[startIndex + valg];
		}


		static List<string> AddParticipants()
		{
			Console.Clear();

			List<string> participants = new List<string>();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("============================");
			Console.WriteLine("       VÆLG DELTAGER(E)");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");

			List<string> employees = new List<string>
		{
			"1) Sofie Møller (SM)",
			"2) Jonas (JO)",
			"3) Amir Rahimi (AR)",
			"4) Louise Falk (LF)",
			"5) Mette Ates (MA)",
			"6) Henrik Krøll (HK)",
		};

			char svar = 'j';

			while (svar == 'j')
			{
				foreach (string employee in employees)
				{
					Console.WriteLine(employee);
				}

				Console.WriteLine("");
				Console.Write("Vælg en ansat og afslut med <Enter>: ");

				int valg;

				while (!int.TryParse(Console.ReadLine(), out valg))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				participants.Add(employees[valg - 1]);

				Console.WriteLine("");
				Console.Write("Vil du vælge flere ansatte til mødet? --- Svar: j/n: ");
				svar = Console.ReadKey().KeyChar;

				Console.WriteLine("");
			}

			Console.WriteLine("Du har nu valgt deltager(e) til mødet:");

			foreach (var participant in participants)
			{
				Console.WriteLine(participant);
			}

			return participants;
		}

		static string AddNote()
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("============================");
			Console.WriteLine("        TILFØJ NOTE");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");

			Console.Write("Skriv note: ");
			string note = Console.ReadLine();

			return note;
		}

		static void MeetingConfirmation(string day, string startTime, string endTime, List<string> participants, string note) // Denne metode kalder al information fra tidligere metoder
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("============================");
			Console.WriteLine("    DIT MØDE ER NU BOOKET");
			Console.WriteLine("============================");
			Console.ResetColor();
			Console.WriteLine("");

			Console.WriteLine("");
			Console.WriteLine($"Dag: {day}");
			Console.WriteLine("");
			Console.WriteLine($"Tid: {startTime} - {endTime}");

			Console.WriteLine("");
			Console.WriteLine("Deltagere:");

			foreach (string participant in participants)
			{
				Console.WriteLine($"{participant}");
			}

			Console.WriteLine("");

			if (!string.IsNullOrWhiteSpace(note))
			{
				Console.WriteLine($"Note: {note}");
			}

			Console.WriteLine("");
			Console.WriteLine("Mødet er nu registreret i systemet.");
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
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
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