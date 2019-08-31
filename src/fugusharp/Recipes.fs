namespace fugusharp.Controllers
open System

module Recipes =
    open fugusharp.Data.DatabaseHelper
    open Microsoft.AspNetCore.Http
    open Giraffe

    type Recipe = {
        Name:string
    }
    
    let getRecipesHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let client = CreateDbDriver
            use session = client.Session()
            let writeTxHandler = fun (tx: Neo4j.Driver.V1.ITransaction) ->
                tx.Run "MATCH (r: Recipe) RETURN r.name" 
                |> Seq.map (fun (rawResult) ->
                   {Name = rawResult.Item(0).ToString()})
        
            let recipes = session.WriteTransaction writeTxHandler

            json recipes next ctx
