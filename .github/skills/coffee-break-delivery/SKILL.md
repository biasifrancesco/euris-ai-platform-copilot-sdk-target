---
name: coffee-break-delivery
description: Procedura per consegnare una modifica in questo repository. Usala sempre quando sviluppi codice qui, prima di scrivere file e prima di preparare la descrizione della pull request.
---

# Consegna di una modifica nel repository Coffee Break

1. Rispetta la struttura del repository:
   - ogni progetto applicativo in `src/<NomeProgetto>/`, ogni progetto di test in `tests/<NomeProgetto>/`;
   - ogni progetto aggiunto alla solution `CoffeeBreak.slnx` della root, in una cartella `/src/` o `/tests/`; non creare altre solution;
   - `TargetFramework`, nullable e analyzer arrivano da `Directory.Build.props`: non ripeterli nei `.csproj`;
   - le versioni dei pacchetti NuGet vanno solo in `Directory.Packages.props`; nei `.csproj` `PackageReference` senza `Version`. Niente dipendenze esterne se il framework basta.
2. Lo stile di `.editorconfig` è verificato in build e i warning sono errori: per esempio tipi espliciti al posto di `var` quando il tipo non è evidente.
3. Scrivi o aggiorna il `README.md` del progetto **in italiano**, con:
   - a cosa serve, in due righe;
   - come avviarlo (`dotnet run --project ...`) e su quale porta;
   - gli endpoint con un esempio di richiesta `curl` e la risposta attesa.
4. Aggiungi un file `<NomeProgetto>.http` accanto al progetto con una richiesta di esempio per ogni endpoint.
5. Prima di finire esegui `dotnet build CoffeeBreak.slnx` e correggi ogni errore.
6. La descrizione della pull request, in italiano, ha esattamente queste sezioni:
   - `## Cosa è stato realizzato`
   - `## Come provarla` (comandi per avviare l'app e richieste di esempio)
   - `## Note` (limiti noti, per esempio dati solo in memoria)
