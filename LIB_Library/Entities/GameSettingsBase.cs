﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public class GameSettingsBase : NotifyerPropertyChangedBase, ICloneable
    {
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}