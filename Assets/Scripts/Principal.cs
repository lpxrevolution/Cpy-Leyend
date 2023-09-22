using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Linq.Expressions;
using TMPro; //Esta libreria es necesaria para que funcione el TextMesh
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Principal : MonoBehaviour
{
    public Image barraVida, ImagenPrincipal;
    public GameObject MainPanel, LoadButton, SavePlusButtons;//No hace falta explicar esto
    public int nivel; //Aquí podremos introducir desde el inspector en nivel al que queramos ir y poder testear
    public CurrentCharacter current; //Personaje principal con todas sus variables
    public GameObject PanelPrincipal, PanelPNJs, PanelLugares, panelAviso, panelInfo; //Panel de la Scena Principal
    public GameObject BotonPNJ; // Botón para consultar PNJs
    public RectTransform imageMain, scroll, scrollPer, scrollDet, scrollMoch, scrollSaveLoad; //Panel donde se encuentra el texto, el que realmente es deslizable
    public GameObject textoPrincipal, panelIntScroll, panelMap, panelMochila, panelIntMochila, panelPersonaje, panelDetalles, panelBatalla, panelInicialCrear, panelMenuPrincipal, panelConfig, panelSaveLoad, panelTutorial; //Paneles principales
    public GameObject Tiempo, EfectoActivo, Lugar; //Texto donde se apunta el tiempo de juego, estado alterado nos afecta , donde se encuentra el Jugador
    public GameObject PanelError, txtEquipment; //Panel de Error en pantalla (aunque se usará para más cosas que no solo para errores)
    public GameObject botonOp1, botonOp2, botonOp3; //Botón opción 1, 2 y 3
    public string StatACambiar; //Almacena que Estadistica del jugador vamos a cambiar
    public int ValorCambio; //Almacena temporalmente cuánto será el cambio
    public int TiempoCambioStat; //Almacena durante cuánto tiempo parmanecera el cambio
    private Vector3 PosPanelPer,  PosPanelPrincRect, PosPanelPrinc; // Vector3 para la posicion de los Paneles
    private int numSuerte, numDaño; //numero resultante de la TiradaSuerte y TiradaDaño
    public GameObject ImagenLugar, Lugares, PNJ, ImagenPNJ, Map, Armadura, PanelSaveLoadInt;
    public int TamañoScroll = 150;
    public TextMeshProUGUI tiempo, txtBotonOp1, txtBotonOp2, txtBotonOp3, txtEstadosAfectados;
    public float tiempoTranscurrido = 0f;

    public void Awake()
    {
        current.CrearPersonaje();
        GetComponent<Config>().userConfig = new UserConfig();
        GetComponent<Config>().userConfig.ConfInic();
        GameObject.Find("TxtVersionP").GetComponent<TextMeshProUGUI>().text = "v." + Application.version;
        GameObject.Find("TxtVersion").GetComponent<TextMeshProUGUI>().text = "v." + Application.version;
        PosPanelPrinc = new Vector2(GameObject.FindWithTag("panelprincipal").transform.position.x,  //
                                    GameObject.FindWithTag("panelprincipal").transform.position.y); // guardamos en PosPanelPrinc la posicion del PanelPrincipal
        PosPanelPrincRect = new Vector2(GameObject.FindWithTag("panelprincipal").GetComponent<RectTransform>().sizeDelta.x, //
                                      GameObject.FindWithTag("panelprincipal").GetComponent<RectTransform>().sizeDelta.y);   // guardamos en PosPanelPrincRect el tamaño RectTransform del PanelPrincipal
        PosPanelPer = new Vector2(PosPanelPrincRect[0] + (PosPanelPrincRect[0] / 2),    //
                              PosPanelPrincRect[1] / 2);                                // Calculamos la posición del PosPanelPer a partir de los datos del RectTranform PanelPrincipal, esto nos permitira colocarlo de forma correcta se cuál sea la resolución del dispositivo
        GetComponent<TextoNiveles>().NextLVL(current.currentLevel);  //llamo a la funcion NextLVL en el Script TextoNivel pasandole la viable NivelActual del Script DatosJugado, ahora valdra 0 asi que carga el texto de nivel 1
        Canvas.ForceUpdateCanvases();                               //Actualizo el estado del Canvas (UI) para que la instrucción anterior coja el tamaño correcto. (los canvas se actualizan al final de cada Frame por lo que si no hago esto cogeria el tamaño sin actualizar, vamos el tamaño antes de la instrucción).
        scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(0, textoPrincipal.GetComponent<RectTransform>().rect.height + 150);    //
        scrollPer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (GameObject.Find("PanelPer1").GetComponent<RectTransform>().rect.height +
                                                                            GameObject.Find("PanelPer2").GetComponent<RectTransform>().rect.height +
                                                                            GameObject.Find("PanelPer3").GetComponent<RectTransform>().rect.height) -
                                                                            GameObject.Find("PanelScrollPers").GetComponent<RectTransform>().rect.height);       //Redimensiona el Panel "Scroll" sumandole 0 al ancho y igualandolo al alto al cuadro de texto TextoPrincipal mas 150px para hacerlo un poco mas grande y que quepa el boton Siguiente
        scrollDet.GetComponent<RectTransform>().sizeDelta = new Vector2(0, textoPrincipal.GetComponent<RectTransform>().rect.height + 150);//
    } //Función al cargar el Script por primera ver (al arrancar el propio juego)
    public void Update()
    {
        CalculaTiempo();//llama a la rutina para contar el tiempo de juego, escribe el tiempo transcurrido
    } //carga en cada frame, cuánto menos se ponga aquí mejor, puesto que cuanto mas carge en cada frame mas cargado irá
    public void RedimensionarScroll(float a, int b)
    {
        if (GetComponent<Principal>().ImagenPrincipal.rectTransform.sizeDelta[1] > 0)
            a += 660;
        if (botonOp1.activeInHierarchy)
        {
            a += 260;
            if (botonOp2.activeInHierarchy)
            {
                a += 260;
                if (botonOp3.activeInHierarchy)
                    a += 260;
            }
        }
        Canvas.ForceUpdateCanvases(); //Actualizo el estado del Canvas (UI) para que la instrucción anterior coja el tamaño correcto. (los canvas se actualizan al final de cada Frame por lo que si no hago esto cogeria el tamaño sin actualizar, vamos el tamaño antes de la instrucción).
        scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(0, textoPrincipal.GetComponent<RectTransform>().rect.height + a + 150); //Redimensiona el Panel "Scroll" sumandole 0 al ancho y igualandolo al alto al cuadro de texto TextoPrincipal mas 150px para hacerlo un poco mas grande y que quepa el boton Siguiente 
        scrollMoch.GetComponent<RectTransform>().sizeDelta = new Vector2(0, TamañoScroll);
        scrollSaveLoad.GetComponent<RectTransform>().sizeDelta = new Vector2(0, TamañoScroll);
        scrollPer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (GameObject.Find("PanelPer1").GetComponent<RectTransform>().rect.height +            //
                                                                            GameObject.Find("PanelPer2").GetComponent<RectTransform>().rect.height +            //
                                                                            GameObject.Find("PanelPer3").GetComponent<RectTransform>().rect.height +            //Sumamos el tamaño de los 3 paneles que se encuentran en scrollPer y le restamos el tamaño de PanelScrollPers(que es lo que es visible de por si)
                                                                            GameObject.Find("PanelPer4").GetComponent<RectTransform>().rect.height) -           // para saber cuánto tiene que agrandarse el Scroll
                                                                            GameObject.Find("PanelScrollPers").GetComponent<RectTransform>().rect.height);      //
        switch (b)
        {
            case 0:
                panelIntScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(0, imageMain.sizeDelta.y + textoPrincipal.GetComponent<RectTransform>().sizeDelta.y + 150f);
                break;
            case 1:
                if (botonOp1.activeInHierarchy)
                    scroll.sizeDelta = new Vector2(0, imageMain.sizeDelta.y + textoPrincipal.GetComponent<RectTransform>().sizeDelta.y + 150f);
                scrollDet.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 1200f + GameObject.Find("TextoDescripcionPNJ").GetComponent<RectTransform>().sizeDelta.y);
                break;
            case 2:
                if (botonOp2.activeInHierarchy)
                    scroll.sizeDelta = new Vector2(0, imageMain.sizeDelta.y + textoPrincipal.GetComponent<RectTransform>().sizeDelta.y + 700f);
                break;
            case 3:
                if (botonOp3.activeInHierarchy)
                    scroll.sizeDelta = new Vector2(0, imageMain.sizeDelta.y + textoPrincipal.GetComponent<RectTransform>().sizeDelta.y + 900f);
                break;
        }
    }//Redimensiona Scroll del Panel Principal
    public void NuevaPartida()
    {
        panelInicialCrear.SetActive(true);
        panelMenuPrincipal.SetActive(false);
        ImagenPrincipal.rectTransform.sizeDelta = new Vector2 (ImagenPrincipal.rectTransform.sizeDelta[0] ,0);
        if (GetComponent<Config>().userConfig.Tuts)
        {
            GetComponent<TextoNiveles>().Tutoriales(1);
        }
        current = new CurrentCharacter();
        current.CrearPersonaje();
        current.memorys = GetComponent<Config>().memorys;
        GameObject.Find("ImageSelect").GetComponent<Image>().sprite = GetComponent<Config>().imgAvatares[0];
        textoPrincipal.GetComponent<TextMeshProUGUI>().text = "";
        MainPanel.SetActive(true);
        panelMenuPrincipal.transform.position = PosPanelPer;
        current.memorys.memory[0].check = false;
        AddBotones(0, "", "", "");
        RedimensionarScroll(MainPanel.GetComponent<RectTransform>().rect.height + 600, 3);
        GameObject.Find("PanelIntScroll").transform.position = new Vector2(1055.00f, 1990.00f); //Fuerza a que el scroll se vaya a la parte superior (a la vista porque en realidad lo baja xD)
        GetComponent<TextoNiveles>().NextLVL(current.currentLevel);  //
        RedimensionarScroll(0, 3);                                   //Ambas instruccioes sirven para redimensionar correctamente al elegir Nueva Partida
        CalculaStatsJugador();
    }//Empezar Nueva Partida
    public void BotonMenu()
    {
        panelMenuPrincipal.SetActive(true);
        panelInicialCrear.SetActive(true);
        GameObject.Find("PanelMenuPrincipal").GetComponent<RectTransform>().position = PosPanelPrinc;
    }//Abrir el Menú Pasusa
    public void BotonContinuar()
    {
        if (current.name != null)
            panelMenuPrincipal.transform.position = PosPanelPer;
    }
    public void BotonConfig()
    {
        panelConfig.transform.position = PosPanelPrinc;
        panelConfig.SetActive(true);
    }
    public void BotonVolverConf()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesSaveLoad"))
            Destroy(o);//Destruimos todos los botones que existan antes de crear los nuevos
        panelSaveLoad.SetActive(false);
        Armadura.SetActive(false);
        PanelSaveLoadInt.SetActive(true);
        GetComponent<SaveLoadSys>().SaveConf();
        panelDetalles.SetActive(false);
        panelTutorial.SetActive(false);
        panelConfig.SetActive(false);
    }
    public void CalculaStatsJugador()
    {
        int tempSalud = current.salud + current.saludExtra1 + current.saludExtra2 + current.suludExtraEquipment;
        int tempFuerza = current.fuerza + current.fuerzaExtra1 + current.fuerzaExtra2 + current.fuerzaExtraEquipment;
        int tempDefensa = current.defensa + current.defensaExtra1 + current.defensaExtra2 + current.defensaExtraEquipment;
        int tempSuerte = current.suerte + current.suerteExtra1 + current.suerteExtra2 + current.suerteExtraEquipment;
        int tempElocuencia = current.elocuencia + current.elocuenciaExtra1 + current.elocuenciaExtra2 + current.elocuenciaExtraEquipment;

        GameObject.Find("NumbreUbicacion").GetComponent<TextMeshProUGUI>().text = current.lugarActual;
        GameObject.Find("numBoostCura").GetComponent<TextMeshProUGUI>().text = current.boosts[0].ToString();        //
        GameObject.Find("numBoostFuerza").GetComponent<TextMeshProUGUI>().text = current.boosts[1].ToString();      //
        GameObject.Find("numBoostDefensa").GetComponent<TextMeshProUGUI>().text = current.boosts[2].ToString();     //  Actualizamos los Boosts del panel del Personaje
        GameObject.Find("numBoostSuerte").GetComponent<TextMeshProUGUI>().text = current.boosts[3].ToString();      //
        GameObject.Find("numBoostElocuencia").GetComponent<TextMeshProUGUI>().text = current.boosts[4].ToString();   //
        float vida = current.statsTotal[0];
        barraVida.fillAmount = (current.salud + current.saludExtra1 + current.saludExtra2) / vida;
        if (panelInicialCrear.activeInHierarchy)
        {
            GameObject.Find("NumSalud2").GetComponent<TextMeshProUGUI>().text = current.salud + "";
            GameObject.Find("NumFuerza2").GetComponent<TextMeshProUGUI>().text = current.fuerza + "";
            GameObject.Find("NumDefensa2").GetComponent<TextMeshProUGUI>().text = current.defensa + "";
            GameObject.Find("NumSuerte2").GetComponent<TextMeshProUGUI>().text = current.suerte + "";
            GameObject.Find("NumElocuencia2").GetComponent<TextMeshProUGUI>().text = current.elocuencia + "";
            GameObject.Find("NumPuntos").GetComponent<TextMeshProUGUI>().text = current.bono.ToString();
        }
        else
        {
            GameObject.Find("NumSalud").GetComponent<TextMeshProUGUI>().text = tempSalud + "/" + current.statsTotal[0];
            GameObject.Find("NumFuerza").GetComponent<TextMeshProUGUI>().text = tempFuerza + "/" + current.statsTotal[1];
            GameObject.Find("NumDefensa").GetComponent<TextMeshProUGUI>().text = tempDefensa + "/" + current.statsTotal[2];
            GameObject.Find("NumSuerte").GetComponent<TextMeshProUGUI>().text = tempSuerte + "/" + current.statsTotal[3];
            GameObject.Find("NumElocuencia").GetComponent<TextMeshProUGUI>().text = tempElocuencia + "/" + current.statsTotal[4];
        }
    }//Calculo los stats del jugador, esto se llama en cada frame por si hay algun cambio, cada punto hace referencia a si mismo a excepcion de la Salud que se calcula tambien a traves de la Fuerza(+1/2) y la Resistencia(+1)
    public void CalculaStatsTotal()
    {
        EfectoActivo.GetComponent<TextMeshProUGUI>().text = current.estAltTxt[0] + "\n" + current.estAltTxt[1];
        for (int i = 0; i < current.statsTotal.Length; i++)
        {
            switch (i)
            {
                case 0:
                    current.statsTotal[i] = (current.salud + current.saludExtra1 + current.saludExtra2) + ((current.fuerza + current.fuerzaExtra1 + current.fuerzaExtra2) / 2) + (current.defensa + current.defensaExtra1 + current.defensaExtra2);
                    current.salud = (current.salud + current.saludExtra1 + current.saludExtra2 + ((current.fuerza + current.fuerzaExtra1 + current.fuerzaExtra2) / 2) + (current.defensa + current.defensaExtra1 + current.defensaExtra2));
                    break;
                case 1:
                    current.statsTotal[i] = current.fuerza + current.fuerzaExtra1 + current.fuerzaExtra2;
                    break;
                case 2:
                    current.statsTotal[i] = current.defensa + current.defensaExtra1 + current.defensaExtra2;
                    break;
                case 3:
                    current.statsTotal[i] = current.suerte + current.suerteExtra1 + current.suerteExtra2;
                    break;
                case 4:
                    current.statsTotal[i] = current.elocuencia + current.elocuenciaExtra1 + current.elocuenciaExtra2;
                    break;
            }
        }
    }//Calcula los Stats totales del jugador almacenados en un Array
    public void BotonSiguiente()
    {
        if (panelInicialCrear.activeInHierarchy)
        {
            if (GetComponent<Principal>().current.bono > 0)                                     //Compruebo que el usuario haya repartido los puntos Stats
                GetComponent<Principal>().MensajeError(GetComponent<Config>().system[38], 3);
        }//Mensaje de Crear pesonaje o cuándo intentas avanzar sin repartir los puntos
        current.canSave = true;
        current.nivelAnterior = current.currentLevel;
        nivel = current.currentLevel;
        if (current.memorys.memory[0].check == false)
        {
            GetComponent<TextoNiveles>().NextLVL(current.currentLevel); //Llama a la función NextLVL alojada en el Script TextoNivel pasandole (Dentro de los parentesis) el dato de la variable NivelActual alojada en el Script DatosJugador
            GameObject.Find("NumbreUbicacion").GetComponent<TextMeshProUGUI>().text = current.lugarActual;
            //RedimensionarScroll(0, 3);
            StatsLimite();
            CalculaStatsJugador();
        }// Función del botón Siguiente
        else
            MensajeError(GetComponent<Config>().system[7], 3);
    }//Tampoco hay que explicar mucho aqui, función del boton "Siguiente"
    public void AbrirPanelPersonaje()
    {
        txtEstadosAfectados.text = "";
        CalculaStatsJugador();
        if (panelInicialCrear.activeInHierarchy)
        {
            MensajeError(GetComponent<Config>().system[9], 3);
        }
        else
        {
            GameObject.Find("ImageAvatar").GetComponent<Image>().sprite = GetComponent<Config>().imgAvatares[current.numAvatar];
            panelPersonaje.transform.position = PosPanelPrinc;
            if (current.tutoPersonaje)
            {
                current.tutoPersonaje = false;
                GetComponent<TextoNiveles>().Tutoriales(3);
            }
            foreach (string stat in current.estAltTxt)
                txtEstadosAfectados.text = txtEstadosAfectados.text + stat + "\n";
        }
    }// Coloca el Panel de Personaje en la posicion de la pantalla principal
    public void AbrirPanelSaveLoad()
    {
        panelSaveLoad.SetActive(true);
        panelSaveLoad.transform.position = PosPanelPrinc;
    }
    public void AbrirArmadura()
    {
        if (!Armadura.activeInHierarchy)
        {
            scrollSaveLoad.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.width * 0.5f);
            GetComponent<Principal>().panelSaveLoad.transform.position = GetComponent<Principal>().PanelPrincipal.transform.position;
            panelSaveLoad.SetActive(true);
            Armadura.SetActive(true);
            PanelSaveLoadInt.SetActive(false);
            Vector2 position = GameObject.Find("PanelIntScrollSaveLoad").transform.localPosition;
            position.y = 0;
            GameObject.Find("PanelIntScrollSaveLoad").transform.localPosition = position;
        }//Cargamos los elementos de la armadura
        else
        {
            Armadura.SetActive(false);
            PanelSaveLoadInt.SetActive(true);
        }
        txtEquipment.GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[60] + "\n";
        foreach (Armament arm in current.armament)
        {
            if (arm.id != 0)
            {
                switch (arm.type)
                {
                    case 0:
                        GameObject.Find("imgHead").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + " " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value + "\n";
                        break;
                    case 1:
                        GameObject.Find("imgAcc").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + " " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value + "\n";
                        break;
                    case 2:
                        GameObject.Find("imgBody").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + " " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value + "\n";
                        break;
                    case 3:
                        GameObject.Find("imgWeapon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + ": " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value + "\n";
                        break;
                    case 4:
                        GameObject.Find("imgLegs").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + " " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value + "\n";
                        break;
                    case 5:
                        GameObject.Find("imgFoot").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + arm.type + arm.id);
                        if (arm.mod != "")
                            txtEquipment.GetComponent<TextMeshProUGUI>().text = txtEquipment.GetComponent<TextMeshProUGUI>().text + arm.name + " " + ObtenerStat(arm.stat) + " " + arm.mod + " " + arm.value;
                        break;
                }                
            }
            else
                switch (arm.type)
                {
                    case 0:
                        GameObject.Find("imgHead").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                    case 1:
                        GameObject.Find("imgAcc").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                    case 2:
                        GameObject.Find("imgBody").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                    case 3:
                        GameObject.Find("imgWeapon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                    case 4:
                        GameObject.Find("imgLegs").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                    case 5:
                        GameObject.Find("imgFoot").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/00");
                        break;
                }
        }
    }
    private string ObtenerStat(string stat) 
    {
        switch (stat)
        {
            case "Salud":
                return GetComponent<Config>().lineasBTN[6];
            case "Fuerza":
                return GetComponent<Config>().lineasBTN[7]; 
            case "Defensa":
                return GetComponent<Config>().lineasBTN[8]; 
            case "Suerte":
                return GetComponent<Config>().lineasBTN[9]; 
            case "Elocuencia":
                return GetComponent<Config>().lineasBTN[10]; 
        }
        return null;
    }
    public void AbrirInfoArmadura(int i)
    {
        if (current.armament[i].id != 0)
        {
            panelInfo.SetActive(true);
            GameObject.Find("TitleInfo").GetComponent<TextMeshProUGUI>().text = current.armament[i].name;
            GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text = current.armament[i].description;
            GameObject.Find("ImageEquipment").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Equipment/" + current.armament[i].type + current.armament[i].id);
            if (current.armament[i].mod != "")
                switch (current.armament[i].stat)
                {
                    case "Salud":
                        GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text += "\n" + ObtenerStat(current.armament[i].stat) + " " + current.armament[i].mod + current.armament[i].value;
                        break;
                    case "Fuerza":
                        GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text += "\n" + ObtenerStat(current.armament[i].stat) + " " + current.armament[i].mod + current.armament[i].value;
                        break;
                    case "Defensa":
                        GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text += "\n" + ObtenerStat(current.armament[i].stat) + " " + current.armament[i].mod + current.armament[i].value;
                        break;
                    case "Suerte":
                        GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text += "\n" + ObtenerStat(current.armament[i].stat) + " " + current.armament[i].mod + current.armament[i].value;
                        break;
                    case "Elocuencia":
                        GameObject.Find("TextInfo").GetComponent<TextMeshProUGUI>().text += "\n" + ObtenerStat(current.armament[i].stat) + " " + current.armament[i].mod + current.armament[i].value;
                        break;
                }

        }
    }
    public void AbrirMapa()
    {
        if (!Map.activeInHierarchy)
        {
            panelIntMochila.SetActive(false);
            Map.SetActive(true);
        }
        else
        {
            Map.SetActive(false);
            panelIntMochila.SetActive(true);
        }
    }
    public void AbrirDetallesPNJ(int id)
    {
        panelDetalles.SetActive(true);
        ImagenLugar.SetActive(false);
        Lugares.SetActive(false);
        ImagenPNJ.SetActive(true);
        PNJ.SetActive(true);
        foreach (PNJ pnj in current.pnj)
        {
            if (pnj.id == id)
            {
                GameObject.Find("PanelDetalles").transform.position = PosPanelPrinc;
                GameObject.Find("TextoDescripcionPNJ").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[29] + "\n\n" + pnj.description;
                GameObject.Find("TextoDescripcionPNJ").GetComponent<TextMeshProUGUI>().fontSize = GameObject.Find("StatDefensa").GetComponent<TextMeshProUGUI>().fontSize;
                GameObject.Find("TextoNombrePNJ").GetComponent<TextMeshProUGUI>().text = pnj.name;
                GameObject.Find("NumSaludPNJ").GetComponent<TextMeshProUGUI>().text = pnj.salud.ToString();
                GameObject.Find("NumFuerzaPNJ").GetComponent<TextMeshProUGUI>().text = pnj.fuerza.ToString();
                GameObject.Find("NumDefensaPNJ").GetComponent<TextMeshProUGUI>().text = pnj.defensa.ToString();
                GameObject.Find("NumSuertePNJ").GetComponent<TextMeshProUGUI>().text = pnj.suerte.ToString();
                GameObject.Find("NumAfinidadPNJ").GetComponent<TextMeshProUGUI>().text = pnj.afinidad.ToString();
                GameObject.Find("ImagePNJ").GetComponent<Image>().sprite = GetComponent<Config>().imgPNJ[pnj.id-1];
                RedimensionarScroll(1000, 1);
                return;
            }
        }
    }//Abrir Panel Detalles de PNJs
    public void AbrirDetallesLugar(int id)
    {
        PanelLugares.SetActive(true);
        panelDetalles.SetActive(true);
        ImagenLugar.SetActive(true);
        Lugares.SetActive(true);
        ImagenPNJ.SetActive(false);
        PNJ.SetActive(false);
        foreach (Lugares lug in current.sites)
        {
            if (lug.id == id)
            {
                GameObject.Find("PanelDetalles").transform.position = PosPanelPrinc;
                GameObject.Find("DescripciónLug").GetComponent<TextMeshProUGUI>().text = lug.description;
                GameObject.Find("TextNombreLugar").GetComponent<TextMeshProUGUI>().text = lug.name;
                GameObject.Find("ImageLuga").GetComponent<Image>().sprite = GetComponent<Config>().imgLugares[lug.id-1];
            }
        }
        RedimensionarScroll(0, 2);
    }//Abrir Panel Detalles de Lugares
    public void CerrarPanel()
    {
        {
            panelDetalles.SetActive(true);
            PanelPNJs.SetActive(true);
            PanelLugares.SetActive(true);
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesMochila"))
            {
                Destroy(o);
            }
            for (int i = 0; i < current.pnj.Count; i++)
            {
                Destroy(GameObject.Find("BotonPNJ" + current.pnj[i].name));
            }
            //PanelMochila.GetComponent<RectTransform>().position = PosPanelPer;
            panelPersonaje.GetComponent<RectTransform>().position = PosPanelPer;
            GameObject.Find("PanelDetalles").GetComponent<RectTransform>().position = PosPanelPer;
            if (panelMochila.activeInHierarchy)
                GameObject.Find("TextoPNJLugares").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().lineasBTN[14];
            GetComponent<Config>().CerrarPaneles();
        }
    }//Elimina los objetos del panel y luego llama a la función CerrarPaneles() para deshabilitar el panel y que no consuma recursos
    public void AbrirPanelMochila()
    {
        panelMochila.SetActive(true);
        panelIntMochila.SetActive(true);
        if (panelInicialCrear.activeInHierarchy)
        {
            MensajeError(GetComponent<Config>().system[9], 3);
        }
        else
        {
            BotonesPNJ();
            panelMochila.transform.position = PosPanelPrinc;
            if (current.tutoMochila)
            {
                current.tutoMochila = false;
                GetComponent<TextoNiveles>().Tutoriales(4);
            }
        }
    }// Coloca el Panel de la Mochila en la posicion de la pantalla principal
    public void CalculaTiempo()
    {

        tiempoTranscurrido += Time.deltaTime;
        string th;
        string tm;
        string ts;

        // Calcula las horas, minutos y segundos
        current.time[2] = Mathf.FloorToInt(tiempoTranscurrido / 3600);
        current.time[1] = Mathf.FloorToInt((tiempoTranscurrido % 3600) / 60);
        current.time[0] = Mathf.FloorToInt(tiempoTranscurrido % 60);

        if (current.time[2] < 10) //Añadimos un 0 delante del tiempo si este es menor a 10, para formatearlo a 00:00:00 
            th = "0";
        else
            th = ""; 
        if (current.time[1] < 10)
            tm = "0";
        else
            tm = ""; 
        if (current.time[0] < 10)
            ts = "0";
        else
            ts = "";

        tiempo.text = th + current.time[2] + ":" + tm + current.time[1] + ":" + ts + (Math.Truncate(current.time[0]));
    }//Calcula y escribe en el texto Time el tiempo de juego transcurrido
    public void MensajeError(string mensaje, float tiempoMensaje)
    {
        if (!PanelError.activeInHierarchy)
            PanelError.SetActive(true); //Activamos el Panel si no está activo
        else
            StopCoroutine(nameof(Returne)); //Si hubiera un mensaje en pantalla, detenemos la corutina para anular el temporizador
        GameObject.Find("TextoError").GetComponent<TextMeshProUGUI>().text = mensaje; //Escribe en el cuadro de texto del PanelError el mensaje enciado al llamar a la funcion (string e)
        StartCoroutine(nameof(Returne), tiempoMensaje); //llama a la corrutina "returne" que esperará los segundos que le enviamos (tiempomensaje)

        return;
    }// Función para hacer aparecer el PanelError con el mensaje que le entregamos a atraves del string e y lo mantiene en pantalla los segundos entregados con el int i
    public void BotonInfo()
    {
        MensajeError(GetComponent<Config>().system[10], 10);
    }//Botón que nos mostrará un panel de info sobre el nombre del personaje
    IEnumerator Returne(int i)
    {
        yield return new WaitForSeconds(i);
        PanelError.SetActive(false);
    }//Funcion para hacer desaparecer el PanelError, recoge un int (i) para esperar esos segundo
    public void BotonOpc1()
    {
        current.canSave = false;
        switch (current.currentLevel)
        {
            case 106:
                if (EstadAlt(GetComponent<Config>().lineasBTN[10], "+ 3", 3, 3, true) != 3)
                    MensajeError(GetComponent<Config>().lineasBTN[11] + "\n" + GetComponent<Config>().system[11] + "\n" + GetComponent<Config>().lineasBTN[10] + "\n" + "+3" + "\n" + GetComponent<Config>().system[12] + "\n" + "3 " + GetComponent<Config>().system[13], 3);
                GetComponent<TextoNiveles>().GuardarMemory(1, true);
                EfectoActivo.SetActive(true);
                break;
            case 107:
                GetComponent<TextoNiveles>().CambioNivel(10711, true, true, 1, false, 0);
                GetComponent<TextoNiveles>().GuardarMemory(2, true);
                AddBotones(0, "", "", "");
                break;
            case 107131:
                if (TiradaSuerte(false, 14))
                {
                    GetComponent<TextoNiveles>().CambioNivel(107121, true, true, 1, false, 0);
                    MensajeError(GetComponent<Config>().system[14], 3);
                    AddBotones(0, "", "", "");
                }
                else
                {
                    numDaño = TiradaDano(12, false, current.defensa + current.defensaExtra1 + current.defensaExtra2);
                    GetComponent<TextoNiveles>().CambioNivel(107122, true, true, 1, false, 0);
                    MensajeError(GetComponent<Config>().system[15] + "\n\n" + GetComponent<Config>().system[16] + "\n" + numDaño + "\n " + GetComponent<Config>().system[17], 3);
                    current.salud -= numDaño;
                    GetComponent<TextoNiveles>().GuardarMemory(1, true);
                    AddBotones(0, "", "", "");
                }
                break;
            case 120:
                current.currentLevel = 1201;
                AddBotones(0, "", "", "");
                break;
            case 125:
                current.currentLevel = 12511;
                GetComponent<Principal>().current.elecciones[0] = true;
                AddBotones(0, "", "", "");
                break;

        }
        CalculaStatsJugador();
        GetComponent<TextoNiveles>().GuardarMemory(0, false);
    }//Acciones del BotonOpc1 dependiendo del nievel en el que nos encontremos
    public void BotonOpc2()
    {
        current.canSave = false;
        switch (current.currentLevel)
        {
            case 106:
                AddBotones(0, "", "", "");
                GetComponent<TextoNiveles>().CambioNivel(-2, true, true, 1, false, 0);
                break;

            case 107:
                GetComponent<TextoNiveles>().CambioNivel(10721, true, true, 1, false, 0);
                AddBotones(0, "", "", "");
                GetComponent<TextoNiveles>().GuardarMemory(2, false);
                break;
            case 107131:
                if (TiradaSuerte(false, 13))
                {
                    GetComponent<TextoNiveles>().CambioNivel(107132, true, true, 1, false, 0);
                    MensajeError(GetComponent<Config>().system[14] + "\n" + GetComponent<Config>().system[18] + "\n" + (current.suerte + current.suerteExtra1 + current.suerteExtra2) + "\n" + GetComponent<Config>().system[19] + "\n" + numSuerte, 3);
                    AddBotones(0, "", "", "");
                    BotonSiguiente();
                }
                else
                {
                    numDaño = TiradaDano(12, true, current.defensa + current.defensaExtra1 + current.defensaExtra2);
                    GetComponent<TextoNiveles>().CambioNivel(107132, true, true, 1, false, 0);
                    MensajeError(GetComponent<Config>().system[15] + "\n\n" + GetComponent<Config>().system[20] + "\n" + numDaño + GetComponent<Config>().system[21], 3);
                    current.salud -= numDaño;
                    AddBotones(0, "", "", "");
                    BotonSiguiente();
                }
                break;
            case 120:
                current.currentLevel = 1202;
                AddBotones(0, "", "", "");
                break;
            case 125:
                current.currentLevel = 12521;
                GetComponent<Principal>().current.elecciones[1] = true;
                AddBotones(0, "", "", "");
                break;
        }
        CalculaStatsJugador();
        GetComponent<TextoNiveles>().GuardarMemory(0, false);

    }//Acciones del BotonOpc2 dependiendo del nievel en el que nos encontremos
    public void BotonOpc3()
    {
        current.canSave = false;
        switch (current.currentLevel)
        {
            case 106:
                AddBotones(0, "", "", "");
                break;
            case 120:
                current.currentLevel = 1203;
                AddBotones(0, "", "", "");
                break;

        }
        CalculaStatsJugador();
        GetComponent<TextoNiveles>().GuardarMemory(0, false);
    }//Acciones del BotonOpc3 dependiendo del nievel en el que nos encontremos
    public void CambioStats(string nombreStat, int boost, int numStat)
    {
        for (int i = 6; i <= 10; i++)
        {
            if (nombreStat == GetComponent<Config>().lineasBTN[i])
            {
                switch (i)
                {
                    case 7:
                        if (numStat == 1)
                            current.fuerzaExtra1 += boost;
                        else
                            current.fuerzaExtra2 += boost;
                        break;
                    case 8:
                        if (numStat == 1)
                            current.defensaExtra1 += boost;
                        else
                            current.defensaExtra2 += boost;
                        break;
                    case 9:
                        if (numStat == 1)
                            current.suerteExtra1 += boost;
                        else
                            current.suerteExtra2 += boost;
                        break;
                    case 10:
                        if (numStat == 1)
                            current.elocuenciaExtra2 += boost;
                        else
                            current.elocuenciaExtra1 += boost;
                        break;
                }
            }
        }
    }//Cuando se coge un potenciador, se aumenta la variable "Extra" del Stat alterado
    public void ResetStat(string a, int numStat)
    {
        for (int i = 7; i < 11; i++)
        {
            switch (i)
            {
                case 7:
                    if (numStat == 1)
                        current.fuerzaExtra1 = 0;
                    else
                        current.fuerzaExtra2 = 0;
                    break;
                case 8:
                    if (numStat == 0)
                        current.defensaExtra1 = 0;
                    else
                        current.defensaExtra2 = 0;
                    break;
                case 9:
                    if (numStat == 1)
                        current.suerteExtra1 = 0;
                    else
                        current.suerteExtra2 = 0;
                    break;
                case 10:
                    if (numStat == 1)
                        current.elocuenciaExtra2 = 0;
                    else
                        current.elocuenciaExtra1 = 0;
                    break;
            }
        }
    }//Resetea el estado alterado "a"
    public void StatsLimite()
    {
        if (current.turnosTotal == current.limiteTurno[0])
        {
            ResetStat(current.estAlt[0], 1);
            current.estAlt[0] = "";
            current.estAltTxt[0] = "";
            current.limiteTurno[0] = 0;
        }//Si el el numero de turnos es el límite del potenciador 1, lo resetea
        if (current.turnosTotal == current.limiteTurno[1])
        {
            ResetStat(current.estAlt[1], 2);
            current.estAlt[1] = "";
            current.estAltTxt[1] = "";
            current.limiteTurno[1] = 0;
        }//Si el el numero de turnos es el límite del potenciador 2, lo resetea
    }//comprueba que los estados alterados han llegado a su turno límite, si es así los borra y llama a la función para resetearlos
    public void CrearPNJ(int iId, bool bMsg)
    {
        PNJ perso = new();
        foreach (PNJ pnj in GetComponent<Config>().pnjs.pnjs)
        {
            if (pnj.id == iId) 
            { 
                perso = pnj;
            }
        }
        foreach (PNJ per in current.pnj)
        {
            if (per.id == iId)
                return;
        }//Tras crear al personaje temporal comprobamos si existe, si exite hace return perdiendo a ese personaje sino, continua
        if (bMsg)
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[2] + "\n" + perso.name, 2); //mensaje en pantalla de "conoces a.."
        current.pnj.Add(perso); //Como hemos comprobado que no existe, lo añadimos a la lista
    } //Crear PNJ con los datos pasados (nombre, edad, salud, fuerza, resistencia y suerte)
    public void CrearLugar(int ID)
    {
        foreach (Lugares luga in GetComponent<Config>().lugares.lugares)
        {
            if (luga.id == ID)
            {
                foreach (Lugares lugb in current.sites)
                {
                    if (luga.id == lugb.id)
                        return;
                    if (lugb.name == null)
                        current.sites.Remove(lugb);
                }
                current.sites.Add(luga);
            }
        }
    }//Crea una nueva entrada en la List Lugares
    public void AnadirBoost(int boost, int value)
    {
        switch (boost)
        {
            case 0:
                if (current.boosts[0] > 5)
                    MensajeError(GetComponent<Config>().system[62], 2);
                else
                {
                    current.boosts[0] += value;
                    MensajeError(GetComponent<Config>().system[61] + value + " " + GetComponent<Config>().system[69], 2);
                    if (current.boosts[0] > 5)
                        current.boosts[0] = 5;
                }
                break;
            case 1:
                if (current.boosts[1] > 5)
                    MensajeError(GetComponent<Config>().system[63], 2);
                else
                {
                    current.boosts[1] += value;
                    MensajeError(GetComponent<Config>().system[61] + value + " " + GetComponent<Config>().system[68], 2);
                    if (current.boosts[1] > 5)
                        current.boosts[1] = 5;
                }
                break;
            case 2:
                if (current.boosts[2] > 5)
                    MensajeError(GetComponent<Config>().system[64], 2);
                else
                {
                    current.boosts[2] += value;
                    MensajeError(GetComponent<Config>().system[61] + value + " " + GetComponent<Config>().system[69], 2);
                    if (current.boosts[2] > 5)
                        current.boosts[2] = 5;
                }
                break;
            case 3:
                if (current.boosts[3] > 5)
                    MensajeError(GetComponent<Config>().system[65], 2);
                else
                {
                    current.boosts[3] += value;
                    MensajeError(GetComponent<Config>().system[61] + value + " " + GetComponent<Config>().system[70], 2);
                    if (current.boosts[3] > 5)
                        current.boosts[3] = 5;
                }
                break;
            case 4:
                if (current.boosts[4] > 5)
                    MensajeError(GetComponent<Config>().system[66], 2);
                else
                {
                    current.boosts[4] += value;
                    MensajeError(GetComponent<Config>().system[61] + value + " " + GetComponent<Config>().system[71], 2);
                    if (current.boosts[4] > 5)
                        current.boosts[4] = 5;
                }
                break;
        }
    }
    public void AnadirArmamento(int idArm, int typeArm)
    {
        Armament arma = new();
        foreach (Armament arm in GetComponent<Config>().armaments.armament)
            if (arm.type == typeArm && arm.id == idArm)
            {
                for (int i = 0; i < current.armament.Count; i++)
                {
                    if (current.armament[i].type == typeArm && current.armament[i].id == 0)
                    {
                        current.armament[i].id = arm.id;
                        current.armament[i].name = arm.name;
                        current.armament[i].description = arm.description;
                        current.armament[i].mod = arm.mod;
                        current.armament[i].stat = arm.stat;
                        current.armament[i].value = arm.value;
                        CambiarStatEquipment(current.armament[i], null, false);
                        MensajeError(GetComponent<Config>().system[61] + current.armament[i].name, 2);
                        break;
                    }
                    else if (current.armament[i].type == typeArm && current.armament[i].id != 0)
                    {
                        panelAviso.SetActive(true); 
                        current.armament[6] = arm;
                        GameObject.Find("TextAvisoInt").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[72] + "\n" + current.armament[typeArm].name + "\n" + current.armament[typeArm].stat + " " + current.armament[typeArm].mod + " " + current.armament[typeArm].value + "\n" + 
                                                                                            GetComponent<Config>().system[73] + "\n" + current.armament[6].name + "\n" + current.armament[6].stat + " " + current.armament[6].mod + " " + current.armament[6].value;
                    }
                }
                break;
            }
    }
    public void AceptarCancelarArmamento(bool option)
    {
        if (option)
            foreach (Armament ar in current.armament)
                if (ar.type == current.armament[6].type)
                {
                    CambiarStatEquipment(current.armament[6], ar, true);
                    ar.id = current.armament[6].id;
                    ar.name = current.armament[6].name;
                    ar.mod = current.armament[6].mod;
                    ar.description = current.armament[6].description;
                    ar.stat = current.armament[6].stat;
                    ar.value = current.armament[6].value;
                    break;
                }
        Armament arOld = new();
        current.armament[6] = arOld;
        panelInfo.SetActive(false);
        panelAviso.SetActive(false);
    }
    private void CambiarStatEquipment(Armament arNew,Armament arOld, bool cambio)
    {
        int value = 0;
        if (cambio)
        {
            if (arOld.stat != "")
            {
                if (arOld.mod == "+")
                    value = -Int32.Parse(arOld.value);
                else if (arOld.mod == "-")
                    value = Int32.Parse(arOld.value);
            }
            if (arOld.stat != "")
            {
                switch (arOld.stat)
                {
                    case "Salud":
                        current.suludExtraEquipment += value;
                        break;
                    case "Fuerza":
                        current.fuerzaExtraEquipment += value;
                        break;
                    case "Defensa":
                        current.defensaExtraEquipment += value;
                        break;
                    case "Suerte":
                        current.suerteExtraEquipment += value;
                        break;
                    case "Elocuencia":
                        current.elocuenciaExtraEquipment += value;
                        break;
                }
            }
        }
        if (arNew.stat != "")
        {
            if (arNew.mod == "+")
                value = Int32.Parse(arNew.value);
            else if (arNew.mod == "-")
                value = -Int32.Parse(arNew.value);
        }
        switch (arNew.stat)
        {
            case "Salud":
                current.suludExtraEquipment += value;
                break;
            case "Fuerza":
                current.fuerzaExtraEquipment += value;
                break;
            case "Defensa":
                current.defensaExtraEquipment += value;
                break;
            case "Suerte":
                current.suerteExtraEquipment += value;
                break;
            case "Elocuencia":
                current.elocuenciaExtraEquipment += value;
                break;
        }
        panelAviso.SetActive(false);
    }
    public int EstadAlt(string stat, string cantidad, int valor, int turnos, bool OcultarBotones)
    {
        StatACambiar = stat;
        ValorCambio = valor;
        TiempoCambioStat = turnos;
        for (int i = 0; i < current.estAlt.Length; i++)
            if (current.estAlt[i] == "")
            {
                current.estAlt[i] = StatACambiar; //Ponemos en EstAlt[i] el string que hemos escrito anteriormente en StatACambiar
                CambioStats(StatACambiar, ValorCambio, i); //Mandamos para cambiar los Stats afectados
                current.estAltTxt[i] = stat + " " + cantidad; //Escribimos en EstAltTxt[i] que estat está afectado, añado espacios porque se superponen y añado la "cantidad"
                if (OcultarBotones)
                {
                    GetComponent<Principal>().botonOp1.SetActive(false);//
                    GetComponent<Principal>().botonOp2.SetActive(false);// Ocultamos los botones
                    GetComponent<Principal>().botonOp3.SetActive(false);//
                }
                current.limiteTurno[i] = current.turnosTotal + TiempoCambioStat; //Escribimos un int en LimiteTurno[i] indicando en que turno se termina el Estado Alterado
                return i;
            }
            else if (current.estAlt[i] != "")
                MensajeError(GetComponent<Config>().system[80], 3);
        CalculaStatsJugador();
        return 3;
    }//Comprobamos que podemos afectar con algun Estado Alterado, guarda el estado alterado en el hueco libre y actualiza los Stats del jugador, si no, dara un mensaje indicandolo
    public bool TiradaSuerte(bool batalla, int a)
    {
        numSuerte = Random.Range(1, a);

        if (!batalla)
        {
            if ((current.suerte + current.suerteExtra1 + current.suerteExtra2) >= numSuerte)
            {
                return true;
            }
            else
                return false;
        }
        else
            if ((GetComponent<Enemies>().currentBattle.suerte) >= numSuerte)
        {
            return true;
        }
        else
            return false;
    }//Funcion que devuelve true o false despues de comparar la suerte del personaje con un numero aleatorio "a"
    public int TiradaDano(int FuerzaAtaque, bool bloqueo, int defensa)
    {
        int t;
        float mitad;
        float tercio;
            mitad = defensa / 2;
            tercio = defensa - (defensa / 3);
        int numDefensa = Random.Range((int)Math.Truncate(mitad), (int)Math.Truncate(tercio));
        if (bloqueo)
        {
            t = (FuerzaAtaque - (numDefensa + (numDefensa - (numDefensa / 3))));
        }
        else
            t = FuerzaAtaque - numDefensa;
        return t;
    }//Funcion que calcula el daño recibido por un ataque, a = fuerza del atacante, b= indica si habrá una reducción del daño por bloqueo o alguna otra circunstancia
    public void EscribirLinea(int idLvl, int numText, bool continua, bool botones, int numBotones, string txtBoton1, string txtBoton2, string txtBoton3)
    {
        //Vector2 position = panelIntScroll.transform.localPosition;
        //position.y = 0;
        panelIntScroll.transform.localPosition = new Vector2 (panelIntScroll.transform.localPosition.x, 0); //Forzamos que el texto vaya al principio
        foreach (History h in GetComponent<Config>().histories.history)
            if (h.id == idLvl)
            {
                switch (numText)
                {
                    case 1:
                        textoPrincipal.GetComponent<TextMeshProUGUI>().text = h.text;
                        break;
                    case 2:
                        if (continua)
                            textoPrincipal.GetComponent<TextMeshProUGUI>().text = h.text + h.text2;
                      else
                            textoPrincipal.GetComponent<TextMeshProUGUI>().text = h.text2;
                        break;
                    case 3:
                        if (continua)
                            textoPrincipal.GetComponent<TextMeshProUGUI>().text = h.text + h.text2 + h.text3;
                        else
                            textoPrincipal.GetComponent<TextMeshProUGUI>().text = h.text3;
                        break;
                }
                break;
            }
        if (botones)
            AddBotones(numBotones, txtBoton1, txtBoton2, txtBoton3);
        else
            AddBotones(0, "", "", "");
        RedimensionarScroll(0, numBotones);
    }//Escribimos en el cuadro de texto general el texto id idLvl este objeto tiene varios textos que se indica en numText si no Continua escribir ese texto, si continua escribira ese y los anteriores
    public void AddBotones(int numBotones, string txtBoton1, string txtBoton2, string txtBoton3)
    {
        switch (numBotones)
        {
            case 0:
                GetComponent<Principal>().botonOp1.SetActive(false);
                GetComponent<Principal>().botonOp2.SetActive(false);
                GetComponent<Principal>().botonOp3.SetActive(false);
                GetComponent<TextoNiveles>().GuardarMemory(0, false);
                break;
            case 1:
                GetComponent<Principal>().botonOp1.SetActive(true);//Habilitamos el botonOpcion1
                txtBotonOp1.text = txtBoton1;
                GetComponent<Principal>().botonOp2.SetActive(false);//Habilitamos el BotonOpcion3
                GetComponent<Principal>().botonOp3.SetActive(false);
                GetComponent<TextoNiveles>().GuardarMemory(0, true);
                break;
            case 2:
                if (txtBoton1 != "")
                {
                    GetComponent<Principal>().botonOp1.SetActive(true);//Habilitamos el botonOpcion1
                    txtBotonOp1.text = txtBoton1;
                }
                if (txtBoton2 != "")
                {
                    GetComponent<Principal>().botonOp2.SetActive(true);//Habilitamos el botonOpcion1
                    txtBotonOp2.text = txtBoton2;
                }
                GetComponent<Principal>().botonOp3.SetActive(false);
                GetComponent<TextoNiveles>().GuardarMemory(0, true);
                break;
            case 3:
                if (txtBoton1 != "")
                {
                    GetComponent<Principal>().botonOp1.SetActive(true);//Habilitamos el botonOpcion1
                    txtBotonOp1.text = txtBoton1;
                }
                if (txtBoton2 != "")
                {
                    GetComponent<Principal>().botonOp2.SetActive(true);//Habilitamos el botonOpcion1
                    txtBotonOp2.text = txtBoton2;
                }
                if (txtBoton3 != "")
                {
                    GetComponent<Principal>().botonOp3.SetActive(true);//Habilitamos el botonOpcion1
                    txtBotonOp3.text = txtBoton3;
                }
                GetComponent<TextoNiveles>().GuardarMemory(0, true);
                break;
            case 12:
                GetComponent<Principal>().botonOp1.SetActive(false);//Habilitamos el botonOpcion1
                GetComponent<Principal>().botonOp2.SetActive(true);//Habilitamos el BotonOpcion3
                txtBotonOp2.text = txtBoton2;
                GetComponent<Principal>().botonOp3.SetActive(false);
                GetComponent<TextoNiveles>().GuardarMemory(0, true);
                break;
        }
    }//Activa o Desactiva los botones de Accion, con el int le indicamos si activamos 2 o 3 botones, le pasamos 3 strings para el texto de cada boton, en caso de que alguno no se vaya a activar podemos pasarle el string que queramos(Al final pongo Bono=1 para que sea necesario pulsar una opcion para continuar)
    public void Cerrar()
    {
        Application.Quit();
    }//Función para cerrar aplicación
    public void CargarEscena()
    {
        BotonesPNJ();
        panelMenuPrincipal.transform.position = PosPanelPer;
        panelSaveLoad.transform.position = PosPanelPer;
        panelInicialCrear.SetActive(false);
        GameObject.Find("NombrePersonaje").GetComponent<TextMeshProUGUI>().text = current.name;
        nivel = current.currentLevel;
        GetComponent<TextoNiveles>().NextLVL(current.nivelAnterior);
        //RedimensionarScroll(0, 3);
        if (current.estAltTxt[0] != "" || current.estAltTxt[1] != "")
        {
            EfectoActivo.GetComponent<TextMeshProUGUI>().text = current.estAltTxt[0] + "\n " + current.estAltTxt[1];
        }
    }//Función Que actualiza la escena entera, la llamaremos si tras hacer algún cambio no se carga correctamente
    public bool BuscarPNJLista(string a)
    {
        foreach (PNJ pnj in current.pnj)
        {
            if (pnj.name == a)
                return true;
        }
        return false;
    }//Busca un PNJ en la List de PNJ conocidos
    public void BotonesPNJ()
    {
        int id = 0;
        TamañoScroll = 150;
        panelMochila.SetActive(true);
        GameObject boton = GameObject.Find("ButtonPNJOriginal");
        foreach (PNJ pnj in current.pnj)
        {
            Instantiate(BotonPNJ, new Vector2(0, 0), Quaternion.identity);
            GameObject.Find("ButtonPNJ(Clone)").tag = "BotonesMochila";
            GameObject.Find("ButtonPNJ(Clone)").name = "BotonPNJ" + pnj.id;
            GameObject.Find("TextoBoton").name = "TextoBoton" + pnj.id;
            GameObject.Find("TextoBoton" + pnj.id).GetComponent<TextMeshProUGUI>().text = pnj.name;
            GameObject.Find("BotonPNJ" + pnj.id).transform.SetParent(GameObject.Find("PanelIntMochila").transform);
            GameObject.Find("BotonPNJ" + pnj.id).transform.position = new Vector2(boton.GetComponent<RectTransform>().transform.position.x,
                                                                                  boton.GetComponent<RectTransform>().transform.position.y - 150);
            GameObject.Find("BotonPNJ" + pnj.id).GetComponent<RectTransform>().sizeDelta = boton.GetComponent<RectTransform>().sizeDelta;
            boton = GameObject.Find("BotonPNJ" + pnj.id);
            boton.GetComponent<BotonesDetalles>().numeroMemoria = pnj.id;
            id = pnj.id;
            TamañoScroll = TamañoScroll + 150;
            GameObject BTN = GameObject.Find("BotonPNJ" + pnj.id);
            BTN.GetComponent<Button>().onClick.AddListener(delegate { AbrirDetallesPNJ(BTN.GetComponent<BotonesDetalles>().numeroMemoria); });
        }
        RedimensionarScroll(0, 3);
    }//Crea los botones de PNJs en el Panel Mochila
    public void BotonesLugares()
    {
        if (current.sites.Count > 0)
        {
            GameObject boton = GameObject.Find("ButtonPNJOriginal");
            foreach (Lugares lug in current.sites)
            {
                Instantiate(BotonPNJ, new Vector2(0, 0), Quaternion.identity);
                GameObject.Find("ButtonPNJ(Clone)").tag = "BotonesMochila";
                foreach (Lugares lugb in GetComponent<Config>().lugares.lugares)
                {
                    if (lug.id == lugb.id)
                    {
                        GameObject.Find("ButtonPNJ(Clone)").name = "BotonLug" + lugb.id;
                        GameObject.Find("TextoBoton").name = "TextoBoton" + lugb.name;
                        GameObject.Find("TextoBoton" + lugb.name).GetComponent<TextMeshProUGUI>().text = lugb.name;
                        GameObject.Find("BotonLug" + lugb.id).transform.SetParent(GameObject.Find("PanelIntMochila").transform);
                        GameObject.Find("BotonLug" + lugb.id).transform.position = new Vector2(boton.GetComponent<RectTransform>().transform.position.x,
                                                                                            boton.GetComponent<RectTransform>().transform.position.y - 150);
                        GameObject.Find("BotonLug" + lugb.id).GetComponent<RectTransform>().sizeDelta = boton.GetComponent<RectTransform>().sizeDelta;
                        boton = GameObject.Find("BotonLug" + lugb.id);
                        boton.GetComponent<BotonesDetalles>().numeroMemoria = lugb.id;
                        TamañoScroll += 150;
                    }
                    
                }
                GameObject BTN = GameObject.Find("BotonLug" + lug.id);
                BTN.GetComponent<Button>().onClick.AddListener(delegate { AbrirDetallesLugar(BTN.GetComponent<BotonesDetalles>().numeroMemoria); });
            }
            RedimensionarScroll(0, 3);
        }
    }//Crea los botones de Lñugares en el Panel Mochila
    public void BotonPNJLugares()
    {
        if (Map.activeInHierarchy)
        {
            panelIntMochila.SetActive(true);
            Map.SetActive(false);
        }
        else
            TamañoScroll = 150;
            if (GameObject.Find("TextoPNJLugares").GetComponent<TextMeshProUGUI>().text == GetComponent<Config>().lineasBTN[14])
            {
                foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesMochila"))
                {
                    Destroy(o);
                }
                BotonesLugares();
                GameObject.Find("TextoPNJLugares").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().lineasBTN[15];
            }
            else
            {
                foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesMochila"))
                {
                    Destroy(o);
                }
                GameObject.Find("TextoPNJLugares").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().lineasBTN[14];
                BotonesPNJ();
            }
    }//Boton para cambiar entre Lugares y PNJs en el Panel Mochila
    public void GenerarBatalla(int IDEnemie)
    {
        if (current.tutoBatalla)
            GetComponent<TextoNiveles>().Tutoriales(5);
        GetComponent<Enemies>().PanelElecciones.SetActive(false);
        GameObject.Find("VidaBatalla").GetComponent<TextMeshProUGUI>().text = current.salud + current.saludExtra1 + current.saludExtra2 + " / " + current.statsTotal[0].ToString();
        GameObject.Find("FuerzaBatalla").GetComponent<TextMeshProUGUI>().text = current.statsTotal[1].ToString();
        GameObject.Find("DefensaBatalla").GetComponent<TextMeshProUGUI>().text = current.statsTotal[2].ToString();
        GameObject.Find("SuerteBatalla").GetComponent<TextMeshProUGUI>().text = current.statsTotal[3].ToString();
        GetComponent<Enemies>().CrearEnemigo(IDEnemie);
        GetComponent<Enemies>().ComprobarVida();
        GetComponent<Config>().botonesBatalla.SetActive(true);
        panelBatalla.transform.position = PosPanelPrinc;
        GameObject.Find("ImageEnemy").GetComponent<Image>().sprite = GetComponent<Config>().imgEnemigos[IDEnemie-1];
        GameObject.Find("TextNombreEnemigo").GetComponent<TextMeshProUGUI>().text = GetComponent<Enemies>().currentBattle.name;
        GameObject.Find("DescripciónEnemy").GetComponent<TextMeshProUGUI>().text = GetComponent<Enemies>().currentBattle.intro;
    }//Generamos batalla desde el nombre del adversario
    public void DestruirBatalla(int a)
    {
        panelBatalla.transform.position = PosPanelPer;
        if (a == 0)
            textoPrincipal.GetComponent<TextMeshProUGUI>().text = "Ganaste la batalla";
        else
            textoPrincipal.GetComponent<TextMeshProUGUI>().text = "Perdiste la batalla";
    } //terminamos batalla volviendo al panel principal
    public void SeleccionarAvatar(int a)// 0 es girar IZQ y 1 DCH
    {
        if (a == 0)
        {
            if (current.numAvatar > 0)
            {
                current.numAvatar--;
                GameObject.Find("ImageSelect").GetComponent<Image>().sprite = GetComponent<Config>().imgAvatares[current.numAvatar];
            }
            else
                return;
        }
        if (a == 1)
        {
            if (current.numAvatar != GetComponent<Config>().numTotAvatares - 1)
            {
                current.numAvatar++;
                GameObject.Find("ImageSelect").GetComponent<Image>().sprite = GetComponent<Config>().imgAvatares[current.numAvatar];
            }
            else
                return;
        }
    }
    public void Boosts(int nunIDBoost)
    {
        switch (nunIDBoost)
        {
            case 0:
                if (current.boosts[0] > 0 && (current.salud + current.saludExtra1 + current.saludExtra2) < current.statsTotal[0])
                {
                    current.salud = current.salud + 2 * (current.salud / 3);
                    if (current.salud > current.statsTotal[0])
                        current.salud = current.statsTotal[0];
                    current.boosts[0]--;
                }
                else if (current.boosts[0] < 1)
                    MensajeError(GetComponent<Config>().system[81], 2);
                else if ((current.salud + current.saludExtra1 + current.saludExtra2) >= current.statsTotal[0])
                    MensajeError(GetComponent<Config>().system[87], 2);
                else
                    MensajeError(GetComponent<Config>().system[86], 2);
                break;
            case 1:
                if (current.boosts[1] > 0 && EstadAlt(GetComponent<Config>().lineasBTN[7], "+ 3", 3, 3, false) != 3)
                {
                    MensajeError(GetComponent<Config>().lineasBTN[11] + "\n" + GetComponent<Config>().lineasBTN[7] + GetComponent<Config>().lineasBTN[7] + " +3" + "\n" + GetComponent<Config>().system[12] + "\n" + "3 " + GetComponent<Config>().system[13], 3);
                    current.boosts[1]--;
                }
                else if (current.boosts[1] == 0)
                    MensajeError(GetComponent<Config>().system[82], 2);
                else
                    MensajeError(GetComponent<Config>().system[86], 2);
                break;
            case 2:
                if (current.boosts[2] > 0 && EstadAlt(GetComponent<Config>().lineasBTN[8], "+ 3", 3, 3, false) != 3)
                {
                    MensajeError(GetComponent<Config>().lineasBTN[11] + "\n" + GetComponent<Config>().lineasBTN[8] + GetComponent<Config>().lineasBTN[8] + " +3" + "\n" + GetComponent<Config>().system[12] + "\n" + "3 " + GetComponent<Config>().system[13], 3);
                    current.boosts[2]--;
                }
                else if (current.boosts[2] == 0)
                    MensajeError(GetComponent<Config>().system[83], 2);
                else
                    MensajeError(GetComponent<Config>().system[86], 2);
                break;
            case 3:
                if (current.boosts[3] > 0 && EstadAlt(GetComponent<Config>().lineasBTN[9], "+ 3", 3, 3, false) != 3)
                {
                    MensajeError(GetComponent<Config>().lineasBTN[11] + "\n" + GetComponent<Config>().lineasBTN[9] + GetComponent<Config>().lineasBTN[8] + " +3" + "\n" + GetComponent<Config>().system[12] + "\n" + "3 " + GetComponent<Config>().system[13], 3);
                    current.boosts[3]--;
                }
                else if (current.boosts[3] == 0)
                    MensajeError(GetComponent<Config>().system[84], 2);
                else
                    MensajeError(GetComponent<Config>().system[86], 2);
                break;
            case 4:
                if (current.boosts[4] > 0 && EstadAlt(GetComponent<Config>().lineasBTN[10], "+ 3", 3, 3, false) != 3)
                {
                    MensajeError(GetComponent<Config>().lineasBTN[11] + "\n" + GetComponent<Config>().lineasBTN[10] + GetComponent<Config>().lineasBTN[8] + " +3" + "\n" + GetComponent<Config>().system[12] + "\n" + "3 " + GetComponent<Config>().system[13], 3);
                    current.boosts[4]--;
                }
                else if (current.boosts[4] == 0)
                    MensajeError(GetComponent<Config>().system[85], 2);
                else
                    MensajeError(GetComponent<Config>().system[86], 2);
                break;
        }
        AbrirPanelPersonaje();
    }
    public void CambiarStatPNJ(int id, string DatoaCambiar, int ValorDato, bool SumaResta)
    {
        foreach (PNJ person in current.pnj)
        {
            if (person.id == id)
            {
                switch (DatoaCambiar)
                {
                    case "id":
                        person.id = ValorDato;
                        break;
                    case "salud":
                        if (SumaResta)
                            person.salud += ValorDato;
                        else
                            person.salud = ValorDato;
                        if (person.salud>10)
                            person.salud = 10;
                        else if (person.salud < 0)
                            person.salud = 0;
                        break;
                    case "fuerza":
                        if (SumaResta)
                            person.fuerza += ValorDato;
                        else
                            person.fuerza = ValorDato;
                        if (person.fuerza < 0)
                            person.fuerza = 0;
                        break;
                    case "defensa":
                        if (SumaResta)
                            person.defensa += ValorDato;
                        else
                            person.defensa = ValorDato;
                        if (person.defensa < 0)
                            person.defensa = 0;
                        break;
                    case "suerte":
                        if (SumaResta)
                            person.suerte += ValorDato;
                        else
                            person.suerte = ValorDato;
                        if (person.suerte < 0)
                            person.suerte = 0;
                        break;
                    case "afinidad":
                        string msg = "";
                        if (SumaResta)
                        {
                            person.afinidad += ValorDato;
                            if (ValorDato > 0)
                                msg = GetComponent<Config>().system[74] + person.name + GetComponent<Config>().system[78] + ValorDato + GetComponent<Config>().system[79];
                            else
                                msg = GetComponent<Config>().system[74] + person.name + GetComponent<Config>().system[77] + ValorDato + GetComponent<Config>().system[79];
                        }
                        else
                            person.afinidad = ValorDato;
                        if (person.afinidad > 10)
                            person.afinidad = 10;
                        else if (person.afinidad < 0)
                            person.afinidad = 0;
                        if (person.afinidad < 3)
                            msg = GetComponent<Config>().system[74] + person.name + GetComponent<Config>().system[75];
                        else if (person.afinidad < 5)
                            msg = GetComponent<Config>().system[74] + person.name + GetComponent<Config>().system[76];
                        MensajeError(msg, 3);
                        break;
                }
                break; //Los ID son únicos por lo que una vez encuentra el PNJ con ese ID forzamos la salida para que no gaste recursos buscando más
            }
        }
    }
    public void EliminarPartida() {
        current = null;
        current = new CurrentCharacter();
        current.CrearPersonaje();
        current.currentLevel = -1;
        BotonMenu();
    }
}