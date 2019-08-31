namespace fugusharp.Data
open Neo4j.Driver.V1

module DatabaseHelper =

    let CreateDbDriver = 
        let auth = AuthTokens.Basic("fugu", "fugu123")
        GraphDatabase.Driver("bolt://100.25.48.12:37018", auth)
