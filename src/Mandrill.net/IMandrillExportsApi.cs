﻿using Mandrill.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mandrill
{
    public partial interface IMandrillExportsApi
    {
        Task<IList<MandrillExportInfo>> ListAsync();
        Task<MandrillExportInfo> InfoAsync(string id);
        Task<MandrillExportInfo> RejectsAsync(string notifyEmail);
        Task<MandrillExportInfo> WhitelistAsync(string notifyEmail);
        Task<MandrillExportInfo> ActivityAsync(string notifyEmail,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            IList<string> tags = null,
            IList<string> senders = null,
            IList<string> states = null,
            IList<string> apiKeys = null);
    }
}