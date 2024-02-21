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
- **Problem:** Fish population tracking.
- **Solution:** Don't try to reinvent a `Queue()` when you've got one built into the language.

### [Day 7](Day%2007) - The Treachery of Whales
- **Problem:** Fuel consumption.
- **Solution:** Didn't need anything fancy. Simple iteration with some sanity checks worked fine. Built a lookup table for the fuel costs in part two.

### [Day 8](Day%2008) - Seven Segment Search
- **Problem:** Determine scrambled codes to work out digits.
- **Solution:** Noodled the reduction rules out on some paper, then abused some Linq to generate the answers.

### [Day 9](Day%2009) - Smoke Basin
- **Problem:** Mapping low points.
- **Solution:** Part one is a simple neighbor comparison. Using those points, it was a simple matter to build a loop around a `Queue` to find the basin sizes.

### [Day 10](Day%2010) - Syntax Scoring
- **Problem:** Matching brackets
- **Solution:** Part one was easy once I got out of my own way and stopped trying to do it recursivly. For part two it really helped to read the problem description.

### [Day 11](Day%2011) - Dumbo Octopus
- **Problem:** Cell automata
- **Solution:** Straight forward implementation, no real surprises. 

### [Day 12](Day%2012) - Passage Pathing
- **Problem:** Finding all the paths.
- **Solution:** No best path optimization here, we need to find all of them. I didn't complicate this for myself and left everything as strings. Second part needed a little noodling, but the addition of a flag the first time backtracking happens solved the problem.

### [Day 13](Day%2013) - Transparent Origami
- **Problem:** Folding paper.
- **Solution:** The trickiest part of this was keeping the X and Y coordinates straight.

### [Day 14](Day%2014) - Extended Polymerization
- **Problem:** Inserting characters in strings.
- **Solution:** Part 1 was easily simulated. Part 2 threw all that away for a memoitization scheme.

### [Day 15](Day%2015) - Chiton 
- **Problem:** Pathfinding
- **Solution:** Brought forward the A* implemention I have and tweaked the cost function to make each step "cost" the risk value.

### [Day 16](Day%2016) - Packet Decoder
- **Problem:** Nested packet decoding
- **Solution:** One misread on the process and I spent way too long debugging a version where I incorrectly flushed the buffer after gathering the packets. Sorting that out and everything came together nicely.

### [Day 17](Day%2017) - Trick Shot
- **Problem:** Curve prediction.
- **Solution:** Straight forward simulation. There's probably some math function that could have done this easier.

### [Day 18](Day%2018) - Snailfish
- **Problem:** Binary tree addition and balancing.
- **Solution:** So much recursion. Once I realized I had an easy way to return the tree to a string, for the add function, I created new `Node()` with concatinated strings. For part two, some foreach trickery gave me all the combos of strings to add up.

### [Day 19](Day%2019) - Beacon Scanner
- **Problem:**
- **Solution:**

### [Day 20](Day%2020) - Trench Map
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Dirac Dice
- **Problem:** Round and round the board game.
- **Solution:** Part one was easy to simulate. Part two required some noodling on how to reduce the search space. The key is realizing that while dice rolls will multiply, only the results and how many times those results happen matter.

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
- **Problem:** Track the migration habits of Sea Cucumbers
- **Solution:** This was a straight forward simulation. I initially did it with flipping boards, but realized that it'd be easier with one board and careful moves.