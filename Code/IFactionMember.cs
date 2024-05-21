using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public enum Faction
    {
        Neutral,
        Player,
        Enemy,
    }

    public interface IFactionMember
    {
        Faction Faction { get; }
    }
}
