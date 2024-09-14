
using System.Reflection;

namespace task_1
{
    public enum PlayerType
    {
        Junior,
        TeamLead
    }

    internal class Player
    {
        private readonly PlayerType _type;
        public readonly int _nameId;
        public List<int> WishList;

        public Player(PlayerType type, int numTeams, int nameId) 
        { 
            _type = type;
            _nameId = nameId;
            GenerateWishList(numTeams);
        }

        private void GenerateWishList(int numTeams)
        {
            string fileName = "";

            switch (_type)
            {
                case PlayerType.TeamLead:
                    fileName = (numTeams == 5) ? "task_1.resources.Juniors5.csv" : "task_1.resources.Juniors20.csv";
                    break;
                case PlayerType.Junior:
                    fileName = (numTeams == 5) ? "task_1.resources.Teamleads5.csv" : "task_1.resources.Teamleads20.csv";
                    break;
            }

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(fileName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    reader.ReadLine();
                    var lines = reader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var ids = lines
                        .Select(line => int.Parse(line.Split(';')[0]))
                        .ToList();

                    var random = new Random();
                    WishList = ids.OrderBy(x => random.Next()).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
