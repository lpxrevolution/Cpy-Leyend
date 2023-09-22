using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[Serializable]
public class SaveLoadSys : MonoBehaviour
{
    public GameObject BotonLoad, BotonsSavePlus, panelSaveLoad;
    public List<CurrentCharacter> savedGames = new ();
    public UserConfig userConf;
    public void LeerArchivo()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames0.rgd"))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames0.rgd", FileMode.Open);
            savedGames = (List<CurrentCharacter>)bf.Deserialize(file);
            file.Close();
        }
    }
    public void GuardarArchivo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames0.rgd");
        bf.Serialize(file, savedGames);
        file.Close();
    }
    public void LoadConf()
    {
        if (File.Exists(Application.persistentDataPath + "/confi.rgd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/confi.rgd", FileMode.Open);
            if (file.Length != 0)
            userConf = (UserConfig)bf.Deserialize(file);
            GetComponent<Config>().userConfig = userConf;
            file.Close();
        }
    }
    public void SaveConf()
    {
        userConf = GetComponent<Config>().userConfig;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/confi.rgd");
        bf.Serialize(file, userConf);
        file.Close();
    }
    public void Continuar()
    {
        if (GetComponent<Principal>().current.currentLevel == 0)
        {
            LeerArchivo();
            if (savedGames.Count > 0)
            {
                GetComponent<Principal>().current = savedGames[0];
                GetComponent<Principal>().panelSaveLoad.transform.position = GetComponent<Principal>().PanelPrincipal.transform.position;
                GetComponent<Principal>().CargarEscena();
                GetComponent<Principal>().panelMenuPrincipal.SetActive(false);
            }
            else
                GetComponent<Principal>().MensajeError(GetComponent<Config>().system[40], 3);
                savedGames.Clear();
        }
        else if (GetComponent<Principal>().current.currentLevel == -1) 
        {
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[40], 3);
            savedGames.Clear();
        }
        else
        {
            GetComponent<Principal>().panelMenuPrincipal.SetActive(false);
            GetComponent<Principal>().panelInicialCrear.SetActive(false);
            GetComponent<Principal>().panelMenuPrincipal.transform.position = GetComponent<Principal>().panelPersonaje.transform.position;
            savedGames.Clear();
        }
    }
    public void BotonesSaveLoad(bool save)
    {
        savedGames.Clear();
        GetComponent<Principal>().panelSaveLoad.SetActive(true);
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesSaveLoad"))
            Destroy(o);//Destruimos todos los botones que existan antes de crear los nuevos
        LeerArchivo();
        GameObject BotonSave = GameObject.Find("ButtonSavePadre");
        int tamaño = 450;
        if ((savedGames.Count == 0 || savedGames.Count == 1) && save)
            BotonSavePlus();//si al el archivo de guardado está vacío o es 1 (guardado rápido) y vamos a guardar, crea el boton para crear nueva partida guarada
        else if ((savedGames.Count < 2) && !save)//si el archivo de guardado está vacío y vamos cargar la partida, nos saltará un mensaje
        {
            GetComponent<Principal>().panelSaveLoad.SetActive(false);
            GetComponent<Principal>().PanelSaveLoadInt.SetActive(false);
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[40], 3);//Si intentamos cargar y no hay saves
        }
        else
        {
            GetComponent<Principal>().panelSaveLoad.transform.position = GetComponent<Principal>().PanelPrincipal.transform.position;
            for (int i = 1; i < savedGames.Count; i++)
            {
                int numemoria = i;
                Instantiate(BotonLoad, GameObject.Find("ButtonSavePadre").transform.position, Quaternion.identity);
                GameObject.Find("ButtonSave(Clone)").transform.SetParent(GameObject.Find("PanelBotonesSaveLoad").transform);
                GameObject.Find("ButtonSave(Clone)").name = "ButtonSave" + i;
                GameObject.Find("BotonTrash").name = "BotonTrash" + i;
                GameObject.Find("ButtonSave" + i).tag = "BotonesSaveLoad";
                GameObject.Find("BotonTrash" + i).tag = "BotonesSaveLoad";
                GameObject.Find("ButtonSave" + i).GetComponent<RectTransform>().sizeDelta = BotonSave.GetComponent<RectTransform>().sizeDelta;
                GameObject.Find("ButtonSave" + i).GetComponent<RectTransform>().transform.position = new Vector2(BotonSave.GetComponent<RectTransform>().transform.position.x,
                                                                                                                 BotonSave.GetComponent<RectTransform>().transform.position.y - tamaño);
                GameObject.Find("ImageBotonSave").name += i;
                GameObject.Find("ImageBotonSave" + i).GetComponent<Image>().sprite = GetComponent<Config>().imgAvatares[savedGames[i].numAvatar];
                GetComponent<Principal>().current.verGame = "v." + Application.version;
                GameObject.Find("NombreSave").GetComponent<TextMeshProUGUI>().text = savedGames[i].name;
                GameObject.Find("VersionSave").GetComponent<TextMeshProUGUI>().text = savedGames[i].verGame;
                GameObject.Find("VersionSave").name += i;
                GameObject.Find("NombreSave").name += i;
                GameObject.Find("UbicacionSave").GetComponent<TextMeshProUGUI>().text = savedGames[i].lugarActual;
                GameObject.Find("UbicacionSave").name += i;
                GameObject.Find("TiempoSave").GetComponent<TextMeshProUGUI>().text = savedGames[i].time[2].ToString() + ":" + savedGames[i].time[1].ToString() + ":" + (Math.Truncate(savedGames[i].time[0]));
                GameObject.Find("TiempoSave").name += i;
                BotonSave = GameObject.Find("ButtonSave" + i);
                tamaño = 250;
                GameObject.Find("ButtonSave" + i).GetComponent<BotonesDetalles>().numeroMemoria = i;
                if (save && i == savedGames.Count - 1 && savedGames.Count < 6)
                {
                    Instantiate(BotonsSavePlus, GameObject.Find("ButtonSave" + i).transform.position, Quaternion.identity);
                    GameObject.Find("ButtonSavePlus(Clone)").transform.SetParent(panelSaveLoad.transform);
                    GameObject.Find("ButtonSavePlus(Clone)").GetComponent<RectTransform>().sizeDelta = BotonSave.GetComponent<RectTransform>().sizeDelta;
                    GameObject.Find("ButtonSavePlus(Clone)").GetComponent<RectTransform>().transform.position = new Vector2(BotonSave.GetComponent<RectTransform>().transform.position.x,
                                                                                                                 BotonSave.GetComponent<RectTransform>().transform.position.y - tamaño);
                    GameObject.Find("NombreSavePlus").GetComponent<TextMeshProUGUI>().text = "New Save";
                    GameObject.Find("ButtonSavePlus(Clone)").GetComponent<Button>().onClick.AddListener(delegate { Save(0, true); });
                }//Si vamos a guardar creo el Boton para Save nuevo
            }//creamos los botones, los instanciamos, los cambiamos de numbre, añadimos los datos y los colocamos correctamente
            if (save)
            {
                GameObject.Find("PanelIntScrollSaveLoad").GetComponent<RectTransform>().sizeDelta = new Vector2(0, (((BotonSave.GetComponent<RectTransform>().sizeDelta.y + 50) * savedGames.Count - 1)
                                                                                                                   - GameObject.Find("PanelScrollSL").GetComponent<RectTransform>().rect.height + 50)
                                                                                                                   + BotonSave.GetComponent<RectTransform>().sizeDelta.y + 50);
            }//Si vamos a guardar, hacemos un poco mas grande para que quepa el nuevo save
            else            //sino pues hacemos el tamaño para los botones
                GameObject.Find("PanelIntScrollSaveLoad").GetComponent<RectTransform>().sizeDelta = new Vector2(0, ((BotonSave.GetComponent<RectTransform>().sizeDelta.y + 50) * (savedGames.Count - 1))
                                                                                                                   - GameObject.Find("PanelScrollSL").GetComponent<RectTransform>().rect.height + 50);
            if (save)
            {
                for (int i = 1; i < savedGames.Count; i++)
                {
                    GameObject BTN = GameObject.Find("ButtonSave" + i);
                    GameObject BTNTrsh = GameObject.Find("BotonTrash" + i);
                    BTNTrsh.GetComponent<BotonesDetalles>().numeroMemoria = BTN.GetComponent<BotonesDetalles>().numeroMemoria;
                    BTN.GetComponent<Button>().onClick.AddListener(delegate { Save(BTN.GetComponent<BotonesDetalles>().numeroMemoria, false); });
                    BTNTrsh.GetComponent<Button>().onClick.AddListener(delegate { BorrarPartida(BTNTrsh.GetComponent<BotonesDetalles>().numeroMemoria, save); });
                }
            }  //si los botones son para guardar los mandamos a guardar a la funcion para ello
            else
            {
                for (int i = 1; i < savedGames.Count; i++)
                {
                    GameObject BTN = GameObject.Find("ButtonSave" + i);
                    GameObject BTNTrsh = GameObject.Find("BotonTrash" + i);
                    BTN.GetComponent<Button>().onClick.AddListener(delegate { Load(BTN.GetComponent<BotonesDetalles>().numeroMemoria); });
                    BTNTrsh.GetComponent<Button>().onClick.AddListener(delegate { BorrarPartida(BTN.GetComponent<BotonesDetalles>().numeroMemoria, save); });
                }
            }           //si son para cargar pues lo mismo
        }//Genera los botones del del panel
    }
    public void BotonSavePlus()
    {
        GameObject BotonSave = GameObject.Find("ButtonSavePadre");
        int tamaño = 450;
        GetComponent<Principal>().panelSaveLoad.transform.position = GetComponent<Principal>().PanelPrincipal.transform.position;
        Instantiate(BotonsSavePlus, GameObject.Find("ButtonSavePadre").transform.position, Quaternion.identity);
        GameObject.Find("ButtonSavePlus(Clone)").name = "ButtonSavePlus";
        GameObject.Find("ButtonSavePlus").transform.SetParent(GameObject.Find("PanelBotonesSaveLoad").transform);
        GameObject.Find("ButtonSavePlus").tag = "BotonesSaveLoad";
        GameObject.Find("NombreSavePlus").GetComponent<TextMeshProUGUI>().text = GetComponent<Config>().system[41];
        GameObject.Find("ButtonSavePlus").transform.SetParent(GameObject.Find("PanelBotonesSaveLoad").transform);
        GameObject.Find("ButtonSavePlus").GetComponent<RectTransform>().sizeDelta = BotonSave.GetComponent<RectTransform>().sizeDelta;
        GameObject.Find("ButtonSavePlus").GetComponent<RectTransform>().transform.position = new Vector2(BotonSave.GetComponent<RectTransform>().transform.position.x,
                                                                                                     BotonSave.GetComponent<RectTransform>().transform.position.y - tamaño);
        GameObject.Find("ButtonSavePlus").GetComponent<Button>().onClick.AddListener(delegate { Save(0, true); });
    }//Creación del botón Save para crear un punto de guardado nuevo
    public void Save (int numPartida, bool SaveNuevo)
    {
        if (GetComponent<Principal>().current.canSave)
        {
            if (SaveNuevo)
            {
                if (savedGames.Count == 0)
                    savedGames.Add(GetComponent<Principal>().current);
                savedGames.Add(GetComponent<Principal>().current);
                GetComponent<Principal>().BotonVolverConf();
            }
            else
            {
                savedGames[numPartida] = (GetComponent<Principal>().current);
                savedGames[0] = (GetComponent<Principal>().current);
                GetComponent<Principal>().BotonVolverConf();
            }
            GuardarArchivo();
        }
        else
            GetComponent<Principal>().MensajeError(GetComponent<Config>().system[30],3);
        savedGames.Clear();
        GetComponent<Config>().CerrarPaneles();

    }//Guarda la partida en un nuevo Slot si "SaveNuevom, sino la gurda en el slot numPartida
    public void Load(int numPartida)//Carga la partida del slot numPartida
    {
        float tiempoTot;
        LeerArchivo();
        GetComponent<TextoNiveles>().CargarImagenPrincipal(false, 0);
        GetComponent<Principal>().current = savedGames[numPartida];
        tiempoTot = savedGames[numPartida].time[2] * 3600 + savedGames[numPartida].time[1] * 60 + savedGames[numPartida].time[0];
        GetComponent<Principal>().tiempoTranscurrido = tiempoTot;
        GetComponent<Principal>().CargarEscena();
        savedGames.Clear();
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("BotonesSaveLoad"))
            Destroy(o);//Destruimos todos los botones que existan antes de crear los nuevos
        GetComponent<Config>().CerrarPaneles();
        GetComponent<Principal>().panelMenuPrincipal.SetActive(false);

    }
    public void BorrarPartida(int numPartida, bool siSave)
    {
        if (savedGames.Count < 3)
            savedGames.Clear();
        else
            savedGames.RemoveAt(numPartida);
        GuardarArchivo();
        GetComponent<Principal>().CerrarPanel();
        if (GetComponent<Principal>().current.currentLevel == 0)
            GetComponent<Principal>().BotonMenu();
        else
            GetComponent<Principal>().panelMenuPrincipal.SetActive(false);
    }
}