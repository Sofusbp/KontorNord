using KontorNord;

internal class Program
{
    static void Main(string[] args)
    {
        Meeting meeting = new Meeting();
        meeting.IsoYear = 2024;
        meeting.IsoWeek = 24;
        meeting.IsoDay = 2; 
        meeting.StartHour = 8;
        meeting.EndHour = 10;
        meeting.Room = "Mødelokale 1";
        meeting.Participants = "Alice, Bob, Charlie";
        meeting.Note = "Diskussion af projektstatus";

        bool isConfirmed = ConfirmMeeting(meeting);
        //return value (ture/false) of the method:ConfirmMeeting is stored in a variable:isConfirmed (this case, "true").
        if (isConfirmed) 
        {
            Console.WriteLine("Mødet er bekræftet.");
        }
        else //else ---> if the value is false, then this block will be executed.
        {
            Console.WriteLine("Mødet er ikke bekræftet.Tast en af keyboard.Du kommer tilbage til menu nu.");
            Console.ReadLine();
        }


    }
    static bool ConfirmMeeting (Meeting meeting)
    {
        Console.WriteLine("Vil du bekræfte dette møde? (ja/nej)");
        Console.WriteLine(meeting.ToString());
        string input = Console.ReadLine();
     
        while (true) //While true ---> keep asking until valid input is received.
        {
            if (input.Equals("ja", StringComparison.OrdinalIgnoreCase))
            {
                return true; //return ---> Finished the methods regardless of true or false.
            }
            else if (input.Equals("nej", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Ugyldigt input. Indtast 'ja' eller 'nej'.");
                input = Console.ReadLine();
            } 
        }
   
    }
}