using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Config : MonoBehaviour
{
    public Vector2 TamaPanel;
    public GameObject[] textos, textosPeq, textosGrandes;
    public GameObject botonContinuarBatalla, botonesBatalla;
    public string[] lineasBTN, system; //Matriz para almacenar las lineas de los .txt
    public TextAsset txtArmament, txtMemory, txtBotones, txtLugares, txtEnemies, txtSystem, txtPNJs, txtHistory; //Asset donde guardaremos el .txt
    public List<Sprite> images, imgAvatares, imgPNJ, imgEnemigos, imgLugares;
    public string idioma;
    public int numTotAvatares, numTotAvataresPNJ, numTotAvataresEnemigos, numTotImagenLugares;
    public bool tuto = true;
    public UserConfig userConfig;
    public Enemiess enemies;
    public Lugaress lugares;
    public PNJs pnjs;
    public Histories histories;
    public Memories memorys;
    public Armaments armaments;

    void Awake()
    {
        Application.targetFrameRate = 60;
        botonContinuarBatalla = GameObject.Find("BotonContinuar");
        botonesBatalla = GameObject.Find("PanelElecciones");
        userConfig = new UserConfig();
        userConfig.ConfInic();
        GetComponent<SaveLoadSys>().LoadConf();
        numTotImagenLugares = 5;
        numTotAvatares = 9;
        numTotAvataresPNJ = 4;
        numTotAvataresEnemigos = 3;
        imgPNJ = new List<Sprite> {};
        imgAvatares = new List<Sprite> {};
        imgEnemigos = new List<Sprite> {};
        images = new List<Sprite> {null, null};
        imgLugares = new List<Sprite> {};
        txtBotones = (TextAsset)Resources.Load("Textos/ButtonsES", typeof(TextAsset)); //Cargamos el .txt en la variable TextAsset
        txtSystem = (TextAsset)Resources.Load("Textos/SystemES", typeof(TextAsset));
        txtLugares = (TextAsset)Resources.Load("Textos/SitesES", typeof(TextAsset));
        txtPNJs = (TextAsset)Resources.Load("Textos/PNJES", typeof(TextAsset));
        txtEnemies = (TextAsset)Resources.Load("Textos/EnemiesES", typeof(TextAsset));
        txtArmament = (TextAsset)Resources.Load("Textos/ArmamentES", typeof(TextAsset));
        txtHistory = (TextAsset)Resources.Load("Historia/historyES", typeof(TextAsset));
        enemies = JsonUtility.FromJson<Enemiess>(txtEnemies.text);
        lugares = JsonUtility.FromJson<Lugaress>(txtLugares.text);
        armaments = JsonUtility.FromJson<Armaments>(txtArmament.text);
        histories = JsonUtility.FromJson<Histories>(txtHistory.text);
        pnjs = JsonUtility.FromJson<PNJs>(txtPNJs.text);
        lineasBTN = txtBotones.text.Split("\n"[0]); //almacenamos las lineas en el Array el ("\n"[0]) hace que pase a la siguiente linea una vez se haya acabado, sino, escribiria todas las lineas en una sola
        system = txtSystem.text.Split("\n"[0]);
        textosGrandes = GameObject.FindGameObjectsWithTag("TextosGrandes");
        textos = GameObject.FindGameObjectsWithTag("Textos");
        textosPeq = GameObject.FindGameObjectsWithTag("TextosPeq");
        TamaPanel = new Vector2(GameObject.FindWithTag("panelprincipal").GetComponent<RectTransform>().sizeDelta.x, //
                                      GameObject.FindWithTag("panelprincipal").GetComponent<RectTransform>().sizeDelta.y);
        txtMemory = (TextAsset)Resources.Load("Textos/Memory", typeof(TextAsset));
        memorys = JsonUtility.FromJson<Memories>(txtMemory.text);
        AñadirImagenes();
        CambiarIdioma(true);
        RedimensionarTextos(userConfig.tamañotamaTxt);
        CerrarPaneles();
    }
    public void CambiarTamaTxt(float tamaño)
    {
        userConfig.tamañotamaTxt = tamaño;
        RedimensionarTextos(tamaño);
    }
    public void RedimensionarTextos(float tamaño)
    {
        if (TamaPanel[0] < 1080) //Resolucion inferior a 1080
        {

            for (int i = 0; i < textosGrandes.Length; i++)
                textosGrandes[i].GetComponent<TextMeshProUGUI>().fontSize = 55 * tamaño;
            for (int i = 0; i < textos.Length; i++)
                textos[i].GetComponent<TextMeshProUGUI>().fontSize = 36 * tamaño;
            for (int i = 0; i < textosPeq.Length; i++)
                textosPeq[i].GetComponent<TextMeshProUGUI>().fontSize = 28;
            GameObject.Find("TextoTituloMenu").GetComponent<TextMeshProUGUI>().fontSize = 50;
            GameObject.Find("TextoTituloMenu2").GetComponent<TextMeshProUGUI>().fontSize = 35;
            GameObject.Find("TextoTitulo").GetComponent<TextMeshProUGUI>().fontSize = 40;
            GameObject.Find("TextoTitulo2").GetComponent<TextMeshProUGUI>().fontSize = 30;
            GameObject.Find("TextoMain").GetComponent<TextMeshProUGUI>().fontSize = 60 * tamaño;
        }
        if (TamaPanel[0] >= 1080) //Resolucion superior a 1080
        {
            for (int i = 0; i < textosGrandes.Length; i++)
                textosGrandes[i].GetComponent<TextMeshProUGUI>().fontSize = 65 * tamaño;
            for (int i = 0; i < textos.Length; i++)
                textos[i].GetComponent<TextMeshProUGUI>().fontSize = 55 * tamaño;
            for (int i = 0; i < textosPeq.Length; i++)
                textosPeq[i].GetComponent<TextMeshProUGUI>().fontSize = 40 * tamaño;
            GameObject.Find("TextoTituloMenu").GetComponent<TextMeshProUGUI>().fontSize = 50;
            GameObject.Find("TextoTituloMenu2").GetComponent<TextMeshProUGUI>().fontSize = 35;
            GameObject.Find("TextoTitulo").GetComponent<TextMeshProUGUI>().fontSize = 40;
            GameObject.Find("TextoTitulo2").GetComponent<TextMeshProUGUI>().fontSize = 30;
            GameObject.Find("TextoMain").GetComponent<TextMeshProUGUI>().fontSize = 70 * tamaño;
        }
        //GetComponent<Principal>().RedimensionarScroll(0,3);
    }
    public void BotonResetStats()
    {
        GetComponent<Principal>().current.CrearPersonaje();
        GetComponent<Principal>().CalculaStatsJugador();
    }
    public void CambiarIdioma(bool carga)
    {
        if (carga)
        {
            switch (userConfig.Idioma)
            {
                case "EN":
                    idioma = "EN";
                    userConfig.Idioma = "EN";
                    
                    break;
                case "ES":
                    idioma = "ES";
                    userConfig.Idioma = "ES";
                    break;
            }
        }
        else
        {
            switch (idioma)
            {
                case "ES":
                    idioma = "EN";
                    userConfig.Idioma = "EN";
                    break;
                case "EN":
                    idioma = "ES";
                    userConfig.Idioma = "ES";
                    break;
            }
        }
        txtBotones = (TextAsset)Resources.Load("Textos/Buttons" + idioma, typeof(TextAsset)); //Cargamos el .txt en la variable TextAsset
        lineasBTN = txtBotones.text.Split("\n"[0]); //almacenamos las lineas en el Array el ("\n"[0]) hace que pase a la siguiente linea una vez se haya acabado, sino, escribiria todas las lineas en una sola
        txtSystem = (TextAsset)Resources.Load("Textos/System" + idioma, typeof(TextAsset));
        system = txtSystem.text.Split("\n"[0]);
        images[0] = (Sprite)Resources.Load("Images/" + idioma, typeof(Sprite));
        txtLugares = (TextAsset)Resources.Load("Textos/Sites" + idioma, typeof(TextAsset));
        txtPNJs = (TextAsset)Resources.Load("Textos/PNJ" + idioma, typeof(TextAsset));
        txtEnemies = (TextAsset)Resources.Load("Textos/Enemies" + idioma, typeof(TextAsset));
        txtArmament = (TextAsset)Resources.Load("Textos/Armament" + idioma, typeof(TextAsset));
        txtHistory = (TextAsset)Resources.Load("Historia/history" + idioma, typeof(TextAsset));
        histories = JsonUtility.FromJson<Histories>(txtHistory.text);
        enemies = JsonUtility.FromJson<Enemiess>(txtEnemies.text);
        lugares = JsonUtility.FromJson<Lugaress>(txtLugares.text);
        pnjs = JsonUtility.FromJson<PNJs>(txtPNJs.text);
        armaments = JsonUtility.FromJson<Armaments>(txtArmament.text);
        CambiarTextosIdioma();
    }
    private void AñadirImagenes()
    {
        for (int i = 0; i < numTotAvatares; i++)
                imgAvatares.Add((Sprite)Resources.Load("Images/Avatars/Portrait" + i, typeof(Sprite)));
        for (int i = 0; i < numTotAvataresPNJ; i++)
                imgPNJ.Add((Sprite)Resources.Load("Images/AvataresPNJ/Avat" + i, typeof(Sprite)));
        for (int i = 0; i < numTotAvataresEnemigos; i++)
                imgEnemigos.Add((Sprite)Resources.Load("Images/EnemiesAvatars/Enemie" + i, typeof(Sprite)));
        for (int i = 0; i < numTotImagenLugares; i++)
                imgLugares.Add((Sprite)Resources.Load("Images/Lugares/Luga" + i, typeof(Sprite)));
    }
    public void CambiarTextosIdioma()
    {
        GetComponent<Principal>().panelAviso.SetActive(true);
        GetComponent<Principal>().panelInfo.SetActive(true);
        GetComponent<Principal>().panelTutorial.SetActive(true);
        GetComponent<Principal>().panelBatalla.SetActive(true);
        GetComponent<Principal>().panelDetalles.SetActive(true);
        GetComponent<Principal>().panelInicialCrear.SetActive(true);
        GetComponent<Principal>().PanelLugares.SetActive(true);
        GetComponent<Principal>().panelMochila.SetActive(true);
        GetComponent<Principal>().panelMap.SetActive(true);
        GetComponent<Principal>().panelSaveLoad.SetActive(true);
        GetComponent<Principal>().panelTutorial.SetActive(true);
        if (GetComponent<Principal>().panelInicialCrear.activeSelf)
        {
            GameObject.Find("TextoInicNombre").GetComponent<TextMeshProUGUI>().text = system[4];
            GameObject.Find("TextoInicImage").GetComponent<TextMeshProUGUI>().text = system[28];
            GameObject.Find("TextoInicStats").GetComponent<TextMeshProUGUI>().text = system[5];
            GameObject.Find("StatSalud3").GetComponent<TextMeshProUGUI>().text = lineasBTN[6];
            GameObject.Find("StatFuerza3").GetComponent<TextMeshProUGUI>().text = lineasBTN[7];
            GameObject.Find("StatDefensa3").GetComponent<TextMeshProUGUI>().text = lineasBTN[8];
            GameObject.Find("StatSuerte3").GetComponent<TextMeshProUGUI>().text = lineasBTN[9];
            GameObject.Find("StatElocuencia3").GetComponent<TextMeshProUGUI>().text = lineasBTN[10];
            GameObject.Find("PntsRestantes").GetComponent<TextMeshProUGUI>().text = system[25];
        }
        GameObject[] Txtvolver = GameObject.FindGameObjectsWithTag("TextoVolver");
        GameObject.Find("ButtonPrue").GetComponent<Image>().sprite = images[0];
        GameObject.Find("TextoSiguiente").GetComponent<TextMeshProUGUI>().text = lineasBTN[0];
        for (int i = 0; i < Txtvolver.Length; i++)
            Txtvolver[i].GetComponent<TextMeshProUGUI>().text = lineasBTN[1];
        GameObject.Find("TextNueva").GetComponent<TextMeshProUGUI>().text = lineasBTN[2];
        GameObject.Find("TextCargar").GetComponent<TextMeshProUGUI>().text = lineasBTN[3];
        GameObject.Find("TextConfig").GetComponent<TextMeshProUGUI>().text = lineasBTN[27];
        GameObject.Find("TextContinuar").GetComponent<TextMeshProUGUI>().text = lineasBTN[4];
        GameObject.Find("TxtContinuarTut").GetComponent<TextMeshProUGUI>().text = lineasBTN[4];
        GameObject.Find("TextSalir").GetComponent<TextMeshProUGUI>().text = lineasBTN[5];
        GameObject.Find("StatSalud").GetComponent<TextMeshProUGUI>().text = lineasBTN[6];
        GameObject.Find("StatSalud2").GetComponent<TextMeshProUGUI>().text = lineasBTN[6];
        GameObject.Find("StatFuerza").GetComponent<TextMeshProUGUI>().text = lineasBTN[7];
        GameObject.Find("StatFuerza2").GetComponent<TextMeshProUGUI>().text = lineasBTN[7];
        GameObject.Find("StatDefensa").GetComponent<TextMeshProUGUI>().text = lineasBTN[8];
        GameObject.Find("StatDefensa2").GetComponent<TextMeshProUGUI>().text = lineasBTN[8];
        GameObject.Find("StatSuerte").GetComponent<TextMeshProUGUI>().text = lineasBTN[9];
        GameObject.Find("StatSuerte2").GetComponent<TextMeshProUGUI>().text = lineasBTN[9];
        GameObject.Find("StatElocuencia").GetComponent<TextMeshProUGUI>().text = lineasBTN[10];
        GameObject.Find("EfectoActivo").GetComponent<TextMeshProUGUI>().text = lineasBTN[11];
        GameObject.Find("Ubicacion").GetComponent<TextMeshProUGUI>().text = lineasBTN[12];
        GameObject.Find("TiempoJuego").GetComponent<TextMeshProUGUI>().text = system[37];
        GameObject.Find("TextoPNJLugares").GetComponent<TextMeshProUGUI>().text = lineasBTN[14];
        GameObject.Find("BotonAtacarText").GetComponent<TextMeshProUGUI>().text = lineasBTN[24];
        GameObject.Find("BotonDefenderText").GetComponent<TextMeshProUGUI>().text = lineasBTN[25];
        GameObject.Find("BotonTacticaText").GetComponent<TextMeshProUGUI>().text = lineasBTN[26];
        GameObject.Find("BotonContinuarText").GetComponent<TextMeshProUGUI>().text = lineasBTN[4]; 
        GameObject.Find("TituloBatalla").GetComponent<TextMeshProUGUI>().text = system[22];
        GameObject.Find("IdiomaTxt").GetComponent<TextMeshProUGUI>().text = system[31];
        GameObject.Find("TBotonTamaTxtPeq").GetComponent<TextMeshProUGUI>().text = system[32];
        GameObject.Find("TBotonTamaTxtMed").GetComponent<TextMeshProUGUI>().text = system[33];
        GameObject.Find("TBotonTamaTxtGran").GetComponent<TextMeshProUGUI>().text = system[34];
        GameObject.Find("TextoInventario").GetComponent<TextMeshProUGUI>().text = system[35];
        GameObject.Find("TamaConfTxt").GetComponent<TextMeshProUGUI>().text = system[36];
        GameObject.Find("TutoConfTxt").GetComponent<TextMeshProUGUI>().text = system[42];
        GameObject.Find("BotonTutTxt").GetComponent<TextMeshProUGUI>().text = lineasBTN[28];
        GameObject.Find("TextoAceptarInfo").GetComponent<TextMeshProUGUI>().text = system[57];
        GameObject.Find("TextoAceptarAviso").GetComponent<TextMeshProUGUI>().text = system[58];
        GameObject.Find("TextoCancelarAviso").GetComponent<TextMeshProUGUI>().text = system[59];
        foreach (Lugares luga in GetComponent<Principal>().current.sites)
        {
            foreach (Lugares lugb in lugares.lugares)
            {
                if (luga.id == lugb.id)
                {
                    luga.name = lugb.name;
                    luga.description = lugb.description;
                }
            }
        }
        foreach (PNJ pnja in GetComponent<Principal>().current.pnj)
        {
            foreach (PNJ pnjb in pnjs.pnjs)
            {
                if (pnja.id == pnjb.id)
                {
                    pnja.name = pnjb.name;
                    pnja.description = pnjb.description;
                }
            }
        }
        foreach (Armament ar in GetComponent<Principal>().current.armament)
            foreach (Armament arm in armaments.armament)
                if (ar.type == arm.type && ar.id == arm.id)
                {
                    ar.name = arm.name;
                    ar.description = arm.description;
                }
        CerrarPaneles();
        GetComponent<Principal>().panelConfig.SetActive(true);
    }
    public void TutsSINO()
    {

        if (userConfig.Tuts == true)
        {
            userConfig.Tuts = false;
            GameObject.Find("BotonTutTxt").GetComponent<TextMeshProUGUI>().text = lineasBTN[29];
        }
        else 
        {
            userConfig.Tuts = true;
            GameObject.Find("BotonTutTxt").GetComponent<TextMeshProUGUI>().text = lineasBTN[28];
        }
    }
    public void CerrarPaneles()//Aquí deshabilitaremos todos los paneles menos los iniciales para mejorar el rendimiento general
    {
        GetComponent<Principal>().panelAviso.SetActive(false);
        GetComponent<Principal>().panelInfo.SetActive(false);
        GetComponent<Principal>().panelTutorial.SetActive(false);
        GetComponent<Principal>().panelBatalla.SetActive(false);
        GetComponent<Principal>().panelConfig.SetActive(false);
        GetComponent<Principal>().panelDetalles.SetActive(false);
        GetComponent<Principal>().panelInicialCrear.SetActive(false);
        GetComponent<Principal>().PanelLugares.SetActive(false);
        GetComponent<Principal>().panelMochila.SetActive(false);
        GetComponent<Principal>().panelMap.SetActive(false);
        GetComponent<Principal>().panelSaveLoad.SetActive(false);
        GetComponent<Principal>().panelTutorial.SetActive(false);
    }
}