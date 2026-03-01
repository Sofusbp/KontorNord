namespace KontorNord
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string workerID = "";
            string name = "";
            bool isValidLogin = false;

            while (!isValidLogin)
            {
                Console.Clear();
                Console.WriteLine("=========================");
                Console.WriteLine("KONTOR NORD BOOKINGSYSTEM");
                Console.WriteLine("=========================");
                Console.WriteLine("");
                Console.WriteLine("");

                Console.WriteLine("Indtast dit medarbejder ID for at begynde: ");
                workerID = Console.ReadLine();



                if (workerID == "SOM")
                {
                    name = "Sofie Møller";
                    isValidLogin = true;
                }
                else if (workerID == "AMR")
                {
                    name = "Amir Rahimi";
                    isValidLogin = true;
                }
                else if (workerID == "JOT")
                {
                    name = "Jonas Tved";
                    isValidLogin = true;
                }
                else if (workerID == "LOF")
                {
                    name = "Louise Falk";
                    isValidLogin = true;
                }
                else if (workerID == "MEA")
                {
                    name = "Mette Ates";
                    isValidLogin = true;
                }
                else if (workerID == "HEK")
                {
                    name = "Henrik Krøll";
                    isValidLogin = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Ugyldigt medarbejder ID!");
                    Console.WriteLine("Tryk på en tast for at prøve igen...");
                    Console.ReadLine(); 
                }
            }
           
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
