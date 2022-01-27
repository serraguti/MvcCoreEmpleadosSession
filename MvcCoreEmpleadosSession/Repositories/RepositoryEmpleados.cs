using MvcCoreEmpleadosSession.Data;
using MvcCoreEmpleadosSession.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreEmpleadosSession.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }
    }
}
