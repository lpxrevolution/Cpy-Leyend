using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using System.Collections;

[System.Serializable]
public class Enemies : MonoBehaviour
{
    public int turno, turnoTact, sucesionAtaques, elecEnemi, iDaño, TurnoLimTact;
    public bool CharEnem = false;
    public Image barraVidaEnemigo, barraTactica;
    public GameObject PanelElecciones;
    public CurrentBattle currentBattle;
    public TextMeshProUGUI registro = null;
    public string msg;
    bool seguir = true; // variable para habilitar funciones
        
    public void CrearEnemigo(int IDEnemy)
    {
        registro = GameObject.Find("DescripciónEnemy").GetComponent<TextMeshProUGUI>();
        currentBattle = new();
        foreach(Enemy enemy in GetComponent<Config>().enemies.enemy)
        {
            if (enemy.id == IDEnemy)
            {
                currentBattle.id = enemy.id;
                currentBattle.name = enemy.name;
                currentBattle.type = enemy.type;
                currentBattle.intro = enemy.intro;
                currentBattle.attacks = enemy.attacks;
                currentBattle.salud = enemy.salud;
                currentBattle.fuerza = enemy.fuerza;
                currentBattle.defensa = enemy.defensa;
                currentBattle.suerte = enemy.suerte;
                currentBattle.saludT = enemy.salud;
                currentBattle.attacks = enemy.attacks;
                currentBattle.sucesionAtaques = enemy.sucesionAtaques;
                currentBattle.turnoLimTact = 1; //almacenamos las elecciones del jugador
                currentBattle.elecJug = 0;
                currentBattle.elecEnemy = 0;    //almacenamos las elecciones del enemigo
                currentBattle.turno = 0;        //Número de turno
                currentBattle.turnoEnemie = 1;  //Turno de enemigo para poder hacer sucesion de ataques controlados
            }
        }
    }
    IEnumerator EsperarYContinuar(int time, string msg, bool actBotones)
    {

        seguir = false;
        yield return new WaitForSeconds(time); // Espera 1 segundo
        seguir = true;
        EscribirRegistro(msg);
        BotonContinuarBatalla();
        if (actBotones)
            PanelElecciones.SetActive(true);
    }
    public void BotonContinuarBatalla()
    {
        GetComponent<Config>().botonContinuarBatalla.SetActive(false);
        if (seguir) 
        {
            iDaño = 0;
            int numAtaque;
            if (!CharEnem)
            {
                switch (currentBattle.sucesionAtaques)
                {
                    case 11213:
                        switch (currentBattle.turnoEnemie)
                        {
                            case 1:
                            case 2:
                            case 4:
                                msg = "El enemigo utiliza " + currentBattle.attacks[0].attack + "\n" + registro.text;
                                EscribirRegistro(msg);
                                if (currentBattle.elecJug == 0)
                                {
                                    if (GetComponent<Principal>().TiradaSuerte(false, 10))
                                    {
                                        iDaño = GetComponent<Principal>().TiradaDano(currentBattle.fuerza, false, GetComponent<Principal>().current.defensa + GetComponent<Principal>().current.defensaExtra1 + GetComponent<Principal>().current.defensaExtra2);
                                        GetComponent<Principal>().current.salud = GetComponent<Principal>().current.salud - iDaño;
                                        msg = "Has recibido <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño" + "\n" + registro.text;
                                    }
                                    else
                                        msg = "El enemigo ha fallado el golpe" + "\n" + registro.text;
                                    StartCoroutine(EsperarYContinuar(1, msg, true));
                                }
                                else 
                                {
                                    if (GetComponent<Principal>().TiradaSuerte(false, 10))
                                    {
                                        iDaño = GetComponent<Principal>().TiradaDano(currentBattle.fuerza, true, GetComponent<Principal>().current.defensa);
                                        GetComponent<Principal>().current.salud = GetComponent<Principal>().current.salud - iDaño;
                                        msg = "Has recibido <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño" + "\n" + registro.text;
                                    }
                                    else
                                        msg = "El enemigo ha fallado el golpe" + "\n" + registro.text;
                                    StartCoroutine(EsperarYContinuar(1, msg, true));
                                }
                                break;
                            case 3:
                                msg = "El enemigo utiliza " + currentBattle.attacks[1].attack + "\n" + registro.text;
                                StartCoroutine(EsperarYContinuar(0, msg, true));
                                elecEnemi = 1;
                                break;
                            case 5:
                                msg = "El enemigo utiliza " + currentBattle.attacks[2].attack + "\n" + registro.text;
                                EscribirRegistro(msg);
                                if (currentBattle.elecJug == 1)//si el jugador elige "defender" sumamos un 33% mas a su defensa
                                {
                                    if (GetComponent<Principal>().TiradaSuerte(false, 10))
                                    {
                                        iDaño = GetComponent<Principal>().TiradaDano((currentBattle.fuerza + (currentBattle.fuerza / 2)), true, (GetComponent<Principal>().current.defensa + (GetComponent<Principal>().current.defensa / 2)));
                                        GetComponent<Principal>().current.salud = GetComponent<Principal>().current.salud - iDaño;
                                        msg = "Has recibido <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño" + "\n" + registro.text;
                                    }
                                    else
                                        msg = "El enemigo ha fallado el golpe" + "\n" + registro.text;
                                    StartCoroutine(EsperarYContinuar(1, msg, true));
                                }
                                else //si no se defiende, solo afecta la defensa básica
                                {
                                    if (GetComponent<Principal>().TiradaSuerte(false, 10))
                                    {
                                        iDaño = GetComponent<Principal>().TiradaDano((currentBattle.fuerza + (currentBattle.fuerza / 2)), false, GetComponent<Principal>().current.defensa);
                                        GetComponent<Principal>().current.salud = GetComponent<Principal>().current.salud - iDaño;
                                        msg = "Has recibido <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño" + "\n" + registro.text;
                                    }
                                    else
                                        msg = "El enemigo ha fallado el golpe" + "\n" + registro.text;
                                    StartCoroutine(EsperarYContinuar(1, msg, true));
                                }
                                break;
                        }
                        CharEnem = true;
                        currentBattle.turnoEnemie++;
                        if(currentBattle.turnoEnemie == 6)
                            currentBattle.turnoEnemie = 1;
                        GameObject.Find("VidaBatalla").GetComponent<TextMeshProUGUI>().text = GetComponent<Principal>().current.salud.ToString() + " / " + GetComponent<Principal>().current.statsTotal[0].ToString();
                        break;
                    case 99:
                        numAtaque = Random.Range(0, 100);
                        if (numAtaque < 60)
                            StartCoroutine(EsperarYContinuar(1, currentBattle.attacks[0].attack + "\n" + registro.text, false));
                        if ((numAtaque > 60) && (numAtaque < 90))
                            StartCoroutine(EsperarYContinuar(1, currentBattle.attacks[1].attack + "\n" + registro.text, false));
                        if (numAtaque > 90)
                            StartCoroutine(EsperarYContinuar(1, currentBattle.attacks[2].attack + "\n" + registro.text, false));
                        CharEnem = true;
                        currentBattle.turnoEnemie++;
                        currentBattle.elecJug = 0;
                        break;
                }
                turno++;
                ComprobarVida();
            }
        }
    }
    public void BotonAtacar()
    {
        GetComponent<Config>().botonesBatalla.SetActive(false);
        if (CharEnem)
        {
            PanelElecciones.SetActive(false);
            currentBattle.elecJug = 0;
            registro.text = "Realizas un ataque normal" + "\n" + registro.text;
            CharEnem = false;
            if (GetComponent<Principal>().TiradaSuerte(true, 10))
            {
                if (elecEnemi == 1)
                    iDaño = (GetComponent<Principal>().TiradaDano(GetComponent<Principal>().current.fuerza, true, currentBattle.defensa) / 3);
                else
                    iDaño = GetComponent<Principal>().TiradaDano(GetComponent<Principal>().current.fuerza, false, currentBattle.defensa);
                currentBattle.salud -= iDaño;
                msg = "Causas <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño al enemigo" + "\n" + registro.text;
            }
            else
                msg = "Has fallado el golpe" + "\n" + registro.text;
            StartCoroutine(EsperarYContinuar(1, msg, false));
            ComprobarVida();
            elecEnemi = 0;
        }
    }
    public void BotonDefender()
    {
        GetComponent<Config>().botonesBatalla.SetActive(false);
        if (CharEnem)
        {
            PanelElecciones.SetActive(false);
            currentBattle.elecJug = 1;
            msg = "Intentas defender sus ataques" + "\n" + registro.text;
            StartCoroutine(EsperarYContinuar(1, msg, false));
            CharEnem = false;
            ComprobarVida();
            elecEnemi = 0;
        }
    }
    public void EscribirRegistro(string tMsg)
    {
        registro.text = tMsg;
    }
    public void BotonTactica()
    {
        if (CharEnem)
        {
            if (TurnoLimTact <= turno)
            {
                GetComponent<Config>().botonesBatalla.SetActive(false);
                PanelElecciones.SetActive(false);
                turnoTact = turno;
                TurnoLimTact = turno + 5;
                currentBattle.elecJug = 2;
                registro.text = "Realizas una Táctica especial" + "\n" + registro.text;
                CharEnem = false;
                if (GetComponent<Principal>().TiradaSuerte(true, 13))
                {
                    if (elecEnemi == 1)
                        iDaño = GetComponent<Principal>().TiradaDano((GetComponent<Principal>().current.fuerza + (GetComponent<Principal>().current.fuerza / 2)), true, currentBattle.defensa);
                    else
                        iDaño = GetComponent<Principal>().TiradaDano((GetComponent<Principal>().current.fuerza + (GetComponent<Principal>().current.fuerza / 2)), false, currentBattle.defensa);
                    currentBattle.salud -= iDaño;
                    msg = "Causas <color=#AA0000><b>" + iDaño + "</b></color> puntos de daño al enemigo" + "\n" + registro.text;
                }
                else
                    msg = "Has fallado el golpe" + "\n" + registro.text;

                StartCoroutine(EsperarYContinuar(1, msg, false));
                ComprobarVida();
                elecEnemi = 0;
            }
            else
                GetComponent<Principal>().MensajeError(GetComponent<Config>().system[27], 3);
        }
    }
    public void ComprobarVida()
    {
        GameObject.Find("PanelIntScrollDet").GetComponent<RectTransform>().sizeDelta = new Vector2(0, GameObject.Find("DescripciónEnemy").GetComponent<RectTransform>().rect.height - 150);
        TextMeshProUGUI registro = GameObject.Find("DescripciónEnemy").GetComponent<TextMeshProUGUI>();
        float tmp = (float)currentBattle.salud / (float)currentBattle.saludT;
        barraVidaEnemigo.fillAmount = tmp;
        Color lerpColor = Color.Lerp(Color.red, Color.green, tmp);
        barraVidaEnemigo.color = lerpColor;
        barraTactica.fillAmount = ((float)turno - turnoTact) / (TurnoLimTact - (float)turnoTact);
        if (currentBattle.salud < 1)
        {
            registro.text = "Has Derrotado al enemigo";
            currentBattle = null;
            GetComponent<Principal>().DestruirBatalla(0);
            GetComponent<Principal>().current.enemigosDerrotados++;
            GetComponent<Config>().botonContinuarBatalla.SetActive(true);
            GetComponent<Config>().botonesBatalla.SetActive(true);
            CharEnem = true;
            msg = "";
            Debug.Log("ha muerto");
        }
        if (GetComponent<Principal>().current.salud < 1)
        {
            registro.text = currentBattle.name + " te ha derrotado.";
            currentBattle = null;
            GetComponent<Principal>().DestruirBatalla(1);
            GetComponent<Principal>().current.enemigosDerrotados++;
            GetComponent<Config>().botonContinuarBatalla.SetActive(true);
            GetComponent<Config>().botonesBatalla.SetActive(true);
            CharEnem = true;
            msg = "";
        }
        else
            StartCoroutine(EsperarYContinuar(1, msg, false));
    }
}
[System.Serializable]
public class CurrentBattle : Enemy
{
    public int saludT;
    public int turnoLimTact = 1;
    public int elecJug = 0;
    public int elecEnemy = 0;
    public int turno = 0;
    public int turnoEnemie = 1;
}
[System.Serializable]
public class Attack
{
    public int id;
    public string attack;
}
[System.Serializable]
public class Enemy
{
    public int id, type, fuerza, defensa, suerte, salud, sucesionAtaques;
    public string name, intro;
    public List<Attack> attacks;
}
[System.Serializable]
public class Enemiess
{   
    public List<Enemy> enemy;
}