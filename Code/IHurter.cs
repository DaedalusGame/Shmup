﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shmup.Code
{
    public interface IHurter : IFactionMember
    {
        public bool TakeDamage(float damage, bool contact = false);
    }
}