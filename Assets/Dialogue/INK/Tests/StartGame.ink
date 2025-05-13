EXTERNAL QuitGame()
EXTERNAL StartMenu()
-> main

=== main ===
Are you ready to start?
    + [Go Back]
        -> go_back
    + [I change my mind]
        -> final_end
    + [Quit Game]
        -> quit_game

=== go_back ===
~StartMenu()
-> final_end

=== quit_game ===
~QuitGame()
-> final_end

=== final_end ===
-> END