# 2024 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Historian Hysteria
- **Problem:** List comparison.
- **Solution:** LINQ, LINQ, wonderful LINQ. 

### [Day 2](Day%2002) - Red-Nosed Reports
- **Problem:** Checking the sort and spread of a sequence. 
- **Solution:** Part one involved some simple logic to process the rules. Part two didn't present with an elegant way to remove elements so it got brute forced.

### [Day 3](Day%2003) - Mull It Over
- **Problem:** String parsing. 
- **Solution:** A simple regex, and understanding the puzzle data. The biggest gotcha of this puzzle was correctly understanding how to deal with newline characters. The answer is ... `don't()`. 

### [Day 4](Day%2004) - Ceres Search
- **Problem:** Word search!
- **Solution:** I thought I could flatten the input to avoid a bunch of bound checks, only to find out that I require a bunch more bound checks.

### [Day 5](Day%2005) - Print Queue
- **Problem:** Sorting lists of numbers.
- **Solution:** Today I learned you can put a function in `Sort()` and not have to write a bubble sort. (Totally wrote a bubble sort, pre-cleanup.)

### [Day 6](Day%2006) - Guard Gallivant
- **Problem:** Basic Pathfinding simulation.
- **Solution:** Brute force with `Cursor()`. There's probably better ways to do this, but my solution was good enough. 

### [Day 7](Day%2007) - Bridge Repair
- **Problem:** Trinary Counting!
- **Solution:** Part one was initially done by using the bits of an integer as the pattern for the solution. Part two blew that out of the water and required it to be reimplemented. I pulled from my `Base10Converter()` library function to do the translation.

### [Day 8](Day%2008) - Resonant Collinearity
- **Problem:** Lines on a map.
- **Solution:** The solution is straight forward. Find the delta between the two points, and follow it up/down the line. The implementation ran into a brick wall when I tried to use my Point2D in a way it wasn't truly intended to be used. While there are `<` and `>` operators built into `Point2D()`, they don't work as expected in the quadrants where the delta signs are not the same between the X and Y values. A small change to the `InRange()` function fixed that.

### [Day 9](Day%2009) - Disk Fragmenter
- **Problem:** Defragmentation
- **Solution:** The hardest part was understanding the setup. The second hardest part was making the linked lists work.

### [Day 10](Day%2010) - Hoof It
- **Problem:** Iterative pathfinding
- **Solution:** This boiled down to a simple recursive function for both parts.

### [Day 11](Day%2011) - Plutonian Pebbles
- **Problem:** Fractal expansion.
- **Solution:** Part one was small enough to do the naive solution. Part two exploded and forced me to rework the solution to collapse duplicate work.

### [Day 12](Day%2012) - Garden Groups
- **Problem:** Area and perimeter of an irregular shape.
- **Solution:** Part one was straight forward. Part two took a lot to noodle, and pulled down a new EnumExtension from Stackoverflow to group consecutive numbers.

### [Day 13](Day%2013) - Claw Contraption
- **Problem:** Linear algebra.
- **Solution:** I vaguly know how to solve this, but had to look up the specifics. [This post](https://www.reddit.com/r/adventofcode/comments/1hd7irq/2024_day_13_an_explanation_of_the_mathematics/) helped me out. Implemented [Cramer's Rule](https://en.wikipedia.org/wiki/Cramer%27s_rule#Explicit_formulas_for_small_systems) as my solution. 

### [Day 14](Day%2014) - Restroom Redoubt 
- **Problem:** Pathfinding and image detection.
- **Solution:** Part one is easy to simulate. Part two needed two realizations, one the solution would be where the most robots are clustered together, and not all robots would form the tree.

### [Day 15](Day%2015) - Warehouse Woes
- **Problem:** Collision detection
- **Solution:** Part one was a straight forward implementation. Part two required a rethink of the problem, and a lot more bounds checking.

### [Day 16](Day%2016) - Reindeer Maze
- **Problem:** Pathfinding!
- **Solution:** A* to the rescue!

### [Day 17](Day%2017) - 
- **Problem:** 
- **Solution:** 

### [Day 18](Day%2018) - 
- **Problem:** 
- **Solution:** 

### [Day 19](Day%2019) - 
- **Problem:** 
- **Solution:** 

### [Day 20](Day%2020) - 
- **Problem:** 
- **Solution:** 

### [Day 21](Day%2021) - 
- **Problem:** 
- **Solution:** 

### [Day 22](Day%2022) - 
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
