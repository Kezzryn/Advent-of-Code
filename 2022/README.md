# 2022 Puzzle Summary 
## Spoiler warnings. I do talk about solutions and techniques I used for the puzzles here, but in a general way.

### [Day 1](Day%2001) - Calorie Counting
- **Problem:** Load lists of integers then group, sum, and sort them.
- **Solution:** Solution uses a pair of LINQ statements.

### [Day 2](Day%2002) - Rock Paper Scissors
- **Problem:** Read input to determine the appropriate outcome in a Rock Paper Scissors contest.
- **Solution:** I use a pair of lookup tables and the input files as the indexes for the lookups. For a larger input set (EG: Rock, Paper, Scissors, Lizard, Spock ) I would precompute the lookup table instead.

### [Day 3](Day%2003) - Rucksack Reorganization
- **Problem:** Search the input for various string patterns. 
- **Solution:** There might be Regex for this, but I did it in a pair of `for` loops.

### [Day 4](Day%2004) - Camp Cleanup
- **Problem:** Determine if the ranges represented in pairs of numbers overlap.
- **Solution:** I pulled the numbers out of the input via Regex and passed them through a pair of `if` statements for the solution.

### [Day 5](Day%2005) - Supply Stacks
- **Problem:** Move stacks of cargo between stacks.
- **Solution:** I used an array of `Stack` objects to represent cargo and pop/push-ed between them. Part two involved moving multiple items at once, and keeping them in order. Since the puzzle enumerated positions between 1 and 9, I used the 0 position Stack in my array as a temporary storage location maintain the order. This works because when we pop from the source stack and push to the temporary stack, we reverse the order of the items. Repeating the process from the temporary stack to the destination restores the original order. ABC -> CBA -> ABC

### [Day 6](Day%2006) - Tuning Trouble
- **Problem:** Test substrings for uniqueness.
- **Solution:** I used looped calls to Distinct() comparing the size of the return to the size of the string I was testing. 

### [Day 7](Day%2007) - No Space Left on Device
- **Problem:** Find free space in a directory hierarchy.
- **Solution:** In this one I worked to the strict requirements of the puzzle. I built a tree structure. When adding files to the tree, I added the size to all parent nodes, with a recursive call to the parent ( `ParentDir?.AddSize(size);` ) This allowed me to sort/query the nodes and retrieve size for any individual node and all of its children without any need to walk the tree.

### [Day 8](Day%2008) - Treetop Tree House
- **Problem:** Testing ranges of numbers on an integer grid.
- **Solution:** Simple loops seemed best. There is probably a solution that involves slicing data that is compact, quick and wrong.

### [Day 9](Day%2009) - Rope Bridge
- **Problem:** Snake simulator.
- **Solution:** In this one we iteratively apply some rules to an array of Points, shifting them one at a time to follow the leader.

### [Day 10](Day%2010) - Cathode-Ray Tube
- **Problem:** Simple register and clock simulation.
- **Solution:** My answer to this was to simulate the two instructions, with the `addx` instruction morphing between cycles to indicate if it was on its load or execute sequence. 

### [Day 11](Day%2011) - Monkey in the Middle
- **Problem:** This is a game of moving items around, with a growing "worry factor" that needs to be tracked and operated on for each move. 
- **Solution:** I had no idea how to work with Part two. Hints in the reddit solution thread pointed towards Chinese Remainder Theorem as a way to manage the super large numbers the "worry factor" would become. 

### [Day 12](Day%2012) - Hill Climbing Algorithm
- **Problem:** Find the shortest path from A to B.
- **Solution:** Implemented an A* pathfinding algorithm based off the Wikipedia pseudocode.

### [Day 13](Day%2013) - Distress Signal
- **Problem:** Comparing nested lists of integers and lists, which can be blank.
- **Solution:** Built a complex packet comparison object that implements `IComparable`, `IEquatable`, and `IComparisonOperators` all so I can use A < B and `List<Packet>.Sort();` operations.

### [Day 14](Day%2014) - Regolith Reservoir
- **Problem:** Tetris!
- **Solution:** Collision detection is a thing. The key to Part two is measuring the height after X rocks is to figure out when the pattern repeats, and then math it out.

### [Day 15](Day%2015) - Beacon Exclusion Zone
- **Problem:** Find a spot not covered by the area of other rectangles. 
- **Solution:** Implemented a solution where I walked the perimeter of each rectangle. 

### [Day 16](Day%2016) - Proboscidea Volcanium
- **Problem:** Traveling Salesman type problem, but for a maximization.
- **Solution:** I struggled with trying to implement a Branch and Bound, but couldn't figure out how to maximize the heuristic. I wound up implementing a very neat solution from reddit user u/juanplopes that uses bit comparison to track the world state. As a bonus this solution very handily solves the part two of the puzzle. 

### [Day 17](Day%2017) - Pyroclastic Flow
- **Problem:** Tetris! 
- **Solution:** The key here of measuring height after X rocks is to figure out when the pattern repeats, and then math it out.

### [Day 18](Day%2018) - Boiling Boulders
- **Problem:** Given a 3d object defined by 1x1x1 cubes, find the area of it then identify the area of pocket within it. 
- **Solution:** I brought back the A* from [Day 12](Day%2012) to find if an area had a path from inside to a known outside point. This required working out the rules for a 3D point that worked with `IComparable` to be used in the algorithm. Initial versions ran for too long. Two optimizations were added. The first, stop the A* the moment it stepped outside the boundary of the object. Second, if we didn't escape, add all the points on that path to known inside points and not test them again.
 
### [Day 19](Day%2019) - Not Enough Minerals
- **Problem:** Optimization of robot tasks.
- **Solution:** Depth first search. The `RoboState` object does the heavy lifting of advancing time and holding a state of the factory. `RoboBlueprint` serves as the primary container for each blueprint string. A key learning item was the formula ( x + (y-1)/y ) to find the ceiling in integer division. If we need 15 items and we're generating them at the rate of 2 items a turn, we need to round up 15/2 to 8. 

### [Day 20](Day%2020) - Grove Positioning System
- **Problem:** Circular double linked list manipulation.
- **Solution:** A straight forward routine to move an item left or right in the list by calculating it's new position (accounting for loops) and updating the links to/from it. For large moves, I needed to account for the item not being in the list when looping, or it introduces an Off By X error where X is the number of times you pass the initial position. 

### [Day 21](Day%2021) - Monkey Math
- **Problem:** Binary tree walking.
- **Solution:** Working down the tree for part one is simple recursion. Working UP the tree for Part two involved some ... trickery. Part of the complexity is the math can change if you're coming up from the right or left of an element, so there are constant checks against the parent node.  

### [Day 22](Day%2022) - Monkey Map
- **Problem:** Pathfinding and cube folding.
- **Solution:** Split the map up into six individual "sides". Each side contains a link to its neighbors. As you walk the map, when you step off one, the map uses the link information to place yourself on the new map side. For part two, we add a translation function when transitioning form map to map to account for differences in orientation.
- **Future Fun** There are 11 nets for an unfolded cube. Replace the current hard coded folding for one that can take any shape input.

### [Day 23](Day%2023) - Unstable Diffusion
- **Problem:** Cell automata problem.
- **Solution:** Fairly straight forward solution. My initial solution needed a performance pass, as the Where/FindAll tests for neighbors was super slow. Speedups were achieved by dropping the Elf object in favor of a `HashSet<Point>` to represent the elves combined with generating a neighbor set to search the `HashSet` with. 

### [Day 24](Day%2024) - Blizzard Basin
- **Problem:** Pathfinding through a shifting map.
- **Solution:** I modified the A* pathfinding routine from [Day 12](Day%2012) to include a 3D point where the Z was used as a time dimension. For each step I calculated backwards to determine where a storm would have to start to intersect the current position, then checked the initial map for that information. 

### [Day 25](Day%2025) - Full of Hot Air
- **Problem:** Number conversion puzzle with the twist that some digits can represent negative numbers. 
- **Solution:** Adapted a general to/from base 10 routine I'd developed a decade ago for another project. I then extended the routine to be a general to/from base 10 converter with the ability to load in custom character dictionaries.
- **Future fun:** Make it a Base X to Base Y converter. IE: Hex to Octal w/o the need to go Hex -> Dec -> Oct