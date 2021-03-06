﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin
{
    public interface ISubQuery<out From>
        where From: ITraversalQuery
    {
    }

    public interface ISubQuery<T, From> : ISubQuery<From>
        where From : ITraversalQuery
    {
    }
}
