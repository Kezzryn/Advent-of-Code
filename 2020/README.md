# 2020 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Report Repair
- **Problem:** Search and sum!
- **Solution:** Made short work of this one with a pair of query clauses.

### [Day 2](Day%2002) - Password Philosophy
- **Problem:** String rules testing.
- **Solution:** Part one used `Count()` and in part two, I found a use for an `xor` operator. 

### [Day 3](Day%2003) - Toboggan Trajectory
- **Problem:** Tree hitting.
- **Solution:** Part one was a straight forward loop. Part two turned it into a function.

### [Day 4](Day%2004) - Passport Processing
- **Problem:** Data validation.
- **Solution:** Rolled each line out into a dictionary, then applied various logical checks to each entry.

### [Day 5](Day%2005) - Binary Boarding
- **Problem:** Binary Tree... kinda. 
- **Solution:** Did what I thought was a very tight solution leveraging a bunch of C# syntax. However, I should have looked at the puzzle input closer, there's an easier conversion by rewriting the strings as binary.

### [Day 6](Day%2006) - Custom Customs
- **Problem:** Character counts in groups of strings.
- **Solution:** I'm getting handy with this entire LINQ thing. 

### [Day 7](Day%2007) - Handy Haversacks
- **Problem:** Nested bags! 
- **Solution:** Recursive tree search.

### [Day 8](Day%2008) - Handheld Halting 
- **Problem:** The year's first VM.
- **Solution:** Worked with a stripped down version of last years Intcode VM, adding infinite loop detection in part one. For part two had the system try to fix itself by iteratively swapping out instructions until it ran successfully. 

### [Day 9](Day%2009) - Encoding Error
- **Problem:** Sums of lists.
- **Solution:** Part one I built a cache of the previous sums, so each step I'd only need to re-compute 24 items, and not the entire set of 300. For part two I initially iterated a fixed sized window over a list of precomputed sums. The mega-thread had a very cool solution with a variable sized window that runs quicker, and with a fraction of the lines of code.

### [Day 10](Day%2010) - Adapter Array
- **Problem:** Combination counting.
- **Solution:** Part one was a straight forward iteration. I spent a lot of time in part two trying to work out a math formula. However, I couldn't account for certain combos without a more direct simulation. I found break points in the number array and build a simple recursive function to walk the possibilities.

### [Day 11](Day%2011) - Seating System
- **Problem:** Cell automata!
- **Solution:** For part one I worked everything into a single dimension array so I could take nice slices to count for hte neighbors. For part two that got thrown out for the system in the code now. Building a buffer around the puzzle data greatly simplified edge and bounds testing.

### [Day 12](Day%2012) - Rain Risk
- **Problem:** Navigation
- **Solution:** Complex Numbers once again. This time they made rotation and translation trivial.

### [Day 13](Day%2013) - Shuttle Search 
- **Problem:** Factoring primes.
- **Solution:** I think I get this now. I recognized several parts, but didn't put it together the right way. The bug that didn't impact my test data didn't help.

### [Day 14](Day%2014) - Docking Data
- **Problem:** Bit-masking.
- **Solution:** Part one worked out to be two masks. Part two had a moment of trickery. The test data worked. However, I had overlooked that `1` is an `int` and it borked the various bit manipulation functions I swiped from [2019](../2019/Day%2024).

### [Day 15](Day%2015) - Rambunctious Recitation
- **Problem:** Number sequence based on a set of rules.
- **Solution:** Careful use of a dictionary to track historical data.

### [Day 16](Day%2016) - Ticket Translation
- **Problem:** Validate data.
- **Solution:** Part one had a simple exclusion. Part two required turning the data on the side, and filtering. Then deducing which columns fit, based on work previously.

### [Day 17](Day%2017) - Conway Cubes
- **Problem:** Game of life in 3 and 4 dimensions.
- **Solution:** Pulled from various code I'd used before. 4th dimension solving took a long time. Based on some code snippets, I changed my code to made a map / cache the neighbors and used that as my comparison, greatly cutting down on duplicate cell lookups. 

### [Day 18](Day%2018) - Operation Order
- **Problem:**
- **Solution:**
 
### [Day 19](Day%2019) - Monster Messages
- **Problem:**
- **Solution:**

### [Day 20](Day%2020) - Jurassic Jigsaw
- **Problem:**
- **Solution:**

### [Day 21](Day%2021) - Allergen Assessment
- **Problem:**
- **Solution:**

### [Day 22](Day%2022) - Crab Combat
- **Problem:**
- **Solution:**

### [Day 23](Day%2023) - Crab Cups
- **Problem:**
- **Solution:**

### [Day 24](Day%2024) - Lobby Layout
- **Problem:**
- **Solution:**

### [Day 25](Day%2025) - Combo Breaker
- **Problem:**
- **Solution:**