﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hydrous.Hosting.Internal
{
    public interface IBuildServiceWrapper
    {
        IServiceWrapper Build();
    }
}
