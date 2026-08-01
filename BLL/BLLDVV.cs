using BE;
using DAL;
using desarrolloweb.BE;
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
                LimpiarAuditoria();
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


                // =====================================================
                // USUARIOS
                // =====================================================
                try
                {
                    BLLusuario bllUsuario = new BLLusuario();

                    List<Usuario> usuarios =
                        bllUsuario.ObtenerTodosParaDVV();


                    int sumaDVH = 0;
                    bool huboError = false;


                    foreach (Usuario usu in usuarios)
                    {
                        int dvhActual =
                            digitoVerificador.CalcularDVH(usu);


                        if (dvhActual != usu.DVH)
                        {
                            huboError = true;
                        }


                        sumaDVH += dvhActual;
                    }


                    int dvvGuardado =
                        DalDvv.ObtenerDvvGuardado("Usuarios");


                    if (sumaDVH != dvvGuardado)
                    {
                        huboError = true;
                    }


                    if (huboError)
                    {
                        listaErrores.AddRange(
                            DalDvv.ObtenerOperaciones("Usuarios")
                        );
                    }

                }
                catch (Exception ex)
                {
                    listaErrores.Add(new BE.Infraccion
                    {
                        Tabla = "Usuarios",
                        IdRegistro = "ERROR",
                        Operacion = ex.Message
                    });
                }




                // =====================================================
                // BITACORA
                // =====================================================
                try
                {
                    BLLbitacora bllBitacora =
                        new BLLbitacora();


                    DataTable dt =
                        bllBitacora.listartodabitacoraparadvv();


                    int sumaDVH = 0;
                    bool huboError = false;



                    foreach (DataRow row in dt.Rows)
                    {
                       Bitacora bit = new Bitacora
                        {
                            Id_Bitacora =
                                Convert.ToInt32(row["Id_Bitacora"]),

                            Id_Usuario =
                                Convert.ToInt32(row["Id_Usuario"]),

                            Actividad =
                                row["Actividad"].ToString(),

                            modulo =
                                row["modulo"].ToString(),

                            Criticidad =
                                row["Criticidad"].ToString(),

                            Fecha =
                                row["Fecha"].ToString(),

                            Hora =
                                row["Hora"].ToString(),

                            DVH =
                                Convert.ToInt32(row["DVH"])
                        };


                        int dvhActual =
                            digitoVerificador.CalcularDVH(bit);



                        if (dvhActual != bit.DVH)
                        {
                            huboError = true;
                        }


                        sumaDVH += dvhActual;
                    }



                    int dvvGuardado =
                        DalDvv.ObtenerDvvGuardado("Bitacora");


                    if (sumaDVH != dvvGuardado)
                    {
                        huboError = true;
                    }



                    if (huboError)
                    {
                        listaErrores.AddRange(
                            DalDvv.ObtenerOperaciones("Bitacora")
                        );
                    }

                }
                catch (Exception ex)
                {
                    listaErrores.Add(new BE.Infraccion
                    {
                        Tabla = "Bitacora",
                        IdRegistro = "ERROR",
                        Operacion = ex.Message
                    });
                }

                // =====================================================
                // RESERVAS
                // =====================================================
                try
                {
                    BLL.BLLReserva bllReserva =
                        new BLL.BLLReserva();


                    List<BE.Reserva> reservas =
                        bllReserva.ObtenerTodasParaDVV();


                    int sumaDVH = 0;
                    bool huboError = false;



                    foreach (BE.Reserva res in reservas)
                    {
                        int dvhActual =
                            digitoVerificador.CalcularDVH(res);



                        if (dvhActual != res.DVH)
                        {
                            huboError = true;
                        }


                        sumaDVH += dvhActual;
                    }



                    int dvvGuardado =
                        DalDvv.ObtenerDvvGuardado("Reservas");


                    if (sumaDVH != dvvGuardado)
                    {
                        huboError = true;
                    }



                    if (huboError)
                    {
                        listaErrores.AddRange(
                            DalDvv.ObtenerOperaciones("Reservas")
                        );
                    }

                }
                catch (Exception ex)
                {
                    listaErrores.Add(new BE.Infraccion
                    {
                        Tabla = "Reservas",
                        IdRegistro = "ERROR",
                        Operacion = ex.Message
                    });
                }





                // =====================================================
                // HABITACIONES
                // =====================================================
                try
                {
                    BLLHabitacion bllHabitacion =
                        new BLLHabitacion();


                    List<Habitacion> habitaciones =
                        bllHabitacion.ObtenerTodasParaDVV();



                    int sumaDVH = 0;
                    bool huboError = false;



                    foreach (Habitacion hab in habitaciones)
                    {
                        int dvhActual =
                            digitoVerificador.CalcularDVH(hab);



                        if (dvhActual != hab.DVH)
                        {
                            huboError = true;
                        }


                        sumaDVH += dvhActual;
                    }



                    int dvvGuardado =
                        DalDvv.ObtenerDvvGuardado("Habitacion");



                    if (sumaDVH != dvvGuardado)
                    {
                        huboError = true;
                    }



                    if (huboError)
                    {
                        listaErrores.AddRange(
                            DalDvv.ObtenerOperaciones("Habitacion")
                        );
                    }

                }
                catch (Exception ex)
                {
                    listaErrores.Add(new BE.Infraccion
                    {
                        Tabla = "Habitacion",
                        IdRegistro = "ERROR",
                        Operacion = ex.Message
                    });
                }



                // Si no hubo errores, no mostrar nada
                if (listaErrores.Count == 0)
                {
                    DalDvv.LimpiarAuditoria();
                }


                return listaErrores;

            }
            catch (Exception ex)
            {
                listaErrores.Add(new BE.Infraccion
                {
                    Tabla = "SISTEMA",
                    IdRegistro = "N/A",
                    Operacion = ex.Message
                });

                return listaErrores;
            }
        }

        public void LimpiarAuditoria()
        {
            DalDvv.LimpiarAuditoria();
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

                // RECALCULAR DVH DE BITACORA
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
                this.RecalcularDVV("Habitacion");

                LimpiarAuditoria();

                bLLbitacora.InsertarBitacora(usu, "Recálculo masivo de DVH y DVV", "Seguridad", "3");
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al recalcular toda la base de datos: " + ex.Message);
            }
        }
    }
}