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

        public class AmbiguousLocationMatch : Exception { }

        public static Location[] getLocationsByName(string name)
        {
            //UNDONE
            return Array.Empty<Location>();
        }
    }

    
}
