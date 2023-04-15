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
- **Solution:** `Rectanges()` and `Intersect()` did the heavy lifting.

### [Day 4](Day%2004) - Repose Record
- **Problem:** Track activity over time.
- **Solution:** Several assumptions were made about the input data that checked out to be true and simplified the processing, and allowed part two to be a small tweak on the part one data.

### [Day 5](Day%2005) - Alchemical Reduction
- **Problem:** String reduction on a 50,000 character string. 
- **Solution:** Datatypes matter. Converting the string to a `LinkedList<int>()` and working on that, made the whole process easier, and faster. String processing ran in a couple hundred milliseconds. Loading the string directly into the `LinkedList()` and the entire program (minus initial data load) ran in under 35 ms.  

### [Day 6](Day%2006) - Chronal Coordinates
- **Problem:** Finding distances over an area.
- **Solution:** The key insight for part one was to realize that any hit from an edge indicates a point is infinate, and to remove those from the solution set. Part two startes as a thought to find the radius of a circle, however, experimentation showed that the area is already within the search area defined in part one.

### [Day 7](Day%2007) - The Sum of Its Parts
- **Problem:** Graph traversal!
- **Solution:** Part one took me a little to get the exact setup right. I used queues to control which jobs got picked up and processed.  Part two formallized the queue process, and added a time element. The major change was to implement a new queue to represent the elf workers, but limit it to 5 entries.

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
- **Problem:** Calcuate the sums of values in a grid.
- **Solution:** Populating the grid was straight forward. Part two stress tested my solution and found it wanting. Brute force took around 8 minutes. I reworked it to take the size from the previous step and add the new boarder to it. That got me down to about 35 seconds. The next optimzation was to recognize that the value drops to zero and does not recover. A [Summed-Area table](https://en.wikipedia.org/wiki/Summed-area_table) would have been a more optimal solution.

### [Day 12](Day%2012) - Subterranean Sustainability
- **Problem:** Cell automata.
- **Solution:** Part one is easy enough to simulate. Fifty billion iterations would have taken no less than 36 hours, and likely many more. Visualizing the sim showed that it stabalized after nearly 100 iterations.

### [Day 13](Day%2013) - Mine Cart Madness
- **Problem:**
- **Solution:**

### [Day 14](Day%2014) - Chocolate Charts
- **Problem:**
- **Solution:**

### [Day 15](Day%2015) - Beverage Bandits
- **Problem:**
- **Solution:**

### [Day 16](Day%2016) - Chronal Classification
- **Problem:**
- **Solution:**

### [Day 17](Day%2017) - Reservoir Research
- **Problem:**
- **Solution:**

### [Day 18](Day%2018) - Settlers of The North Pole
- **Problem:**
- **Solution:**
 
### [Day 19](Day%2019) - Go With The Flow
- **Problem:**
- **Solution:**

### [Day 20](Day%2020) - A Regular Map
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Chronal Conversion
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Mode Maze
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Experimental Emergency Teleportation
- **Problem:**
- **Solution:**

### [Day 24](Day%2024) - Immune System Simulator 20XX
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Four-Dimensional Adventure
- **Problem:**
- **Solution:**