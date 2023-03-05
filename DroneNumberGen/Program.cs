using Newtonsoft.Json.Linq;

namespace DroneNumberGen;

internal static class Program
{
    public static void Main(string[] args)
    {
        var savedData = SavedData.Load();

        while (true)
        {
            Console.WriteLine("Press enter to make a new drone number or type 'exit' to exit");
            var input = Console.ReadLine();
            if (input == "exit")
            {
                break;
            }
            const int max = 9999;
            var randomNumber = Random.Shared.Next(0, max);
            while (savedData?.AssignedIds.Contains(randomNumber) ?? false)
            {
                randomNumber = Random.Shared.Next(0, max);
                if (savedData.AssignedIds.Count != max) continue;
                Console.WriteLine("All drone numbers have been assigned, exiting... oh shit ~8726");
                break;
            }
            Console.WriteLine($"Drone number: {randomNumber:0000}");
            savedData ??= new SavedData();
            savedData.AssignedIds.Add(randomNumber);
            savedData.Save();
        }
    }
}


public class SavedData
{
    public void Save()
    {
        var jObj = JObject.FromObject(this);
        File.WriteAllText("savedData.json", jObj.ToString());
    }

    public static SavedData? Load()
    {
        try
        {
            var jObject = JObject.Parse(File.ReadAllText("savedData.json"));
            return jObject.ToObject<SavedData>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public List<int> AssignedIds { get; set; } = new();
}