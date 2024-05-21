VAR allCollectiblesGathered = false

Welcome explorer, you are the first soul to reach the top of my tower.

That's right, I am the owner of the Nirak Tower...

...or how I like to call it. The Tower of Death.

May I introduce myself, you are speaking to the one and only, Thanatos.

In your long way up to the tower you may have regenerated your soul right?

RIGHT??

And also you may have gathered all the 7 souls spheres am i wrong?
-> collectible_check

== collectible_check ==
{allCollectiblesGathered:
    * [Yes, there you have them] -> yes_option
    * [Yes, but I wanna end this journey] -> no_option
- else:
    * [Yes] You cannot lie to Nirak. -> no_option
    * [No] -> no_option
}

== yes_option ==
You may leave as new. -> DONE

== no_option ==
Well, I only have one thing to tell you......
this is now the end, you seem to have learned nothing from it.
And that betrays the souls of the tower, so for that you shall rest in peace.

-> END
