﻿using System;

namespace Frame.PluginLoader.Interfaces
{

    public interface IPluginFactory
    {
        object Create(Type t, string name);
    }

}