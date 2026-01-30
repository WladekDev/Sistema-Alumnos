using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Integrador_1
{
    //Superclase que no se va a utilizar más que para herencia, por lo tanto, es abstracta
	public abstract class Persona
    {
		private string _nombre;

		public string Nombre
		{
			get { return _nombre; }
			set { _nombre = value; }
		}

		private string _apellido;

		public string Apellido
		{
			get { return _apellido; }
			set { _apellido = value; }
		}

		private DateTime _fechanacim;

		public DateTime FechaNacim
		{
			get { return _fechanacim; }
			set { _fechanacim = value; }
		}

        //Uso de la funcion CalcularEdad para obtener la edad actual de la persona
        public int Edad
		{
			get {
				var hoy = DateTime.Today;
				return CalcularEdad(hoy, _fechanacim);
            }
		}

		//Calcula edad de la persona en una fecha especificada
		public static int CalcularEdad(DateTime fecha, DateTime nacimiento)
		{
            int edad = fecha.Year - nacimiento.Year;
            if (fecha.Month < nacimiento.Month || (fecha.Month == nacimiento.Month && fecha.Day < nacimiento.Day))
            {
                edad--;
            }
            return edad;
        }
	}
}
