namespace merge2projektknontornord
{
	namespace ProjektNordUgevisning_Aflysning_af_møde_isa
	{
		using System;
		using System.Collections.Generic; // for List<>
		using System.Globalization; // til ISOWeek
		using System.IO; // for File I/O hvis vi skulle gemme møderne i en fil (ikke implementeret endnu)
		using System.Linq; // for Where, OrderBy, FirstOrDefault
        using System.Reflection;
        using System.Runtime.Intrinsics.X86;
		using System.Text.Json; // for JSON serialization hvis vi skulle gemme møderne i en fil (ikke implementeret endnu)

		internal class Programg
		{
			static string filePath = "meetings.json"; // filsti for hvor møderne skal gemmes. Vi gemmer møderne i en JSON-fil i samme mappe som programmet, så det er nemt at finde.

			static void Main(string[] args)
			{
				Console.OutputEncoding = System.Text.Encoding.UTF8; // for at kunne bruge pile-symboler i menuen og ÆØÅ
				Console.Title = "♥♥ Bookingsystem - KontorNord ♥♥"; // Titel på konsol-vinduet <3 
				Console.ResetColor();


				// Konsol-indstillinger for at gøre det pænere og mere brugervenligt, uden dette åbnes konsollen i en for lille størrelse, som gør at fx uge grid og andre informationer ikke kan tilgås.
				int width = Math.Min(150, Console.LargestWindowWidth); // Sætter bredden på konsol-vinduet, ved at tage den største bredde og sætte et max på 150 for at undgå at det bliver alt for bredt.
				int height = Console.LargestWindowHeight - 2; // Sætter højden på konsol - vinduet, ved at tage den største højde og trække 2 fra for at undgå scroll - bar i bunden.Højden kan ikke sættes statisk, da det afhænger af brugerens skærmopløsning.
				Console.SetBufferSize(width, height); // Sætter buffer-størrelsen til samme som vinduesstørrelsen, for at undgå scroll-bar i bunden og siden.
				Console.SetWindowSize(width, height); // Sætter vinduesstørrelsen til den tidligere definerede bredde og højde. Det er vigtigt at sætte buffer-størrelsen før vinduesstørrelsen, for at undgå fejl i konsollen.

				string workerID = "";    // Gemmer det medarbejder ID som brugeren indtaster				 
				string name = "";        // Gemmer medarbejderens fulde navn, som senere bruges i velkomstbeskeden.
				string password = "";    // Gemmer adgangskoden som brugeren indtaster
				bool isValidLogin = false; // Der bruges en boolean der bestemmer om login er godkendt. Så længe den er false, så kører login loopet videre.

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
					Console.WriteLine("");

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
				Console.WriteLine("======================");
				Console.WriteLine($"VELKOMMEN {name}");
				Console.WriteLine("======================");
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
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("============================");
					Console.WriteLine($"  Velkommen, {name} "); // Det første man ser efter log-in 
					Console.WriteLine("============================");
					Console.ResetColor();
					Console.WriteLine(""); // Lidt spacing
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"Aktuelt lokale: {rooms[selectedRoomIndex].Name}");
					Console.WriteLine($"Aktuel uge: {isoWeek}");
					Console.ResetColor();
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



								bool isConfirmed = ConfirmMeeting(newMeeting, selectedRoom);  // bool isConfirmed tager return værdien fra ConfirmMeeting metoden som tager 2 parametere,
																							  // som er enten true eller false, alt efter om brugeren bekræfter mødet eller ej.
																							  // if()
																							  // Hvis isConfirmed ==true, så viser det en bekræftelse af mødet beskeden 
																							  // og newMeetings er tilføjet til .meetings listen og gemt. Så mødet er oprettet og bekræftet.                                     
																							  // else
																					      	  // Hvis isConfirmed == false, så vise det beskeden som (Console.Writeline())
																						      //Brugeren kommer tilbage til menuen uden at gemme mødet.

								if (isConfirmed)
								{
									Console.WriteLine("Mødet er bekræftet."); // denne besked bliver ikke vist i det nuværende flow, da ConfirmMeeting-metoden også spørger om bekræftelse
									meetings.Add(newMeeting);  // Tilføjer det nye møde til listen over møder som gemmes i programmet. Det er vigtigt at dette sker EFTER bekræftelsen, så mødet ikke bliver gemt hvis brugeren fortryder i bekræftelses-steget.
									SaveMeetings(meetings);	  // Gemmer møderne i JSON-filen ved at kalde på SaveMeetings-metoden.
									Console.ForegroundColor = ConsoleColor.Blue;
									Console.WriteLine();
									Console.WriteLine("Mødet er nu oprettet.");

								}
								else
								{
									Console.WriteLine("Mødet er ikke bekræftet.");
								}

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
				Console.WriteLine("1) KontorNord Kalender");
				Console.WriteLine("2) Opret møde og book mødelokale");
				Console.WriteLine("3) Aflys et møde");
				Console.WriteLine("4) Exit");
				Console.WriteLine("");
				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Console.Write("Vælg nummer og afslut med <Enter>: ");
				Console.ResetColor();
			}

			static void CalendarLoop(List<Meeting> meetings, List<MeetingRoom> rooms, ref int isoYear, ref int isoWeek, ref int selectedRoomIndex) // Metode der håndterer hoved-loopet for kalenderen, hvor møder vises og brugeren kan navigere mellem uger og lokaler
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

						case ConsoleKey.N:  // Her kalder vi metoden: ConfirmMeeting som tager newMeeting og selectedRoom som parametre.
											// Bool bruges til at tjekke om mødet skal gemmes eller ej. 
											// Hvis isConfirmed == true, så bliver newMeeting tilføjet til meetings listen og gemt i JSON filen.
											// Hvis isConfirmed == false, så vises en besked via (Console.Writeline("Mødet er ikke bekræftet....))
										    // hvor efter brugeren kommer tilbage til menuen uden at gemme mødet.
											
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
								bool isConfirmed = ConfirmMeeting(newMeeting, selectedRoom);
								if (isConfirmed)
								{
									Console.WriteLine("Mødet er bekræftet.");
									meetings.Add(newMeeting);
									SaveMeetings(meetings);
								}
								else
								{
									Console.WriteLine("Mødet er ikke bekræftet.Tast en af keyboard.Du kommer tilbage til menu nu.");
									Console.ReadLine();

								}

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

							if (meeting != null) // Tjekker om der findes et møde i den aktuelle celle i kalenderen
							{
								Console.BackgroundColor = ConsoleColor.Blue; //Gør farven på et booket møde til blå

								string cellText; // Variabel der skal indeholde teksten der vises i kalendercellen
								if (innerLine == 0) // Hvis det er første linje i cellen (IndexStart == 0)
									cellText = $"{meeting.TimeRangeText()}  {selectedRoom.Name}"; // Viser mødetid og mødelokalets navn
								else // Hvis det er anden linje i cellen
									cellText = $"{meeting.Participants} | {meeting.Note}"; // Viser deltagere og eventuel note til mødet

								Console.Write(cellText.PadRight(dayColWidth) + "|"); // Udskriver teksten og fylder resten af cellens bredde med mellemrum
								Console.ResetColor();
							}
							else // Hvis der ikke findes et møde i denne celle
							{
								Console.Write("".PadRight(dayColWidth) + "|"); // Udskriver en tom celle med samme bredde som de andre
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

			static Meeting BuildMeetingFromBookingFlow(int isoYear, int isoWeek, MeetingRoom selectedRoom, string day, int startHour, int endHour, List<string> participants, string note) // Metode der opretter et Meeting-objekt ud fra booking-flowets input
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

				return m; // Returnerer det færdige Meeting-objekt
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


			
			static bool ConfirmMeeting(Meeting meeting, MeetingRoom selectedRoom)  
																					// ConfirmMeeting er en metode der returnerer bool som datatype.
																					// Den tager en Meeting og et MeetingRoom som parametre.
																					// Den kalder MeetingConfirmation metoden for at vise en besked om bekræftelse af mødet .
																					// OSB: Navngivningen er lidt forvirrende, men MeetingConfirmation er metoden der viser bekræftelses beskeden.
																					// ConfirmMeeting er metoden der returnerer bool og både viser bekræftelses beskeden via MeetingConfirmation
																					// og spørger brugeren om de vil bekræfte mødet eller ej, og så returnerer true/false alt efter svaret før retur til menu .
																					
			{
				MeetingConfirmation(meeting, selectedRoom);
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Er du sikker på at du vil bekræfte dette møde, før retur til menu? (ja/nej)");
				Console.ResetColor();
				string input = Console.ReadLine();

				while (true)                                                        // While-loop , der kører og tjekker indtil brugeren skriver "ja" eller "nej". 
																					// "ja", "nej" er case insensitive med build-in enum :Equals(StringComparison.OrdinalIgnoreCase
																					// if ()
																					// Hvis "ja", returneres true og mødet bliver bekræftet.
																					// Se i Main-metoden while (isRunning), case 2 , if sætningen, hvor det tjekkes om isConfirmed == true, og så bliver mødet tilføjet til listen og gemt meetings listen.
																					// else if ()
																					// Hvis false "nej", returneres false og mødet bliver ikke bekræftet. 
																					// else
																					// Hvis andet, så får brugeren en fejlbesked og kan prøve igen.
																					
				{
					if (input.Equals("ja", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					else if (input.Equals("nej", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Ugyldigt input. Indtast 'ja' eller 'nej'.");
						Console.ResetColor();
						input = Console.ReadLine();
					}
				}
			}

			static void MeetingConfirmation(Meeting meeting, MeetingRoom selectedRoom) // Metode, der giver info-display i form af gemte variabler i tidligere metoder
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
				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > 5) // Out = At tasten bliver sendt ud i variablen: "valg" -- Sikrer, at brugere ikke kan vælge et tal under 1 og over 5
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				switch (valg) // Her bruger jeg ikke "break", da jeg har "return" i switch. "Return", får koden til at stoppe automatisk
				{
					case 1: return "Mandag";
					case 2: return "Tirsdag";
					case 3: return "Onsdag";
					case 4: return "Torsdag";
					case 5: return "Fredag";
					default: return "Mandag";
				}
			}

			static int SelectStartTime() // Metode, der lader brugeren vælge en dag, mødet skal afholdes
			{
				Console.Clear();

				List<int> tider = new List<int>(); // Benytter en liste for at undgå mange cases i en switch -- Undgår senere for meget refactoring

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
					tider.Add(tid); // Tilføjer list item
					mulighed++; // Plusser med 1 for hver mulighed (en tæller)
				}

				Console.WriteLine("");
				Console.Write("Vælg nummer og afslut med <Enter>: ");

				int valg; // Opretter variablen 'valg', som skal gemme brugerens indtastede tal
				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > tider.Count) // Kører en løkke indtil brugeren indtaster et gyldigt tal mellem 1 og antal tider
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Indtast venligst et tal: ");
					Console.ResetColor();
				}

				return tider[valg - 1]; // - 1, da Index altid starter ved 0 (én forrige)
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

				for (int tid = startHour + 1; tid <= 18; tid++) // For-løkke der opretter mulige sluttider efter starttidspunktet
				{
					tider.Add(tid); // Tilføjer tidspunktet til listen 'tider'
				}

				for (int i = 0; i < tider.Count; i++) // Løber gennem alle tider i listen for at vise dem som valgmuligheder
				{
					Console.WriteLine($"{i + 1}) {tider[i]:00}:00");
				}

				Console.WriteLine("");
				Console.Write("Vælg nummer og afslut med <Enter>: ");

				int valg; // Variabel der gemmer brugerens valg

				while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > tider.Count) // Kører en løkke indtil brugeren indtaster et gyldigt tal mellem 1 og antal tider
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write($"Indtast et tal mellem 1 og {tider.Count}: ");
					Console.ResetColor();
				}

				return tider[valg - 1]; // Returnerer den valgte sluttid (minus 1 fordi liste-index starter ved 0)
			}

			static List<string> AddParticipants() // Metode, der tillader brugeren at tilføje detlagere til mødet
			{
				Console.Clear();

				List<string> participants = new List<string>();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("============================");
				Console.WriteLine("       VÆLG DELTAGER(E)");
				Console.WriteLine("============================");
				Console.ResetColor();
				Console.WriteLine("");

				List<string> employees = new List<string> // Liste, vi skriver mulige deltagere i
			{
				"Sofie Møller (SM)",
				"Jonas Tved (JT)",
				"Amir Rahimi (AR)",
				"Louise Falk (LF)",
				"Mette Ates (MA)",
				"Henrik Krøll (HK)",
			};

				char svar = 'j'; // Hvis brugeren indtaster 'j' (ja), så fortsætter loopet


				while (char.ToLower(svar) == 'j') // Hvis svaret er lig med 'j', så fortsætter loopet
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("============================");
					Console.WriteLine("       VÆLG DELTAGER(E)");
					Console.WriteLine("============================");
					Console.ResetColor();
					Console.WriteLine("");

					for (int i = 0; i < employees.Count; i++) // For-løkke der gennemløber alle medarbejdere i listen 'employees'
					{
						Console.WriteLine($"{i + 1}) {employees[i]}");
					}

					Console.WriteLine("");
					Console.Write("Vælg en ansat og afslut med <Enter>: ");

					int valg; // Variabel der gemmer brugerens indtastede valg

					while (!int.TryParse(Console.ReadLine(), out valg) || valg < 1 || valg > employees.Count) // Sikrer, at brugeren kun kan vælge mellem antallet af medarbejdere i Index
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write("Indtast venligst et tal: ");
						Console.ResetColor();
					}

					string valgtDeltager = employees[valg - 1];  // Validerer input: sikrer at input er et tal og inden for listen employees

					if (!participants.Contains(valgtDeltager)) // Tjekker om deltageren allerede findes i listen 'participants'
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
					svar = Console.ReadKey().KeyChar; // Læser et enkelt tegn fra tastaturet og gemmer det i variablen 'svar'

					Console.WriteLine("");
				}

				Console.WriteLine("Du har nu valgt deltager(e) til mødet:");

				foreach (var participant in participants) // 'var', da C# godt kan læse en simpel string variabel
				{
					Console.WriteLine(participant);
				}

				Console.WriteLine("Tryk på en tast for at fortsætte...");
				Console.ReadKey(true); // Venter på et tastetryk før programmet fortsætter (true skjuler den tast brugeren trykker)

				return participants; // Returnerer listen med alle valgte deltagere til den metode der kaldte denne metode
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

			public class MeetingRoom // Definerer klassen MeetingRoom, som repræsenterer et mødelokale
			{
				public int Id; // Gemmer et unikt ID for mødelokalet
				public string Name = ""; // Gemmer navnet på mødelokalet (initialiseret som tom tekst)
				public int Capacity; // Gemmer hvor mange personer lokalet maksimalt kan rumme

				public MeetingRoom() { } // parameterløs constructor så JSON og andre dele af programmet ikke brokker sig hvis vi senere vil gemme lokaler også

				public MeetingRoom(int id, string name, int capacity)  // Constructor der opretter et mødelokale med alle nødvendige værdier
				{
					Id = id;
					Name = name;
					Capacity = capacity;
				}

				// metode der tjekker om lokalet er ledigt i et bestemt tidsrum i en bestemt uge/dag
				public bool IsAvailable(List<Meeting> meetings, int isoYear, int isoWeek, int isoDay, int startHour, int endHour)  // Metode der tjekker om mødelokalet er ledigt i et bestemt tidsrum
				{
					foreach (Meeting meeting in meetings) // Gennemløber alle møder i listen 'meetings'
					{
						if (meeting.RoomId == Id && // Tjekker om mødet foregår i dette mødelokale
							meeting.IsoYear == isoYear && // Tjekker om mødet er i samme år
							meeting.IsoWeek == isoWeek && // Tjekker om mødet er i samme uge
							meeting.IsoDay == isoDay) // Tjekker om mødet er på samme dag
						{
							bool overlap = startHour < meeting.EndHour && endHour > meeting.StartHour; // Undersøger om det ønskede tidsrum overlapper med et eksisterende møde

							if (overlap) // Hvis tiderne overlapper
							{
								return false;  // Returnerer false fordi lokalet ikke er ledigt
							}
						}
					}

					return true;  // Returnerer true hvis der ikke blev fundet nogen overlappende møder
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
			static void CancelFlow(List<Meeting> meetings, int isoYear, int isoWeek, MeetingRoom selectedRoom)  // Metode der håndterer hele processen for at aflyse et møde
			{
				Console.Clear();

				var weekMeetings = meetings  // Opretter en variabel der skal indeholde filtrerede møder
					.Where(m => m.IsoYear == isoYear && m.IsoWeek == isoWeek && m.RoomId == selectedRoom.Id) // Filtrerer møder så kun dem fra valgt år, uge og lokale medtages
					.OrderBy(m => m.IsoDay) // Sorterer møderne efter hvilken dag i ugen de ligger
					.ThenBy(m => m.StartHour) // Sorterer derefter møderne efter starttidspunkt
					.ToList(); // Konverterer resultatet til en liste

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("AFLYS MØDE");
				Console.ResetColor();
				Console.WriteLine();
				Console.WriteLine($"Valgt mødelokale: {selectedRoom.Name}");
				Console.WriteLine();

				if (weekMeetings.Count == 0) // Tjekker om listen med ugens møder er tom
				{
					Console.WriteLine("Der er ingen møder at aflyse i denne uge for dette lokale. Tryk på en tast for at gå tilbage..."); 
					Console.ReadKey(true);  // Venter på et tastetryk før programmet fortsætter
					return; // Afslutter metoden og går tilbage hvis der ikke findes møder at aflyse
				}

				DateTime monday = ISOWeek.ToDateTime(isoYear, isoWeek, DayOfWeek.Monday); // find datoen for mandagen i den uge vi kigger på

				for (int i = 0; i < weekMeetings.Count; i++) // For-løkke der gennemløber alle møder i listen weekMeetings
				{
					var m = weekMeetings[i];  // Henter det aktuelle møde fra listen
					DateTime meetingDate = monday.AddDays(m.IsoDay - 1);  // Beregner den præcise dato for mødet ved at lægge dag-forskellen til mandagen

					Console.ForegroundColor = ConsoleColor.Blue;
					Console.WriteLine($"{i + 1}. {meetingDate:dd/MM}  {m.TimeRangeText()}  Lokale: {selectedRoom.Name}  Deltagere: {m.Participants}  Note: {m.Note}");
					Console.ResetColor();
				}

				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Vælg nummer der skal slettes (Enter for at annullere): ");
				Console.ResetColor();

				string input = Console.ReadLine() ?? ""; // Læser brugerens input fra konsollen, eller sætter tom tekst hvis input er null
				if (string.IsNullOrWhiteSpace(input)) // Tjekker om input er tomt eller kun indeholder mellemrum
					return; // Afslutter metoden hvis brugeren ikke har indtastet noget

				if (!int.TryParse(input, out int choice) || choice < 1 || choice > weekMeetings.Count) // Validerer at input er et tal og inden for gyldigt interval
				{
					Console.WriteLine("Ugyldigt valg. Tryk en tast...");
					Console.ReadKey(true);
					return;
				}

				var target = weekMeetings[choice - 1];  // Finder det valgte møde i listen (minus 1 fordi liste-index starter ved 0)

				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write("Er du sikker? Tryk J for JA eller N for NEJ: ");
				Console.ResetColor();

				var confirmKey = Console.ReadKey(true).Key; // Læser hvilken tast brugeren trykker (skjult i konsollen)
				if (confirmKey == ConsoleKey.J)  // Tjekker om brugeren bekræfter med tasten 'J'
				{
					meetings.Remove(target); // Fjerner det valgte møde fra listen 'meetings'
					SaveMeetings(meetings);  // Gemmer den opdaterede mødeliste efter sletningen

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

			static void SaveMeetings(List<Meeting> meetings) // Metode der gemmer alle møder til en JSON-fil
			{
				var options = new JsonSerializerOptions  // Opretter konfigurationsindstillinger til JSON-serialisering
				{
					WriteIndented = true, // Gør JSON-filen pænt formateret og lettere at læse
					IncludeFields = true // Sikrer at klassens fields også bliver gemt i JSON
				};

				string json = JsonSerializer.Serialize(meetings, options); // Konverterer listen af møder til en JSON-string
				File.WriteAllText(filePath, json); // Skriver JSON-dataen til filen på den angivne filsti
			}

			static List<Meeting> LoadMeetings()  // Metode der indlæser møder fra JSON-filen
			{
				if (!File.Exists(filePath)) // Tjekker om filen eksisterer
				{
					return new List<Meeting>(); // Returnerer en tom liste hvis filen ikke findes endnu
				}

				var options = new JsonSerializerOptions // Opretter konfigurationsindstillinger til JSON-deserialisering
				{
					IncludeFields = true // Sikrer at fields også læses korrekt fra JSON
				};

				string json = File.ReadAllText(filePath); // Læser hele JSON-filen ind som tekst
				List<Meeting>? meetings = JsonSerializer.Deserialize<List<Meeting>>(json, options); // Konverterer JSON-tekst til en liste af Meeting-objekter

				return meetings ?? new List<Meeting>(); // Returnerer listen, eller en tom liste hvis deserialisering fejlede
			}
		}
	}
}