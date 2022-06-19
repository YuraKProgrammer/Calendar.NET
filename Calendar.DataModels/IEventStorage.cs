﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.DataModels
{
    public interface IEventStorage
    {
        /// <summary>
        /// Вовращает id добавленной записи 
        /// </summary>
        Task<uint> AddAsync(EventRecord eventRecord, CancellationToken cancellationToken);
    }
}