using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALPerfil
    {
        private DALconexion accesos = new DALconexion();

        //perfiles
        public List<Perfil> ObtenerTodos()
        {
            List<Perfil> lista = new List<Perfil>();
            string sql = "SELECT Id_Perfil, Nombre FROM Perfiles";
            DataTable dt = accesos.LeerText(sql);
            foreach (DataRow dr in dt.Rows)
                lista.Add(MapearPerfil(dr));
            return lista;
        }

        public void AgregarPerfil(Perfil perfil)
        {
            string sql = "INSERT INTO Perfiles (Nombre) VALUES (@n)";
            SqlParameter[] p = {
                new SqlParameter("@n", perfil.Nombre)
            };
            accesos.EscribirText(sql, p);
        }

        public void EliminarPerfil(int idPerfil)
        {
            // limpiar relaciones primero
            accesos.EscribirText("DELETE FROM PerfilFamilia WHERE Id_Perfil = @id",
                new SqlParameter[] { new SqlParameter("@id", idPerfil) });
            accesos.EscribirText("DELETE FROM PerfilPatente WHERE Id_Perfil = @id",
                new SqlParameter[] { new SqlParameter("@id", idPerfil) });
            accesos.EscribirText("DELETE FROM Perfiles WHERE Id_Perfil = @id",
                new SqlParameter[] { new SqlParameter("@id", idPerfil) });
        }

        //familias
        public List<Familia> ObtenerFamilias()
        {
            List<Familia> lista = new List<Familia>();
            string sql = "SELECT Id_Familia, Nombre FROM Familias";
            DataTable dt = accesos.LeerText(sql);
            foreach (DataRow dr in dt.Rows)
                lista.Add(MapearFamilia(dr));
            return lista;
        }

        public void AgregarFamilia(Familia familia)
        {
            string sql = "INSERT INTO Familias (Nombre) VALUES (@n)";
            SqlParameter[] p = {
                new SqlParameter("@n", familia.Nombre)
            };
            accesos.EscribirText(sql, p);
        }

        public void EliminarFamilia(int idFamilia)
        {
            accesos.EscribirText("DELETE FROM FamiliaPatente  WHERE Id_Familia = @id",
                new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            accesos.EscribirText("DELETE FROM FamiliaFamilia  WHERE Id_Familia_Padre = @id OR Id_Familia_Hijo = @id",
                new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            accesos.EscribirText("DELETE FROM PerfilFamilia   WHERE Id_Familia = @id",
                new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            accesos.EscribirText("DELETE FROM Familias        WHERE Id_Familia = @id",
                new SqlParameter[] { new SqlParameter("@id", idFamilia) });
        }

        // patentes
        public List<Patente> ObtenerPatentes()
        {
            List<Patente> lista = new List<Patente>();
            string sql = "SELECT Id_Patente, Nombre FROM Patentes";
            DataTable dt = accesos.LeerText(sql);
            foreach (DataRow dr in dt.Rows)
                lista.Add(MapearPatente(dr));
            return lista;
        }

        public void AgregarPatente(Patente patente)
        {
            string sql = "INSERT INTO Patentes (Nombre) VALUES (@n)";
            SqlParameter[] p = {
                new SqlParameter("@n", patente.Nombre)
            };
            accesos.EscribirText(sql, p);
        }

        public void EliminarPatente(int idPatente)
        {
            accesos.EscribirText("DELETE FROM FamiliaPatente WHERE Id_Patente = @id",
                new SqlParameter[] { new SqlParameter("@id", idPatente) });
            accesos.EscribirText("DELETE FROM PerfilPatente  WHERE Id_Patente = @id",
                new SqlParameter[] { new SqlParameter("@id", idPatente) });
            accesos.EscribirText("DELETE FROM Patentes       WHERE Id_Patente = @id",
                new SqlParameter[] { new SqlParameter("@id", idPatente) });
        }

        // relaciones
        public void AgregarFamiliaAPerfil(int idPerfil, int idFamilia)
        {
            string sql = "INSERT INTO PerfilFamilia (Id_Perfil, Id_Familia) VALUES (@p, @f)";
            SqlParameter[] par = {
                new SqlParameter("@p", idPerfil),
                new SqlParameter("@f", idFamilia)
            };
            accesos.EscribirText(sql, par);
        }

        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia)
        {
            string sql = "DELETE FROM PerfilFamilia WHERE Id_Perfil = @p AND Id_Familia = @f";
            SqlParameter[] par = {
                new SqlParameter("@p", idPerfil),
                new SqlParameter("@f", idFamilia)
            };
            accesos.EscribirText(sql, par);
        }

        public void AgregarPatenteAPerfil(int idPerfil, int idPatente)
        {
            string sql = "INSERT INTO PerfilPatente (Id_Perfil, Id_Patente) VALUES (@p, @pat)";
            SqlParameter[] par = {
                new SqlParameter("@p",   idPerfil),
                new SqlParameter("@pat", idPatente)
            };
            accesos.EscribirText(sql, par);
        }

        public void EliminarPatenteDePerfil(int idPerfil, int idPatente)
        {
            string sql = "DELETE FROM PerfilPatente WHERE Id_Perfil = @p AND Id_Patente = @pat";
            SqlParameter[] par = {
                new SqlParameter("@p",   idPerfil),
                new SqlParameter("@pat", idPatente)
            };
            accesos.EscribirText(sql, par);
        }

        public void AgregarFamiliaAFamilia(int idPadre, int idHijo)
        {
            string sql = "INSERT INTO FamiliaFamilia (Id_Familia_Padre, Id_Familia_Hijo) VALUES (@p, @h)";
            SqlParameter[] par = {
                new SqlParameter("@p", idPadre),
                new SqlParameter("@h", idHijo)
            };
            accesos.EscribirText(sql, par);
        }

        public void EliminarFamiliaDeFamily(int idPadre, int idHijo)
        {
            string sql = "DELETE FROM FamiliaFamilia WHERE Id_Familia_Padre = @p AND Id_Familia_Hijo = @h";
            SqlParameter[] par = {
                new SqlParameter("@p", idPadre),
                new SqlParameter("@h", idHijo)
            };
            accesos.EscribirText(sql, par);
        }

        public void AgregarPatenteAFamilia(int idFamilia, int idPatente)
        {
            string sql = "INSERT INTO FamiliaPatente (Id_Familia, Id_Patente) VALUES (@f, @p)";
            SqlParameter[] par = {
                new SqlParameter("@f", idFamilia),
                new SqlParameter("@p", idPatente)
            };
            accesos.EscribirText(sql, par);
        }

        public void EliminarPatenteDeFamilia(int idFamilia, int idPatente)
        {
            string sql = "DELETE FROM FamiliaPatente WHERE Id_Familia = @f AND Id_Patente = @p";
            SqlParameter[] par = {
                new SqlParameter("@f", idFamilia),
                new SqlParameter("@p", idPatente)
            };
            accesos.EscribirText(sql, par);
        }

        //cargar arbol completo
        public Perfil CargarArbol(int idPerfil)
        {
            string sql = "SELECT Id_Perfil, Nombre FROM Perfiles WHERE Id_Perfil = @id";
            DataTable dt = accesos.LeerText(sql, new SqlParameter[] { new SqlParameter("@id", idPerfil) });
            if (dt.Rows.Count == 0) return null;

            Perfil perfil = MapearPerfil(dt.Rows[0]);

            //agregar familias del perfil
            string sqlFamilias = @"SELECT f.Id_Familia, f.Nombre
                                   FROM Familias f 
                                   INNER JOIN PerfilFamilia pf ON f.Id_Familia = pf.Id_Familia
                                   WHERE pf.Id_Perfil = @id";
            DataTable dtFamilias = accesos.LeerText(sqlFamilias, new SqlParameter[] { new SqlParameter("@id", idPerfil) });
            foreach (DataRow dr in dtFamilias.Rows)
            {
                Familia fam = CargarFamiliaConHijos(Convert.ToInt32(dr["Id_Familia"]));
                perfil.AgregarHijo(fam);
            }

            //agregar patentes directas del perfil
            string sqlPatentes = @"SELECT p.Id_Patente, p.Nombre
                                   FROM Patentes p 
                                   INNER JOIN PerfilPatente pp ON p.Id_Patente = pp.Id_Patente
                                   WHERE pp.Id_Perfil = @id";
            DataTable dtPatentes = accesos.LeerText(sqlPatentes, new SqlParameter[] { new SqlParameter("@id", idPerfil) });
            foreach (DataRow dr in dtPatentes.Rows)
                perfil.AgregarHijo(MapearPatente(dr));

            return perfil;
        }

        private Familia CargarFamiliaConHijos(int idFamilia)
        {
            string sql = "SELECT Id_Familia, Nombre FROM Familias WHERE Id_Familia = @id";
            DataTable dt = accesos.LeerText(sql, new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            Familia familia = MapearFamilia(dt.Rows[0]);

            // subfamilias
            string sqlHijos = @"SELECT f.Id_Familia, f.Nombre
                                FROM Familias f 
                                INNER JOIN FamiliaFamilia ff ON f.Id_Familia = ff.Id_Familia_Hijo
                                WHERE ff.Id_Familia_Padre = @id";
            DataTable dtHijos = accesos.LeerText(sqlHijos, new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            foreach (DataRow dr in dtHijos.Rows)
                familia.AgregarHijo(CargarFamiliaConHijos(Convert.ToInt32(dr["Id_Familia"])));

            // patentes de la familia
            string sqlPatentes = @"SELECT p.Id_Patente, p.Nombre
                                   FROM Patentes p 
                                   INNER JOIN FamiliaPatente fp ON p.Id_Patente = fp.Id_Patente
                                   WHERE fp.Id_Familia = @id";
            DataTable dtPatentes = accesos.LeerText(sqlPatentes, new SqlParameter[] { new SqlParameter("@id", idFamilia) });
            foreach (DataRow dr in dtPatentes.Rows)
                familia.AgregarHijo(MapearPatente(dr));

            return familia;
        }

        // mapeos
        private Perfil MapearPerfil(DataRow dr) => new Perfil { Id = Convert.ToInt32(dr["Id_Perfil"]), Nombre = dr["Nombre"].ToString() };
        private Familia MapearFamilia(DataRow dr) => new Familia { Id = Convert.ToInt32(dr["Id_Familia"]), Nombre = dr["Nombre"].ToString()};
        private Patente MapearPatente(DataRow dr) => new Patente { Id = Convert.ToInt32(dr["Id_Patente"]), Nombre = dr["Nombre"].ToString()};
    }
}
