using BE;
using DAL;
using desarrolloweb.BLL;
using desarrolloweb.DAL;
using SEG.singleton;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLDVV
    {
        private DAL.DVV DalDvv = new DAL.DVV();
        int usu = SingletonSession.Instancia.Usuario != null ? SingletonSession.Instancia.Usuario.Id_Usuario : 0;

        public void RecalcularDVV(string nombreTabla)
        {
            try
            {
                List<int> listaDvhs = DalDvv.ObtenerTodosLosDVH(nombreTabla);
                SEG.DigitoVerificador motorDV = new SEG.DigitoVerificador();
                int sumaTotalDvv = motorDV.CalcularDVV(listaDvhs);
                DalDvv.ActualizarSumaDVV(nombreTabla, sumaTotalDvv);
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo de integridad al recalcular DVV: " + ex.Message);
            }
        }

        public List<BE.Infraccion> VerificarIntegridadGlobal()
        {
            List<BE.Infraccion> listaErrores = new List<BE.Infraccion>();

            try
            {
                SEG.DigitoVerificador digitoVerificador = new SEG.DigitoVerificador();

                //VERIFICAR TABLA: USUARIOS
                try
                {
                    desarrolloweb.BLL.BLLusuario bllUsu = new desarrolloweb.BLL.BLLusuario();
                    List<desarrolloweb.BE.Usuario> listaUsuarios = bllUsu.ObtenerTodosParaDVV();
                    int sumaDvhCalculadaUsuarios = 0;

                    foreach (desarrolloweb.BE.Usuario usu in listaUsuarios)
                    {
                        int dvhEnVivo = digitoVerificador.CalcularDVH(usu);
                        if (dvhEnVivo != usu.DVH)
                        {
                            listaErrores.Add(new BE.Infraccion { Tabla = "Usuarios", IdRegistro = usu.Id_Usuario.ToString(), TipoError = "Horizontal (Fila Alterada)" });
                        }
                        sumaDvhCalculadaUsuarios += dvhEnVivo;
                    }

                    int dvvGuardadoUsu = DalDvv.ObtenerDvvGuardado("Usuarios");
                    if (sumaDvhCalculadaUsuarios != dvvGuardadoUsu)
                    {
                        listaErrores.Add(new BE.Infraccion { Tabla = "Usuarios", IdRegistro = "COLUMNA", TipoError = "Vertical (Suma total no coincide)" });
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error validando Usuarios: " + ex.Message); }

                //VERIFICAR TABLA: BITACORA
                try
                {
                    desarrolloweb.BLL.BLLbitacora bLLbitacora = new desarrolloweb.BLL.BLLbitacora();
                    DataTable dtbitacora = bLLbitacora.listartodabitacoraparadvv();
                    int sumaDvhCalculadaBitacora = 0;

                    foreach (DataRow row in dtbitacora.Rows)
                    {
                        desarrolloweb.BE.Bitacora BIT = new desarrolloweb.BE.Bitacora
                        {
                            Id_Bitacora = row["Id_Bitacora"] != DBNull.Value ? Convert.ToInt32(row["Id_Bitacora"]) : 0,
                            Id_Usuario = row["Id_Usuario"] != DBNull.Value ? Convert.ToInt32(row["Id_Usuario"]) : 0,
                            Actividad = row["Actividad"] != DBNull.Value ? row["Actividad"].ToString() : string.Empty,
                            modulo = row["modulo"] != DBNull.Value ? row["modulo"].ToString() : string.Empty,
                            Criticidad = row["Criticidad"] != DBNull.Value ? row["Criticidad"].ToString() : string.Empty,
                            Fecha = row["Fecha"] != DBNull.Value ? row["Fecha"].ToString() : string.Empty,
                            Hora = row["Hora"] != DBNull.Value ? row["Hora"].ToString() : string.Empty,
                            DVH = row["DVH"] != DBNull.Value ? Convert.ToInt32(row["DVH"]) : 0
                        };

                        int dvhEnVivo = digitoVerificador.CalcularDVH(BIT);
                        if (dvhEnVivo != BIT.DVH)
                        {
                            listaErrores.Add(new BE.Infraccion { Tabla = "Bitacora", IdRegistro = BIT.Id_Bitacora.ToString(), TipoError = "Horizontal (Fila Alterada)" });
                        }
                        sumaDvhCalculadaBitacora += dvhEnVivo;
                    }

                    int dvvGuardadoBitacora = DalDvv.ObtenerDvvGuardado("Bitacora");
                    if (sumaDvhCalculadaBitacora != dvvGuardadoBitacora)
                    {
                        listaErrores.Add(new BE.Infraccion { Tabla = "Bitacora", IdRegistro = "COLUMNA", TipoError = "Vertical (Suma total no coincide)" });
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error validando Bitácora: " + ex.Message); }

                //  VERIFICAR TABLA: RESERVAS
                try
                {
                    BLL.BLLReserva bllReserva = new BLL.BLLReserva();
                    List<BE.Reserva> listaReservas = bllReserva.ObtenerTodasParaDVV();
                    int sumaDvhCalculadaReservas = 0;

                    foreach (BE.Reserva res in listaReservas)
                    {
                        int dvhEnVivo = digitoVerificador.CalcularDVH(res);
                        if (dvhEnVivo != res.DVH)
                        {
                            listaErrores.Add(new BE.Infraccion { Tabla = "Reservas", IdRegistro = res.Id_Reserva.ToString(), TipoError = "Horizontal (Fila Alterada)" });
                        }
                        sumaDvhCalculadaReservas += dvhEnVivo;
                    }

                    int dvvGuardadoReservas = DalDvv.ObtenerDvvGuardado("Reservas");
                    if (sumaDvhCalculadaReservas != dvvGuardadoReservas)
                    {
                        listaErrores.Add(new BE.Infraccion { Tabla = "Reservas", IdRegistro = "COLUMNA", TipoError = "Vertical (Suma total no coincide)" });
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error validando Reservas: " + ex.Message); }

                return listaErrores;
            }
            catch (Exception ex)
            {
                listaErrores.Add(new BE.Infraccion { Tabla = "SISTEMA", IdRegistro = "N/A", TipoError = "Falla técnica general: " + ex.Message });
                return listaErrores;
            }
        }

        public void RecalcularTodosLosDigitos()
        {
            try
            {
                SEG.DigitoVerificador motorDV = new SEG.DigitoVerificador();
                desarrolloweb.DAL.DALusuario dALusuario = new desarrolloweb.DAL.DALusuario();
                desarrolloweb.DAL.DALbitacora dALbitacora = new desarrolloweb.DAL.DALbitacora();
                DAL.DALreserva.DalReserva dALreserva = new DAL.DALreserva.DalReserva();

                // RECALCULAR DVH DE USUARIOS
                desarrolloweb.BLL.BLLusuario bllUsu = new desarrolloweb.BLL.BLLusuario();
                List<desarrolloweb.BE.Usuario> listaUsuarios = bllUsu.ObtenerTodosParaDVV();

                foreach (desarrolloweb.BE.Usuario usu in listaUsuarios)
                {
                    int nuevoDvh = motorDV.CalcularDVH(usu);
                    dALusuario.ActualizarDVH(usu.Id_Usuario, nuevoDvh);
                }

                //  RECALCULAR DVH DE BITACORA
                desarrolloweb.BLL.BLLbitacora bLLbitacora = new desarrolloweb.BLL.BLLbitacora();
                DataTable dt = bLLbitacora.listartodabitacoraparadvv();

                foreach (DataRow dr in dt.Rows)
                {
                    desarrolloweb.BE.Bitacora bit = new desarrolloweb.BE.Bitacora
                    {
                        Id_Bitacora = dr["Id_Bitacora"] != DBNull.Value ? Convert.ToInt32(dr["Id_Bitacora"]) : 0,
                        Id_Usuario = dr["Id_Usuario"] != DBNull.Value ? Convert.ToInt32(dr["Id_Usuario"]) : 0,
                        Actividad = dr["Actividad"] != DBNull.Value ? dr["Actividad"].ToString() : string.Empty,
                        modulo = dr["modulo"] != DBNull.Value ? dr["modulo"].ToString() : string.Empty,
                        Criticidad = dr["Criticidad"] != DBNull.Value ? dr["Criticidad"].ToString() : string.Empty,
                        Fecha = dr["Fecha"] != DBNull.Value ? dr["Fecha"].ToString() : string.Empty,
                        Hora = dr["Hora"] != DBNull.Value ? dr["Hora"].ToString() : string.Empty,
                        DVH = dr["DVH"] != DBNull.Value ? Convert.ToInt32(dr["DVH"]) : 0
                    };
                    int nuevoDvh = motorDV.CalcularDVH(bit);
                    dALbitacora.ActualizarDVH(bit.Id_Bitacora, nuevoDvh);
                }

                // RECALCULAR DVH DE RESERVAS
                BLL.BLLReserva bllReserva = new BLL.BLLReserva();
                List<BE.Reserva> listaReservas = bllReserva.ObtenerTodasParaDVV();

                foreach (BE.Reserva res in listaReservas)
                {
                    int nuevoDvh = motorDV.CalcularDVH(res);
                    dALreserva.ActualizarDVH(res.Id_Reserva, nuevoDvh);
                }

                // RECALCULAR VERTICALES (DVV) DE TODAS LAS TABLAS DEL SISTEMA
                this.RecalcularDVV("Usuarios");
                this.RecalcularDVV("Bitacora");
                this.RecalcularDVV("Reservas");


                bLLbitacora.InsertarBitacora(usu, "Recálculo masivo de DVH y DVV", "Seguridad", "3");
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al recalcular toda la base de datos: " + ex.Message);
            }
        }
    }
}