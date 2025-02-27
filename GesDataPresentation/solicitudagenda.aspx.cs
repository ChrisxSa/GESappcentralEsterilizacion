﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ges.data.model;
using ges.data.business;
using System.Text;

namespace ges.data.presentation
{
    public partial class solicitudagenda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            idUsuario.Value = Session["usuarioId"].ToString();
            cmpfecha.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
            comboServicios.Text = CrearComboServicios("cmpidservicio", idServicio.Value);

            if (accionAProcesar.Value.Equals("generarComboPabellones") && idServicio.Value != "")
            {
                comboPabellon.Text = CrearComboPabellon("cmpidpabellon", idPabellon.Value, idServicio.Value);
            }
            else
            {
                comboPabellon.Text = "<select runat=\"server\" class=\"form-control\" aria-required=\"True\" required=\"required\" \">"
                    + " <option value=\"\">Seleccione</option></select> ";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                GestorDataBusinessAgenda gb = new GestorDataBusinessAgenda();
                Agenda b = new Agenda();
                Respuesta r = new Respuesta();
                b.idUsuario = Convert.ToInt32(idUsuario.Value);
                b.idPabellon = Convert.ToInt32(idPabellon.Value);
                b.asunto = cmpasunto.Value;
                b.descripcion = cmpdescripcion.Value;
                b.fechaAgenda = cmpfecha.Value;
                b.horaAgenda = cmphora.Value;

                if (b.idUsuario != 0)
                {
                    r = gb.Grabar(b);

                    if (r.estado == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Error:" + r.descripcion + "!');", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert(':) Ingresado Correctamente!');", true);
                        idServicio.Value = "";
                        idPabellon.Value = "";
                        cmpasunto.Value = "";
                        cmpdescripcion.Value = "";
                        cmpfecha.Value = "";
                        cmphora.Value = "";
                        accionAProcesar.Value = "";
                        comboServicios.Text = CrearComboServicios("cmpidservicio", idServicio.Value);
                        comboPabellon.Text = "<select runat=\"server\" class=\"form-control\" aria-required=\"True\" required=\"required\" \">"
                            + " <option value=\"\">Seleccione</option></select> ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string CrearComboServicios(string nombrecampo, string idServicio)
        {
            try
            {
                GestorDataBusinessServicioClinico gb = new GestorDataBusinessServicioClinico();
                listaServicioClinico obj = gb.ListarServicioClinico();
                string combo = "<select runat=\"server\" id=\"" + nombrecampo + "\" name=\"" + nombrecampo + "\" class=\"form-control\" onchange=\"CambioServicio()\" aria-required=\"True\" required=\"required\" \"> ";

                combo = combo + "<option value=\"\">Seleccione</option>";
                if (obj.List.Count() > 0)
                {

                    IEnumerable<ServicioClinico> query = obj.List.OrderBy(num => num.nombre);

                    foreach (var l in query)
                    {
                        string t1 = l.idServicioClinico.ToString();
                        string t2 = l.nombre.ToString();
                        string selected = "selected";
                        if (idServicio == t1)
                        {
                            combo = combo + "<option value='" + t1 + "' " + selected + ">" + t2 + "</option>";
                        }
                        else
                        {
                            combo = combo + "<option value='" + t1 + "'>" + t2 + "</option>";
                        }
                    }
                }
                combo = combo + "</select>";
                return combo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string CrearComboPabellon(string nombrecampo, string idPabellon, string idServicio)
        {
            try
            {
                GestorDataBusinessPabellones gb = new GestorDataBusinessPabellones();
                listaPabellones obj = gb.ListarPabellones(Convert.ToInt32(idServicio));
                string combo = "<select runat=\"server\" id=\"" + nombrecampo + "\" name=\"" + nombrecampo + "\" class=\"form-control\" onchange=\"CambioPabellon()\" aria-required=\"True\" required=\"required\" \"> ";
                combo = combo + "<option value=\"\">Seleccione</option>";

                if (obj.ListPabellones.Count() > 0)
                {

                    IEnumerable<Pabellones> query = obj.ListPabellones.OrderBy(num => num.nombre);

                    foreach (var l in query)
                    {
                        string t1 = l.idPabellon.ToString();
                        string t2 = l.nombre.ToString();
                        string selected = "selected";
                        if (idServicio == l.idServicioClinico.ToString())
                        {
                            if (idPabellon == t1)
                            {
                                combo = combo + "<option value='" + t1 + "' " + selected + ">" + t2 + "</option>";
                            }
                            else
                            {
                                combo = combo + "<option value='" + t1 + "'>" + t2 + "</option>";
                            }
                        }
                    }
                }
                combo = combo + "</select>";
                return combo;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}