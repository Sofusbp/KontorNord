namespace merge2projektknontornord
{
	namespace ProjektNordUgevisning_Aflysning_af_møde_isa
	{
		using System;
		using System.Collections.Generic; // for List<>
		using System.Globalization; // til ISOWeek
		using System.Linq; // for Where, OrderBy, FirstOrDefault
		using System.IO; // for File I/O hvis vi skulle gemme møderne i en fil (ikke implementeret endnu)
		using System.Text.Json; // for JSON serialization hvis vi skulle gemme møderne i en fil (ikke implementeret endnu)

		internal class Program
		{
			static string filePath = "meetings.json"; // filsti for hvor møderne skal gemmes. Vi gemmer møderne i en JSON-fil i samme mappe som programmet, så det er nemt at finde.

			static void Main(string[] args)
			{
				Console.OutputEncoding = System.Text.Encoding.UTF8; // for at kunne bruge pile-symboler i menuen og ÆØÅ
				Console.Title = "♥ Bookingsystem - KontorNord ♥"; // Titel på konsol-vinduet <3 ELSKER
				Console.ResetColor();

				int width = Math.Min(150, Console.LargestWindowWidth);
				int height = Console.LargestWindowHeight - 2;
				Console.SetBufferSize(width, height);
				Console.SetWindowSize(width, height);

				string workerID = "";
				string name = "";
				string password = "";
				bool isValidLogin = false;

				while (!isValidLogin) // Så længe der ikke kommer et "ValidLogin" kører loopet videre og brugeren kan forsøge igen.
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("=========================");
					Console.WriteLine("KONTOR NORD BOOKINGSYSTEM");
					Console.WriteLine("=========================");
					Console.WriteLine("");
					Console.WriteLine("");
					Console.ResetColor();

					Console.WriteLine("Indtast dit medarbejder ID for at begynde: ");
					workerID = Console.ReadLine() ?? "";

					// Selve WorkerID godkendelsen - der er brugt de to første initialer i fornavn og et inital i efternavn.
					// Adgangskoden er lavet omvendt af WorkerID - så de samme initialer blot baglæns.
					// Hvis WorkerID er korrekt vil den spørge om en adgangskode og hvis den også er korrekt kommer man videre ind til velkommen. Hvis ikke koden eller workerid er korrekt, kan man prøve igen.
					if (workerID == "SOM")
					{
						name = "Sofie Møller";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "MOS")
						{
							isValidLogin = true;
						}
					}
					else if (workerID == "AMR")
					{
						name = "Amir Rahimi";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "RMA")
						{
							isValidLogin = true;
						}
					}
					else if (workerID == "JOT")
					{
						name = "Jonas Tved";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "TOJ")
						{
							isValidLogin = true;
						}
					}
					else if (workerID == "LOF")
					{
						name = "Louise Falk";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "FOL")
						{
							isValidLogin = true;
						}
					}
					else if (workerID == "MEA")
					{
						name = "Mette Ates";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "EAM")
						{
							isValidLogin = true;
						}
					}
					else if (workerID == "HEK")
					{
						name = "Henrik Krøll";

						Console.WriteLine("Indtast kodeord: ");
						password = Console.ReadLine() ?? "";
						if (password == "KEH")
						{
							isValidLogin = true;
						}
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine();
						Console.WriteLine("Ugyldigt medarbejder ID!");
						Console.ResetColor();
						Console.WriteLine("Tryk på en tast for at prøve igen...");
						Console.ReadLine();
						Console.Clear();
						continue;
					}

					if (!isValidLogin)
					{
						Console.WriteLine();
						Console.WriteLine("Forkert adgangskode eller login");
						Console.WriteLine("Tryk for at forsøge igen");
						Console.ReadKey();
					}
				}

				// Name fra tidligere kaldes herned og så vises det fulde navn i velkomst menuen.
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("=========================");
				Console.WriteLine($"VELKOMMEN {name}");
				Console.WriteLine("=========================");
				Console.WriteLine("");
				Console.WriteLine("Tryk på en tast for at komme ind i menuen");
				Console.ResetColor();
				Console.ReadLine();

				DateTime today = DateTime.Today; // dagens dato hentes af DateTime fra systemet
				int isoYear = ISOWeek.GetYear(today); // ISO-året kan være forskelligt fra kalenderåret i de første og sidste uger af året
				int isoWeek = ISOWeek.GetWeekOfYear(today); // ISO-ugenummeret for dagens dato

				List<Meeting> meetings = LoadMeetings();

				// Liste med fast definerede mødelokaler som brugeren kan bladre mellem i ugevisningen
				List<MeetingRoom> rooms = new List<MeetingRoom>
			{
				new MeetingRoom(1, "A", 4),
				new MeetingRoom(2, "B", 6),
				new MeetingRoom(3, "C", 8),
			};

				int selectedRoomIndex = 0; // hvilket lokale der aktuelt er valgt i visningen. Vi starter med første lokale i listen.

				bool isRunning = true;

				while (isRunning)
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine($"===== Welcome, {workerID} ====="); // Det første man ser efter log-in - Indsæt $ når workerID er sat op (interpolation)
					Console.ResetColor();
					Console.WriteLine(""); // Lidt spacing
					Console.WriteLine($"Aktuelt lokale: {rooms[selectedRoomIndex].Name}");
					Console.WriteLine($"Aktuel uge: {isoWeek}");
					Console.WriteLine("");

					ShowMainMenu(); // Kalder Menu-Metode, der viser Menu Options

					int input;
					while (!int.TryParse(Console.ReadLine(), out input))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("Indtast venligst et tal: ");
						Console.ResetColor();
					}

					switch (input) // Hvis brugeren trykker på en tast (1-4), så kører/kalder vi på den givne metode
					{
						case 1:
							CalendarLoop(meetings, rooms, ref isoYear, ref isoWeek, ref selectedRoomIndex);
							break;

						case 2:
							{
								MeetingRoom selectedRoom = rooms[selectedRoomIndex];
								Meeting? newMeeting = BookMeetingFlow(isoYear, isoWeek, selectedRoom);

								if (newMeeting == null)
								{
									break;
								}

								// Tjek om lokalet er ledigt i det ønskede tidsrum før vi gemmer mødet.
								// Nu kan samme tidspunkt godt findes i andre lokaler, men ikke i det lokale brugeren står på.
								if (!selectedRoom.IsAvailable(meetings, newMeeting.IsoYear, newMeeting.IsoWeek, newMeeting.IsoDay, newMeeting.StartHour, newMeeting.EndHour))
								{
									Console.WriteLine();
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("Lokalet er ikke ledigt i det tidsrum. Tryk en tast...");
									Console.ResetColor();
									Console.ReadKey(true);
									break;
								}

								meetings.Add(newMeeting);
								SaveMeetings(meetings);

								Console.ForegroundColor = ConsoleColor.Blue;
								Console.WriteLine();
								Console.WriteLine("Mødet er nu oprettet.");
								Console.ResetColor();
								Console.WriteLine("Tryk på en tast for at gå tilbage til menuen...");
								Console.ReadKey(true);
								break;
							}

						case 3:
							{
								MeetingRoom selectedRoom = rooms[selectedRoomIndex];
								CancelFlow(meetings, isoYear, isoWeek, selectedRoom);
								break;
							}

						case 4:
							isRunning = false;
							break;

						default:
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Please input a valid number (1-4)");
							Console.ResetColor();
							Console.ReadKey(true);
							break;
					}
				}
			}

			static void ShowMainMenu() // Metoden, der viser vores menu efter log-in
			{
				Console.WriteLine("1) Show current, past and/or future meetings via our Calendar");
				Console.WriteLine("2) Book a meeting");
				Console.WriteLine("3) Cancel a meeting");
				Console.WriteLine("4) Exit");
				Console.WriteLine("");
				Console.Write("Vælg nummer og afslut med <Enter>: ");
			}

			static void CalendarLoop(List<Meeting> meetings, List<MeetingRoom> rooms, ref int isoYear, ref int isoWeek, ref int selectedRoomIndex)
			{
				while (true)
				{
					Console.Clear();
					Console.WriteLine();
					PrintCalendarMenu();
					Console.WriteLine();

					MeetingRoom selectedRoom = rooms[selectedRoomIndex]; // det lokale brugeren lige nu kigger på i ugevisningen

					int currentIsoYear = isoYear;
					int currentIsoWeek = isoWeek;

					// Filtrer og sorter møder
					List<Meeting> weekMeetings = meetings
						.Where(m => m.IsoYear == currentIsoYear && m.IsoWeek == currentIsoWeek && m.RoomId == selectedRoom.Id)
						.OrderBy(m => m.IsoDay)
						.ThenBy(m => m.StartHour)
						.ToList();

					// vis ugegrid
					DrawWeek(isoYear, isoWeek, weekMeetings, selectedRoom);

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

						case ConsoleKey.UpArrow:
							{
								selectedRoomIndex--;
								if (selectedRoomIndex < 0)
									selectedRoomIndex = rooms.Count - 1;
								break;
							}

						case ConsoleKey.DownArrow:
							{
								selectedRoomIndex++;
								if (selectedRoomIndex >= rooms.Count)
									selectedRoomIndex = 0;
								break;
							}

						case ConsoleKey.N:
							{
								Meeting? newMeeting = BookMeetingFlow(isoYear, isoWeek, selectedRoom);

								if (newMeeting == null)
								{
									break;
								}

								if (!selectedRoom.IsAvailable(meetings, newMeeting.IsoYear, newMeeting.IsoWeek, newMeeting.IsoDay, newMeeting.StartHour, newMeeting.EndHour))
								{
									Console.WriteLine();
									Console.ForegroundColor = ConsoleColor.Red;
									Console.WriteLine("Lokalet er ikke ledigt i det tidsrum. Tryk en tast...");
									Console.ResetColor();
									Console.ReadKey(true);
									break;
								}

								meetings.Add(newMeeting);
								SaveMeetings(meetings);
								break;
							}

						case ConsoleKey.D:
							{
								CancelFlow(meetings, isoYear, isoWeek, selectedRoom);
								break;
							}

						case ConsoleKey.Q:
						case ConsoleKey.Escape:
							return;
					}
				}
			}

			static void PrintCalendarMenu()
			{
				string menuText = "←/→ skift uge       ↑/↓ skift lokale       N Opret nyt møde       D aflys møde       Q/Esc tilbage";

				int windowWidth = Console.WindowWidth;
				int padding = Math.Max(0, (windowWidth - menuText.Length) / 2); // beregn hvor mange mellemrum der skal til for at centrere menuen

				Console.Write(new string(' ', padding));

				// Menu med farver:

				// ←/→ skift uge (gul)
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("←/→");
				Console.Write(" skift uge       ");
				Console.ResetColor();

				// ↑/↓ skift lokale (cyan)
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("↑/↓");
				Console.Write(" skift lokale       ");
				Console.ResetColor();

				// N nyt møde (blå)
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("N");
				Console.Write(" nyt møde       ");
				Console.ResetColor();

				// D aflys møde (rød)
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("D");
				Console.Write(" aflys møde       ");
				Console.ResetColor();

				// Q/Esc afslut (Magenta)
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.Write("Q/Esc");
				Console.WriteLine(" tilbage");
				Console.ResetColor();
			}

			//  Ugegrid
			static void DrawWeek(int isoYear, int isoWeek, List<Meeting> weekMeetings, MeetingRoom selectedRoom)
			{
				int timeColWidth = 6;   // fx "08:00  |"
				int dayColWidth = 25;   // justér efter console-bredde

				string[] dayNames = { "MAN", "TIR", "ONS", "TOR", "FRE" };

				DateTime monday = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday);
				DateTime friday = monday.AddDays(4);

				string weekRangeText = $"←    Uge {isoWeek} : {monday:dd/MM} - {friday:dd/MM}    →"; // tekst der viser hvilken uge og dato-interval vi kigger på
				int windowWidth = Console.WindowWidth;
				int padding = Math.Max(0, (windowWidth - weekRangeText.Length) / 2); // beregn hvor mange mellemrum der skal til for at centrere menuen

				Console.WriteLine();

				Console.Write(new string(' ', padding)); // print antal af mellemrum lige beregnet for at centrere uge-overskriften
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"←    Uge {isoWeek} : {monday:dd/MM} - {friday:dd/MM}    →");
				Console.ResetColor();

				string selectedRoomText = $"Viser møder for mødelokale: {selectedRoom.Name}  |  Med kapacitet på: {selectedRoom.Capacity}";
				int paddingRoomText = Math.Max(0, (windowWidth - selectedRoomText.Length) / 2); // beregn hvor mange mellemrum der skal til for at centrere menuen

				Console.WriteLine();
				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write(new string(' ', paddingRoomText));
				Console.WriteLine($"Viser møder for mødelokale: {selectedRoom.Name}  |  Med kapacitet på: {selectedRoom.Capacity}");
				Console.ResetColor();
				Console.WriteLine();

				// Top-linje (lukker boksen over headeren i tabellen - lige ugedagene)
				Console.Write("     ");
				Console.WriteLine("".PadRight(timeColWidth) + "-" + new string('-', (dayColWidth + 1) * 5));

				// Header med ugedage og datoer
				Console.Write("".PadRight(timeColWidth + 5) + "|"); // sørger for at den lodrette linje starter efter tidskolonnen
				for (int d = 0; d < 5; d++) // loop der printer headeren for hver dag
				{
					string header = $"       {dayNames[d]} {monday.AddDays(d):dd/MM}";
					Console.Write(header.PadRight(dayColWidth) + "|"); // print headeren for hver dag og sørg for at den fylder hele cellen
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
									cellText = $"{meeting.TimeRangeText()}  {selectedRoom.Name}";
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

			static Meeting? BookMeetingFlow(int isoYear, int isoWeek, MeetingRoom selectedRoom) // Metoden, der tillader brugeren at booke møder -- Kig på den, som vores "Controller", der styrer hele booking processen
			{
				Console.Clear(); // Intern C# Metode, der sletter alt fra det forrige trin

				string day = SelectDay(); // Brugerens valgte dag returneres som en String og gemmes i variablen 'day'

				int startHour = SelectStartTime(); // Brugerens valgte starttidspunkt returneres som et helt tal og gemmes i variablen 'startHour'

				int endHour = SelectEndTime(startHour); // Starttidspunkt bliver her sendt som parameter, så brugeren KUN kan vælge tider efter valgte 'startHour'

				List<string> participants = AddParticipants(); // Deltager Liste (taget fra Casen)

				string note = AddNote(); // Metode, der tillader noter til møder

				Meeting newMeeting = BuildMeetingFromBookingFlow(isoYear, isoWeek, selectedRoom, day, startHour, endHour, participants, note);

				MeetingConfirmation(newMeeting, selectedRoom);

				Console.ForegroundColor = ConsoleColor.Blue;
				ReturnToMenu(); // Her kalder vi på Return-Metoden, der får brugeren tilbage til Menu
				Console.ResetColor(); // Ikke nødvendig, men god skik

				return newMeeting;
			}

			static Meeting BuildMeetingFromBookingFlow(int isoYear, int isoWeek, MeetingRoom selectedRoom, string day, int startHour, int endHour, List<string> participants, string note)
			{
				Meeting m = new Meeting();
				m.IsoYear = isoYear;
				m.IsoWeek = isoWeek;
				m.IsoDay = DayNameToIsoDay(day);
				m.StartHour = startHour;
				m.EndHour = endHour;
				m.RoomId = selectedRoom.Id;
				m.Participants = string.Join(", ", participants);
				m.Note = note;

				return m;
			}

			static int DayNameToIsoDay(string day)
			{
				switch (day)
				{
					case "Mandag": return 1;
					case "Tirsdag": return 2;
					case "Onsdag": return 3;
					case "Torsdag": return 4;
					case "Fredag": return 5;
					default: return 1;
				}
			}

			static void MeetingConfirmation(Meeting meeting, MeetingRoom selectedRoom)
			{
				Console.Clear();

				string[] dayNames = { "", "Mandag", "Tirsdag", "Onsdag", "Torsdag", "Fredag" };

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("============================");
				Console.WriteLine("     MØDEBEKRÆFTELSE");
				Console.WriteLine("============================");
				Console.ResetColor();
				Console.WriteLine("");

				Console.WriteLine($"Dag: {dayNames[meeting.IsoDay]}");
				Console.WriteLine($"Lokale: {selectedRoom.Name}");
				Console.WriteLine($"Tid: {meeting.TimeRangeText()}");
				Console.WriteLine($"Deltagere: {meeting.Participants}");
				Console.WriteLine($"Note: {meeting.Note}");
			}

			static string SelectDay() // Metoder, der hører til en anden metode. Her vælger brugeren hvilken dag, de vil booke et møde på
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
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				switch (valg) // Her bruger jeg ikke "break", da jeg har "return" i min metode. "Return", får koden til at stoppe automatisk
				{
					case 1: return "Mandag";
					case 2: return "Tirsdag";
					case 3: return "Onsdag";
					case 4: return "Torsdag";
					case 5: return "Fredag";
					default: return "Mandag";
				}
			}

			static int SelectStartTime()
			{
				Console.Clear();

				List<int> tider = new List<int>(); // Benytter en liste for at undgå mange cases i en switch

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("============================");
				Console.WriteLine("       STARTTIDSPUNKT");
				Console.WriteLine("============================");
				Console.ResetColor();
				Console.WriteLine("");

				int mulighed = 1; // Mulighed, altså sætter tal foran mulighederne 1-xx

				for (int tid = 8; tid <= 17; tid++) // For Loop, der genererer forskellige tidspunkter
				{
					Console.WriteLine($"{mulighed}) {tid:00}:00");
					tider.Add(tid);
					mulighed++; // Plusser med 1 for hver mulighed (en tæller)
				}

				Console.WriteLine("");
				Console.Write("Vælg nummer og afslut med <Enter>: ");

				int valg;
				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > tider.Count)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				return tider[valg - 1];
			}

			static int SelectEndTime(int startHour) // Basically en kopi af ovenstående metode, men bare som slut-tidspunkt i stedet
			{
				Console.Clear();

				List<int> tider = new List<int>();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("============================");
				Console.WriteLine("       SLUTTIDSPUNKT");
				Console.WriteLine("============================");
				Console.ResetColor();
				Console.WriteLine("");

				for (int tid = startHour + 1; tid <= 18; tid++) // Sluttid skal være efter starttidspunktet
				{
					tider.Add(tid);
				}

				for (int i = 0; i < tider.Count; i++)
				{
					Console.WriteLine($"{i + 1}) {tider[i]:00}:00");
				}

				Console.WriteLine("");
				Console.Write("Vælg nummer og afslut med <Enter>: ");

				int valg;

				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > tider.Count)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write($"Indtast et tal mellem 1 og {tider.Count}: ");
					Console.ResetColor();
				}

				return tider[valg - 1];
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
				"Sofie Møller (SM)",
				"Jonas Tved (JT)",
				"Amir Rahimi (AR)",
				"Louise Falk (LF)",
				"Mette Ates (MA)",
				"Henrik Krøll (HK)",
			};

				char svar = 'j';

				// Jeg mangler stadig at lave en char 'svarNej'
				// Jeg mangler stadig logik for, at man ikke kan vælge samme deltager 2 gange

				while (char.ToLower(svar) == 'j') // Hvis svaret er lig med 'j', så fortsætter loopet
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("============================");
					Console.WriteLine("       VÆLG DELTAGER(E)");
					Console.WriteLine("============================");
					Console.ResetColor();
					Console.WriteLine("");

					for (int i = 0; i < employees.Count; i++)
					{
						Console.WriteLine($"{i + 1}) {employees[i]}");
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

					string valgtDeltager = employees[valg - 1];

					if (!participants.Contains(valgtDeltager))
					{
						participants.Add(valgtDeltager); // Tilføjer den valgte medarbejder til listen over mødedeltagere
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Denne deltager er allerede valgt.");
						Console.ResetColor();
					}

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

				Console.WriteLine("Tryk på en tast for at fortsætte...");
				Console.ReadKey(true);

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
				string note = Console.ReadLine() ?? "";

				return note;
			}

			static void ReturnToMenu() // Return-Metode, der bliver kaldt i andre menu-metoder, så brugeren kan komme tilbage til Menu
			{
				Console.WriteLine();
				Console.WriteLine("Press 'M' to return to menu");

				while (true)
				{
					var key = Console.ReadKey(true).KeyChar; // Var, da C# sagtens selv kan aflæse en nem 'char'

					if (char.ToUpper(key) == 'M') // ToUpper - "Fejlsikring" i fald af, at brugeren ikke skriver stort 'M'
					{
						Console.Clear();
						break;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("Tryk venligst på M for at gå tilbage: ");
						Console.ResetColor();
					}
				}
			}

			public class MeetingRoom
			{
				public int Id;
				public string Name = "";
				public int Capacity;

				public MeetingRoom() { } // parameterløs constructor så JSON og andre dele af programmet ikke brokker sig hvis vi senere vil gemme lokaler også

				public MeetingRoom(int id, string name, int capacity)
				{
					Id = id;
					Name = name;
					Capacity = capacity;
				}

				// metode der tjekker om lokalet er ledigt i et bestemt tidsrum i en bestemt uge/dag
				public bool IsAvailable(List<Meeting> meetings, int isoYear, int isoWeek, int isoDay, int startHour, int endHour)
				{
					foreach (Meeting meeting in meetings)
					{
						if (meeting.RoomId == Id &&
							meeting.IsoYear == isoYear &&
							meeting.IsoWeek == isoWeek &&
							meeting.IsoDay == isoDay)
						{
							bool overlap = startHour < meeting.EndHour && endHour > meeting.StartHour;

							if (overlap)
							{
								return false;
							}
						}
					}

					return true;
				}
			}

			public class Meeting
			{
				public int IsoYear;
				public int IsoWeek;
				public int IsoDay; // 1=Man ... 5=Fre

				// Forenklet møde-model der kun har helt tal mellem 8-17 som værdi
				public int StartHour;
				public int EndHour;

				public int RoomId; // vi gemmer nu id på mødelokalet i stedet for en tekststreng, så vi kan koble mødet til et fast defineret lokale
				public string Participants = "";
				public string Note = "";

				public Meeting() { } // parameterløs constructor nødvendig for JSON deserialization

				public override string ToString()
				{
					return $"{TimeRangeText()}  Lokale-id: {RoomId}  Deltagere: {Participants}  Note: {Note}";
				}

				public string TimeRangeText() // metode der laver heltallene StartHour og EndHour om til et tekst-format der kan vises i cellen
				{
					return $"{StartHour:00}:00-{EndHour:00}:00";
				}

				public bool CoversHour(int hour) // metode der tjekker om dette møde dækker det givne time-slot
				{
					return hour >= StartHour && hour < EndHour;
				}
			}

			//  Aflysning
			static void CancelFlow(List<Meeting> meetings, int isoYear, int isoWeek, MeetingRoom selectedRoom) // metode der håndterer hele flowet for at aflyse et møde
			{
				Console.Clear();

				var weekMeetings = meetings
					.Where(m => m.IsoYear == isoYear && m.IsoWeek == isoWeek && m.RoomId == selectedRoom.Id)
					.OrderBy(m => m.IsoDay)
					.ThenBy(m => m.StartHour)
					.ToList();

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("AFLYS MØDE");
				Console.ResetColor();
				Console.WriteLine();
				Console.WriteLine($"Valgt mødelokale: {selectedRoom.Name}");
				Console.WriteLine();

				if (weekMeetings.Count == 0)
				{
					Console.WriteLine("Der er ingen møder at aflyse i denne uge for dette lokale. Tryk på en tast for at gå tilbage...");
					Console.ReadKey(true);
					return;
				}

				DateTime monday = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday); // find datoen for mandagen i den uge vi kigger på

				for (int i = 0; i < weekMeetings.Count; i++)
				{
					var m = weekMeetings[i];
					DateTime meetingDate = monday.AddDays(m.IsoDay - 1);

					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine($"{i + 1}. {meetingDate:dd/MM}  {m.TimeRangeText()}  Lokale: {selectedRoom.Name}  Deltagere: {m.Participants}  Note: {m.Note}");
					Console.ResetColor();
				}

				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Vælg nummer der skal slettes (Enter for at annullere): ");
				Console.ResetColor();

				string input = Console.ReadLine() ?? "";
				if (string.IsNullOrWhiteSpace(input))
					return;

				if (!int.TryParse(input, out int choice) || choice < 1 || choice > weekMeetings.Count)
				{
					Console.WriteLine("Ugyldigt valg. Tryk en tast...");
					Console.ReadKey(true);
					return;
				}

				var target = weekMeetings[choice - 1];

				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Er du sikker? Tryk J for JA eller N for NEJ: ");
				Console.ResetColor();

				var confirmKey = Console.ReadKey(true).Key;
				if (confirmKey == ConsoleKey.J)
				{
					meetings.Remove(target);
					SaveMeetings(meetings);

					Console.WriteLine();
					Console.WriteLine("Følgende møde er slettet: ");
					Console.ResetColor();
					Console.WriteLine();
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.WriteLine($"============== [SLETTET] {target.TimeRangeText()}  Lokale: {selectedRoom.Name}  Deltagere: {target.Participants}  Note: {target.Note} [SLETTET] ===============");
					Console.ResetColor();

					Console.WriteLine("Tryk en tast...");
					Console.ReadKey(true);
				}
				else
				{
					Console.WriteLine("Annulleret. Tryk på en hvilkensomhelst tast...");
					Console.ReadKey(true);
				}
			}

			static void SaveMeetings(List<Meeting> meetings)
			{
				var options = new JsonSerializerOptions
				{
					WriteIndented = true,
					IncludeFields = true
				};

				string json = JsonSerializer.Serialize(meetings, options);
				File.WriteAllText(filePath, json);
			}

			static List<Meeting> LoadMeetings()
			{
				if (!File.Exists(filePath))
				{
					return new List<Meeting>();
				}

				var options = new JsonSerializerOptions
				{
					IncludeFields = true
				};

				string json = File.ReadAllText(filePath);
				List<Meeting>? meetings = JsonSerializer.Deserialize<List<Meeting>>(json, options);

				return meetings ?? new List<Meeting>();
			}
		}
	}
}