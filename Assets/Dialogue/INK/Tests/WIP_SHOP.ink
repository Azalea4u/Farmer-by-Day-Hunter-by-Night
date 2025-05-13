EXTERNAL ShowBuyMenu()
EXTERNAL CloseBuyMenu()

Wow, it seems we meet yet again! #speaker:Elenore #layout:left #audio:Elenore

- It seems so. #speaker:Joanne #layout:right #audio:Player
-> main

=== main ===
~ CloseBuyMenu()
What would you like to do? #speaker: Elenore #layout:left #audio:Elenore
    + [Buy]
        -> buy_UI
    + [Bye]
        -> end
        
=== buy_UI ===
~ ShowBuyMenu()
Here is what I currently have in stock!
    + [Go Back]
        -> main
    + [Bye]
        -> end   

=== end ===
~ CloseBuyMenu()
Hopefully, this won't be the last time I see you alive!

- I agree. See you later. #speaker:Joanne #layout:right #audio:Player
-> final_end

=== final_end ===
-> END