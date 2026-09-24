using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio4 
{
    public class Producto
    {
        public int Id { get; set; }           // Mapea el campo 'id' int de MySQL
        public string Nombre { get; set; }     // Mapea el campo 'nombre' varchar
        public decimal Precio { get; set; }   // Mapea el campo 'precio' decimal
        public int Cantidad { get; set; }     // Mapea el campo 'cantidad' int
        public byte[] Imagen { get; set; }    // Mapea el campo 'imagen' longblob (Arreglo de bytes)
    }
}
