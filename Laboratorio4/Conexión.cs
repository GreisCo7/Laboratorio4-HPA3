using Laboratorio4;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ProyectoProductos
{

    public class Conexion
    {
        private static string cadenaConexion = "Server=localhost;Database=productosdb;Uid=root;Pwd=chiripa07;";

        public static MySqlConnection ObtenerConexion()
        {

            try
            {

                //Crear un tipo de dato de MySQLConnection
                MySqlConnection conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return (conexion);
            }

            catch (MySqlException ex)
            {

                Console.WriteLine("Error al conectar: " + ex.Message);
                return null;

            }//fin del catch

        }

        public static bool InsertSeguro(string tbName, Dictionary<string, object> data)
        {
            var columns = string.Join(", ", data.Keys);
            var placeholders = "@" + string.Join(", @", data.Keys);
            string sql = $"INSERT INTO {tbName} ({columns}) VALUES ({placeholders})";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null) return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        foreach (var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }

                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Esto imprimirá el error real en la consola de Visual Studio si la tabla no existe
                Console.WriteLine("Error en INSERT: " + ex.Message);
                return false;
            }
        }

        public static List<Producto> GetProductos(string filtro)
        {
            List<Producto> listaProductos = new List<Producto>();
            string query = "SELECT id, nombre, precio, cantidad, imagen FROM productos";

            if (!string.IsNullOrEmpty(filtro))
            {
                query += " WHERE id LIKE @filtro OR nombre LIKE @filtro OR precio LIKE @filtro OR cantidad LIKE @filtro";
            }

            try
            {
                using (MySqlConnection conn = ObtenerConexion())
                {
                    if (conn == null) return listaProductos;

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(filtro))
                        {
                            cmd.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                        }

                        using (MySqlDataReader mReader = cmd.ExecuteReader())
                        {
                            while (mReader.Read())
                            {
                                Producto prod = new Producto();
                                prod.Id = Convert.ToInt32(mReader["id"]);
                                prod.Nombre = mReader["nombre"].ToString();
                                prod.Precio = Convert.ToDecimal(mReader["precio"]);
                                prod.Cantidad = Convert.ToInt32(mReader["cantidad"]);
                                prod.Imagen = mReader["imagen"] != DBNull.Value ? (byte[])mReader["imagen"] : null;

                                listaProductos.Add(prod);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al obtener productos: " + ex.Message);
            }

            return listaProductos;
        }

        public static bool UpdateSeguro(string tbName, Dictionary<string, object> data, string idColumn, object idValue)
        {
            // Construye la lista de columnas a modificar
            List<string> sets = new List<string>();
            foreach (var key in data.Keys)
            {
                sets.Add($"{key} = @{key}");
            }
            string setClause = string.Join(", ", sets);

            // Armamos la consulta final UPDATE
            string sql = $"UPDATE {tbName} SET {setClause} WHERE {idColumn} = @idValueParam";

            try
            {
                using (MySqlConnection conexion = ObtenerConexion())
                {
                    if (conexion == null) return false;

                    using (MySqlCommand stmt = new MySqlCommand(sql, conexion))
                    {
                        
                        foreach (var kvp in data)
                        {
                            stmt.Parameters.AddWithValue("@" + kvp.Key, kvp.Value ?? DBNull.Value);
                        }

                     
                        stmt.Parameters.AddWithValue("@idValueParam", idValue);

                        stmt.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error en UPDATE: " + ex.Message);
                return false;
            }
        }


    }

}

