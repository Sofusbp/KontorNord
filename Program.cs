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
						break;

					default:
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Please input a valid number (1-4)");
						Console.ResetColor();
						Console.ReadKey(true); // Betyder: "Hvis ikke denne tast i consollen" -- Altså hvis brugeren taster "5" f.eks.
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

		static void bookMeetings() // Metoden, der tillader brugeren at booke møder -- Kig på den, som vores "Controller", der styrer hele booking processen
		{
			Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

			string day = SelectDay(); // Brugerens valgte dag retuneres som en String og gemmes i variablen 'day'

			MødeLokaler lokale = SelectRoom(); // Metonden returnerer et objekt fra MødeLokaler-Klassen og gemmes i variablen 'lokale'

			string startTime = SelectStartTime(); // Brugerens valgte starttidspunkt returneres som en String og gemmes i variablen 'startTime'

			string endTime = SelectEndTime(startTime); // Starttidspunkt bliver her sendt som parameter, så brugeren KUN kan vælge tider efter valgte 'startTime' -- Brugerens valgte sluttidspunkt returneres som en String og gemmes i variablen 'endTime'

		    List<string> participants = AddParticipants(); // Deltager Liste (taget fra Casen)

			string note = AddNote(); // Metode, der tillader noter til møder

			MeetingConfirmation(day, lokale, startTime, endTime, participants, note); // Metoder, der har alle variabler gemt fra tidligere metoder i flowet

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

			int valg; 
			while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > 5) // Out = At tasten bliver sendt ud i variablen: "valg"
																						// Sikrer, at hvis brugen IKKE taster et tal(int), så får brugeren en fejl besked - C# prøver at konvetere string til int - Lokal Variable til metoden
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Indtast venligst et tal: ");
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

		static MødeLokaler SelectRoom()
		{
			Console.Clear();

			List<MødeLokaler> rooms = new List<MødeLokaler> // Hvert lokale oprettes som et objekt i klassen MødeLokaler
		{
			new MødeLokaler("Hytten"),
			new MødeLokaler("Kælderen"),
			new MødeLokaler("Fulgeburet"),
			new MødeLokaler("Grotten")
		};

			Console.WriteLine("Vælg mødelokale:");
			Console.WriteLine("");

			for (int i = 0; i < rooms.Count; i++)
			{
				Console.WriteLine($"{i + 1}) {rooms[i].Name}");
			}
			// Viser hvert mødelokale i konsollen.
			// i + 1 bruges for at nummereringen starter ved 1 i stedet for 0 (som ellers er standard i arrays/lister).
			// rooms[i] henter objektet på position i i listen.
			// .Name henter navnet på mødelokalet fra objektet.

			Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

			int valg; // Variabel der gemmer brugerens valg

			while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > rooms.Count)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"Indtast et tal mellem 1 og {rooms.Count}: ");
				Console.ResetColor();
			}
			// Programmet forsøger at konvertere brugerens input til et helt tal ved hjælp af TryParse
			// Hvis input ikke er et tal, eller hvis tallet ligger uden for intervallet af gyldige lokaler
			// vil betingelsen være sand, og brugeren bliver bedt om at indtaste et nyt tal
			// Loopet fortsætter derfor indtil brugeren indtaster et gyldigt tal mellem 1 og antallet af lokaler

			return rooms[valg - 1];
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
			while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > tider.Count)
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

			int mulighed = 1; // Variabel der bruges til nummerering af de tider der vises til brugeren

			for (int tid = 8; tid <= 17; tid++) // For-loop der genererer alle mulige tidspunkter mellem kl. 08:00 og 17:00
			{
				tider.Add($"{tid:D2}:00"); // Tilføjer hele klokkeslæt (fx 08:00, 09:00 osv) til listen -- :D2 sikrer at tallet altid vises med to cifre

				if (tid != 17) // Hvis timen ikke er 17, tilføjes også et halvt tidspunkt -- '!' Operatør
				{
					tider.Add($"{tid:D2}:30"); // Tilføjer halvtime (fx 08:30)
				}
			}

			int startIndex = tider.IndexOf(startTime); // Finder positionen i listen hvor det valgte starttidspunkt ligger

			for (int i = startIndex + 1; i < tider.Count; i++) // For-loop der viser alle tider efter starttidspunktet --  Loopet starter derfor ved startIndex + 1
			{
				Console.WriteLine($"{i - startIndex}) {tider[i]}"); // Viser sluttiderne for brugeren
			}

			int max = tider.Count - startIndex - 1; // Beregner hvor mange mulige sluttider der findes efter starttidspunktet

			Console.WriteLine("");
			Console.Write("Vælg nummer og afslut med <Enter>: ");

			int valg; // Variabel der gemmer brugerens valg

			while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > max) // Loopet fortsætter indtil brugeren indtaster et tal der ligger indenfor intervallet af de viste sluttider
																						  // Efter inputtet er læst og forsøgt konverteret til et tal kontrolleres det også om tallet ligger inden for det tilladte interval
																						  // || er 'Or/Eller' operatøren
																						  // valg < 1 betyder at brugeren har indtastet et tal der er mindre end 1
																						  // valg > max betyder at brugeren har indtastet et tal der er større end det højeste tilladte valg
																						  // Hvis en af disse betingelser er sand, fortsætter while-loopet og brugeren bliver bedt om at indtaste et nyt tal
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"Indtast et tal mellem 1 og {max}: ");
				Console.ResetColor();
			}

			return tider[startIndex + valg]; // Returnerer det valgte sluttidspunkt --  startIndex + valg bruges til at finde den korrekte position i listen

			// Eksempel på denne return:
			// Hvis starttidspunktet er 10:00 og ligger på indeks 4 i listen
			// og brugeren vælger mulighed 2 (f.eks. 11:00)
			// vil beregningen blive:
			// startIndex (4) + valg (2(endTime)) = indeks 6

				/* | Index | Tid               |
				| ----- | ----------------- |
				| 0 | 08:00 |
				| 1 | 08:30 |
				| 2 | 09:00 |
				| 3 | 09:30 |
				| 4 | 10:00 ← startTime |
				| 5 | 10:30 |
				| 6 | 11:00 |
				| 7 | 11:30 | */
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

			while (svar == 'j') // Hvis svaret er lig med 'j', så fortsætter loopet
			{
				foreach (string employee in employees)
				{
					Console.WriteLine(employee);
				}

				Console.WriteLine("");
				Console.Write("Vælg en ansat og afslut med <Enter>: ");

				int valg;

				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > employees.Count) // Sikrer, at brugeren kun kan vælge mellem antallet af medarbejdere i Index
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				participants.Add(employees[valg - 1]); // Tilføjelse af flere deltagere

				Console.WriteLine("");
				Console.Write("Vil du vælge flere ansatte til mødet? --- Svar: j/n: ");
				svar = Console.ReadKey().KeyChar;

				Console.WriteLine("");
			}

			Console.WriteLine("Du har nu valgt deltager(e) til mødet:");

			foreach (var participant in participants) // 'var', da C# godt kan læse en simpel string variabel
			{
				Console.WriteLine(participant);
			}


			return participants;
		}

		static string AddNote() // Metode, der tillader en tilføjelse af en note
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

		static void MeetingConfirmation(string day, MødeLokaler lokaler, string startTime, string endTime, List<string> participants, string note) // Denne metode kalder alle variabler fra tidligere metoder
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
			Console.WriteLine($"Lokale: {lokaler.Name}");
			Console.WriteLine("");
			Console.WriteLine($"Tid: {startTime} - {endTime}");
			Console.WriteLine("");
			Console.WriteLine("Deltagere:");

			

			foreach (string participant in participants) // Loopet bruges til at vise alle deltagere der blev valgt under bookingprocessen
			{
				Console.WriteLine($"{participant}");
			}

			Console.WriteLine("");

			if (!string.IsNullOrWhiteSpace(note)) // Kontrollerer om brugeren har indtastet en note
												  // IsNullOrWhiteSpace sikrer at noten ikke er tom eller kun består af mellemrum
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

	}
}