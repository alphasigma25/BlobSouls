# Idée de simulation
Un système avec des petits êtres formant différents clans. Chaque jour, ils vont chercher des ressources et le rapporter à leur base.
Lorsqu’ils se promènent, les petits êtres vont croiser d’autres petits êtres, et ils ont différentes capacités d’interactions : ils peuvent ne rien faire, attaquer l’autre petit être, ou le soigner s’il est blessé.
Lorsqu’une confrontation a lieu, celui qui attaque a 50% de survivre, 50% de chances de perdre des points de vie. Celui qui se fait attaquer a 50% de chances de mourir, et 50% de chance de perdre des points de vie.
Les petits êtres sont capables de détecter si l’être qu’il croise est de son équipe ou non.
Les petits êtres peuvent avoir un trait de personnalité : à quel point ils aident leur groupe ou non. 
Les petits êtres ont également une âme qui définit son comportement lorsqu’il croise un autre petit être. Il définit les chances d’attaquer un petit être de son équipe (AA), un petit être de l’équipe adverse (AE), les chances de soigner un petit être de son équipe (HA), et les chances de soigner un petit être des équipes adverses (HE).
Un petit être à l’âme neutre a les scores suivants : 
AA : 0% | AE : x + a | HA : x | HE : 0%
Un petit être à l’âme + a les scores suivants :
AA : 0% | AE : x + a | HA : 50% + x | HE : 50%
Un petit être à l’âme - a les scores suivants :
AA : 50% | AE : 50% + x + a| HA : x | HE : 0%
Un petit être à l’âme ++ a les scores suivants :
AA : 0% | AE : 0% + a | HA : 100% | HE : 100%
Un petit être à l’âme -- a les scores suivants :
AA : 100% | AE : 100% + a | HA : 0% | HE : 0%
Le x varie en fonction du taux d’implication du blob pour son camps, le a en fonction du taux d’agressivité.



