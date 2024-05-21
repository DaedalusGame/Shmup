using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    internal interface IWeapon
    {
        Player Player { get; set; }
    }
}
