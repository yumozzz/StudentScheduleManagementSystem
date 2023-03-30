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

        public static Location[] GetLocationsByName(string name)
        {
            //UNDONE
            return new Location[] { new() { Id = 0, PlaceName = "default match" } };
        }
    }

    
}
