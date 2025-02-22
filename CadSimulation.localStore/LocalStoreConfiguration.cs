﻿using CadSimulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.localStore
{
    public class LocalStoreConfiguration : IConfigurationForStore
    {
        public string Filename { get; set; }
        public LocalStoreConfiguration(string sFilename)
        {
            Filename = sFilename;
        }
    }
}
