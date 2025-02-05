﻿using Service_Solution_Project2_PBA.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Solution_Project2_PBA.services
{
    public interface RedisCacheServiceIF
    {
        public Task SaveToCacheParkingService<T>(string recordId, T data);
        public Task<string> RetrieveFromCacheParkingService<T>(string recordId);
        public Task<string> RetrieveFromCacheAdService<T>(string recordId);
    }
}
