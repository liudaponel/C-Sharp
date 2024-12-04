using Nsu.HackathonProblem.Contracts;

public class PonomarevaTeamBuildingStrategy : ITeamBuildingStrategy
{
    public IEnumerable<Team> BuildTeams(
        IEnumerable<Employee> teamLeads,
        IEnumerable<Employee> juniors,
        IEnumerable<Wishlist> teamLeadsWishlists,
        IEnumerable<Wishlist> juniorsWishlists)
    {
        var teamLeadsList = teamLeads.ToList();
        var juniorsList = juniors.ToList();

        if (teamLeadsList.Count != juniorsList.Count)
        {
            throw new Exception("Количество teamleads и juniors должно быть одинаковое");
        }

        var teamCount = teamLeadsList.Count;

        // индекс по ID сотрудника
        var teamLeadIndexMap = CreateIndexMap(teamLeadsList);
        var juniorIndexMap = CreateIndexMap(juniorsList);

        var teamLeadWishlists = CreateWishlistMap(teamLeadsWishlists);
        var juniorWishlists = CreateWishlistMap(juniorsWishlists);

        int[,] matrix = BuildMatrix(
            teamLeadsList,
            juniorsList,
            teamLeadWishlists,
            juniorWishlists,
            teamCount);

        // распределение с использованием алгоритма Венгера
        var assignments = HungarianAlgorithm.HungarianAlgorithm.FindAssignments(matrix);

        return BuildTeams(teamLeadsList, juniorsList, assignments);
    }

    private Dictionary<int, int> CreateIndexMap(IEnumerable<Employee> employees)
    {
        return employees
            .Select((employee, index) => new { employee.Id, index })
            .ToDictionary(x => x.Id, x => x.index);
    }

    private Dictionary<int, int[]> CreateWishlistMap(IEnumerable<Wishlist> wishlists)
    {
        return wishlists
            .ToDictionary(wishlist => wishlist.EmployeeId, wishlist => wishlist.DesiredEmployees);
    }

    private int[,] BuildMatrix(
        List<Employee> teamLeads,
        List<Employee> juniors,
        Dictionary<int, int[]> teamLeadWishlists,
        Dictionary<int, int[]> juniorWishlists,
        int teamCount)
    {
        var costMatrix = new int[teamCount, teamCount];

        for (int i = 0; i < teamCount; ++i)
        {
            for (int j = 0; j < teamCount; ++j)
            {
                var teamLead = teamLeads[i];
                var junior = juniors[j];

                var teamLeadWishlist = teamLeadWishlists.GetValueOrDefault(teamLead.Id, Array.Empty<int>());
                var juniorWishlist = juniorWishlists.GetValueOrDefault(junior.Id, Array.Empty<int>());

                int preferenceScore = CalculatePreferenceScore(teamLeadWishlist, junior.Id) +
                                      CalculatePreferenceScore(juniorWishlist, teamLead.Id);

                // Отрицательный знак, так как алгоритм работает с минимизацией затрат
                costMatrix[i, j] = -preferenceScore;
            }
        }

        return costMatrix;
    }

    private int CalculatePreferenceScore(int[] wishlist, int employeeId)
    {
        int index = Array.IndexOf(wishlist, employeeId);
        
        if (index < 0) return 0;

        return wishlist.Length - index;
    }

    private IEnumerable<Team> BuildTeams(List<Employee> teamLeads, List<Employee> juniors, int[] assignments)
    {
        var teams = new List<Team>();

        for (int i = 0; i < assignments.Length; ++i)
        {
            int juniorIdx = assignments[i];
            if (juniorIdx >= 0 && juniorIdx < juniors.Count)
            {
                teams.Add(new Team(teamLeads[i], juniors[juniorIdx]));
            }
        }

        return teams;
    }
}
