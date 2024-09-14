using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace task_1
{
    internal class Hakaton
    {
        private readonly int _numTeams;

        public Hakaton(int numTeams)
        {
            this._numTeams = numTeams;
        }

        public void Start()
        {
            double countRounds = 1000.0;
            double sumHarmonics = 0;
            for(int i = 0; i < countRounds; i++)
            {
                double harmonic = Round();
                Console.WriteLine(harmonic);
                sumHarmonics += harmonic;
            }
            double main_harmpnic = sumHarmonics / countRounds;
            Console.WriteLine("Общее среднее гармоническое: " + main_harmpnic);
        }

        private double Round()
        {
            var teamLeads = new List<Player>();
            var juniors = new List<Player>();

            GeneratePlayers(teamLeads, juniors);

            var hr = new HR();
            Dictionary<int, int> teams = hr.Distribute(teamLeads, juniors);

            var hrDir = new HRDirector();
            return hrDir.CountHarmonic(teams, teamLeads, juniors);
        }

        private void GeneratePlayers(List<Player> teamLeads, List<Player> juniors)
        {
            for (int i = 1; i <= _numTeams; i++)
            {
                teamLeads.Add(new Player(PlayerType.TeamLead, _numTeams, i));
                juniors.Add(new Player(PlayerType.Junior, _numTeams, i));
            }
        }
    }
}
