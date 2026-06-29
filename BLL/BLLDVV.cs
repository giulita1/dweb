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
        int usu = SingletonSession.Instancia.Usuario != null ? SingletonSession.Instancia.Usuario.Cod_Usuario : 0;

        public void RecalcularDVV(string nombreTabla_62_RS)
        {
            try
            {
                List<int> listaDvhs = DalDvv.ObtenerTodosLosDVH(nombreTabla_62_RS);
                SEG.DigitoVerificador motorDV = new SEG.DigitoVerificador();
                int sumaTotalDvv = motorDV.CalcularDVV(listaDvhs);
                DalDvv.ActualizarSumaDVV(nombreTabla_62_RS, sumaTotalDvv);
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

                //BITACORA
                try
                {
                    BLLbitacora bLLbitacora = new BLLbitacora();
                    DataTable dtbitacora = bLLbitacora.listartodabitacoraparadvv();
                    int sumaDvhCalculadaBitacora = 0;

                    foreach (DataRow row in dtbitacora.Rows)
                    {
                        desarrolloweb.BE.Bitacora BIT = new desarrolloweb.BE.Bitacora
                        {
                            Id_Bitacora = Convert.ToInt32(row["Id_Bitacora"]),
                            Id_Usuario = Convert.ToInt32(row["Id_Usuario"]),
                            Actividad = row["Actividad"].ToString(),
                            modulo = row["modulo"].ToString(),
                            Criticidad = row["Criticidad"].ToString(),
                            Fecha = row["Fecha"].ToString(),
                            Hora = row["Hora"].ToString(),
                            DVH = Convert.ToInt32(row["DVH"])
                        };
                        int dvhEnVivo = digitoVerificador.CalcularDVH(BIT);
                        if (dvhEnVivo != BIT.DVH)
                        {
                            listaErrores.Add(new BE.Infraccion { Tabla = "BitacoraS", IdRegistro = BIT.Id_Bitacora.ToString(), TipoError = "Horizontal (Fila Alterada)" });
                        }
                        sumaDvhCalculadaBitacora += dvhEnVivo;
                    }
                    int dvvGuardadoBitacora = DalDvv.ObtenerDvvGuardado("Bitacora");
                    if (sumaDvhCalculadaBitacora != dvvGuardadoBitacora)
                    {
                        listaErrores.Add(new BE.Infraccion { Tabla = "Bitacora_62_RS", IdRegistro = "COLUMNA", TipoError = "Vertical (Suma total no coincide)" });
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Error validando Bitácora: " + ex.Message); }
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
                SEG.DigitoVerificador motorDV=new SEG.DigitoVerificador();
                desarrolloweb.DAL.DALbitacora dALbitacora = new desarrolloweb.DAL.DALbitacora();
                desarrolloweb.DAL.DALusuario dALusuario = new desarrolloweb.DAL.DALusuario();

                //Recalculo Bitacora
                desarrolloweb.BLL.BLLbitacora bLLbitacora = new desarrolloweb.BLL.BLLbitacora();
                DataTable dt = bLLbitacora.listartodabitacoraparadvv();
                foreach (DataRow dr in dt.Rows)  
                {
                    desarrolloweb.BE.Bitacora bit = new desarrolloweb.BE.Bitacora
                    {
                        Id_Bitacora = Convert.ToInt32(dr["Id_Bitacora"]),
                        Id_Usuario = Convert.ToInt32(dr["Id_Usuario"]),
                        Actividad = dr["Actividad"].ToString(),
                        modulo = dr["modulo"].ToString(),
                        Criticidad = dr["Criticidad"].ToString(),
                        Fecha = dr["Fecha"].ToString(),
                        Hora = dr["Hora"].ToString(),
                        DVH = Convert.ToInt32(dr["DVH"])

                    };
                    int nuevoDvh = motorDV.CalcularDVH(bit);
                    dALbitacora.ActualizarDVH(bit.Id_Bitacora, nuevoDvh);
                }



                //RECALCULAR VERTICALES (DVV) DE TODAS LAS TABLAS
                this.RecalcularDVV("Usuarios");
                this.RecalcularDVV("Bitacora");


                //registro bitacora
                bLLbitacora.InsertarBitacora(usu, "Recálculo masivo de DVH y DVV", "Seguridad", "3");

            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al recalcular toda la base de datos: " + ex.Message);

            }
        }
    }
}
