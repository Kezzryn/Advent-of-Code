# 2023 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Trebuchet?!
- **Problem:** String parsing. 
- **Solution:** Part one had simple `IndexOf()` calls. Part two complicated things, requiring some clever replacing.

### [Day 2](Day%2002) - Cube Conundrum
- **Problem:** Finding the maximum number within lists.
- **Solution:** An unholy mix of `Regex` and `Select().Split()` combined with a bit of LINQ and that's done.

### [Day 3](Day%2003) - Gear Ratios
- **Problem:** Finding neighboring numbers in a string
- **Solution:** Straight forward logic. Explored some additional applications of `Select().Where()` when parsing down the data for each symbol.

### [Day 4](Day%2004) - Scratch cards 
- **Problem:**  Scratch and win!
- **Solution:** `Intersect()` for part one, and a double `for` loop for part two.

### [Day 5](Day%2005) - If You Give A Seed A Fertilizer
- **Problem:** Number translation through range maps. 
- **Solution:** Part one was simple to solve. Part two was not. I eventually paralyzed my query and the whole mess ran in about 180 seconds.
- Future fun: Go back and code a real query for this.

### [Day 6](Day%2006) - Wait For It
- **Problem:** Optimization of ranges.
- **Solution:** Brute force. There is probably a more math way to do this, but the solution set is small enough that the whole thing runs quickly.

### [Day 7](Day%2007) - Camel Cards
- **Problem:** Score Poker, Jokers might be wild.
- **Solution:** Part one was a case of building a table to set bitflags for the winning order of hands, then converting the hands themselves to values. Part two had me add more values to the table.

### [Day 8](Day%2008) - Haunted Wasteland
- **Problem:** Follow a map.
- **Solution:** Part one is a very simple dictionary follow. Part two is the same, with a splash of LCM. Thanks [2015 Day 20](../2015/Day%2020) for the factorization code.

### [Day 9](Day%2009) - Mirage Maintenance
- **Problem:** Follow a path.
- **Solution:** Part one was easy to do by following the example. The hard part was squashing everything down to a couple lines of `LINQ.`

### [Day 10](Day%2010) - Pipe Maze
- **Problem:** Walk a map perimeter then divide the area into inside and outside.
- **Solution:** Part one is a simple walk around the perimeter. Part two required an expansion of the map, as I couldn't figure out clean logic for "squeezing".

### [Day 11](Day%2011) - Cosmic Expansion
- **Problem:** Distances on a 2D map. 
- **Solution:** Taxi distance with a mutilative offset on the X and Y.

### [Day 12](Day%2012) - Hot Springs
- **Problem:** Counting combinations
- **Solution:** Part one is straight forward and easily brute force-able. Part two took a pile of loop optimization and ultimately I had the right idea. The final bit of pruning I found in a reddit post.

### [Day 13](Day%2013) - Point of Incidence
- **Problem:** Finding the reflection line in a pattern.
- **Solution:** Part one was straight forward. Part two was initially overcomplicated with a scheme to fix the bad data. It almost worked, however, the program was matching non-reflecting lines. Sleeping on the issue had me throw out all that code and add an "Off by one" check to the main reflection check.

### [Day 14](Day%2014) - Parabolic Reflector Dish
- **Problem:** Tilting rocks
- **Solution:** Slicing the map into arrays that could be processed individually saved a lot of time and headaches. Which simply pushed the headache to rediscovering how to calculate the final answer for part two.

### [Day 15](Day%2015) - Lens Library
- **Problem:** Shuffling data
- **Solution:** So much `LINQ`.

### [Day 16](Day%2016) - The Floor Will Be Lava
- **Problem:** Follow a beam of light past mirrors and prisms. 
- **Solution:** Pathfinding with complex numbers. Implemented a `Cursor()` class. Surprised I've not felt the need to do that before. Loop detection was the hardest thing.

### [Day 17](Day%2017) - Clumsy Crucible
- **Problem:** A to B with the least heat loss.
- **Solution:** This looked like a solution for A*. It was not. There's no good heuristic for the algorithm to function. Additionally, part two found an incorrect answer first due to the way stepping works, so I had to back up and cut it down to a normal search. I did get to reuse my `Cursor()` class from [Day 16](Day%2016) so that was good.

### [Day 18](Day%2018) - Lavaduct Lagoon
- **Problem:** Get the area of a polygon. 
- **Solution:** From Day 12, I'd seen some references to the shoelace formula. This is half of the solution. The other half is using the shoelace to feed into Pick's Theorem. The third half is making sure the X and Y are `long` and not `int`. 

### [Day 19](Day%2019) - Aplenty
- **Problem:** Combonomics.
- **Solution:** Part one was a simple tree to walk. Part two took a lot of noodling, but got it down to a recursive call that sectioned the data.

### [Day 20](Day%2020) - Pulse Propagation
- **Problem:** Work out the flow of pulses through a grid. 
- **Solution:** With all my previous knowledge, part one was a straight forward simulation. Part two required working out what the machine was doing. An accidental reddit spoiler showed people using Graphvis to do so, so I followed suit. Interpreting the graph was the second challenge.

### [Day 21](Day%2021) - Step Counter
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Sand Slabs
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) -
- **Problem:** 
- **Solution:**

### [Day 24](Day%2024) -
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) -
- **Problem:**
- **Solution:**
