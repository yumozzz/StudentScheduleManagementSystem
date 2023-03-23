namespace StudentScheduleManagementSystem.Map
{

    public class Location
    {
        public string PlaceName{ get; init; }

        public int Id { get; init; }

        public static void ArrangeForRoutes(Location[] locations)
        {
            //UNDONE
            Console.WriteLine("test");
        }

        public static Location[] getLocationsByName(string name)
        {
            //UNDONE
            return new Location[1]{new(){Id=0,PlaceName="default match"}};
        }
    }

    
}
