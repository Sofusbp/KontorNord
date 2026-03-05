

namespace ProjektNordUgevisning_Aflysning_af_møde_isa
{
	using System;
	using System.Collections.Generic; // for List<>
	using System.Globalization; // til ISOWeek
	using System.Linq; // for Where, OrderBy, FirstOrDefault
	using System.Security.Cryptography.X509Certificates;

	internal class Program
	{
		static void Main(string[] args)
		{
			Console.ResetColor();
			Console.OutputEncoding = System.Text.Encoding.UTF8; // for at kunne bruge pile-symboler i menuen og ÆØÅ
			Console.Title = "♥ Bookingsystem - KontorNord ♥"; // Titel på konsol-vinduet <3 ELSKER

			int width = Math.Min(150, Console.LargestWindowWidth);
			int height = Console.LargestWindowHeight - 4;

			Console.SetBufferSize(width, height);
			Console.SetWindowSize(width, height);

			DateTime today = DateTime.Today; // dagens dato hentes af DateTime fra systemet
			int isoYear = ISOWeek.GetYear(today); // ISO-året kan være forskelligt fra kalenderåret i de første og sidste uger af året
			int isoWeek = ISOWeek.GetWeekOfYear(today); // ISO-ugenummeret for dagens dato

			List<Meeting> meetings = new List<Meeting>();

			Console.WriteLine();
			PrintMenu();
			Console.WriteLine();

			// UI loop
			while (true)
			{
				Console.Clear();
				Console.WriteLine();
				PrintMenu();
				Console.WriteLine();

				// Filtrer og sorter møder
				List<Meeting> weekMeetings = meetings
					.Where(m => m.IsoYear == isoYear && m.IsoWeek == isoWeek)
					.OrderBy(m => m.IsoDay)
					.ThenBy(m => m.StartHour)
					.ToList();


				// vis ugegrid 
				DrawWeek(isoYear, isoWeek, weekMeetings);



				switch (Console.ReadKey(true).Key) // true = ikke vis tastetryk i konsollen
				{
					case ConsoleKey.RightArrow:
						{
							var nextWeekDate = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday).AddDays(7); // gå til næste uge ved at tage mandag i nuværende uge og lægge 7 dage til
							isoYear = ISOWeek.GetYear(nextWeekDate);
							isoWeek = ISOWeek.GetWeekOfYear(nextWeekDate);
							break;
						}

					case ConsoleKey.LeftArrow:
						{
							var prevWeekDate = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday).AddDays(-7);
							isoYear = ISOWeek.GetYear(prevWeekDate);
							isoWeek = ISOWeek.GetWeekOfYear(prevWeekDate);
							break;
						}

					case ConsoleKey.N:
						{
							meetings.Add(Meeting.CreateFromUserInput(isoYear, isoWeek));
							break;
						}

					case ConsoleKey.D:
						{

							CancelFlow(meetings, isoYear, isoWeek);
							break;
						}

					case ConsoleKey.Q:
					case ConsoleKey.Escape:

						return;
				}
			}
		}



		static void PrintMenu()
		{
			string menuText = "←/→ skift uge   N nyt møde   D aflys møde   Q/Esc afslut";

			int windowWidth = Console.WindowWidth;
			int padding = Math.Max(0, (windowWidth - menuText.Length) / 2); // beregn hvor mange mellemrum der skal til for at centrere menuen

			Console.Write(new string(' ', padding));

			// Menu med farver:

			// ←/→ skift uge (gul)
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("←/→");
			Console.Write(" skift uge   ");
			Console.ResetColor();

			// N nyt møde (blå)
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write("N");
			Console.Write(" nyt møde   ");
			Console.ResetColor();

			// D aflys møde (rød)
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("D");
			Console.Write(" aflys møde   ");
			Console.ResetColor();

			// Q/Esc afslut (grå)
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.Write("Q/Esc");
			Console.WriteLine(" afslut");
			Console.ResetColor();

		}
		//  Ugegrid 
		static void DrawWeek(int isoYear, int isoWeek, List<Meeting> weekMeetings)
		{
			int timeColWidth = 6;   // fx "08:00  |"
			int dayColWidth = 25;   // justér efter console-bredde

			string[] dayNames = { "MAN", "TIR", "ONS", "TOR", "FRE" };

			DateTime monday = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday);
			DateTime friday = monday.AddDays(4);

			string weekRangeText = $"Uge {isoWeek} - {monday:dd/MM} → {friday:dd/MM}"; // tekst der viser hvilken uge og dato-interval vi kigger på, fx "Uge 42 - 14/10 → 18/10"
			int windowWidth = Console.WindowWidth;
			int padding = Math.Max(0, (windowWidth - weekRangeText.Length) / 2); // beregn hvor mange mellemrum der skal til for at centrere menuen

			Console.WriteLine();
			Console.WriteLine();

			Console.Write(new string(' ', padding)); // print antal af mellemrum lige beregnet for at centrere uge-overskriften
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Uge {isoWeek} - {monday:dd/MM} → {friday:dd/MM}");
			Console.ResetColor();
			Console.WriteLine();
			Console.WriteLine();
			

			// Top-linje (lukker boksen over headeren i tabellen - lige ugedagene)
			Console.Write("     ");
			Console.WriteLine("".PadRight(timeColWidth) + "-" + new string('-', (dayColWidth + 1) * 5));

			// Header med ugedage og datoer
			Console.Write("".PadRight(timeColWidth + 5) + "|"); // sørger for at den lodrette linje starter efter tidskolonnen
			for (int d = 0; d < 5; d++) // loop der printer headeren for hver dag (kunne også gøres med en array af strings for hver ugedag eller med enum)
			{
				string header = $"       {dayNames[d]} {monday.AddDays(d):dd/MM}";

				Console.Write(header.PadRight(dayColWidth) + "|"); // print headeren for hver dag og sørg for at den fylder hele cellen (dayColWidth) og så en lodret linje efter hver header
			}
			Console.WriteLine();

			// Divide efter header
			Console.Write("     ");
			Console.WriteLine(new string('-', timeColWidth + 1 + (dayColWidth + 1) * 5));



			// Timeslots 08..17 (18 er slut)
			for (int hour = 8; hour < 18; hour++)
			{
				// 2 linjer pr timeslot: linje 0 = overskrift, linje 1 = detaljer
				for (int innerLine = 0; innerLine < 2; innerLine++)
				{
					// tid kun på første linje
					string timeText = (innerLine == 0) ? $"{hour:00}:00" : "";
					Console.Write("     ");
					Console.Write(timeText.PadRight(timeColWidth) + "|");

					for (int day = 1; day <= 5; day++)
					{
						// Find det møde der dækker dette slot
						var meeting = weekMeetings
							.Where(m => m.IsoDay == day && m.CoversHour(hour))
							.OrderBy(m => m.StartHour)
							.FirstOrDefault();

						if (meeting != null)
						{
							Console.BackgroundColor = ConsoleColor.Blue; //Gør farven på et booket møde til blå

							string cellText;
							if (innerLine == 0)
								cellText = $"{meeting.TimeRangeText()}  {meeting.Room}";

							else


								cellText = $"{meeting.Participants} | {meeting.Note}";


							Console.Write(cellText.PadRight(dayColWidth) + "|");
							Console.ResetColor();
						}
						else
						{

							Console.Write("".PadRight(dayColWidth) + "|");
						}


					}
					Console.WriteLine();
				}

				// vandret streg efter hver time
				Console.Write("     ");
				Console.WriteLine(new string('-', timeColWidth + 1 + (dayColWidth + 1) * 5));
			}
		}



		class Meeting
		{
			public int IsoYear;
			public int IsoWeek;
			public int IsoDay; // 1=Man ... 5=Fre

			// Forenklet møde-model der kun har helt tal mellem 8-17 som værdi
			public int StartHour;
			public int EndHour;

			public string Room = "";
			public string Participants = "";
			public string Note = "";

			public override string ToString()
			{
				return $"{TimeRangeText()}  Lokale: {Room}  Deltagere: {Participants}  Note: {Note}";
			}


			public string TimeRangeText() // metode der laver heltallene StartHour og EndHour om til et tekst-format der kan vises i cellen, fx 8 og 10 bliver til "08:00-10:00"
			{
				return $"{StartHour:00}:00-{EndHour:00}:00";
			}

			public bool CoversHour(int hour) // metode der tjekker om dette møde dækker det givne time-slot (hour). Det gør den hvis hour er større eller lig med StartHour og mindre end EndHour, fordi EndHour er tidspunktet hvor mødet slutter og så dækker det ikke længere.
			{

				return hour >= StartHour && hour < EndHour; // fx et møde der starter kl 8 og slutter kl 10 dækker time 8 (fordi 8 >= 8 og 8 < 10) og time 9 (fordi 9 >= 8 og 9 <10)
			}

			public static Meeting CreateFromUserInput(int isoYear, int isoWeek) // statisk metode der opretter et Meeting-objekt ved at spørge brugeren om input i konsollen. Den tager isoYear og isoWeek som parametre fordi et møde altid skal høre til en bestemt uge, så vi sætter det automatisk ud fra hvilken uge brugeren kigger på når de opretter mødet.
			{


				Console.Clear();
				Console.WriteLine("Opret nyt møde");
				Console.WriteLine();

				int day = ReadInt("Indtast det nummer som svarer til ugedagen du vil booke | Man = 1 | Tir = 2 | Ons = 3 | Tor = 4 | Fre = 5 |: ", 1, 5); // kunne også laves til en enum eller noget med navne i stedet for

				int startHour = ReadInt("Start tid (8-17): ", 8, 17); // format kunne ændres til 00:00 men gider ikke lige rode med det nu, så vi kører bare med heltal for timer og så laver det om til tekst i TimeRangeText() metoden når det skal vises i cellen. Start tid kan være mellem 8 og 17 fordi sidste møde kan starte kl 17 og så slutte kl 18.   
				int endHour = ReadInt("Slut tid (9-18): ", 9, 18); // endHour kan tidligst være 9 fordi et møde fra kl 8-8 self ikke ville give mening

				while (endHour <= startHour)
				{
					Console.WriteLine("Slut tid skal være efter start tid.");
					endHour = ReadInt("Slut tid (9-18): ", 9, 18);
				}

				string room = ReadTextFromUser("Mødelokale: ");
				string participants = ReadTextFromUser("Deltagere (kommasepareret): ");
				string note = ReadTextFromUser("Note (valgfri): ");

				Meeting m = new Meeting();
				m.IsoYear = isoYear;
				m.IsoWeek = isoWeek;
				m.IsoDay = day;
				m.StartHour = startHour;
				m.EndHour = endHour;
				m.Room = room;
				m.Participants = participants;
				m.Note = note;

				return m;

			}

			// evt helpers til input for at forkerte kode andre steder og for at sikre brugeren ikke taster noget vrøvl som crasher programmet
			private static int ReadInt(string question, int min, int max) // en hjælpemetode til CreateFromUserInput som sparer lidt gentagelser. 
																		  // Metoden samler spørgsmålet og det interval svaret fra brugeren kan være i mellem fx 8-18 i tidsrum. Og så tilføjes svaret til variablen input.
			{
				while (true)
				{
					Console.Write(question);
					string? input = Console.ReadLine();

					if (int.TryParse(input, out int value) && value >= min && value <= max) // int.TryParse forsøger at konvertere input til et heltal og returnerer true hvis det lykkes, ellers false. Hvis det lykkes, gemmer det det konverterede tal i variablen value. 
						return value;                                                       // Og så tjekker vi også om value er inden for det angivne interval (min-max). Det betyder at brugeren også kan taste fx 09 eller 000000008 ind og det vil stadig blive accepteret som 9, så længe det er mellem min og max.
																							// out int value betyder output fra metoden er en variabel (value) som bliver sat til det konverterede tal hvis konverteringen lykkes.
					Console.WriteLine($"Ugyldigt. Indtast et tal mellem {min} og {max}.");
				}
			}

			private static string ReadTextFromUser(string question) // måske lidt overflødig metode men sparer os fra lidt gentagelse i CreateFromUserInput
			{
				Console.Write(question);
				return Console.ReadLine() ?? ""; // ?? "" betyder at hvis Console.ReadLine() returnerer null (fx hvis brugeren bare trykker enter), så skal den bare returnere en tom streng i stedet for null. og ellers skal den returnere det brugeren skrev.
			}
		}



		//  Aflysning 
		static void CancelFlow(List<Meeting> meetings, int isoYear, int isoWeek) // metode der håndterer hele flowet for at aflyse et møde. Den tager listen af møder og den uge vi kigger på som parametre, så den kan vise møderne i den uge og fjerne det møde brugeren vælger at aflyse fra listen.
		{
			Console.Clear();

			var weekMeetings = meetings
				.Where(m => m.IsoYear == isoYear && m.IsoWeek == isoWeek)
				.OrderBy(m => m.IsoDay)
				.ThenBy(m => m.StartHour)
				.ToList();

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("AFLYS MØDE");
			Console.ResetColor();
			Console.WriteLine();

			if (weekMeetings.Count == 0)
			{
				Console.WriteLine("Der er ingen møder at aflyse i denne uge. Tryk på en tast for at gå tilbage...");
				Console.ReadKey(true);
				return;
			}

			DateTime monday = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday); // find datoen for mandagen i den uge vi kigger på, så vi kan vise datoerne for hvert møde i listen

			for (int i = 0; i < weekMeetings.Count; i++)
			{
				var m = weekMeetings[i]; // for hvert møde i ugen, find datoen ved at tage mandagens dato og lægge antal dage til ud fra hvilken ugedag mødet er på (IsoDay - 1, fordi IsoDay starter på 1 for mandag men systemet tæller fra 0)
				DateTime meetingDate = monday.AddDays(m.IsoDay - 1);

				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine($"{i + 1}. {meetingDate:dd/MM}  {m.TimeRangeText()}  Lokale: {m.Room}  Deltagere: {m.Participants}  Note: {m.Note}"); // Print en nummereret liste over møderne i ugen med dato, tid, lokale, deltagere og note, så brugeren kan vælge hvilket møde de vil aflyse ud fra det
			}

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("Vælg nummer der skal slettes (Enter for at annullere): ");
			Console.ResetColor();

			string input = Console.ReadLine() ?? ""; // læs brugerens input for hvilket møde de vil aflyse. Hvis de bare trykker enter, så er input en tom streng, og så annullerer vi aflysningen.
			if (string.IsNullOrWhiteSpace(input))
				return;

			if (!int.TryParse(input, out int choice) || choice < 1 || choice > weekMeetings.Count) // tjek om input er et gyldigt tal og inden for intervallet af møder i listen. Hvis ikke, så vis en fejlmeddelelse og returner til menuen.
			{
				Console.WriteLine("Ugyldigt valg. Tryk en tast...");
				Console.ReadKey(true);
				return;
			}

			var target = weekMeetings[choice - 1];  // find det møde der skal aflyses ud fra brugerens valg (choice - 1 fordi listen starter på 0 inde i systemet men på listen brugeren kan se starter det på 1)

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("Er du sikker? Tryk J for JA eller N for NEJ: ");
			Console.ResetColor();



			var confirmKey = Console.ReadKey(true).Key; // læs et tastetryk fra brugeren for at bekræfte at de vil aflyse mødet. Hvis de trykker J, så slettes mødet, ellers annulleres aflysningen.
			if (confirmKey == ConsoleKey.J)
			{
				meetings.Remove(target);

				//Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine();
				Console.WriteLine("Følgende møde er slettet: ");
				Console.ResetColor();
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine($"============== [SLETTET] {target} [SLETTET] ===============");
				Console.ResetColor();




				Console.WriteLine("Tryk en tast...");
				Console.ReadKey(true);
			}
			else
			{
				Console.WriteLine("Annulleret. Tryk en tast...");
				Console.ReadKey(true);
			}


		}
	}
}


	
			
		
	
