# 2015 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### Day 1 - Not Quite Lisp
- **Problem:** Track location based on inputs. 
- **Solution:** Loop through the string incrementing and decrementing a counter.

### Day 2 - I Was Told There Would Be No Math
- **Problem:** Find the volume, area, perimeter and smallest side calculations on a rectangular box.
- **Solution:** Abuse of anonymous arrays and LINQ syntax to calculate the smallest sides of things. Honestly, I was surprised I was allowed to define an anonymous array like that. Learning is fun. :)

### Day 3 - Perfectly Spherical Houses in a Vacuum
- **Problem:** Trace all visited locations.
- **Solution:** HashSets are nice for this, as duplicates are automatically discarded. This was a simple simulation, adding each moves to a `HashSet<Point>`, then querying the `HashSet` for the answers. 

### Day 4 - The Ideal Stocking Stuffer
- **Problem:** Creating MD5 hashs that fill a certain pattern. 
- **Solution:** Pulled code from documentation examples for the MD5 hash creation. I'm not sure if there's a faster way to find the answer to either part, other than to simply brute force it.

### Day 5 - Doesn't He Have Intern-Elves For This?
- **Problem:** Substring parsing.
- **Solution:** In part one, I used language tools to find the target strings. Part two is some simple looping, but could probably be improved with liberal application of Regex.

### Day 6 - Probably a Fire Hazard
- **Problem:** Change the values in a 1000x1000 grid.
- **Solution:** Simulated the changes with a `Dictionary<Point, int>` that grows as I add lights to it. 

### Day 7 - Some Assembly Required
- **Problem:** Bitwise logic gate emulation.
- **Solution:** This works out into a tree. Initial tests were taking a very long time. The fix was to cache the value of each node after it's calculated, so I don't wind up recalculating portions over and over again.

### Day 8 - Matchsticks
- **Problem:** String parsing and the comparison of representation vs storage lengths.
- **Solution:** Straight forward solution using built in language functions `Regex.Escape()` and `Regex.Unescape()`.

### Day 9 - All in a Single Night
- **Problem:** Traveling salesman problem. And hey! I can use my unused code from [2022 Day 16](../2022/Day%2016/) for this.
- **Solution:** My key insight was to realize that the sum of A -> B and B -> A is the edge weight between the nodes. The second insight was to NOT apply the Floyd–Warshall algorithm, as it does not apply to this puzzle. After that, I build a dictionary of answers with Branch and Bound. I need to figure out a better function for bounding on max.

### Day 10 - Elves Look, Elves Say
- **Problem:** This is a "Look and Say" problem. Sometimes known as the "Morris Number Sequence".
- **Solution:** String parsing with `IndexOfAny()` some range work and `.Except()`

### Day 11 - Corporate Policy
- **Problem:** Password generation that is required to follow certain rules.
- **Solution:** I could have created a string incrementer. Instead I borrow from the [2022 Day 25](../2022/Day%2025/) puzzle for a way to increment the password string while searching for a valid one.

### Day 12 - JSAbacusFramework.io
- **Problem:** JSON parsing.
- **Solution:** I used the [Newtonsoft.Json.Linq](https://www.newtonsoft.com/json) libraries to walk through the object model. Doing so required learning the object model. 

### Day 13 - Knights of the Dinner Table
- **Problem:** Traveling Salesmen of a circular list.
- **Solution:** This was brute forced with a simple depth first search. An improvement would be to introduce bounds checking or other heuristic.

### Day 14 - Reindeer Olympics
- **Problem:** Interval processing. The `TravelDistance` function does the heavy lifting. Working out first the overall interval, then determining how far the reindeer has travelled in the current interval.
- **Solution:** The tricky part was getting the LINQ right for the part two `GroupBy` clause. 

### Day 15 - Science for Hungry People
- **Problem:** Maximization problem.
- **Solution:** This has used the same form of search loop I've used in other solutions. The main change here is the next step system, which spreads out up to 28 points from each successful test. 
- **Future fun:** Create start keys and steps algorithmically. Narrow the search space with lower/upper bounds for each of the variables. I **think** this is a partial deritive calculation. 

### Day 16 - Aunt Sue
- **Problem:** Pattern matching with incomplete data.
- **Solution:** A `if` statement and `switch` statement.
- **Future fun:** Rework as a `Where` clause. 

### Day 17 - No Such Thing as Too Much
- **Problem:** Combinatorial problem.
- **Solution:** Used the bit technique from [2022 Day 16](../2022/Day%2016/) to track the container patterns.

### Day 18 - Like a GIF For Your Yard
- **Problem:** Game of Life simulation.
- **Solution:** I used a 3D array, with `x,y` as the puzzle state, with a flip flopping pointer to the third dimension as my next move. I'd read the state from `x,y,0` and write to `x,y,1`, then reverse the to/from.

### Day 19 - Medicine for Rudolph
- **Problem:** Part one was a straight forward string substitution. Part two turned out to be reversing the string substitution back down to a start point.
- **Solution:** I implemented a backwards greedy replacement. There's a lot more to this puzzle than I expected. See the [reddit solution megathread](https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/) for further information on how this puzzle works.

### Day 20 - Infinite Elves and Infinite Houses
- **Problem:** Factoring numbers!
- **Solution:** - I didn't know the method for factoring, so I watched some YouTube videos to understand it, then went and found a function from StackOverflow to do it for me. 

### Day 21 - RPG Simulator 20XX
- **Problem:** Part one is a minimization problem, part two is the maximization to the same problem. 
- **Solution:** Once again, the techniques from [2022 Day 16](../2022/Day%2016/) came to our rescue. The trickier part for part two was figuring out exactly what to test (and where) to break our loop early. Part two also exposed a bug where we were able to equip the same equipment multiple times, so that didn't help when trying to find a maximum.

### Day 22 - Wizard Simulator 20XX
- **Problem:** 
- **Solution:** 

### Day 23 - Opening the Turing Lock
- **Problem:** Basic assembly type instruction set and a short program to decode.
- **Solution:** Straight forward string parsing with a switch statement.

### Day 24 - It Hangs in the Balance
- **Problem:** 
- **Solution:** 

### Day 25 - Let It Snow
- **Problem:** 
- **Solution:** 