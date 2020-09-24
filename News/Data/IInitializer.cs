﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Data
{
    public interface IInitializer
    {
        void Run(ModelBuilder modelBuilder);
    }
}
