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

        public RepositoryEmpleados
            (EmpleadosContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }

        public Empleado FindEmpleado(int idempleado)
        {
            return
                this.context.Empleados
                .SingleOrDefault(x => x.IdEmpleado == idempleado);
        }

        public Empleado BuscarEmpleado(string apellido)
        {
            return this.context.Empleados
                .FirstOrDefault(x => x.Apellido == apellido);
        }

        public List<Empleado> GetEmpleadosSession(List<int> idsEmpleados)
        {
            //CUANDO UTILIZAMOS BUSQUEDA EN COLECCIONES SE UTILIZA
            //EL METODO Contains
            var consulta = from datos in this.context.Empleados
                           where idsEmpleados.Contains(datos.IdEmpleado)
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }
    }
}
