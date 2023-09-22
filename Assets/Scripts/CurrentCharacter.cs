using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentCharacter
{
    public List<PNJ> pnj = new(); //List donde añadiremos cada PNJ conocido (quizas solo los relevantes)
    public List<Lugares> sites = new();//List donde añadiremos cada Lugar visitador o conocido
    public List<Armament> armament = new(); //Contenedor de Armamento (sin imagenes porque no se pueden almacenar en un archivo de save)
    public Memories memorys;    //Contenedor de Decisiones
    public string name, lugarActual, verGame;
    public int currentLevel, nivelAnterior, enemigosDerrotados,                     //================================================================
        salud, saludExtra1, saludExtra2, suludExtraEquipment,                       //
        fuerza, fuerzaExtra1, fuerzaExtra2, fuerzaExtraEquipment,                   //
        defensa, defensaExtra1, defensaExtra2, defensaExtraEquipment,               //Stats originales al crear le personaje
        suerte, suerteExtra1, suerteExtra2, suerteExtraEquipment,                   //
        elocuencia, elocuenciaExtra1, elocuenciaExtra2, elocuenciaExtraEquipment,   //
        numAvatar, alineamiento, bono;                                              //================================================================
    public int[] statsTotal, //// Array que almacena los Stats totales, si alguno se reduce en algún momento podemos venir aquí a consultar que dato tenía [Sulud, Fuerza, Resistencia, Elocuencia, Suerte]
         limiteTurno, boosts; //Array(Int) que están almacenados los turnos en los que terminarán los estados alterados activos
    public int turnosTotal; //Numero de turnos pasados desde el inicio del juego, cada vez que se pulsa a Siguiente pasa un turno
    public string[] estAlt; //Declaramos un Array de Strings con 2 valores como máximo para los estados alterados, bueno o malos.
    public string[] estAltTxt; //Array(string) de los estados alterados
    public bool[] elecciones;
    public float[] time;//Tiempo 0:0:0
    public bool canSave, tutoMochila, tutoPersonaje, tutoBatalla;

    public void CrearPersonaje()
    {
        name = "";
        elecciones = new bool[5];
        currentLevel = 0;
        boosts = new int[5] { 0, 0, 0, 0, 0 };
        canSave = true;
        numAvatar = 0;
        time = new[] { 0f, 0f, 0f};
        estAltTxt = new string[] { "", "" };
        estAlt = new string[] { "", "" };
        statsTotal = new int[5] { 1, 0, 0, 0, 0 };
        limiteTurno = new int[2] { 0, 0 };
        salud = 50;
        fuerza = 7;
        defensa = 5;
        suerte = 5;
        elocuencia = 5;
        bono = 7;
        tutoMochila = true;
        tutoPersonaje = true;
        tutoBatalla = true;
        verGame = "";
    }
}
