using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Integrador_1
{
    //Subclase con herencia de la superclase "persona"
	public class Alumno : Persona
    {
		private int _legajo;
		public int Legajo
		{
			get { return _legajo; }
			set { _legajo = value; }
		}

		private DateTime _fechaingreso;
		public DateTime FechaIngreso
		{
			get { return _fechaingreso; }
			set { _fechaingreso = value; }
		}

		private bool _activo;
		public bool Activo
		{
			get { return _activo; }
			set { _activo = value; }
		}

		private int _no_aprobadas;
		public int No_Aprobadas
		{
			get { return _no_aprobadas; }
			set {_no_aprobadas = value; }
		}

		private int _edad_ingreso;
        public int Edad_Ingreso
        {
            get{ return _edad_ingreso; }
			set { _edad_ingreso = value; }
        }


        public Alumno()
        {
            
        }
		//Constructor de la clase alumno
		public Alumno(int legajo, string nombre, string apellido, DateTime fechaing, DateTime fechanac, int edading, bool activo, int noaprobadas)
        {
			Legajo = legajo;
			Nombre = nombre;
			Apellido = apellido;
			FechaIngreso = fechaing;
            FechaNacim = fechanac;
            Edad_Ingreso = edading;
			Activo = activo;
			No_Aprobadas = noaprobadas;
        }

		//Muestra la antiguedad del alumno según el parámetro especificado
		public int Antiguedad(string unidad)
		{
            var ahora = DateTime.Today;
			var tiempo = ahora - FechaIngreso;

			switch (unidad.ToLower())
			{
				case "años":
					return (int)(tiempo.Days / 365);
				case "meses":
					return (int)(tiempo.Days / 30);
				case "dias":
					return tiempo.Days;
				default:
					return 0;
			}
		}

		//Cálculo de materias desaprobadas
		public static int Desaprobadas(int totalmaterias, int aprobadas)
		{
            int materiasNoAprobadas = totalmaterias - aprobadas;
			return materiasNoAprobadas;
        }
    }
}
