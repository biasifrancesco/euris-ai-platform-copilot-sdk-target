# Coffee Break – repository dimostrativo

Repository bersaglio del POC di sviluppo avviato dall'agente Euris. La solution `CoffeeBreak.slnx` non contiene ancora progetti: il codice dell'API lo scrive un worker con Copilot SDK in un branch dedicato e lo propone con una pull request draft.

## Struttura

- `CoffeeBreak.slnx` – unica solution.
- `src/` – progetti dell'applicazione, uno per cartella.
- `tests/` – progetti di test.
- `Directory.Build.props` – .NET 10, nullable, analyzer raccomandati, warning trattati come errori.
- `Directory.Packages.props` – versioni dei pacchetti NuGet in un solo punto (Central Package Management).
- `global.json` – versione dell'SDK .NET.

La procedura che il worker segue è la Skill in `.github/skills/coffee-break-delivery/`.
