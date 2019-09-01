namespace fugusharp.Data
open Neo4j.Driver.V1

module DatabaseHelper =

    type IDatabaseHelper =
        abstract CreateDbDriver: unit -> IDriver

    type Neo4jDatabaseHelper =
        interface IDatabaseHelper with
            member this.CreateDbDriver() =
                let auth = AuthTokens.Basic("fugu", "fugu123")
                GraphDatabase.Driver("bolt://100.25.48.12:37018", auth)
