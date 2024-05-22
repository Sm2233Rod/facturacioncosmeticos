using System.Text;
using System.Windows.Forms;

namespace facturacion
{
    public partial class Form1 : Form
    {
        //variables que se usan en el programa
        public double precioTotal = 0;
        public double totalconiva;
        public double iva;
        public double Subtotal;
        public double totalfinal;

        public Form1()//metodo del formulario
        {
            InitializeComponent();

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }

        private void dataGridView1_SelectionChanged(object? sender, EventArgs? e)
        {
            // Habilitar o deshabilitar el botón de eliminar según si hay alguna fila seleccionada
            Eliminar.Enabled = dataGridView1.SelectedRows.Count > 0;
        }//permite que se elimine un afila del datagridview para eliminar 
        private void Limpia_texto()
        {
            maskedTextBoxcodigoproducto.Text = "";
            textBoxnombreproducto.Text = "";
            textBoxcantidad.Text = "";
            maskedTextBox1.Text = "";
            textBoxnombrecliente.Text = "";
            textBoxNRC.Text = "";
            textBoxpropietario.Text = "";

        }//metodo que limpia texto donde sea que fuese llamado
        public void Add_Click(object sender, EventArgs e)//metodo del boton add para agregar los productos al datagridview
        {
            DataGridViewRow fila = new DataGridViewRow();
            fila.CreateCells(dataGridView1);

            fila.Cells[0].Value = maskedTextBoxcodigoproducto.Text;
            fila.Cells[1].Value = textBoxnombreproducto.Text;
            fila.Cells[2].Value = textBoxcantidad.Text;
            fila.Cells[3].Value = maskedTextBox1.Text;

            //variables locales
            double cantidad = double.Parse(textBoxcantidad.Text);
            double precio = double.Parse(maskedTextBox1.Text);
            precioTotal = cantidad * precio;
            fila.Cells[4].Value = Math.Round(precioTotal, 2);
            // Redondea a dos cifras decimales


            dataGridView1.Rows.Add(fila);
            //para limpiar el texto cuando se agrega los datos al la factura
            maskedTextBoxcodigoproducto.Text = "";
            textBoxnombreproducto.Text = "";
            textBoxcantidad.Text = "";
            maskedTextBox1.Text = "";


            calculartotalcon_iva();
            CalcularSubtotal();
            if (rbcreditofiscal.Checked)
            {
                CalcularIVA();

            }
            calcular_total();

            Actualizartotal();

        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                RecalcularTotales();

                Actualizartotal();
            }



        }//al eliminar se actualizan los datos
        private void CalcularSubtotal()
        {
            Subtotal = 0;
            if (rbcreditofiscal.Checked)
            {
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {


                    if (fila.Cells[4].Value != null)
                    {
                        Subtotal += Convert.ToSingle(fila.Cells[4].Value) / 1.13;
                    }

                }
                sutotaltext.Text = Subtotal.ToString("0.00"); // Formatea el subtotal a dos decimales
            }
        }//calculo del subtotal sin iva

        private void CalcularIVA()
        {
            Subtotal = Convert.ToSingle(sutotaltext.Text);
            iva = totalconiva - Subtotal;
            IVAtext.Text = iva.ToString("0.00"); // Formatea el IVA a dos decimales
        }//calcula el iba del 13%

        private void calculartotalcon_iva()
        {
            totalconiva = 0;
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells[4].Value != null)
                {
                    totalconiva += Convert.ToSingle(fila.Cells[4].Value);
                }
            }
            totalconivatext.Text = totalconiva.ToString("0.00"); // Formatea el subtotal a dos decimales


        }//es la suma de todos los totales del la columna de precio total del datagridview

        private void calcular_total()
        {
            totalfinal = iva + Subtotal;
            totaltext.Text = totalfinal.ToString("0.00");
        }//es el total a pagar

        private void radioButtonconsumidorfinal_CheckedChanged(object? sender, EventArgs? e)
        {

            // Si se selecciona el radioButtonconsumidorfinal, desactiva 
            if (radioButtonconsumidorfinal.Checked)
            {
                IVAtext.Enabled = false;
                textBoxNRC.Enabled = false;
                sutotaltext.Enabled = false;
                textBoxnombrecliente.Enabled = true;
                textBoxpropietario.Enabled = true;


                totalfinal = totalconiva;
                totaltext.Text = totalfinal.ToString("0.00");

            }
            else
            {
                // Si no se selecciona el radioButtonconsumidorfinal, activa 
                IVAtext.Enabled = true;
                textBoxNRC.Enabled = true;
                sutotaltext.Enabled = true;
                textBoxnombrecliente.Enabled = true;
                textBoxpropietario.Enabled = true;


                totalfinal = Subtotal + iva;
                totaltext.Text = totalfinal.ToString("0.00");

            }



        }//boton que se selecciona cuando el cliente es consumidor final

        private void Actualizartotal()
        {
            radioButtonconsumidorfinal_CheckedChanged(null, null);

        }//actualiza el total cuando es consumidor final


        private void iconButtonexit_Click(object sender, EventArgs e)//icono de boton para salir de la aplicacion
        {
            Application.Exit();
        }

        public void RecalcularTotales()
        {
            Subtotal = 0;
            iva = 0;
            totalfinal = 0;
            totalconiva = 0;

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells[4].Value != null)
                {
                    totalconiva += Convert.ToSingle(fila.Cells[4].Value);
                }
            }

            if (rbcreditofiscal.Checked)
            {
                foreach (DataGridViewRow fila in dataGridView1.Rows)
                {
                    if (fila.Cells[4].Value != null)
                    {
                        Subtotal += Convert.ToDouble(fila.Cells[4].Value) / 1.13;
                    }
                }


                {
                    // Si se ha seleccionado el radio button de crédito fiscal, calcula el IVA
                    iva = totalconiva - Subtotal; // Suponiendo que el IVA es del 13%
                }
            }

            totalfinal = Subtotal + iva;

            // Actualiza los valores en los controles de texto
            sutotaltext.Text = Subtotal.ToString("0.00");
            IVAtext.Text = iva.ToString("0.00");
            totaltext.Text = totalfinal.ToString("0.00");
            totalconivatext.Text = totalconiva.ToString("0.00");
        }//recalcula el subtotal, total,totalconiva e IVA cuando se elimina un producto

        public void rbcreditofiscal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbcreditofiscal.Checked)
            {
                textBoxnombrecliente.Enabled = true;
                textBoxpropietario.Enabled = true;
                textBoxNRC.Enabled = true;

            }
        }//se selecciona que el cliente es credito fiscal y se habilitan algunos campos

        public void Validarcamposproductos()
        {
            var codigoproducto = !string.IsNullOrEmpty(maskedTextBoxcodigoproducto.Text);
            var nombreproducto = !string.IsNullOrEmpty(textBoxnombreproducto.Text);
            var cantidadproducto = !string.IsNullOrEmpty(textBoxcantidad.Text);
            var precioproducto = !string.IsNullOrEmpty(maskedTextBox1.Text);

            Add.Enabled = cantidadproducto && nombreproducto && codigoproducto && precioproducto;
        }//valida si los campos para agregar el producto estan llennos y asi habilitar el boton add

        private void maskedTextBoxcodigoproducto_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            Validarcamposproductos();
        }  //son los campos ha evaluar donde se llama al metodo validarcamposproductos

        private void textBoxnombreproducto_TextChanged(object sender, EventArgs e)
        {
            Validarcamposproductos();
        }

        private void textBoxcantidad_TextChanged(object sender, EventArgs e)
        {
            Validarcamposproductos();
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            Validarcamposproductos();
        }

        private void iconButtonanular_Click(object sender, EventArgs e)
        {
            // Limpiar el DataGridView
            dataGridView1.Rows.Clear();

            // Limpiar los campos de entrada de datos
            Limpia_texto();

            // Restablecer los totales a cero
            totalconiva = 0;
            iva = 0;
            Subtotal = 0;
            totalfinal = 0;

            // Actualizar los campos de texto de los totales
            sutotaltext.Text = Subtotal.ToString("0.00");
            IVAtext.Text = iva.ToString("0.00");
            totaltext.Text = totalfinal.ToString("0.00");
            totalconivatext.Text = totalconiva.ToString("0.00");

            // Restablecer el estado de los radio buttons y otros controles según sea necesario
            radioButtonconsumidorfinal.Checked = false;
            rbcreditofiscal.Checked = false;
            IVAtext.Enabled = false;
            textBoxNRC.Enabled = false;
            sutotaltext.Enabled = false;
            textBoxnombrecliente.Enabled = false;
            textBoxpropietario.Enabled = false;
        }//boton anular factura que al hacer clic se vacian todos los campos para volver agregar otra factura



        private void iconButtoncrear_Click_1(object sender, EventArgs e)
        {
    StringBuilder facturaBuilder = new StringBuilder();

    // Encabezado de la factura
    facturaBuilder.AppendLine("Factura:");

    // Información del cliente
    facturaBuilder.AppendLine($"Cliente: {textBoxnombrecliente.Text}");
    facturaBuilder.AppendLine($"NRC: {textBoxNRC.Text}");
    facturaBuilder.AppendLine($"Propietario: {textBoxpropietario.Text}");

    // Detalles de los productos
    facturaBuilder.AppendLine("Detalles:");

    foreach (DataGridViewRow fila in dataGridView1.Rows)
    {
        if (fila.Cells[0].Value != null)
        {
            facturaBuilder.AppendLine($"Código: {fila.Cells[0].Value}");
            facturaBuilder.AppendLine($"Nombre: {fila.Cells[1].Value}");
            facturaBuilder.AppendLine($"Cantidad: {fila.Cells[2].Value}");
            facturaBuilder.AppendLine($"Precio unitario: {fila.Cells[3].Value}");
            facturaBuilder.AppendLine($"Total: {fila.Cells[4].Value}");
            facturaBuilder.AppendLine();
        }
    }

    // Totales
    facturaBuilder.AppendLine($"Subtotal: {sutotaltext.Text}");
    facturaBuilder.AppendLine($"IVA: {IVAtext.Text}");
    facturaBuilder.AppendLine($"Total con IVA: {totalconivatext.Text}");
    facturaBuilder.AppendLine($"Total a pagar: {totaltext.Text}");

    // Mostrar la factura en el TextBox
    textBoxFactura.Text = facturaBuilder.ToString();

        }
    }

}

