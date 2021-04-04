#  Mancala Defence - Game Mechanism

## Overview

*Mancala Defence* is a **tower defence game** combining mechanisms of **mancala** and **card drawing**.  The game goal, like many other tower defence games, is to prevent *enemies* from entering *base*. To achieve this, the player should dynamically placing *units* on the *map*, and the *units* will automatically attacking the *enemies* until they are killed. Once placed, the *units* cannot be easily moved to the other *cells* except through the *Mancala* operation. One of the economy system is built upon card drawing.

The Game Mechanism needs to be designed to encourage player to play *Mancala*, to use *cards* instead of *coins* (another resource). And it should also force players to balance the advantage and disadvantage placing *units* on *Vulnerable Cells*.

## The Map and Cells

### The Map

The *map* consists an *enemy spawning point* (*ESP*), a *base*, a *path* where enemies will follow towards the *base*, and may *cells* where friendly *units* can be placed on.

![the *map*, only an example](./DocFig/map.jpg)

### The Favourite Cells and Vulnerable Cells

The *cells* are divided into two categories: *Favourite Cells* (shown as Fav. Cell above) and *Vulnerable Cells* (shown as Vul. Cell above). *Base* is a special *favourite cell*, while *enemy spawning point* is **not** a *cell*, as player cannot place *units* on it.

Units on *vul. cells* will decrease attack damage, but units on *fav. cells* stay unaffected.

### Overloading and Exhaustion

*Cell* will be *overloaded* when too many *units* are placed on it. After some time (determined by how many *units* are placed on it) it will become *exhausted*. All the *units* on it are all disabled (will no attack) unless they are moved to other *cells* (might by *Mancala* move). The *cell* will stay *exhausted* unless *unit* on it is cleared out. *Base* can **neither** be *overloaded* **nor** be *exhausted*.

### Special Rules for Spawning Units on Cells

- *Units* can not be directly spawned on a *Vul. cell* unless it is *activated*, that is, any *unit* has been move onto it beforehand (through *Mancala*).

- *Units* can **never** be spawned on the *base*.
- *Units* can **never** be spawned on the *cell* that has number of unit reached the limit.

(In code the *base* is represented by `BaseCell`)

## Mancala

### General Rule for Mancala

*Mancala*, which name comes form the tabletop game *Mancala*, is a move allowing player to redistribute *units*. Player should specify a *cell* to perform *Mancala* first. All the *units* on it will then distributed into the succeeding *cells*, one *unit* per *cell*.

![*Mancala* Example](./DocFig/Mancala.jpg)

The sequence of *cells* is: The *fav. cell* closest to *ESP* ->the adjacent *fav. cell* ->... -> the *fav. cell* closest to *Base* -> the *vul. cell* closest to *Base* -> the adjacent *vul. cell* -> ... -> the *vul. cell* closest to *ESP* -> the *fav. cell* closest to *ESP*. The following figure is a clearer description. (Notice that the loop **does not** pass *ESP*)

![*Mancala* Sequence](./DocFig/MancalaSeq.jpg)

### Special Rules for Mancala

1. When the last *unit* is placed on the *base*, player can preform another *Mancala* **for free**.
2. When the last *unit* is placed on a empty *cell*, player can instantly send all the *units* at the opposite *cell*.

### Chain Effect of Mancala

*Mancala* is a way to resolve *exhaustion*. But due to its redistribution mechanism, it may trigger chain effect. The player should think twice before they move.

![*Mancala* Chain Effect](./DocFig/MancalaChain.jpg)

## Units

### Unit Attributes and the Building Activity

*Units* has three attribute values: life, damage, and *skill*. *Skill* is the amount that a *unit* can contribute to *building activity* at a time.

When placed on the *base*, *units* will contribute to the *building activity* **instead of **attacking the *enemies*. The rate of *building activity* (the sum of *skill* for all *units* on the *base*) is related the frequency of card drawing.

*Units* has 3 types: White, Green and blue. It has a base attribute values shown as following:

| type  | life | damage | skill |
| ----- | ---- | ------ | ----- |
| White | +    | -      | ++    |
| Green | ++   | +      | -     |
| Red   | -    | ++     | +     |

(-: disadvantage, +: advantage, ++: great advantage)

### Unit AI

*Unit* will automatically attack *enemy* entered its range.

## Enemies

### Enemy Types

There are two types of *enemy*: *Common enemy* and *elite enemy*. *C. enemy* attacks **one** *unit* at a time, while *e. enemy* gives **all** *units* on a *cell* certain damage at a time.

### Waves

Enemies are organised in *waves*.

(To be written)

### Enemy AI

(Proposal) *Enemy* do not attack at all, *unit* gets damage when it attacks an *enemy*.

## Cards and Coins

### Cards

*Card* is an important resource. Player can draw cards periodically if there are any *units* on *base*. The frequency is depends on the sum of *skill* for all *units* on the *base*.

The *card* has types corresponding to *units*. Player can use *card* in following ways:

- By discarding 2 cards of the same type, player can spawn a corresponding *unit* on a *cell* (subject to spawning restrictions).
- By discarding 3 cards of the same type, player can update the corresponding *unit factory*, and then player can spawn more powerful *units*.
- By discarding 3 cards of the same type, player can perform *Mancala*.

There exists a *hand limit*. Once the limit is reached, the game will discard the earliest drawn card automatically.

#### Coins

*Coins* are acquired by killing *enemies*. *Coin* can be consumed in following ways:

- Upgrade **one** attribute of a *unit*.
- Extend *hand limit*.

## Game Setup

### Unit setup

(To be written)

### Card Setup

(To be written)

## List of Terms

| Term                            | Meaning                                                      |
| ------------------------------- | ------------------------------------------------------------ |
| *activation*                    | (see above)                                                  |
| *base*                          | A special *favourite cell* that player should prevent *enemies* from. |
| *building activity*             | (see above)                                                  |
| *card*                          | A major resource that player can use to spawn *units*, etc.. |
| *cell*                          | Where player can place units.                                |
| *coin*                          | A minor resource that player can use to upgrade *units*, etc.. |
| *enemy spawning point* (*ESP*)  | Place where *enemies* are spawned.                           |
| *enemy*                         | Entity that keeps approaching the *base*. Player will lose when one gets into the *base*. |
| *exhaustion*                    | A state of *cell*. All the *units* on it are all disabled on an *exhausted cell*. |
| *favourite cell* (*fav. cell*)  | *Cell* where *units* stay unaffected when placing on it.     |
| *hand limit*                    | How many *cards* player can hold.                            |
| *Mancala*                       | A special move redistributing *units*.                       |
| *map*                           | Collection of *enemy spawning point*, *base*, *path* and *cells*. |
| *overloading*                   | A state of *cell*, entered when too many *units* are placed on it. It may become *exhausted*  afterwards. |
| *skill*                         | The amount that a *unit* can contribute to *building activity* at a time. |
| *unit factory*                  | (see above)                                                  |
| *unit*                          | Object that will automatically damaging the *enemies* nearby once placed. |
| *vulnerable cell* (*vul. cell*) | *Cell* where *units* are debuffed when placing on it.        |

