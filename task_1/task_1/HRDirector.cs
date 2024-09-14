using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_1
{
    internal class HRDirector
    {
        public double CountHarmonic(Dictionary<int, int> teams, List<Player> teamLeads, List<Player> juniors)
        {
            List<int> list = new List<int>();
            foreach(KeyValuePair<int, int> kvp in teams)
            {
                foreach (Player tl in teamLeads)
                {
                    if(tl._nameId == kvp.Value)
                    {
                        list.Add(20 - tl.WishList.IndexOf(kvp.Key));
                        break;
                    }
                }

                foreach (Player junior in juniors)
                {
                    if (junior._nameId == kvp.Key)
                    {
                        list.Add(20 - junior.WishList.IndexOf(kvp.Value));
                        break;
                    }
                }
            }

            return list.Count() / (list.Select(x => 1.0 / x)
                                       .Sum());
        }
    }
}
