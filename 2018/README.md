# 2018 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Chronal Calibration
- **Problem:** Some simple list calculations to start off the year. 
- **Solution:** `.Sum()` for part one. Part two needed a loop and a `HashSet()`.

### [Day 2](Day%2002) - Inventory Management System
- **Problem:** String manipulation. 
- **Solution:** I'm pleased I was able to pull out part one with LINQ, part two I resorted to a more direct set of loops. Scanning the mega solution thread, there was a clever LINQ solution for it that involved removing a letter then grouping the dataset.

### [Day 3](Day%2003) - No Matter How You Slice It
- **Problem:** Calculate overlapping areas. 
- **Solution:** `Rectangles()` and `Intersect()` did the heavy lifting.

### [Day 4](Day%2004) - Repose Record
- **Problem:** Track activity over time.
- **Solution:** Several assumptions were made about the input data that checked out to be true and simplified the processing, and allowed part two to be a small tweak on the part one data.

### [Day 5](Day%2005) - Alchemical Reduction
- **Problem:** String reduction on a 50,000 character string. 
- **Solution:** Datatypes matter. Converting the string to a `LinkedList<int>()` and working on that, made the whole process easier, and faster. String processing ran in a couple hundred milliseconds. Loading the string directly into the `LinkedList()` and the entire program (minus initial data load) ran in under 35 ms.  

### [Day 6](Day%2006) - Chronal Coordinates
- **Problem:** Finding distances over an area.
- **Solution:** The key insight for part one was to realize that any hit from an edge indicates a point is infinite, and to remove those from the solution set. Part two starts as a thought to find the radius of a circle, however, experimentation showed that the area is already within the search area defined in part one.

### [Day 7](Day%2007) - The Sum of Its Parts
- **Problem:** Graph traversal!
- **Solution:** Part one took me a little to get the exact setup right. I used queues to control which jobs got picked up and processed.  Part two formalized the queue process, and added a time element. The major change was to implement a new queue to represent the elf workers, but limit it to 5 entries.

### [Day 8](Day%2008) - Memory Maneuver
- **Problem:** First tree puzzle. 
- **Solution:** A recursive load and a lightweight `Node()` class solved part one. Part two extended the load function with the value rules.

### [Day 9](Day%2009) - Marble Mania
- **Problem:** Circular list manipulation. 
- **Solution:** I was initially put in the mind of the double queue I used for [2016 Day 19](../2016/Day%2019/). However, some doodling on paper showed that a simple linked list would suffice. Part two made me use `long` for the answer values. 

### [Day 10](Day%2010) - The Stars Align
- **Problem:** A large scatter of points that are coming together.
- **Solution:** Lots of experimentation showed that the area of the points reduces down to a minimal area. The program pauses when the points come close and allows the user to step through to find the answer. 

### [Day 11](Day%2011) - Chronal Charge
- **Problem:** Calculate the sums of values in a grid.
- **Solution:** Populating the grid was straight forward. Part two stress tested my solution and found it wanting. Brute force took around 8 minutes. I reworked it to take the size from the previous step and add the new border to it. That got me down to about 35 seconds. The next optimization was to recognize that the value drops to zero and does not recover. A [Summed-Area table](https://en.wikipedia.org/wiki/Summed-area_table) would have been a more optimal solution.

### [Day 12](Day%2012) - Subterranean Sustainability
- **Problem:** Cell automata.
- **Solution:** Part one is easy enough to simulate. Fifty billion iterations would have taken no less than 36 hours, and likely many more. Visualizing the sim showed that it stabilized after nearly 100 iterations.

### [Day 13](Day%2013) - Mine Cart Madness
- **Problem:** Collision detection in a maze.
- **Solution:** Motion around the mine cart maze was the easy part. Reliable collision detection wasn't until I moved first to tracking positions in a `HashSet()` then to a `Dictionary()` to ID the other cart in the collision. 

### [Day 14](Day%2014) - Chocolate Charts
- **Problem:** Pattern matching on an ever growing list.
- **Solution:** Part one was nice and easy with a simple loop and a clear end point. Part two threw most of that out, making me come up with a scheme to efficiently compare arrays. I struggled for a little bit, finding answers in the 800M range (Actual answer is in the 20 M range), before I realized I was inadvertently skipping past potential matches. Adding a `Queue()` to the middle of my loop solved the issue.

### [Day 15](Day%2015) - Beverage Bandits
- **Problem:** Elves vs Goblins, FIGHT!
- **Solution:** There are a lot of little details that all need to be tracked. The program spends a lot of time in A* working out pathfinding.

### [Day 16](Day%2016) - Chronal Classification
- **Problem:** Build a VM and figure out which opcodes do what. 
- **Solution:** My biggest enemy was myself. The VM had a silly number of typos in it. Working out the opcodes was a simple matter of deduction. After fixing the VM code, that step became a sanity check for future runs.

### [Day 17](Day%2017) - Reservoir Research
- **Problem:** Used the code from [2022 Day 14](../2022/Day%2014), Regolith Reservoir as the basis for this one. 
- **Solution:** Had a little trouble with unrolling back up, until I converted the drip function to be recursive. I am glad I kept the water tagged as standing or set, as that made part two trivial.

### [Day 18](Day%2018) - Settlers of The North Pole
- **Problem:** Three state cell automata.
- **Solution:** As with previous iterations, a grid with a double buffer was used. The adjacency check initially used an enumeration, however, I got a significant performance gain by changing it to nested for loops.
 
### [Day 19](Day%2019) - Go With The Flow
- **Problem:** Add jump instruction to [Day 16s](Day%2016) VM and see what happens. 
- **Solution:** What happens is finding the sum of the factors of a number. The flow control in this program is crazy hard to follow.

### [Day 20](Day%2020) - A Regular Map
- **Problem:** Text parsing to make a map.
- **Solution:** I solved this two ways. The first way was to build the map and then search each point on it with A*. This takes 20-40 seconds to search across 10,000 points. The second, more efficient way, is to count the steps as the map is being built. This can be done in under 100 ms.

### [Day 21](Day%2021) - Chronal Conversion
- **Problem:** [Day 19s](Day%2019) VM called on to run one more program.
- **Solution:** I initially re-implemented the code to try to better understand what it was doing. This got me most of the way, but it wasn't until I was stepping through the sequences generated that I had my light bulb moment. The key to the puzzle was to realize it's generating a sequence that loops, and the puzzle answers are the first and last entries of that loop. My vm runs the loop through in about 45 seconds. The rewrite of it runs in about 4 ms.

### [Day 22](Day%2022) - Mode Maze
- **Problem:** Generate a maze and path-find through it. 
- **Solution:** Trusty A* makes a reappearance with a new `Map()` class to generate maze locations on the fly. The biggest change was to track the tool usage. This was done by using the Z coordinate of a `Point3D()` object.

### [Day 23](Day%2023) - Experimental Emergency Teleportation
- **Problem:** Searching 3D space.
- **Solution:** Looking through the solution thread, there are some clever solutions and insights. I put all the bots in a `Box()` and subdivided the search space until I found the solution. The hardest part was adapting the "Sphere intersects Cube" calculation from normal Euclidean distances to work on taxi distances.

### [Day 24](Day%2024) - Immune System Simulator 20XX
- **Problem:** The immune system of reindeer is weird.
- **Solution:** Read every step carefully. I initially screwed up my tiebreaks. And then for part two, there is a third condition that needs to be considered. Stalemate!

### [Day 25](Day%2025) - Four-Dimensional Adventure
- **Problem:** 4D grouping!
- **Solution:** A slightly inefficient, but simple, loop to sort/collect elements into buckets.
