using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class DALidioma
    {
        private readonly DALconexion conexion = new DALconexion();

        public Iidioma ObtenerIdiomaDefault()
        {
            return ObtenerIdiomas().FirstOrDefault(i => i.Default);
        }

        public IList<Iidioma> ObtenerIdiomas()
        {
            IList<Iidioma> _idiomas = new List<Iidioma>();
            DataTable dt = conexion.LeerText("SELECT Id_Idioma, Nombre, Idioma_Default FROM Idioma");

            foreach (DataRow row in dt.Rows)
            {
                _idiomas.Add(new Idiomas()
                {
                    Id = Convert.ToInt32(row["Id_Idioma"]),
                    Nombre = row["Nombre"].ToString(),
                    Default = Convert.ToBoolean(row["Idioma_Default"])
                });
            }
            return _idiomas;
        }

        public IDictionary<string, ITraduccion> ObtenerTraducciones(Iidioma idioma = null)
        {
            if (idioma == null)
                idioma = ObtenerIdiomaDefault();

            IDictionary<string, ITraduccion> _traducciones = new Dictionary<string, ITraduccion>();
            if (idioma == null) return _traducciones;

            string query = @"SELECT t.Id_Idioma, t.Traduccion AS traduccion_traduccion, 
                                    e.Id_Etiqueta, e.Nombre AS nombre_etiqueta 
                             FROM Traduccion t 
                             INNER JOIN Etiqueta e ON t.Id_Etiqueta = e.Id_Etiqueta 
                             WHERE t.Id_Idioma = @id_idioma";

            SqlParameter[] parametros = {
                new SqlParameter("@id_idioma", SqlDbType.Int) { Value = idioma.Id }
            };

            DataTable dt = conexion.LeerText(query, parametros);

            foreach (DataRow row in dt.Rows)
            {
                string etiqueta = row["nombre_etiqueta"].ToString();
                _traducciones[etiqueta] = new Traduccion()
                {
                    Texto = row["traduccion_traduccion"].ToString(),
                    Etiqueta = new Etiqueta()
                    {
                        Id = Convert.ToInt32(row["Id_Etiqueta"]),
                        Nombre = etiqueta
                    }
                };
            }
            return _traducciones;
        }

        public List<Etiquetas> ObtenerEtiquetasYTraduccion(string nombreIdioma)
        {
            List<Etiquetas> lista = new List<Etiquetas>();
            string query = @"SELECT et.Nombre AS Nombre_Etiqueta, t.Traduccion 
                             FROM Etiqueta et 
                             INNER JOIN Traduccion t ON t.Id_Etiqueta = et.Id_Etiqueta 
                             INNER JOIN Idioma i ON i.Id_Idioma = t.Id_Idioma 
                             WHERE i.Nombre = @n";

            SqlParameter[] parametros = {
                new SqlParameter("@n", SqlDbType.VarChar, 100) { Value = nombreIdioma }
            };

            DataTable dt = conexion.LeerText(query, parametros);

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Etiquetas()
                {
                    Nombre_Etiqueta = row["Nombre_Etiqueta"].ToString(),
                    Traduccion = row["Traduccion"].ToString()
                });
            }
            return lista;
        }

        public List<Etiqueta> GetAllEtiquetas()
        {
            List<Etiqueta> lista = new List<Etiqueta>();
            DataTable dt = conexion.LeerText("SELECT Id_Etiqueta, Nombre FROM Etiqueta");

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Etiqueta()
                {
                    Id = Convert.ToInt32(row["Id_Etiqueta"]),
                    Nombre = row["Nombre"].ToString()
                });
            }
            return lista;
        }

        public int ObtenerTraduccion(string nombreEtiqueta)
        {
            string query = "SELECT Id_Etiqueta FROM Etiqueta WHERE Nombre = @nombre";
            SqlParameter[] parametros = {
                new SqlParameter("@nombre", SqlDbType.VarChar, 100) { Value = nombreEtiqueta }
            };

            DataTable dt = conexion.LeerText(query, parametros);
            if (dt.Rows.Count == 0) return 0;

            return Convert.ToInt32(dt.Rows[0]["Id_Etiqueta"]);
        }

        public string EditarTraduccion(string traduccion, string nombreEtiqueta, int idIdioma)
        {
            string query = @"UPDATE t 
                             SET t.Traduccion = @Trad 
                             FROM Traduccion t 
                             INNER JOIN Etiqueta e ON t.Id_Etiqueta = e.Id_Etiqueta 
                             WHERE e.Nombre = @Nombre AND t.Id_Idioma = @id";

            SqlParameter[] parametros = {
                new SqlParameter("@Trad", SqlDbType.VarChar, 500) { Value = traduccion },
                new SqlParameter("@Nombre", SqlDbType.VarChar, 100) { Value = nombreEtiqueta },
                new SqlParameter("@id", SqlDbType.Int) { Value = idIdioma }
            };

            int filas = conexion.EscribirText(query, parametros);
            return filas > 0 ? "Traducción actualizada correctamente." : "No se encontró el registro.";
        }

        public string InsertarIdioma(Idiomas id)
        {
            string query = @"INSERT INTO Idioma (Id_Idioma, Nombre, Idioma_Default) 
                             VALUES ((SELECT ISNULL(MAX(Id_Idioma), 0) + 1 FROM Idioma), @nombre, @default)";

            SqlParameter[] parametros = {
                new SqlParameter("@nombre", SqlDbType.VarChar, 100) { Value = id.Nombre },
                new SqlParameter("@default", SqlDbType.Bit) { Value = id.Default }
            };

            int filas = conexion.EscribirText(query, parametros);
            return filas > 0 ? "Idioma insertado correctamente." : "Error al insertar idioma.";
        }

        public string InsertarE(int idIdioma, int idEtiqueta, string traduccion)
        {
            string query = "INSERT INTO Traduccion (Id_Idioma, Id_Etiqueta, Traduccion) VALUES (@IdI, @idE, @T)";
            SqlParameter[] parametros = {
                new SqlParameter("@IdI", SqlDbType.Int) { Value = idIdioma },
                new SqlParameter("@idE", SqlDbType.Int) { Value = idEtiqueta },
                new SqlParameter("@T", SqlDbType.VarChar, 500) { Value = traduccion }
            };

            int filas = conexion.EscribirText(query, parametros);
            return filas > 0 ? "Etiqueta traducida insertada." : "Error al insertar.";
        }

        public int AgregarNuevoIdioma(string nombreIdioma)
        {
            // Usa OUTPUT INSERTED.Id_Idioma para devolver el ID generado sin importar si es IDENTITY o calculado
            string query = @"INSERT INTO Idioma (Id_Idioma, Nombre, Idioma_Default)
                             OUTPUT INSERTED.Id_Idioma
                             VALUES ((SELECT ISNULL(MAX(Id_Idioma), 0) + 1 FROM Idioma), @nombreIdioma, 0);";

            SqlParameter[] parametros = {
                new SqlParameter("@nombreIdioma", SqlDbType.VarChar, 100) { Value = nombreIdioma }
            };

            return conexion.EscribirYDevolverId_62_RS(query, parametros);
        }

        public void DuplicarRegistrosParaNuevoIdioma(int nuevoIdiomaId)
        {
            // Copia todas las traducciones del idioma default actual al nuevo idioma
            string duplicarTraduccion = @"
                INSERT INTO Traduccion (Id_Idioma, Id_Etiqueta, Traduccion)
                SELECT @nuevoIdiomaId, t.Id_Etiqueta, t.Traduccion
                FROM Traduccion t
                INNER JOIN Idioma i ON t.Id_Idioma = i.Id_Idioma
                WHERE i.Idioma_Default = 1";

            SqlParameter[] parametros = {
                new SqlParameter("@nuevoIdiomaId", SqlDbType.Int) { Value = nuevoIdiomaId }
            };

            conexion.EscribirText(duplicarTraduccion, parametros);
        }
    }
}