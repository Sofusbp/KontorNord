using System.ComponentModel;

namespace KontorNord
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string workerID = "";
            string name = "";
            string password = "";
            bool isValidLogin = false;

            while (!isValidLogin) // Så længe der ikke kommer et "ValidLogin" kører loopet videre og brugeren kan forsøge igen.
            {
                Console.Clear();
                Console.WriteLine("=========================");
                Console.WriteLine("KONTOR NORD BOOKINGSYSTEM");
                Console.WriteLine("=========================");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("Indtast dit medarbejder ID for at begynde: ");
                workerID = Console.ReadLine();


                // Selve WorkerID godkendelsen - der er brugt de to første initialer i fornavn og et inital i efternavn
                if (workerID == "SOM")
                {
                    name = "Sofie Møller";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "MOS")
                    {
                        isValidLogin = true;
                    }
                }
                else if (workerID == "AMR")
                {
                    name = "Amir Rahimi";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "RMA")
                    {
                        isValidLogin = true;
                    }
                }
                else if (workerID == "JOT")
                {
                    name = "Jonas Tved";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "TOJ")
                    {
                        isValidLogin = true;
                    }
                }
                else if (workerID == "LOF")
                {
                    name = "Louise Falk";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "FOL")
                    {
                        isValidLogin = true;
                    }
                }
                else if (workerID == "MEA")
                {
                    name = "Mette Ates";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "EAM")
                    {
                        isValidLogin = true;
                    }
                }
                else if (workerID == "HEK")
                {
                    name = "Henrik Krøll";

                    Console.WriteLine("Indtast kodeord: ");
                    password = Console.ReadLine();
                    if (password == "KEH")
                    {
                        isValidLogin = true;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Ugyldigt medarbejder ID!");
                    Console.WriteLine("Tryk på en tast for at prøve igen...");
                    Console.ReadLine(); 
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
            Console.WriteLine("=========================");
            Console.WriteLine($"VELKOMMEN {name}");
            Console.WriteLine("=========================");
            Console.WriteLine("");
            Console.WriteLine("Tryk på en tast for at komme ind i menuen");
            Console.ReadLine();
        }
    }
}
