# 2019 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - The Tyranny of the Rocket Equation
- **Problem:** 2019 and we're off with some basic sums!
- **Solution:** Part two had a small hiccup where I was returning negative fuel, which was a no-no.

### [Day 2](Day%2002) - 1202 Program Alarm
- **Problem:** Build an Intcode computer.
- **Solution:** Borrowed a pile of patterns from my [Synacor VM](../Synacor%20Challenge). Need to be careful when assigning arrays, an early bug had the puzzle input assigned by reference, making resetting the machine impossible.

### [Day 3](Day%2003) - Crossed Wires
- **Problem:** Create maps and find the intersections between the maps.
- **Solution:** Once I had my data loaded, `Intersect()` and `Min()` to the rescue.

### [Day 4](Day%2004) - Secure Container
- **Problem:** Password generation.
- **Solution:** Part one was fairly straight forward. I got to learn about using indexes in `.While()` statements. Part two broke my brain for a little bit. I worked out a whole pile of pattern matching, only to throw it all away when I had a lightbulb moment on how to group the input.

### [Day 5](Day%2005) - Sunny with a Chance of Asteroids
- **Problem:** Extend the Intcode computer with new commands.
- **Solution:** Pilfered more code from [Synacor VM](../Synacor%20Challenge) for the I/O. Immediate vs. Position mode was a little tricky to line.

### [Day 6](Day%2006) - Universal Orbit Map
- **Problem:** Graph traversal.
- **Solution:** Part one involved counting up from the bottom of the graph to the root. Part two looked hard, until I realized that I never need to travel down the graph, only up it, until I find an intersection point. 

### [Day 7](Day%2007) - Amplification Circuit
- **Problem:** Intcode computer extension, now with more IO.
- **Solution:** Fairly straigh forward extensions to the I/O systems. Made it work manually first, then automated the process. Once again (still?) borrowed heavily from [Synacor VM](../Synacor%20Challenge).

### [Day 8](Day%2008) - Space Image Format
- **Problem:** Data slicing!
- **Solution:** Build a little cube of data. Slice it horizontally for part one, then drill vertically for part two.

### [Day 9](Day%2009) - Sensor Boost
- **Problem:** Intcode computer extension, now with more address modes.
- **Solution:** Just as in the Synacore puzzle, I really struggled with getting the modes right. Returning the value vs. the value at an address is my bane. In the end I had to refer to [reddit]( https://www.reddit.com/r/adventofcode/comments/e8aw9j/2019_day_9_part_1_how_to_fix_203_error/) to resolve the conflict.

### [Day 10](Day%2010) - Monitoring Station
- **Problem:** 
- **Solution:** 

### [Day 11](Day%2011) - Space Police
- **Problem:** 
- **Solution:** 

### [Day 12](Day%2012) - The N-Body Problem
- **Problem:** 
- **Solution:** 

### [Day 13](Day%2013) - Care Package
- **Problem:** 
- **Solution:** 

### [Day 14](Day%2014) - Space Stoichiometry
- **Problem:** 
- **Solution:** 

### [Day 15](Day%2015) - Oxygen System
- **Problem:**
- **Solution:**

### [Day 16](Day%2016) - Flawed Frequency Transmission
- **Problem:** 
- **Solution:** 

### [Day 17](Day%2017) - Set and Forget
- **Problem:** 
- **Solution:** 

### [Day 18](Day%2018) - Many-Worlds Interpretation
- **Problem:** 
- **Solution:** 
 
### [Day 19](Day%2019) - Tractor Beam
- **Problem:** 
- **Solution:** 

### [Day 20](Day%2020) - Donut Maze
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Springdroid Adventure
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Slam Shuffle
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Category Six
- **Problem:**
- **Solution:**

### [Day 24](Day%2024) - Planet of Discord
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Cryostasis
- **Problem:**
- **Solution:**