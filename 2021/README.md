# 2021 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Sonar Sweep
- **Problem:** Compare and sum!
- **Solution:** Indexed where clauses did the comparisons. A basic loop grouped the values for part two.

### [Day 2](Day%2002) - Dive!
- **Problem:** Distance iteration. 
- **Solution:** Basic loop and switch.

### [Day 3](Day%2003) - Binary Diagnostic
- **Problem:** Processing data in columns.
- **Solution:** I got clever here and used bit manipulation for both parts. I converted everything to integers at the top. [2019 Day 24](../2019/Day%2024) provided `getBit()` and `setBit()` functions, which I used to sum and set bits set in the various columns for part one. Part two used `List<int>` and some light predicate functions to filter things down.

### [Day 4](Day%2004) - Giant Squid
- **Problem:** B.I.N.G.O and giant squid is his name-o. 
- **Solution:** More bitmasking with [2019 Day 24's](../2019/Day%2024) helper functions. I turned the 2D card into a 1D list and bit flipped an int to track which numbers had been called on the card. A precomputed bitmask was then applied to check for a win condition. Part two took me a little bit to figure out how to stop processing cards that had already won, but it was obvious in hindsight. 

### [Day 5](Day%2005) - Hydrothermal Venture
- **Problem:** Line intersections.
- **Solution:** Walked the lines with basic loops. The diagonal took a bit to noodle out, and had an early error where it would skip the final point on the line. Oops.

### [Day 6](Day%2006) - Lanternfish
- **Problem:**
- **Solution:**

### [Day 7](Day%2007) - The Treachery of Whales
- **Problem:**
- **Solution:**

### [Day 8](Day%2008) - Seven Segment Search
- **Problem:**
- **Solution:**

### [Day 9](Day%2009) - Smoke Basin
- **Problem:**
- **Solution:**

### [Day 10](Day%2010) - Syntax Scoring
- **Problem:**
- **Solution:**

### [Day 11](Day%2011) - Dumbo Octopus
- **Problem:**
- **Solution:**

### [Day 12](Day%2012) - Passage Pathing
- **Problem:**
- **Solution:**

### [Day 13](Day%2013) - Transparent Origami
- **Problem:**
- **Solution:**

### [Day 14](Day%2014) - Extended Polymerization
- **Problem:**
- **Solution:**

### [Day 15](Day%2015) - Chiton 
- **Problem:**
- **Solution:**

### [Day 16](Day%2016) - Packet Decoder
- **Problem:**
- **Solution:**

### [Day 17](Day%2017) - Trick Shot
- **Problem:**
- **Solution:**

### [Day 18](Day%2018) - Snailfish
- **Problem:** 
- **Solution:**

### [Day 19](Day%2019) - Beacon Scanner
- **Problem:**
- **Solution:**

### [Day 20](Day%2020) - Trench Map
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Dirac Dice
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Reactor Reboot
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Amphipod
- **Problem:** 
- **Solution:**

### [Day 24](Day%2024) - Arithmetic Logic Unit
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Sea Cucumber
- **Problem:**
- **Solution:**