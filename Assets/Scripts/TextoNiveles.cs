using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextoNiveles : MonoBehaviour
{
    public string[] lineas; //Donde almacenaremos en un Array(String) todas las lineas de la historia
    public void NextLVL(int lvl)//Funciona para llamar a escribir en el texto principal, recogemos el dato que había entre parentesis(los datos se mandan al llamar la función) y lo guaramos en un varible temporal "lvl", cambia el texto, apunta el nivel actual y el lugar en el que te encuentras
    {
        switch (lvl)
        {
            case -3: //Volver al menu
                GetComponent<Principal>().EliminarPartida();
                break;
            case -2: //Pantalla de muerte por elección
                GetComponent<Principal>().textoPrincipal.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[55];
                GetComponent<Principal>().current.canSave = false;
                CambioNivel(-3, false, false, 1, true , 3);
                break;
            case -1: //Pantalla de muerte por batalla
                GetComponent<Principal>().textoPrincipal.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[56];
                GetComponent<Principal>().current.canSave = false;
                CambioNivel(-3, false, false, 1, true, 3);

                break;
            case 0:
                GetComponent<Principal>().current.canSave = false;
                GetComponent<Principal>().current.lugarActual = CambioLugar(1);
                GetComponent<Principal>().textoPrincipal.GetComponent<TextMeshProUGUI>().text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
                if (GetComponent<Principal>().current.bono < 1)
                {
                    if (GetComponent<Principal>().current.bono > 0 && GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text.Length <= 1)
                        GetComponent<Principal>().MensajeError(GetComponent<Config>().system[9], 3);
                    else if (GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text.Length <= 1) //Compruebo que el usuario haya metido nombre del Personaje
                        GetComponent<Principal>().MensajeError(GetComponent<Config>().system[0], 3);    //Llama a la funcion que hace aparecer el PanelError con el mensaje entre comillas y lo hace permanecer 3 segundos
                    else if (GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text.Length >= 1 && GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text.Length < 4)
                        GetComponent<Principal>().MensajeError(GetComponent<Config>().system[1], 3);    //Llama a la funcion que hace aparecer el PanelError con el mensaje entre comillas y lo hace permanecer 3 segundos
                    else if (GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text.Length >= 4)
                    {
                        GameObject.Find("NombrePersonaje").GetComponent<TextMeshProUGUI>().text = GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text;
                        GetComponent<Principal>().current.name = GameObject.Find("TextNombre").GetComponent<TextMeshProUGUI>().text;
                        GetComponent<Principal>().current.currentLevel = 101;
                        NextLVL(GetComponent<Principal>().current.currentLevel);
                        GameObject.Find("PanelInicPersonaje").SetActive(false);
                        GetComponent<Principal>().CalculaStatsJugador();
                        GetComponent<Principal>().current.turnosTotal++;
                        GetComponent<TextoNiveles>().GuardarMemory(0, false);
                        GetComponent<TextoNiveles>().Tutoriales(2);
                        for (int i = 0; i < 7; i++)//Creamos el armamento, ahora vacío, hay 6 elementos, el 7 es para el objeto temporal si el huevo está ocupado y el jugador quiere cambiarlo o no.
                        {
                            Armament arm = new Armament();
                            arm.type = i;
                            arm.description = "Vacío";
                            GetComponent<Principal>().current.armament.Add(arm);
                        }
                        GetComponent<Principal>().AnadirArmamento(1, 1);
                        GetComponent<Principal>().CrearPNJ(4, false);
                        ////Si el nombre tiene 3 letras o más. continua al siguiente nivel
                    }
                }//Configuración del Personaje
                break;
            case 101:
                GetComponent<Principal>().CalculaStatsTotal();
                CambioNivel(102, true, false, 1, true, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0 , "", "", "");
                break;
            case 102:
                CambioNivel(103, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().current.canSave = true;
                break;
            case 103:
                CambioNivel(104, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 104:
                CambioNivel(105, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 105:
                Tutoriales(6);
                CambioNivel(106, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, true, 3, GetComponent<Config>().lineasBTN[17], GetComponent<Config>().lineasBTN[18], GetComponent<Config>().lineasBTN[19]);
                GuardarMemory(0, true);
                break;
            case 106:
                CambioNivel(107, true, false, 1, true, 1);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, true, 2, GetComponent<Config>().lineasBTN[20], GetComponent<Config>().lineasBTN[21], "");
                break;
            case 107:
                break;
            case 10711:
                CambioNivel(10712, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 10712:
                CambioNivel(107131, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, true, 2, GetComponent<Config>().lineasBTN[22], GetComponent<Config>().lineasBTN[23], "");
                GuardarMemory(0, true);
                break;
            case 107121:
                CambioNivel(10714, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 107122:
                CambioNivel(10714, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 107131:
                GetComponent<Principal>().AddBotones(0, "", "", "");
                break;
            case 107132:
                CambioNivel(10714, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 10714:
                CambioNivel(10715, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 10715:
                CambioNivel(10716, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().CrearPNJ(1, true);
                break;
            case 10716:
                CambioNivel(108, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().CrearPNJ(2, true);
                break;
            case 10721:
                CambioNivel(10722, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 10722:
                CambioNivel(10723, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().CrearPNJ(1, true);
                break;
            case 10723:
                CambioNivel(108, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                GetComponent<Principal>().CrearPNJ(2, true);
                break;
            case 108:
                CambioNivel(109, true, false, 1, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 109:
                CambioNivel(110, true, true, 3, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                CambiarNombreLugar (1,2,false);
                break;
            case 110:
                CambioNivel(111, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 111:
                CambioNivel(112, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().CrearPNJ(1, true);
                break;
            case 112:
                CambioNivel(113, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 113:
                CambioNivel(114, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 114:
                CambioNivel(115, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 115:
                CambioNivel(116, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 116:
                CambioNivel(117, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 117:
                CambioNivel(118, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 118:
                CambioNivel(119, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 119:
                CambioNivel(120, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GetComponent<Principal>().CambiarStatPNJ(1,"afinidad", 0, false);
                GetComponent<Principal>().CambiarStatPNJ(2, "afinidad", 0, false);
                break;
            case 120:
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, true, 3, GetComponent<Config>().lineasBTN[24], GetComponent<Config>().lineasBTN[25], GetComponent<Config>().lineasBTN[30]);
                GetComponent<Principal>().CrearPNJ(3, true);
                break;
            case 1201:
                CambioNivel(121, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GuardarMemory(3, true);
                GetComponent<Principal>().CambiarStatPNJ(3, "afinidad", -1, true);
                //memoria ataque a frost true
                break;
            case 1202:
                CambioNivel(121, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GuardarMemory(3, true);
                GetComponent<Principal>().CambiarStatPNJ(3, "afinidad", -1,true);
                //memoria ataque a frost true
                break;
            case 1203:
                CambioNivel(122, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                GuardarMemory(3, false);
                GetComponent<Principal>().CambiarStatPNJ(3, "afinidad", 1,true);
                //memoria ataque a frost false
                break;
            case 121:
                CambioNivel(122, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 122:
                CambioNivel(123, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 123:
                CambioNivel(124, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 124:
                CambioNivel(125, true, false, 0, false, 0);
                if (GetComponent<Principal>().current.memorys.memory[1].check)
                    GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                else
                    GetComponent<Principal>().EscribirLinea(lvl, 2, false, false, 0, "", "", "");
                break;
            case 125:
                if (!GetComponent<Principal>().current.elecciones[0] && !GetComponent<Principal>().current.elecciones[1])
                {
                    GetComponent<Principal>().EscribirLinea(lvl, 2, true, true, 2, GetComponent<Config>().lineasBTN[31], GetComponent<Config>().lineasBTN[32], "");
                }
                else if (GetComponent<Principal>().current.elecciones[0] && !GetComponent<Principal>().current.elecciones[1])
                {
                    GetComponent<Principal>().EscribirLinea(lvl, 3, false, true, 2, "", GetComponent<Config>().lineasBTN[32], "");
                }
                else if (!GetComponent<Principal>().current.elecciones[0] && GetComponent<Principal>().current.elecciones[1])
                    GetComponent<Principal>().EscribirLinea(lvl, 3, false, true, 1, GetComponent<Config>().lineasBTN[31], "", "");
                break;
            case 12511:
                CambioNivel(12512, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                break;
            case 12512:
                CambioNivel(12513, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12513:
                CambioNivel(12514, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12514:
                CambioNivel(12515, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12515:
                CambioNivel(12516, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12516:
                CambioNivel(12517, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12517:
                CambioNivel(12518, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12518:
                if (GetComponent<Principal>().current.elecciones[0] && GetComponent<Principal>().current.elecciones[1])
                    CambioNivel(126, true, false, 0, false, 0);
                else
                    CambioNivel(125, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12521:
                CambioNivel(12522, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12522:
                CambioNivel(12523, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 12523:
                if (GetComponent<Principal>().current.elecciones[0] && GetComponent<Principal>().current.elecciones[1])
                    CambioNivel(126, true, false, 0, false, 0);
                else
                    CambioNivel(125, true, false, 0, false, 0);
                break;
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");

            case 126:
                EliminarElecciones();
                CambioNivel(127, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 127:
                CambioNivel(128, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 2, true, false, 0, "", "", "");
                foreach (PNJ pnj in GetComponent<Principal>().current.pnj) //Si la afinidad con Frost (id:3) es mayor a 6 le dará una daga afilada, si es 6 o menor una daga oxidada
                    if (pnj.id == 3)
                        if (pnj.afinidad > 6)
                            GetComponent<Principal>().AnadirArmamento(1, 3);
                        else
                            GetComponent<Principal>().AnadirArmamento(2, 3);
                GetComponent<Principal>().AnadirBoost(2,3);
                break;
            case 128:
                CambioNivel(129, true, false, 0, true, 4);
                GetComponent<Principal>().EscribirLinea(lvl, 1, false, false, 0, "", "", "");
                break;
            case 129:
                CambioNivel(130, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;
            case 130:
                CambioNivel(131, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;
            case 131:
                CambioNivel(132, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;
            case 132:
                CambioNivel(133, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;
            case 133:
                CambioNivel(134, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;
            case 134:
                CambioNivel(100, true, false, 0, false, 0);
                GetComponent<Principal>().EscribirLinea(lvl, 3, true, false, 0, "", "", "");
                break;

        }
    }
    public void GuardarMemory(int position, bool dato)
    {
        GetComponent<Principal>().current.memorys.memory[position].check = dato;
    }
    public void EliminarElecciones()
    {
        for (int i = 0; GetComponent<Principal>().current.elecciones.Length>i; i++)
        {
            GetComponent<Principal>().current.elecciones[i] = false;
        }
    }
    public void BotonFuerzaPlus()
    {
        if (GetComponent<Principal>().current.bono > 0)
        {
            GetComponent<Principal>().current.fuerza ++;
            GetComponent<Principal>().current.bono -= 1;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[23],2);
        GetComponent<Principal>().CalculaStatsJugador();

    }//Boton + del Stat Fuerza
    public void BotonFuerzaMenos()
    {
        if (GetComponent<Principal>().current.bono <7 && GetComponent<Principal>().current.fuerza > 7)
        {
            GetComponent<Principal>().current.fuerza --;
            GetComponent<Principal>().current.bono ++;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[24], 2);
        GetComponent<Principal>().CalculaStatsJugador();
    }//Boton - del Stat Fuerza
    public void BotonDefensaPlus()
    {
        if (GetComponent<Principal>().current.bono > 0)
        {
            GetComponent<Principal>().current.defensa++;
            GetComponent<Principal>().current.bono -= 1;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[23], 2);
        GetComponent<Principal>().CalculaStatsJugador();

    }//Boton - del Stat Defensa
    public void BotonDefensaMenos()
    {
        if (GetComponent<Principal>().current.bono < 7 && GetComponent<Principal>().current.defensa > 5)
        {
            GetComponent<Principal>().current.defensa--;
            GetComponent<Principal>().current.bono++;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[24], 2);
        GetComponent<Principal>().CalculaStatsJugador();
    }//Boton + del Stat Defensa
    public void BotonSuertePlus()
    {
        if (GetComponent<Principal>().current.bono > 0)
        {
            GetComponent<Principal>().current.suerte++;
            GetComponent<Principal>().current.bono -= 1;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[23], 2);
        GetComponent<Principal>().CalculaStatsJugador();
    }//Boton + del Stat Suerte
    public void BotonSuerteMenos()
    {
        if (GetComponent<Principal>().current.bono < 7 && GetComponent<Principal>().current.suerte > 5)
        {
            GetComponent<Principal>().current.suerte--;
            GetComponent<Principal>().current.bono++;
        }
        else
            GetComponent<Principal>().CalculaStatsJugador();
        GetComponent<Principal>().MensajeError(GetComponent<Config>().system[24], 2);
    }//Boton - del Stat Suerte
    public void BotonElocuenciaPlus()
    {
        if (GetComponent<Principal>().current.bono > 0)
        {
            GetComponent<Principal>().current.elocuencia++;
            GetComponent<Principal>().current.bono -= 1;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[23], 2);
        GetComponent<Principal>().CalculaStatsJugador();
    }//Boton + del Stat Elocuencia
    public void BotonElocuenciaMenos()
    {
        if (GetComponent<Principal>().current.bono < 7 && GetComponent<Principal>().current.elocuencia > 5)
        {
            GetComponent<Principal>().current.elocuencia--;
            GetComponent<Principal>().current.bono++;
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[24], 2);
        GetComponent<Principal>().CalculaStatsJugador();
    }//Boton - del Stat Elocuencia
    public void CambioNivel(int numLvl, bool sumaTurno, bool cambioLugar, int ID, bool cargarImagen, int idImagen)
    {
        if (numLvl == -1)
        {
            GetComponent<Principal>().current.currentLevel = 0;
            GetComponent<Principal>().current = new CurrentCharacter();
            GetComponent<Principal>().current.CrearPersonaje();
            GetComponent<Config>().userConfig = new UserConfig();
            GetComponent<Config>().userConfig.ConfInic();
        }
        GetComponent<Principal>().current.currentLevel = numLvl;
        if (cambioLugar)
            GetComponent<Principal>().current.lugarActual = CambioLugar(ID);
        if (sumaTurno)
            GetComponent<Principal>().current.turnosTotal++;
        if (cargarImagen)
            CargarImagenPrincipal(true, idImagen);
        else
            CargarImagenPrincipal(false, 0);
    }//Acción de pasar de nivel, le pasamos el numero del nivel, si este cambio cuenta como turno y nombre de la ubicación
    public string CambioLugar(int id)
    {
        for (int i = 0; i < GetComponent<Principal>().current.sites.Count; i++)
        {
            if (GetComponent<Principal>().current.sites[i].id == id)
            return GetComponent<Principal>().current.sites[i].name;
        }
        GetComponent<Principal>().CrearLugar(id);
        for (int i = 0; i < GetComponent<Principal>().current.sites.Count; i++)
        {
            if (GetComponent<Principal>().current.sites[i].id == id)
                return GetComponent<Principal>().current.sites[i].name;
        }
        return "error al nombrar lugar";
    }//Cambia la ubicación del personaje, si no existe la crea
    public void CambiarNombreLugar(int IDa, int IDb, Boolean del)
    {
        Lugares lugb = new();
        Lugares lugc = new();
        foreach (Lugares luga in GetComponent<Config>().lugares.lugares)
        {
            if (luga.id == IDb)
            {
                lugb = luga;
                break;
            }
        }
        foreach (Lugares lug in GetComponent<Principal>().current.sites)
        {
            if (lug.id == IDa)
            {
                lug.name = lugb.name;
                lug.id = lugb.id;
                lug.description = lugb.description;
                if (del)
                    GetComponent<Principal>().current.sites.Remove(lug);
                break;
            }
        }
    }//Cambia el nombre de una ubicación que exista (IDa) por otro (IDb)
    public void Tutoriales(int numTutorial)
    {
        if (GetComponent<Config>().userConfig.Tuts)
        {
            GetComponent<Principal>().panelTutorial.SetActive(true);
            GameObject PanelTuto = GetComponent<Principal>().panelTutorial;
            PanelTuto.transform.position = GetComponent<Principal>().PanelPrincipal.transform.position;
            GameObject TextoSup = GameObject.Find("TextTutoSup");
            GameObject TextoInf = GameObject.Find("TextTutoInf");
            switch (numTutorial)
            {
                case 1:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[43];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[44];
                    break;
                case 2:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[45];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[46];
                    break;
                case 3:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[47];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[48];
                    break;
                case 4:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[49];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[50];
                    break;
                case 5:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[51];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[52];
                    break;
                case 6:
                    TextoSup.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[53];
                    TextoInf.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[54];
                    break;
            }
        }
    }
    public void CargarImagenPrincipal(bool imagen, int idImage)
    {
        if (imagen)
        {
            GetComponent<Principal>().ImagenPrincipal.rectTransform.sizeDelta = new Vector2(GetComponent<Principal>().ImagenPrincipal.rectTransform.sizeDelta[0], 650);
            GetComponent<Principal>().ImagenPrincipal.GetComponent<Image>().sprite = GetComponent<Config>().imgLugares[idImage];
        }
        else
            GetComponent<Principal>().ImagenPrincipal.rectTransform.sizeDelta = new Vector2(GetComponent<Principal>().ImagenPrincipal.rectTransform.sizeDelta[0], 0);
    }
}