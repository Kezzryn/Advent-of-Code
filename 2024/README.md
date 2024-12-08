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
- **Solution:** A simple regex, and undestanding the puzzle data. The biggest gotcha of this puzzle was correctly understanding how to deal with newline characters. The answer is ... `don't()`. 

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
- **Solution:** The solution is straight forward. Find the delta between the two points, and follow it up/down the line. The implmentation ran into a brick wall when I tried to use my Point2D in a way it wasn't truly intended to be used. While there are `<` and `>` operators built into `Point2D()`, they don't work as expected in the quadrents where the delta signs are not the same between the X and Y values. A small change to the `InRange()` function fixed that.

### [Day 9](Day%2009) - 
- **Problem:** 
- **Solution:** 

### [Day 10](Day%2010) - 
- **Problem:** 
- **Solution:** 

### [Day 11](Day%2011) - 
- **Problem:** 
- **Solution:** 

### [Day 12](Day%2012) - 
- **Problem:** 
- **Solution:** 

### [Day 13](Day%2013) - 
- **Problem:** 
- **Solution:** 

### [Day 14](Day%2014) - 
- **Problem:** 
- **Solution:** 

### [Day 15](Day%2015) - 
- **Problem:** 
- **Solution:** 

### [Day 16](Day%2016) - 
- **Problem:** 
- **Solution:** 

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
