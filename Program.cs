using System;
using System.Net;
class Program{
    public static void Main(string[] args){
        new Server();
        while(true);
    }
}

class Server{
    public DataBase db = new("db/");
    string address = "http://localhost:9999/";
    HttpListener server;
    public Server(){
        server = new();
        server.Prefixes.Add(address);
        server.Start();
        Listener();
    }
    async void Listener(){
        while(true){
            var context = await server.GetContextAsync();
            #pragma warning disable
            Task.Run(()=> {ContextWork(context);});
            #pragma warning restore
        }
    }
    void ContextWork(HttpListenerContext context){
        string response = "0";
        StreamReader reader = new(context.Request.InputStream);
        string[] query = reader.ReadToEnd().Split(DataBase.emDash);
        if(query[0] == "s"){
            response = db.Select(int.Parse(query[1]),int.Parse(query[2]),query[3]);
        }else if(query[0] == "i"){
            string q = "";
            for(int i = 2;i < query.Length;i++){
                q += query[i];
                if(i +1 < query.Length){
                    q += DataBase.emDash;
                }
            }
            db.Insert(int.Parse(query[1]),q);
        }else if(query[0] == "g"){
            response = db.GetDbTables();
        }else if(query[0] == "c"){
            db.CreateTable(query[1]);
        }
        byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        context.Response.OutputStream.Write(responseBytes, 0,responseBytes.Length);
        context.Response.OutputStream.Flush();
        context.Response.Close();
    }
}