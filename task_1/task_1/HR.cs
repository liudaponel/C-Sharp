using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_1
{
    internal class HR
    {
        public Dictionary<int, int> Distribute(List<Player> teamLeads, List<Player> juniors)
        {
            // key - junior id, value - teamLead id
            var teams = new Dictionary<int, int>();

            foreach (Player tl in teamLeads)
            {
                foreach(int junior in tl.WishList)
                {
                    if (!teams.ContainsKey(junior))
                    {
                        teams.Add(junior, tl._nameId);
                        break;
                    }
                }
            }

            return teams;
        }
    }
}
