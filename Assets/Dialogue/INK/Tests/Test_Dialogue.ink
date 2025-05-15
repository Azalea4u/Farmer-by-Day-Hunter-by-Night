EXTERNAL Open_Buy()
EXTERNAL Open_Sell()

-> main

=== main ===
Welcome in! What would you like to do? #speaker:Shopkeeper
    + [Buy]
        -> buy
    + [Sell]
        -> sell

=== buy ===
Here is what I currently have!
~ Open_Buy()
-> END

=== sell ===
What would you like to sell?
~ Open_Sell()
-> END

=== end ===
-> END 