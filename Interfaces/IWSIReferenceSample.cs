﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Measurement_Evaluator.Interfaces
{
    interface IWSIReferenceSample : IReferenceSample
    {
        bool DoFlipDefects { get; set; }
    }
}