using CNBot.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Infrastructure
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
    }
}
