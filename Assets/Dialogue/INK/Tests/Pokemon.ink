-> main

=== main ===
Why, hello!

Nice to meet you.

What <b><color=\#DA5656>pokemon</color></b> would you like?
    + [Charmander]
        -> chosen("Charmander")
    + [Bulbasaur]
        -> chosen("Bulbasaur")
    + [Squirtle]
        -> chosen("Squirtle")
        
=== chosen(pokemon) ===
You chose {pokemon}!
-> END