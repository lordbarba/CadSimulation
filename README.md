# CadSimulation

Ogni Iterazione richiesta è nel branch corrispettivo, quindi per testare ogni singola iterazione bisogna eseguire il checkout del singolo branch

Il server JS è nella root della repo

Come da indicazioni, non sono stati fatti check di errori particolari.
Vista la natura dell'esercizio e dello scopo per cui doveva essere svolto, ho scelto di focalizzarmi soprattutto sull'Iterazione 4.

Considerazioni tecniche generali
La classe Helper, statica, è stata fatta per "semplicità" nella gestione dei tipi disponibili (con cache). L'alternativa era di creare un servizio anche per questo scopo.
La serializzazione JSon è stata fatta creando un CustomConverter, questo perchè il formato suggerito non poteva essere soddisfatto dalla serializzazione nativa. 

Iterazione 4:
Per la gestione della dependecy injection non ho volutamente usato alcun container, vista la semplicità del progetto: sarebbe stato comunque possibile usare la classe "ServiceCollection"
