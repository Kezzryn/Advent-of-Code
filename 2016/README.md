# 2016 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - No Time for a Taxicab
- **Problem:** Starting off the year with a distance travelled problem. Part one could have been solved with a simple +/- counter, but part two needs to know where we've been. 
- **Solution:** Add to a `Point()` with an array of `Size()` to represent each direction, much like we've done in other years. A `HashSet` tracks where we've been.

### [Day 2](Day%2002) - Bathroom Security
- **Problem:** Another pathfinding problem. 
- **Solution:** Another iterator, but with bounds checking!

### [Day 3](Day%2003) - Squares With Three Sides
- **Problem:** Given a list of three numbers, are these measurements for a valid triangle? 
- **Solution:** Reused a good part of the solution from [2015 Day 2](../2015/Day%2002/) for the first part. This turned out to be a fortuitous decision, as it lead first to the realization that all three combos of sides need to match the (a + b) > c. This can be short cut if you do the math on the two smallest sides. Part two had a bit of a twist, having to read and parse column data. This may have been my first use of the `Range()` object. 

### [Day 4](Day%2004) - Security Through Obscurity
- **Problem:**
- **Solution:**

### [Day 5](Day%2005) - How About a Nice Game of Chess?
- **Problem:**
- **Solution:**

### [Day 6](Day%2006) - Signals and Noise
- **Problem:**
- **Solution:**

### [Day 7](Day%2007) - Internet Protocol Version 7
- **Problem:**
- **Solution:**

### [Day 8](Day%2008) - Two-Factor Authentication
- **Problem:**
- **Solution:**

### [Day 9](Day%2009) - Explosives in Cyberspace
- **Problem:**
- **Solution:**

### [Day 10](Day%2010) - Balance Bots
- **Problem:**
- **Solution:**

### [Day 11](Day%2011) - Radioisotope Thermoelectric Generators
- **Problem:**
- **Solution:**

### [Day 12](Day%2012) - Leonardo's Monorail
- **Problem:**
- **Solution:**

### [Day 13](Day%2013) - A Maze of Twisty Little Cubicles
- **Problem:**
- **Solution:**

### [Day 14](Day%2014) - One-Time Pad
- **Problem:**
- **Solution:**

### [Day 15](Day%2015) - Timing is Everything
- **Problem:**
- **Solution:**

### [Day 16](Day%2016) - Dragon Checksum
- **Problem:**
- **Solution:**

### [Day 17](Day%2017) - Two Steps Forward
- **Problem:**
- **Solution:**

### [Day 18](Day%2018) - Like a Rogue
- **Problem:**
- **Solution:**

### [Day 19](Day%2019) - An Elephant Named Joseph
- **Problem:**
- **Solution:**

### [Day 20](Day%2020) - Firewall Rules
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Scrambled Letters and Hash
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Grid Computing 
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Safe Cracking
- **Problem:**
- **Solution:**

### [Day 24](Day%2024) - Air Duct Spelunking
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Clock Signal
- **Problem:**
- **Solution:**