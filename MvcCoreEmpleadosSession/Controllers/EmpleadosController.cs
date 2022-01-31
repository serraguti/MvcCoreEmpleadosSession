using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreEmpleadosSession.Extensions;
using MvcCoreEmpleadosSession.Models;
using MvcCoreEmpleadosSession.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreEmpleadosSession.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public IActionResult SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                int sumasalarial = 0;
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    //RECUPERAMOS SU VALOR
                    sumasalarial =
                        int.Parse(HttpContext.Session.GetString("SUMASALARIAL"));
                }
                //SUMAMOS EL SALARIO RECIBIDO CON LO QUE TENGAMOS YA ALMACENADO
                sumasalarial += salario.Value;
                //ALMACENAMOS EL NUEVO VALOR EN SESSION
                HttpContext.Session.SetString("SUMASALARIAL", sumasalarial.ToString());
                ViewData["MENSAJE"] = "Salario almacenado: " + salario.Value;
            }
            return View(this.repo.GetEmpleados());
        }

        public IActionResult SumaSalarios()
        {
            return View();
        }


        public IActionResult SessionEmpleados(int? idempleado)
        {
            if (idempleado != null)
            {
                //BUSCAMOS AL EMPLEADO
                Empleado empleado = this.repo.FindEmpleado(idempleado.Value);
                //NECESITAMOS ALMACENAR UN CONJUNTO DE EMPLEADOS
                List<Empleado> empleadossession;
                //COMPROBAMOS SI EXISTEN EMPLEADOS EN SESSION
                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadossession =
                        HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    empleadossession = new List<Empleado>();
                }
                //ALMACENAMOS EL EMPLEADO EN LA COLECCION
                empleadossession.Add(empleado);
                //ALMACENAMOS LOS DATOS EN SESSION
                HttpContext.Session.SetObject("EMPLEADOS", empleadossession);
                ViewData["MENSAJE"] = "Empleado "
                    + empleado.IdEmpleado + ", " + empleado.Apellido
                    + " almacenado en Session";
            }
            return View(this.repo.GetEmpleados());
        }

        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public IActionResult SessionEmpleadosCorrecto(int? idempleado)
        {
            if (idempleado != null)
            {
                List<int> listIdEmpleados;
                if (HttpContext.Session.GetString("IDSEMPLEADOS") == null)
                {
                    //NO EXISTE NADA EN SESSION, CREAMOS LA COLECCION
                    listIdEmpleados = new List<int>();
                }
                else
                {
                    //EXISTE Y RECUPERAMOS LA COLECCION DE SESSION
                    listIdEmpleados =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                //ALMACENAMOS EL ID DENTRO DE LA COLECCION
                listIdEmpleados.Add(idempleado.Value);
                //ALMACENAMOS LA COLECCION DE NUEVO EN SESSION
                HttpContext.Session.SetObject("IDSEMPLEADOS", listIdEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + listIdEmpleados.Count;
            }
            return View(this.repo.GetEmpleados());
        }

        public IActionResult EmpleadosAlmacenadosCorrecto(int? ideliminar)
        {
            List<int> listIdEmpleados =
                    HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (listIdEmpleados == null)
            {
                ViewData["MENSAJE"] =
                    "No existen empleados almacenados";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    listIdEmpleados.Remove(ideliminar.Value);
                    if (listIdEmpleados.Count == 0)
                    {
                        //ESTO ELIMINA LA SESSION?????
                        //HttpContext.Session.SetObject("IDSEMPLEADOS", null);
                        HttpContext.Session.Remove("IDSEMPLEADOS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSEMPLEADOS", listIdEmpleados);
                    }
                }
                //NECESITAMOS UN METODO EN EL REPO QUE LE ENVIAREMOS
                //UNA COLECCION DE ID Y NOS DEVOLVERA LOS EMPLEADOS
                //NECESITAMOS UNA CONSULTA IN, DENTRO DE LINQ
                List<Empleado> empleados =
                    this.repo.GetEmpleadosSession(listIdEmpleados);
                return View(empleados);
            }
        }
    }
}
