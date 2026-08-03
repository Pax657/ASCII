[System.Serializable]
public class opcionesChar
{
    public string displayName; //lo que ve el usuario en el dropdown
    public string densityTable; //la tabla real, de denso a claro


    public opcionesChar(string displayName, string densityTable)
    {
        this.displayName = displayName;
        this.densityTable = densityTable;
    }
}