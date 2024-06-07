# 2015 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Not Quite Lisp
- **Problem:** Track location based on inputs. 
- **Solution:** Loop through the string incrementing and decrementing a counter.
- **Update Feb 25, 2022:** I learned (re-learned?) that you can put a lambda expression in `Sum()`. 

### [Day 2](Day%2002) - I Was Told There Would Be No Math
- **Problem:** Find the volume, area, perimeter and smallest side calculations on a rectangular box.
- **Solution:** Abuse of anonymous arrays and LINQ syntax to calculate the smallest sides of things. Honestly, I was surprised I was allowed to define an anonymous array like that. Learning is fun. :)
- **Update Feb 25, 2022:** More learning. After the changes in [Day 1](Day%2001), I reworked the helper functions and removed the loop to collapse the program down to a pair of LINQ statements.

### [Day 3](Day%2003) - Perfectly Spherical Houses in a Vacuum
- **Problem:** Trace all visited locations.
- **Solution:** HashSets are nice for this, as duplicates are automatically discarded. This was a simple simulation, adding each moves to a `HashSet<Point>`, then querying the `HashSet` for the answers. 

### [Day 4](Day%2004) - The Ideal Stocking Stuffer
- **Problem:** Creating MD5 hashes that fill a certain pattern. 
- **Solution:** Pulled code from documentation examples for the MD5 hash creation. I'm not sure if there's a faster way to find the answer to either part, other than to simply brute force it.

### [Day 5](Day%2005) - Doesn't He Have Intern-Elves For This?
- **Problem:** Substring parsing.
- **Solution:** In part one, I used language tools to find the target strings. Part two is some simple looping, but could probably be improved with liberal application of Regex.
- **Update:** 5/22/2024: Now with triple the Regex!

### [Day 6](Day%2006) - Probably a Fire Hazard
- **Problem:** Change the values in a 1000x1000 grid.
- **Solution:** Simulated the changes with a `Dictionary<Point, int>` that grows as I add lights to it. 

### [Day 7](Day%2007) - Some Assembly Required
- **Problem:** Bitwise logic gate emulation.
- **Solution:** This works out into a tree. Initial tests were taking a very long time. The fix was to cache the value of each node after it's calculated, so I don't wind up recalculating portions over and over again.

### [Day 8](Day%2008) - Matchsticks
- **Problem:** String parsing and the comparison of representation vs storage lengths.
- **Solution:** Straight forward solution using built in language functions `Regex.Escape()` and `Regex.Unescape()`.

### [Day 9](Day%2009) - All in a Single Night
- **Problem:** Traveling salesman problem. And hey! I can use my unused code from [2022 Day 16](../2022/Day%2016/) for this.
- **Solution:** My key insight was to realize that the sum of A -> B and B -> A is the edge weight between the nodes. The second insight was to NOT apply the Floyd–Warshall algorithm, as it does not apply to this puzzle. After that, I build a dictionary of answers with Branch and Bound. I need to figure out a better function for bounding on max.

### [Day 10](Day%2010) - Elves Look, Elves Say
- **Problem:** This is a "Look and Say" problem. Sometimes known as the "Morris Number Sequence".
- **Solution:** String parsing with `IndexOfAny()` some range work and `.Except()`

### [Day 11](Day%2011) - Corporate Policy
- **Problem:** Password generation that is required to follow certain rules.
- **Solution:** I could have created a string incrementor. Instead I borrow from the [2022 Day 25](../2022/Day%2025/) puzzle for a way to increment the password string while searching for a valid one.

### [Day 12](Day%2012) - JSAbacusFramework.io
- **Problem:** JSON parsing.
- **Solution:** I used the [Newtonsoft.Json.LINQ](https://www.newtonsoft.com/json) libraries to walk through the object model. Doing so required learning the object model. 

### [Day 13](Day%2013) - Knights of the Dinner Table
- **Problem:** Traveling Salesmen of a circular list.
- **Solution:** This was brute forced with a simple depth first search. An improvement would be to introduce bounds checking or other heuristic.

### [Day 14](Day%2014) - Reindeer Olympics
- **Problem:** Interval processing. The `TravelDistance` function does the heavy lifting. Working out first the overall interval, then determining how far the reindeer has travelled in the current interval.
- **Solution:** The tricky part was getting the LINQ right for the part two `GroupBy` clause. 

### [Day 15](Day%2015) - Science for Hungry People
- **Problem:** Maximization problem.
- **Solution:** This has used the same form of search loop I've used in other solutions. The main change here is the next step system, which spreads out up to 28 points from each successful test. 
- **Future fun:** Create start keys and steps algorithmically. Narrow the search space with lower/upper bounds for each of the variables. I **think** this is a partial derivative calculation. 

### [Day 16](Day%2016) - Aunt Sue
- **Problem:** Pattern matching with incomplete data.
- **Solution:** A `if` statement and `switch` statement.
- **Future fun:** Rework as a `Where` clause. 

### [Day 17](Day%2017) - No Such Thing as Too Much
- **Problem:** Combinatorial problem.
- **Solution:** Used the bit technique from [2022 Day 16](../2022/Day%2016/) to track the container patterns.

### [Day 18](Day%2018) - Like a GIF For Your Yard
- **Problem:** Game of Life simulation.
- **Solution:** I used a 3D array, with `x,y` as the puzzle state, with a flip flopping pointer to the third dimension as my next move. I'd read the state from `x,y,0` and write to `x,y,1`, then reverse the to/from.
- **Update June 3, 2024:** Now uses library function.

### [Day 19](Day%2019) - Medicine for Rudolph
- **Problem:** Part one was a straight forward string substitution. Part two turned out to be reversing the string substitution back down to a start point.
- **Solution:** I implemented a backwards greedy replacement. There's a lot more to this puzzle than I expected. See the [reddit solution mega thread](https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/) for further information on how this puzzle works.

### [Day 20](Day%2020) - Infinite Elves and Infinite Houses
- **Problem:** Factoring numbers!
- **Solution:** - I didn't know the method for factoring, so I watched some YouTube videos to understand it, then went and found a function from StackOverflow to do it for me. 

### [Day 21](Day%2021) - RPG Simulator 20XX
- **Problem:** Part one is a minimization problem, part two is the maximization to the same problem. 
- **Solution:** Once again, the techniques from [2022 Day 16](../2022/Day%2016/) came to our rescue. The trickier part for part two was figuring out exactly what to test (and where) to break our loop early. Part two also exposed a bug where we were able to equip the same equipment multiple times, so that didn't help when trying to find a maximum.
- **Update June 2, 2024:** Radically simplified the solution. The framework was in place to simply build out all the combinations of weapon, rings, and armor,then test them in a simple. `Where().Min()` type statement.

### [Day 22](Day%2022) - Wizard Simulator 20XX
- **Problem:** A minimization problem, where the solution took the form of a breadth first search.
- **Solution:** I (over?) built one hell of a pair of classes for the simulation. Starting with an `Entity` class that I broke into `Boss` and `Wizard`, I built the basic rules for the two to interact. The `Boss` got an `Attack` method, while the `Wizard` gained `CastSpell`. After that was setup, I built a `Spell` class that broke down to `Effects`. Everything went into the simulation and looped until we found our answer.

### [Day 23](Day%2023) - Opening the Turing Lock
- **Problem:** Basic assembly type instruction set and a short program to decode.
- **Solution:** Straight forward string parsing with a switch statement.

### [Day 24](Day%2024) - It Hangs in the Balance
- **Problem:** This looks like a knapsack problem, which it is, but.. 
- **Solution:** After bashing at a couple of ways to search (and watching my ram and CPU usage explode), I realized that what I really needed was to stop at the first depth I found a valid entry, then test the remaining subset of unused items to ensure they could also generate a solution. If both of those are true, then it's just a matter of `Sum()` and `Min()`. I implemented this, and ... it still ate ALL the processor time, but this time on the secondary check. I did a manual check, and then disabled the secondary check to get my answers.
- Uses a `CartesianProduct()` function from [Eric Lippert](https://ericlippert.com/)

### [Day 25](Day%2025) - Let It Snow
- **Problem:** Figure out a missing code based on a formula.
- **Solution:** A tight little loop that spends more time tracking where it is in the code book, than it does doing math. 