﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.FrontDesk.DTOModels
{
    public class JobOrderAccessoriesDTO
    {
        public long JobOrderAccessoriesId { get; set; }
        public long AccessoriesId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public long JobOrderId { get; set; }

        // Custom Property
        public string AccessoriesName { get; set; }
        public string EntryUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
