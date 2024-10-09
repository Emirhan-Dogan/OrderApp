﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Color : BaseEntity, IEntity
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
}