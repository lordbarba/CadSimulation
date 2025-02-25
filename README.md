# CadSimulation

## Istruzioni
Ogni Iterazione richiesta è nel branch corrispettivo, quindi per testare ogni singola iterazione bisogna eseguire il checkout del singolo branch
Il server JS è nella root della repo

## Considerazioni tecniche generali
- Vista la natura dell'esercizio e dello scopo per cui doveva essere svolto, ho scelto di focalizzarmi soprattutto sull'Iterazione 4.
- Per la gestione dei parametri della riga di comando, ho utilizzato un pacchetto Nuget.
- Come da indicazioni, non sono stati fatti check di errori particolari.
- La classe Helper, statica, è stata fatta per "semplicità" nella gestione dei tipi disponibili (con cache). L'alternativa era di creare un servizio anche per questo scopo.
- La serializzazione JSon è stata fatta creando un CustomConverter, questo perchè il formato suggerito non poteva essere soddisfatto dalla serializzazione nativa. 
- Nelle primi 3 iterazioni, per la gestione delle diverse istanze di Shape non è stata volutamente utilizzata alcuna reflection, ma dei semplici IF per differenziari i casi, volendo gestire la logica completa direttamente all'ultima iterazione

## Iterazione 4:
- Per la gestione della dependecy injection non ho volutamente usato alcun container, vista la semplicità del progetto: sarebbe stato comunque possibile usare la classe "ServiceCollection" (o altro pacchetto Nuget)
- La struttura della soluzione è basata anche attraverso l'uso di folder di soluzione. Ogni progetto ha il prefisso CardSimulation seguito dalla sua specificità: questo è uno standard che uso da diversi anni
- In generale sono solito utilizzare il minuscolo per il nome delle cartelle (e del relativo namespace creato), per evitare conflitti con il nome delle classi
- Le cartelle sono divise per "scope", cosi da rendere facile per gli sviluppatori sapere dove sono i progetti
- Le interfacce sono per: tipo di formato di registrazione (IRepositoryDataFormat), tipo di store (IRepositoryStore), UI (IUserInterface) e Shape(IShape)
- Le implementazioni delle interfacce IRepositoryDataFormat e IRepositoryStore sono ricavate dai parametri passati dalla riga di comando
- L'implementazione dell'interfaccia IUserinterface è una sola (basata su Console)
- La business è gestita dalla classe Manager
- Tutta l'applicazione fa uso di Reflection per poter esseere il più modulare e aperta possibile. In questo modo, solo il Program ha delle esplicite implementazioni 
- Ho sviluppato 2 unit test per poter testare velocemente la parte di DataFormat, dato che era quella più delicata, in quanto le shape vengono create dinamicamente.
- Il format Custom proposto non è proprio l'ideale in quanto l'elenco delle proprietà senza alcuna indicazione puà creare confusione ed errore
