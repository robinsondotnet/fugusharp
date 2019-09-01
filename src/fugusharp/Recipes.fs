namespace fugusharp.Controllers

module Recipes =
    open fugusharp.Data.DatabaseHelper
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe

    type Recipe = {
        Name:string
    }
    
    let getRecipesHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let client = ctx.GetService<IDatabaseHelper>().CreateDbDriver() 
            use session = client.Session()
            let writeTxHandler = fun (tx: Neo4j.Driver.V1.ITransaction) ->
                tx.Run "MATCH (r: Recipe) RETURN r.name, id(r)" 
                |> Seq.map (fun (rawResult) ->
                   {Name = rawResult.Item(0).ToString()})
        
            let recipes = session.WriteTransaction writeTxHandler

            json recipes next ctx
    
    let postRecipesHandler: HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! request = ctx.BindJsonAsync<Recipe>()
                let client = ctx.GetService<IDatabaseHelper>().CreateDbDriver() 
                use session = client.Session()
                let writeTxHandler = fun (tx: Neo4j.Driver.V1.ITransaction) ->
                    let result = tx.Run("CREATE (r: Recipe {name: $Name}) RETURN r", { Name = request.Name})
                    result |>
                    Seq.exactlyOne
                
                let createdRecipe = session.WriteTransaction writeTxHandler

                return! json createdRecipe next ctx
            }
    
    let putRecipesHandler(id: int): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! request = ctx.BindJsonAsync<Recipe>()
                let client = ctx.GetService<IDatabaseHelper>().CreateDbDriver() 
                use session = client.Session()
                let writeTxHandler = fun (tx: Neo4j.Driver.V1.ITransaction) ->
                    let result = tx.Run("CREATE (r: Recipe {name: $Name}) RETURN r", { Name = request.Name})
                    result |>
                    Seq.exactlyOne
                
                let createdRecipe = session.WriteTransaction writeTxHandler

                return! json createdRecipe next ctx
            }
    
    let deleteRecipesHandler(id: int): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! request = ctx.BindJsonAsync<Recipe>()
                let client = ctx.GetService<IDatabaseHelper>().CreateDbDriver() 
                use session = client.Session()
                let writeTxHandler = fun (tx: Neo4j.Driver.V1.ITransaction) ->
                    let result = tx.Run("CREATE (r: Recipe {name: $Name}) RETURN r", { Name = request.Name})
                    result |>
                    Seq.exactlyOne
                
                let createdRecipe = session.WriteTransaction writeTxHandler

                return! json createdRecipe next ctx
            }     
