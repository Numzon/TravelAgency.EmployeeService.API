﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.EmployeeService.Application.Common.Models;
public sealed class CategoryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}
