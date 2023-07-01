using System.IO;
class DataBase{
    public static char emDash = '—';
    string dbPath;
    List<string> tablePaths;
    public DataBase(string path){
        dbPath = path;
        tablePaths = new();
        GetTables();
    }
    void GetTables(){
        tablePaths.Clear();
        string[] tables = Directory.GetFiles(dbPath);
        foreach(string table in tables){
            FileInfo fi = new FileInfo(table);
            tablePaths.Add(fi.Name);
        }
    }
    public string Select(int tableIndex, int tableSearchIndex, string query){
        StreamReader reader = new(
            Path.Combine(dbPath,tablePaths[tableIndex])
        );
        string? d;
        string response = "";
        while((d = reader.ReadLine()) != null){
            string[] ds = d.Split(emDash);
            if(ds.Length <= tableSearchIndex)
                continue;
            if(ds[tableSearchIndex].Contains(query)){
                response += d + emDash;
            }
        }
        reader.Close();
        return response;
    }
    public void Insert(int tableIndex, string insertstring){
        try{
        StreamWriter writer = new(
            Path.Combine(dbPath,tablePaths[tableIndex]),
            true
        );
        writer.WriteLine(insertstring);
        writer.Close();
        }catch(Exception ex){Console.Write(ex.Message);}
    }
    public string GetDbTables(){
        string toRet = "";
        foreach(string path in tablePaths){
            FileInfo fi = new FileInfo(Path.Combine(dbPath,path));
            toRet += fi.Name + "-";
        }
        return toRet;
    }
    public void CreateTable(string name){
        File.Create(Path.Combine(dbPath,name));
        GetTables();
    }
}
