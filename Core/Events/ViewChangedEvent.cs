﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public delegate void ViewChangedEvent(string viewToCall, object viewParam);
}
