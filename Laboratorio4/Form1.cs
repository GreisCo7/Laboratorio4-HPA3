using MySql.Data.MySqlClient;
using ProyectoProductos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio4
{
    public partial class Form1 : Form
    {
        private List<Producto> listaProductos;
        private Dictionary<string, object> myProducto = new Dictionary<string, object>();

        private int idProducto = 0;

        public Form1()
        {
            InitializeComponent();

            listaProductos = new List<Producto>();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargarProductos();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar imagen del producto";
                openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }


        private bool datosCorrectos()
        {
            if (txtNombre.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el Nombre del Producto");
                return false;
            }

            if (txtPrecio.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese el Precio");
                return false;
            }

            if (txtCantidad.Text.Trim().Equals(""))
            {
                MessageBox.Show("Ingrese la Cantidad");
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
            {
                MessageBox.Show("Ingrese un Precio correcto");
                return false;
            }

            if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad))
            {
                MessageBox.Show("Ingrese una cantidad correcta");
                return false;
            }

            return true;
        }

        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream mMemoryStream = new MemoryStream())
            {
                image.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return mMemoryStream.ToArray();
            }
        }

        private void CargarDatosProductos()
        {
            myProducto.Clear();

            myProducto["nombre"] = txtNombre.Text.Trim();
            myProducto["precio"] = decimal.Parse(txtPrecio.Text.Trim());
            myProducto["cantidad"] = int.Parse(txtCantidad.Text.Trim());
            myProducto["imagen"] = ImageToByteArray(pictureBox1.Image);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (!datosCorrectos())
            {
                return;
            }

            CargarDatosProductos();

            if (Conexion.InsertSeguro("productos", myProducto))
            {
                MessageBox.Show("Se ha guardado satisfactoriamente el producto");

                cargarProductos();

                btnLimpiar_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Error al guardar. Verifica que la tabla exista y el servicio MySQL esté encendido.");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Vacía las cajas de texto
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";

            // Si tienes un campo para el Folio/ID, también lo vaciamos
            txtFolio.Text = "";

            // Quita la imagen del PictureBox para dejarlo en blanco
            pictureBox1.Image = null;
        }

        private void cargarProductos(string filtro = "")
        {
            dgvProductos.Rows.Clear();
            dgvProductos.Refresh();

            listaProductos = Conexion.GetProductos(filtro);

            foreach (var prod in listaProductos)
            {
                Image img = null;
                if (prod.Imagen != null && prod.Imagen.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(prod.Imagen))
                    {
                        using (Bitmap bmp = new Bitmap(ms))
                        {
                            img = new Bitmap(bmp); 
                        }
                    }
                }

                dgvProductos.Rows.Add(prod.Id, prod.Nombre, prod.Precio, prod.Cantidad, img);
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProductos.Rows[e.RowIndex].Cells[0].Value != null)
            {

                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtFolio.Text = fila.Cells[0].Value.ToString();      
                txtNombre.Text = fila.Cells[1].Value.ToString();      
                txtPrecio.Text = fila.Cells[2].Value.ToString();      
                txtCantidad.Text = fila.Cells[3].Value.ToString();

                idProducto = Convert.ToInt32(fila.Cells[0].Value);

                if (fila.Cells[4].Value != null && fila.Cells[4].Value is Image)
                {
                    pictureBox1.Image = (Image)fila.Cells[4].Value;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null; 
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
 
            if (string.IsNullOrEmpty(txtFolio.Text))
            {
                MessageBox.Show("Por favor, seleccione primero un producto de la tabla haciendo clic sobre él.");
                return;
            }


            DialogResult boton = MessageBox.Show("¿Está seguro de eliminar este producto?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


            if (boton == DialogResult.Yes)
            {
                string query = $"DELETE FROM productos WHERE id = {txtFolio.Text}";

                try
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto eliminado correctamente.");

                            cargarProductos(); 
                            btnLimpiar_Click(sender, e);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al intentar eliminar: " + ex.Message);
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!datosCorrectos())
            {
                return;
            }

            ModificarDatosBD();
        }

        private void ModificarDatosBD()
        {
            if (idProducto == 0)
            {
                MessageBox.Show("Por favor, seleccione primero un producto de la tabla.");
                return;
            }

            CargarDatosProductos();

            bool resultado = Conexion.UpdateSeguro("productos", myProducto, "id", idProducto);

            if (resultado)
            {
                MessageBox.Show("Producto modificado correctamente.");
                cargarProductos(); 
                btnLimpiar_Click(null, null);
            }

        }

        private void txtFolio_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            cargarProductos(txtBusqueda.Text.Trim());
        }
    }
}


