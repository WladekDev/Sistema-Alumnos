using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP_Integrador_1
{
    public partial class Form1 : Form
    {   //Creacion de la lista alumnos para guardar los datos del CSV
        private List<Alumno> listaAlumnos = new List<Alumno>();

        //Constante para el total de materias
        private const int TotalMaterias = 36;

        public Form1()
        {
            InitializeComponent();
            dgvalumnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvalumnos.ReadOnly = true;
            nudAprobadas.Minimum = 0;
            nudAprobadas.Maximum = TotalMaterias;
        }

        private Alumno infoAlumno()
        {
            int legajo;

            //Verifica si el número de legajo es un número válido
            if (!int.TryParse(txtLegajo.Text, out legajo))
            {
                MessageBox.Show("El legajo debe ser un número válido.");
                return null;
            }

            //Verifica que la fecha de nacimiento sea válida
            if (dtpFechaNacim.Value > dtpFechaIngreso.Value)
            {
                MessageBox.Show("La fecha de nacimiento no puede ser mayor a la fecha de ingreso.");
                return null;
            }

            //Verifica que los campos de texto no estén vacíos
            if (string.IsNullOrEmpty(txtLegajo.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtApellido.Text))
            {
                MessageBox.Show("Por favor complete todos los campos.");
                return null;
            }

            int edadingreso = Persona.CalcularEdad(dtpFechaIngreso.Value, dtpFechaNacim.Value);

            int materiasdesaprobadas = Alumno.Desaprobadas(TotalMaterias, (int)nudAprobadas.Value);

            //Creación del nuevo alumno con los datos ingresados
            Alumno alumno = new Alumno
            (
                legajo,
                txtNombre.Text,
                txtApellido.Text,
                dtpFechaIngreso.Value,
                dtpFechaNacim.Value,
                edadingreso,
                chkActivo.Checked,
                materiasdesaprobadas
            );

            return alumno;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Llama al método infoAlumno para obtener los datos del nuevo alumno
            Alumno nuevoalumno = infoAlumno(); 
            //Verifica si hubo error en la entrada de datos
            if (nuevoalumno == null)
            {
                return;
            }

            //Agrega el nuevo alumno a la lista y actualiza la grilla
            listaAlumnos.Add(nuevoalumno);
            ActualizarGrilla();

            //Muestra por mensaje el alumno agregado
            MessageBox.Show($"Se agregó el alumno {nuevoalumno.Nombre} {nuevoalumno.Apellido}, legajo nro. {nuevoalumno.Legajo} al sistema");

            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Toma el alumno seleccionado por indice del DataGridView
            int indice = dgvalumnos.CurrentRow.Index;

            //Llama al método infoAlumno para obtener los nuevos datos del nuevo alumno
            Alumno modificar = infoAlumno(); 
            //Verifica si hubo error en la entrada de datos
            if(modificar == null)
            {
                return;
            }

            //Agrega los datos modificados al indice del alumno a modificar y actualiza la grilla
            listaAlumnos[indice] = modificar;
            ActualizarGrilla();

            //Muestra por mensaje que se modificaron los datos correctamente
            MessageBox.Show($"Se modificó el alumno {modificar.Nombre} {modificar.Apellido}, legajo nro. {modificar.Legajo} en el sistema");

            LimpiarCampos();
        }

        //Evento cuando se selecciona un alumno en la grilla
        private void dgvalumnos_SelectionChanged(object sender, EventArgs e)
        {
            //Verifica si hay un alumno seleccionado y si tiene datos
            if (dgvalumnos.CurrentRow == null || dgvalumnos.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            //Toma el alumno seleccionado de la grilla
            Alumno seleccionado = dgvalumnos.CurrentRow.DataBoundItem as Alumno;

            //Verifica que el alumno no sea null
            if (seleccionado == null)
            {
                return;
            }

            //Rellena los campos y controles con los datos del alumno seleccionado
            txtLegajo.Text = seleccionado.Legajo.ToString();
            txtNombre.Text = seleccionado.Nombre;
            txtApellido.Text = seleccionado.Apellido;
            dtpFechaNacim.Value = seleccionado.FechaNacim;
            dtpFechaIngreso.Value = seleccionado.FechaIngreso;
            nudAprobadas.Value = (TotalMaterias - seleccionado.No_Aprobadas);
            chkActivo.Checked = seleccionado.Activo;
        }


        //Funcion para borrar los datos de los campos de texto y controles
        private void LimpiarCampos()
        {
            txtLegajo.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            dtpFechaNacim.Value = DateTime.Today;
            dtpFechaIngreso.Value = DateTime.Today;
            nudAprobadas.Value = 0;
            chkActivo.Checked = false;
        }

        //Boton para borrar el alumno seleccionado de la grilla
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Alumno seleccionado = (Alumno)dgvalumnos.CurrentRow.DataBoundItem;
            listaAlumnos.Remove(seleccionado);

            ActualizarGrilla();

            MessageBox.Show($"Se eliminó el alumno {seleccionado.Nombre} {seleccionado.Apellido}, legajo nro. {seleccionado.Legajo} del sistema");
        }

        private void ActualizarGrilla()
        {
            //Limpia el DataGridView y configura las columnas antes de asignar la fuente de datos
            dgvalumnos.DataSource = null;
            dgvalumnos.AutoGenerateColumns = false;

            dgvalumnos.Columns.Clear();
            //Da formato a las columnas
            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Legajo",
                HeaderText = "Número de Legajo"
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre"
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Apellido",
                HeaderText = "Apellido"
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaNacim",
                HeaderText = "Fecha de Nacimiento",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d" }
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Edad",
                HeaderText = "Edad"
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaIngreso",
                HeaderText = "Fecha de Ingreso",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "d"}
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Edad_Ingreso",
                HeaderText = "Edad al Ingreso",
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Antiguedad",
                HeaderText = "Antigüedad",
                Name = "Antiguedad"
            });

            dgvalumnos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "No_Aprobadas",
                HeaderText = "Materias No Aprobadas"
            });

            dgvalumnos.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo"
            });

            //Da como fuente de datos la lista de alumnos
            dgvalumnos.DataSource = listaAlumnos;

            //Selecciona por defecto años, pero se puede cambiar con los radialbuttons
            string unidad = "años";
            if (rbMeses.Checked)
                unidad = "meses";
            else if (rbDias.Checked)
                unidad = "dias";

            //Actualiza la antigüedad de todos los alumnos para que tengan la misma unidad de tiempo
            foreach (DataGridViewRow row in dgvalumnos.Rows)
            {
                Alumno alumno = row.DataBoundItem as Alumno;
                if (alumno != null)
                {
                    row.Cells["Antiguedad"].Value = $"{alumno.Antiguedad(unidad)} {unidad}";
                }
            }
        }

        //Boton para salir del programa
        private async void btnSalir_Click(object sender, EventArgs e)
        {
            //Muestra una notificación de salida y espera 1.5 segundos para cerrar la aplicación
            NotifyIcon notificacion = new NotifyIcon();
            notificacion.Icon = SystemIcons.Application;
            notificacion.Visible = true;
            notificacion.BalloonTipText = "Saliendo del programa...";
            notificacion.ShowBalloonTip(1500);

            await Task.Delay(1500);

            notificacion.Visible = false;
            notificacion.Dispose();
            Application.Exit();
        }
    }
}
