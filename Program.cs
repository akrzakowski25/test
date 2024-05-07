using RestMockCore;

namespace proj;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("\nType option: ");
        Console.WriteLine("1. Register ");
        Console.WriteLine("2. Backup");

        var ans = Console.ReadLine();

        switch(ans)
        {
            case "1":
                ForRegistration();
                break;
            case "2":
                Console.WriteLine("\nType option: ");
                Console.WriteLine("1. UnavailableForLegalReasons ");
                Console.WriteLine("2. Forbidden");
                Console.WriteLine("3. Other");
                var ans2 = Console.ReadLine();
                switch(ans2)
                {
                     case "1":
                        ForBackup(System.Net.HttpStatusCode.UnavailableForLegalReasons);
                        break;
                    case "2":
                        ForBackup(System.Net.HttpStatusCode.Forbidden);
                        break;
                   case "3":
                    ForBackup(System.Net.HttpStatusCode.NotAcceptable);
                        break;
                    default:
                        return;
                }
                ForRegistration();
                break;
            default:
                Console.WriteLine("ERROR not supported option");
                break;
        }
            
    }

    static void ForRegistration()
    {
        using HttpServer mockServer = new HttpServer(5004);
        mockServer.Config.Get("/api/v3/user").Send(File.ReadAllText(@"./user.json"), System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/user/orgs").Send("[]", System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/user/repos?sort=full_name&per_page=100&page=1").Send(File.ReadAllText(@"./repos.json"), System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/user/repos?sort=full_name&per_page=100&page=2").Send("[]", System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/repos/akrzakowski25/test1").Send(File.ReadAllText(@"./repo.json"), System.Net.HttpStatusCode.OK);

        mockServer.Run();
        Console.WriteLine("Listening. Press any key to exit");
        Console.ReadKey();
    }

    static void ForBackup(System.Net.HttpStatusCode code)
    {
        using HttpServer mockServer = new HttpServer(5004);
        mockServer.Config.Get("/api/v3/user").Send(File.ReadAllText(@"./user.json"), System.Net.HttpStatusCode.OK);

        mockServer.Config.Get("/api/v3/user/repos?sort=full_name&per_page=100&page=1").Send(File.ReadAllText(@"./repos.json"), System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/user/repos?sort=full_name&per_page=100&page=2").Send("[]", System.Net.HttpStatusCode.OK);
        mockServer.Config.Get("/api/v3/repos/akrzakowski25/test1").Send(File.ReadAllText(@"./repo.json"), System.Net.HttpStatusCode.OK);

        mockServer.Config.Get("/api/v3/user/orgs?per_page=100&page=1").Send("unav", code);
        mockServer.Run();

        Console.WriteLine("Listening. Press any key to exit");
        Console.ReadKey();
    }
}
