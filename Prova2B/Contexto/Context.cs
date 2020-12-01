using Prova2B.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prova2B.Contexto
{
    public class Context : DbContext
    {
        public  DbSet<Dados> Dados{ get; set; }


    }
}